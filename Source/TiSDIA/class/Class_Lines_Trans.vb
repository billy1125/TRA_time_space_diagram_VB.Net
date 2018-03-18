Imports XMLRead

Public Class Class_Lines_Trans

    'Dim category As category = New category(Application.StartupPath & "\Category.xml")
    'Dim stations As category = New category(Application.StartupPath & "\Stations.xml")
    'Dim xmlRead As XMLRead.xmlRead


    Dim dtLocate As DataTable = New DataTable


    Dim _Category As categoryBuild
    Public Property Category() As categoryBuild
        Get
            Return _Category
        End Get
        Set(ByVal value As categoryBuild)
            _Category = value
        End Set
    End Property

    Dim _Stations As categoryBuild
    Public Property Stations() As categoryBuild
        Get
            Return _Stations
        End Get
        Set(ByVal value As categoryBuild)
            _Stations = value
        End Set
    End Property

    Dim _LineName As String = ""
    Public Property LineName() As String
        Get
            Return _LineName
        End Get
        Set(ByVal value As String)
            _LineName = value
        End Set
    End Property

    Dim _XMLRead As XMLRead.xmlRead
    Public Property XMLRead() As XMLRead.xmlRead
        Get
            Return _XMLRead
        End Get
        Set(ByVal value As XMLRead.xmlRead)
            _XMLRead = value
        End Set
    End Property

    Dim _line_table As DataTable
    Public Property LineTable() As DataTable
        Get
            Return _line_table
        End Get
        Set(ByVal value As DataTable)
            _line_table = value
        End Set
    End Property

    Public Sub New(ByVal _locate As DataTable)
        'Public Sub New(ByVal _link_name As String, ByVal _line_table As DataTable, ByVal _XMLRead As XMLRead.xmlRead)
        'Me.LineName = _link_name
        'dtLine = _line_table
        Me.dtLocate = _locate
        'Me.XMLRead = _XMLRead
    End Sub

    ''' <summary>
    ''' 依照營運路線的沿途車站，找出會經過車站的車次
    ''' </summary>
    ''' <param name="_trainNo"></param>
    ''' <returns></returns>
    Public Function getAllTrainNoInLines(Optional ByVal _trainNo As String = "") As DataTable

        Dim dtTrainsToDraw As DataTable = New DataTable

        With dtTrainsToDraw.Columns
            .Add("Train")
            .Add("LineDir")
            .Add("Line")
            .Add("OverNightStn")
        End With

        If _trainNo = "" Then
            For Each rowStations In Me.LineTable.Rows '根據營運路線的沿途車站，找出所有會經過這些車站的車次，例如：找出所有通過與停靠台東車站的車次
                For Each row In XMLRead.dtTimeInfo.Select("Station = '" & rowStations.item("ID").ToString & "'")
                    dtTrainsToDraw.Rows.Add()
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("Train") = row.Item("Train")
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("LineDir") = row.Item("LineDir")
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("Line") = XMLRead.dtTrain.Select("Train = '" & row.Item("Train") & "'")(0).Item("Line")
                    'dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("Line") = row.Item("Line")
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("OverNightStn") = XMLRead.dtTrain.Select("Train = '" & row.Item("Train") & "'")(0).Item("OverNightStn")
                Next
            Next
        ElseIf _trainNo <> "" Then '指定單一車次來繪製，可用於debug或者未來繪製單一運行線的功能
            For Each rowStations In Me.LineTable.Rows '根據營運路線的沿途車站，找出所有會經過這些車站的車次，例如：找出所有通過與停靠台東車站的車次
                For Each row In XMLRead.dtTrain.Select("Train = '" & _trainNo & "'")
                    dtTrainsToDraw.Rows.Add()
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("Train") = row.Item("Train")
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("LineDir") = row.Item("LineDir")
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("Line") = row.Item("Line")
                    dtTrainsToDraw.Rows(dtTrainsToDraw.Rows.Count - 1).Item("OverNightStn") = XMLRead.dtTrain.Select("Train = '" & row.Item("Train") & "'")(0).Item("OverNightStn")
                Next
            Next
        End If

        'Distinct 處理，過濾重複資料
        Dim tmpView As DataView = dtTrainsToDraw.DefaultView
        dtTrainsToDraw = tmpView.ToTable(True, "Train", "LineDir", "Line", "OverNightStn")

        Return dtTrainsToDraw

    End Function



    ''' <summary>
    ''' 依照篩選出來的車次進行停靠車站時間的設定與推算
    ''' </summary>
    ''' <param name="_drTrains"></param>
    ''' <param name="_over_Night_Stn">是否為過午夜車次</param>
    ''' <param name="_line_name">營運線名稱</param>
    ''' <param name="_line_kind">營運線類型</param>
    ''' <returns></returns>
    Public Function setTrainNoTable(ByVal _drTrains As DataRow,
                                    ByVal _over_Night_Stn As String,
                                    ByVal _line_name As String,
                                    ByVal _line_kind As Integer) As DataTable

        Dim bolResults As Boolean = False


        Dim dtLineTable As DataTable = buildLineTable(Me.LineTable) '車次停靠車站時間表，用來儲存每個車站停靠時間的DataTable，然後用於繪製車次線

        Dim dtTrainTable As DataTable = New DataTable '用來暫存單一車次停靠車站資訊列表的DataTable

        With dtTrainTable.Columns
            .Add("Train")
            .Add("Route")
            .Add("LineDir")
            .Add("Station")
            .Add("Order")
            .Add("DEPTime")
            .Add("ARRTime")
            .Add("KM")
        End With

        Dim ary As New ArrayList()

        For Each row In XMLRead.dtTimeInfo.Select("Train = '" & _drTrains.Item("Train") & "'")
            ary.Add(row.Item("Station"))
        Next

        Dim strStartStation As String = ary(0)
        Dim strEndStation As String = ary(ary.Count - 1)

        dtLineTable = test(_drTrains)

        If dtLineTable.Select("TERMINAL = 'S'")(0).Item("ARRTIME").ToString = "" Or dtLineTable.Select("TERMINAL = 'E'")(0).Item("ARRTIME").ToString = "" Then
            bolResults = True
            If dtLineTable.Select("Order = '1'").Length > 0 AndAlso dtLineTable.Select("Order = '1'")(0).Item("Station") = ary(0) Then
                If _drTrains.Item("LineDir") = "1" And dtLineTable.Select("TERMINAL = 'E'")(0).Item("ARRTIME").ToString <> "" Then
                    bolResults = False
                ElseIf _drTrains.Item("LineDir") = "0" And dtLineTable.Select("TERMINAL = 'S'")(0).Item("ARRTIME").ToString <> "" Then
                    bolResults = False
                End If
            ElseIf dtLineTable.Select("Order = '" & ary.Count & "'").Length > 0 AndAlso dtLineTable.Select("Order = '" & ary.Count & "'")(0).Item("Station") = ary(ary.Count - 1) Then
                If _drTrains.Item("LineDir") = "1" And dtLineTable.Select("TERMINAL = 'S'")(0).Item("ARRTIME").ToString <> "" Then
                    bolResults = False
                ElseIf _drTrains.Item("LineDir") = "0" And dtLineTable.Select("TERMINAL = 'E'")(0).Item("ARRTIME").ToString <> "" Then
                    bolResults = False
                End If
            End If
        End If



        If bolResults = True Then


            Dim intStartOrder As Integer = -1
            Dim intEndOrder As Integer = -1

            If Stations.dtStations_KM.Select("ID = '" & strStartStation & "'").Count > 0 Then
                'Dim row As DataRow = Stations.dtStations_KM.Select("ID = '" & strStartStation & "'")(0)
                'rowStartIndex = Stations.dtStations_KM.Rows.IndexOf(row)
                intStartOrder = Stations.dtStations_KM.Select("ID = '" & strStartStation & "'")(0).Item("EXTRA2")
            End If

            If Stations.dtStations_KM.Select("ID = '" & strEndStation & "'").Count > 0 Then
                'Dim row As DataRow = Stations.dtStations_KM.Select("ID = '" & strEndStation & "'")(0)
                'rowEndIndex = Stations.dtStations_KM.Rows.IndexOf(row)
                intEndOrder = Stations.dtStations_KM.Select("ID = '" & strEndStation & "'")(0).Item("EXTRA2")
            End If

            If intStartOrder >= 0 And intEndOrder >= 0 Then '沒辦法確定起訖車站順序則跳出，避掉支線(暫時先這樣)
                If _drTrains.Item("LineDir") = "1" Then

                    If intStartOrder < intEndOrder Then
                        '逆行
                        dtTrainTable = buildTrainTable_in_unClockwise(intStartOrder, intEndOrder, dtTrainTable, _drTrains, Stations.dtStations_KM)
                    ElseIf intStartOrder > intEndOrder Then
                        '逆行，且有跨過八堵
                        dtTrainTable = buildTrainTable_in_unClockwise(intStartOrder, 224, dtTrainTable, _drTrains, Stations.dtStations_KM1)
                        dtTrainTable = buildTrainTable_in_unClockwise(2, intEndOrder, dtTrainTable, _drTrains, Stations.dtStations_KM)
                    End If

                Else

                    If intStartOrder > intEndOrder Then
                        '順行，且沒有跨過八堵
                        dtTrainTable = buildTrainTable_in_Clockwise(intStartOrder, intEndOrder, dtTrainTable, _drTrains, Stations.dtStations_KM)
                    ElseIf intStartOrder < intEndOrder Then
                        '順行，且有跨過八堵
                        dtTrainTable = buildTrainTable_in_Clockwise(intStartOrder, 2, dtTrainTable, _drTrains, Stations.dtStations_KM)

                        dtTrainTable = buildTrainTable_in_Clockwise(224, intEndOrder, dtTrainTable, _drTrains, Stations.dtStations_KM1)

                    End If
                End If



                For Each row In dtTrainTable.Rows
                    If row.item("ARRTIME").ToString <> "" And row.item("DEPTIME").ToString <> "" Then
                        row.item("ARRTIME") = dtLocate.Select("DSC = '" & row.item("ARRTIME") & "'")(0).Item("ID")
                        row.item("DEPTIME") = dtLocate.Select("DSC = '" & row.item("DEPTIME") & "'")(0).Item("ID")
                    End If
                Next

                For i = 0 To ary.Count - 1
                    If i + 1 <= ary.Count - 1 Then
                        Dim sinA1 As Single = dtTrainTable.Select("Station = '" & ary(i) & "'")(0).Item("KM").ToString
                        Dim sinA2 As Single = dtTrainTable.Select("Station = '" & ary(i + 1) & "'")(0).Item("KM").ToString

                        Dim sinB1 As Single = dtTrainTable.Select("Station = '" & ary(i) & "'")(0).Item("DEPTime").ToString
                        Dim sinB2 As Single = dtTrainTable.Select("Station = '" & ary(i + 1) & "'")(0).Item("ARRTime").ToString

                        Dim sinSpeed As Single = (sinB2 - sinB1) / (sinA2 - sinA1) '跨車站間的速度

                        Dim intStart As Integer
                        Dim intEnd As Integer

                        If dtTrainTable.Select("Station = '" & ary(i) & "'").Count > 0 Then
                            Dim row As DataRow = dtTrainTable.Select("Station = '" & ary(i) & "'")(0)
                            intStart = dtTrainTable.Rows.IndexOf(row)
                        End If

                        If dtTrainTable.Select("Station = '" & ary(i + 1) & "'").Count > 0 Then
                            Dim row As DataRow = dtTrainTable.Select("Station = '" & ary(i + 1) & "'")(0)
                            intEnd = dtTrainTable.Rows.IndexOf(row)
                        End If

                        For j = intStart To intEnd
                            If j + 1 < intEnd Then
                                Dim sinA3 As Single = dtTrainTable.Rows(j + 1).Item("KM")
                                dtTrainTable.Rows(j + 1).Item("ARRTime") = sinSpeed * (sinA3 - sinA1) + sinB1
                                dtTrainTable.Rows(j + 1).Item("DEPTime") = sinSpeed * (sinA3 - sinA1) + sinB1
                            End If
                        Next
                    End If
                Next


                '根據營運路線，將時間填入車次停靠車站時間表
                For Each row In dtLineTable.Rows
                    If dtTrainTable.Select("STATION = '" & row.item("STATION") & "'").Length > 0 Then
                        row.item("ORDER") = dtTrainTable.Select("STATION = '" & row.item("STATION") & "'")(0).Item("ORDER").ToString
                        row.item("ARRTIME") = dtTrainTable.Select("STATION = '" & row.item("STATION") & "'")(0).Item("ARRTime").ToString
                        row.item("DEPTIME") = dtTrainTable.Select("STATION = '" & row.item("STATION") & "'")(0).Item("DEPTime").ToString
                    End If
                Next

                '時間轉換成座標值
                'For Each row In dtLineTable.Rows
                '    If row.item("ARRTIME").ToString <> "" And row.item("DEPTIME").ToString <> "" Then
                '        row.item("ARRTIME") = dtLocate.Select("DSC = '" & row.item("ARRTIME") & "'")(0).Item("ID")
                '        row.item("DEPTIME") = dtLocate.Select("DSC = '" & row.item("DEPTIME") & "'")(0).Item("ID")
                '    End If
                'Next

                '成追線轉換順逆行，因部分經成追線之車次，順逆行方向與西部幹線不同
                'If _mountain_sea = "WESTMOUNTAIN" AndAlso dtLineTable.Select("STATION = '1321'")(0).Item("ORDER").ToString <> "" Then
                '    If _drTrains.Item("LINEDIR") = "0" And
                '        (CInt(dtLineTable.Select("STATION = '1321'")(0).Item("ORDER").ToString) >
                '        CInt(dtLineTable.Select("STATION = '1319'")(0).Item("ORDER").ToString)) Then
                '        _drTrains.Item("LINEDIR") = "1"
                '    ElseIf _drTrains.Item("LINEDIR") = "1" And
                '        (CInt(dtLineTable.Select("STATION = '1321'")(0).Item("ORDER").ToString) <
                '        CInt(dtLineTable.Select("STATION = '1319'")(0).Item("ORDER").ToString)) Then
                '        _drTrains.Item("LINEDIR") = "0"
                '    End If
                'End If

                '支線就不需要檢查起點與終點車站
                'If _line_kind = 0 Or _line_kind = 1 Then
                '    'If _set_corss_line = False Then

                '    '找出這個車次最後一個停靠車站之順序碼 
                '    'Dim intEndOrder As Integer = xmlRead.dtTimeInfo.Select("Train = '" & _drTrains.Item("Train") & "'").Length

                '    '計算營運路線起始站時間
                '    'If _drTrains.Item("LineDir") = "1" And intStations = 1 And _yilan_line = True Then
                '    'Else

                '    'If _drTrains.Item(0) <> "5929A" Then
                '    dtLineTable = check_Terminals_Start(dtLineTable, dtTrainTable, _drTrains.Item("LINEDIR"), _over_Night_Stn)

                '        'End If

                '        '計算營運路線終點站時間
                '        'If _drTrains.Item("LineDir") = "0" And intStations = 1 And _yilan_line = True Then
                '        'Else

                '        dtLineTable = check_Terminals_End(dtLineTable, dtTrainTable, _drTrains.Item("LINEDIR"), _over_Night_Stn)
                '    'End If
                '    'End If

                '    'If intStations = 1 Then
                '    '    dtLineTable = buildLineTable(dtLine)
                '    'End If
                '    'ElseIf _set_corss_line = True Then
                '    '    set_corss_line(dtLineTable, dtTrainTable, _drTrains.Item("LINEDIR"))
                '    'End If
                'End If

            End If
        End If

        Return dtLineTable

    End Function

    ''' <summary>
    ''' 轉換資料
    ''' </summary>
    ''' <param name="_drTrains"></param>
    ''' <returns></returns>
    Private Function test(ByVal _drTrains As DataRow) As DataTable
        Dim dtLineTable As DataTable = buildLineTable(Me.LineTable) '車次停靠車站時間表，用來儲存每個車站停靠時間的DataTable，然後用於繪製車次線

        Dim dtTrainTable As DataTable = New DataTable '用來暫存單一車次停靠車站資訊列表的DataTable

        With dtTrainTable.Columns
            .Add("Train")
            .Add("Route")
            .Add("LineDir")
            .Add("Station")
            .Add("Order")
            .Add("DEPTime")
            .Add("ARRTime")
            .Add("KM")
        End With

        Dim bolResults As Boolean = False

        '依照車次找出停靠車站
        For Each row In XMLRead.dtTimeInfo.Select("Train = '" & _drTrains.Item("Train") & "'")
            dtTrainTable.Rows.Add()
            dtTrainTable.Rows(dtTrainTable.Rows.Count - 1).Item("Train") = row.Item("Train")
            dtTrainTable.Rows(dtTrainTable.Rows.Count - 1).Item("Route") = row.Item("Route")
            dtTrainTable.Rows(dtTrainTable.Rows.Count - 1).Item("LineDir") = row.Item("LineDir")
            dtTrainTable.Rows(dtTrainTable.Rows.Count - 1).Item("Station") = row.Item("Station")
            dtTrainTable.Rows(dtTrainTable.Rows.Count - 1).Item("Order") = row.Item("Order")
            dtTrainTable.Rows(dtTrainTable.Rows.Count - 1).Item("DEPTime") = row.Item("DEPTime")
            dtTrainTable.Rows(dtTrainTable.Rows.Count - 1).Item("ARRTime") = row.Item("ARRTime")
        Next

        '根據營運路線，將時間填入車次停靠車站時間表
        For Each row In dtLineTable.Rows
            If dtTrainTable.Select("STATION = '" & row.item("STATION") & "'").Length > 0 Then
                row.item("ORDER") = dtTrainTable.Select("STATION = '" & row.item("STATION") & "'")(0).Item("ORDER").ToString
                row.item("ARRTIME") = dtTrainTable.Select("STATION = '" & row.item("STATION") & "'")(0).Item("ARRTime").ToString
                row.item("DEPTIME") = dtTrainTable.Select("STATION = '" & row.item("STATION") & "'")(0).Item("DEPTime").ToString
            End If
        Next

        '時間轉換成座標值
        For Each row In dtLineTable.Rows
            If row.item("ARRTIME").ToString <> "" And row.item("DEPTIME").ToString <> "" Then
                row.item("ARRTIME") = dtLocate.Select("DSC = '" & row.item("ARRTIME") & "'")(0).Item("ID")
                row.item("DEPTIME") = dtLocate.Select("DSC = '" & row.item("DEPTIME") & "'")(0).Item("ID")
            End If
        Next


        Return dtLineTable

    End Function


    ''' <summary>
    ''' 順行資料處理
    ''' </summary>
    ''' <param name="_start_order"></param>
    ''' <param name="_end_order"></param>
    ''' <param name="_dt"></param>
    ''' <param name="_dr"></param>
    ''' <param name="_dtStation"></param>
    ''' <returns></returns>
    Private Function buildTrainTable_in_Clockwise(ByVal _start_order As Integer,
                                                  ByVal _end_order As Integer,
                                                  ByVal _dt As DataTable,
                                                  ByVal _dr As DataRow,
                                                  ByVal _dtStation As DataTable) As DataTable

        Do While _start_order >= _end_order
            'For i = rowStartIndex To rowEndIndex
            If XMLRead.dtTimeInfo.Select("Train = '" & _dr.Item("Train") & "' AND Station = '" &
                                         _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID") & "'").Count > 0 Then
                Dim row As DataRow = XMLRead.dtTimeInfo.Select("Train = '" & _dr.Item("Train") & "' AND Station = '" &
                                                              _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID") & "'")(0)
                _dt.Rows.Add()
                _dt.Rows(_dt.Rows.Count - 1).Item("Train") = row.Item("Train")
                _dt.Rows(_dt.Rows.Count - 1).Item("Route") = row.Item("Route")
                _dt.Rows(_dt.Rows.Count - 1).Item("LineDir") = row.Item("LineDir")
                _dt.Rows(_dt.Rows.Count - 1).Item("Station") = row.Item("Station")
                _dt.Rows(_dt.Rows.Count - 1).Item("Order") = _start_order 'row.Item("Order")
                _dt.Rows(_dt.Rows.Count - 1).Item("DEPTime") = row.Item("DEPTime")
                _dt.Rows(_dt.Rows.Count - 1).Item("ARRTime") = row.Item("ARRTime")

            Else
                If _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID") <> "NA" Then
                    _dt.Rows.Add()
                    _dt.Rows(_dt.Rows.Count - 1).Item("Train") = _dr.Item("Train")
                    _dt.Rows(_dt.Rows.Count - 1).Item("Route") = ""
                    _dt.Rows(_dt.Rows.Count - 1).Item("LineDir") = ""
                    _dt.Rows(_dt.Rows.Count - 1).Item("Station") = _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID")
                    _dt.Rows(_dt.Rows.Count - 1).Item("Order") = _start_order
                    _dt.Rows(_dt.Rows.Count - 1).Item("DEPTime") = ""
                    _dt.Rows(_dt.Rows.Count - 1).Item("ARRTime") = ""
                End If
            End If
            _dt.Rows(_dt.Rows.Count - 1).Item("KM") = _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("EXTRA1")

            _start_order -= 1
            'Next
        Loop

        Return _dt

    End Function

    ''' <summary>
    ''' 逆行資料處理
    ''' </summary>
    ''' <param name="_start_order"></param>
    ''' <param name="_end_order"></param>
    ''' <param name="_dt"></param>
    ''' <param name="_dr"></param>
    ''' <param name="_dtStation"></param>
    ''' <returns></returns>
    Private Function buildTrainTable_in_unClockwise(ByVal _start_order As Integer,
                                                    ByVal _end_order As Integer,
                                                    ByVal _dt As DataTable,
                                                    ByVal _dr As DataRow,
                                                    ByVal _dtStation As DataTable) As DataTable

        Do While _start_order <= _end_order
            'For i = rowStartIndex To rowEndIndex
            If XMLRead.dtTimeInfo.Select("Train = '" & _dr.Item("Train") & "' AND Station = '" &
                                             _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID") & "'").Count > 0 Then
                Dim row As DataRow = XMLRead.dtTimeInfo.Select("Train = '" & _dr.Item("Train") & "' AND Station = '" &
                                                                   _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID") & "'")(0)
                _dt.Rows.Add()
                _dt.Rows(_dt.Rows.Count - 1).Item("Train") = row.Item("Train")
                _dt.Rows(_dt.Rows.Count - 1).Item("Route") = row.Item("Route")
                _dt.Rows(_dt.Rows.Count - 1).Item("LineDir") = row.Item("LineDir")
                _dt.Rows(_dt.Rows.Count - 1).Item("Station") = row.Item("Station")
                _dt.Rows(_dt.Rows.Count - 1).Item("Order") = _start_order 'row.Item("Order")
                _dt.Rows(_dt.Rows.Count - 1).Item("DEPTime") = row.Item("DEPTime")
                _dt.Rows(_dt.Rows.Count - 1).Item("ARRTime") = row.Item("ARRTime")

            Else
                If _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID") <> "NA" Then
                    _dt.Rows.Add()
                    _dt.Rows(_dt.Rows.Count - 1).Item("Train") = _dr.Item("Train")
                    _dt.Rows(_dt.Rows.Count - 1).Item("Route") = ""
                    _dt.Rows(_dt.Rows.Count - 1).Item("LineDir") = ""
                    _dt.Rows(_dt.Rows.Count - 1).Item("Station") = _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("ID")
                    _dt.Rows(_dt.Rows.Count - 1).Item("Order") = _start_order
                    _dt.Rows(_dt.Rows.Count - 1).Item("DEPTime") = ""
                    _dt.Rows(_dt.Rows.Count - 1).Item("ARRTime") = ""
                End If
            End If
            _dt.Rows(_dt.Rows.Count - 1).Item("KM") = _dtStation.Select("EXTRA2 = '" & _start_order & "'")(0).Item("EXTRA1")

            _start_order += 1
            'Next
        Loop

        Return _dt

    End Function

    ''' <summary>
    ''' 營運線上的停靠車站表，用於填入各車次所停靠車站的資訊
    ''' </summary>
    ''' <param name="_dtline"></param>
    ''' <returns></returns>
    Private Function buildLineTable(ByVal _dtline As DataTable) As DataTable

        Dim dt As DataTable = New DataTable

        With dt.Columns
            .Add("STATION")
            .Add("TERMINAL")
            .Add("ORDER")
            .Add("KM")
            .Add("ARRTIME")
            .Add("DEPTIME")
        End With

        For Each row In _dtline.Rows '.Select("EXTRA2 = 'Y'")
                dt.Rows.Add()
            dt.Rows(dt.Rows.Count - 1).Item("STATION") = row.Item("ID")
            dt.Rows(dt.Rows.Count - 1).Item("KM") = row.Item("EXTRA1")
            dt.Rows(dt.Rows.Count - 1).Item("TERMINAL") = row.Item("EXTRA2")
        Next

        Return dt

    End Function

    Private Function checkChengzhuiLine(ByVal _train_table As DataTable) As Boolean

        '確認是否為成追線車次（成功站1321的下一站為追分，追分站1118的下一站為成功之車次）
        Dim bolResults As Boolean = False
        Dim aryOrder() As Integer = {0, 0}

        If _train_table.Select("STATION = '1321'").Length <> 0 Then
            aryOrder(0) = _train_table.Select("STATION = '1321'")(0).Item("ORDER")
        End If

        If _train_table.Select("STATION = '1118'").Length <> 0 Then
            aryOrder(1) = _train_table.Select("STATION = '1118'")(0).Item("ORDER")
        End If

        If System.Math.Abs(aryOrder(0) - aryOrder(1)) = 1 Then
            bolResults = True
        End If

        Return bolResults

    End Function

    ''' <summary>
    ''' 計算營運線起點車站經過時間
    ''' </summary>
    ''' <param name="_line_table"></param>
    ''' <param name="_train_table"></param>
    ''' <param name="_line_dir"></param>
    ''' <returns></returns>
    Private Function check_Terminals_Start(ByVal _line_table As DataTable,
                                           ByVal _train_table As DataTable,
                                           ByVal _line_dir As String,
                                           ByVal _Over_Night_Stn As String) As DataTable

        Dim bolTrans As Boolean = False '確認資料OK可進行轉換

        Dim intEndOrder As Integer = _train_table.Rows.Count

        '確認起點車站沒有資料，才進行計算
        If _line_table.Select("TERMINAL = 'S'")(0).Item("ARRTIME").ToString = "" And _line_table.Rows(0).Item("ARRTIME").ToString = "" Then
            'If _line_table.Select("TERMINAL = 'S'")(0).Item("ARRTIME").ToString = "" AndAlso checkChengzhuiLine(_train_table) = False Then

            Dim intHighestOrder As Integer
            Dim aryList As ArrayList = New ArrayList
            'Dim i As Integer = 0

            '一行行查詢繪製表的順序碼，找出繪製表中最上方有值的順序
            For Each row In _line_table.Rows
                If row.item("ORDER").ToString <> "" Then
                    aryList.Add(row.item("ORDER").ToString)
                End If
            Next
            'Do Until _line_table.Rows(i).Item("ORDER").ToString <> "" Or i = _line_table.Rows.Count - 1
            '    i += 1
            'Loop

            intHighestOrder = aryList(0) 'CInt(_line_table.Rows(i).Item("ORDER"))

            '該值如果與此車次最後停靠車站相同，則要另外依照繪製表車站順序與順逆行，決定是否要找出上一個停靠的車站
            If Me.LineName <> "TAITUNG" Then
                If _line_dir = 0 Then
                    If intHighestOrder <> intEndOrder Then
                        bolTrans = True
                    ElseIf intHighestOrder = intEndOrder Then
                        bolTrans = False
                    End If
                ElseIf _line_dir = 1 Then
                    If intHighestOrder <> 1 Then
                        bolTrans = True
                    ElseIf intHighestOrder = 1 Then
                        bolTrans = False
                    End If
                End If
            Else
                If _line_dir = 1 Then
                    If intHighestOrder <> intEndOrder Then
                        bolTrans = True
                    ElseIf intHighestOrder = intEndOrder Then
                        bolTrans = False
                    End If
                ElseIf _line_dir = 0 Then
                    If intHighestOrder <> 1 Then
                        bolTrans = True
                    ElseIf intHighestOrder = 1 Then
                        bolTrans = False
                    End If
                End If
            End If



            'If intHighestOrder.ToString <> _line_table.Select("TERMINAL = 'E'")(0).Item("ORDER").ToString Then
            '    If intHighestOrder <> 1 Then
            '        If intHighestOrder <> _train_no_last_order Then
            '            bolTrans = True
            '        ElseIf intHighestOrder = _train_no_last_order Then
            '            If _line_dir = 1 Then
            '                bolTrans = True
            '            End If
            '        End If
            '    End If
            'ElseIf intHighestOrder.ToString = _line_table.Select("TERMINAL = 'E'")(0).Item("ORDER").ToString Then
            '    If _line_dir = 0 Then
            '        bolTrans = True
            '    End If
            'End If


            If bolTrans = True Then

                Dim intFindOrder As Integer

                If _line_dir = "0" Then
                    intFindOrder = intHighestOrder + 1
                ElseIf _line_dir = "1" Then
                    intFindOrder = intHighestOrder - 1
                End If

                '找出上一個停靠車站(不在要繪製的營運路線中)
                Dim row As DataRow = _train_table.Select("ORDER = '" & intFindOrder & "'")(0)

                '宜蘭線跨八堵車站問題，避免將基隆與三坑車站列入計算(特殊例外處理)
                If Me.LineName <> "YILAN" OrElse (row.Item("STATION") <> "1001" And row.Item("STATION") <> "1029") Then

                    '轉換該停靠車站的時間座標值
                    If row.Item("ARRTIME").ToString <> "" And row.Item("DEPTIME").ToString <> "" Then
                        row.Item("ARRTIME") = dtLocate.Select("DSC = '" & row.Item("ARRTIME") & "'")(0).Item("ID")
                        row.Item("DEPTIME") = dtLocate.Select("DSC = '" & row.Item("DEPTIME") & "'")(0).Item("ID")
                    End If

                    '檢查上一個停靠車站是不是在里程表內，沒有代表為支線
                    If Stations.dtStations_KM.Select("ID = '" & row.Item("STATION") & "'").Length > 0 Then

                        '推算上一個停靠車站的座標值
                        Dim intPreviousKM As Single = getSation_KM_not_in_line(row.Item("STATION"),
                                                                               _line_table.Rows(0).Item("STATION"),
                                                                                Me.LineName,
                                                                               "S")
                        '推算上一個停靠車站座標值
                        Dim intTime As Single
                        '如果上一個停靠車站為過夜車站，另外將離站時間增加1441，處理跨午夜車次問題
                        If _line_dir = "0" Then
                            If _Over_Night_Stn = row.Item("STATION") Then
                                intTime = caculate_middle_time(row.Item("ARRTIME") + 1441,
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("DEPTIME"),
                                                              intPreviousKM,
                                                              _line_table.Select("TERMINAL = 'S'")(0).Item("KM"),
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("KM"))
                            Else
                                intTime = caculate_middle_time(row.Item("ARRTIME"),
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("DEPTIME"),
                                                              intPreviousKM,
                                                              _line_table.Select("TERMINAL = 'S'")(0).Item("KM"),
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("KM"))
                            End If
                        ElseIf _line_dir = "1" Then
                            If _Over_Night_Stn = row.Item("STATION") Then
                                intTime = caculate_middle_time(row.Item("DEPTIME") - 1441,
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("ARRTIME"),
                                                              intPreviousKM,
                                                              _line_table.Select("TERMINAL = 'S'")(0).Item("KM"),
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("KM"))
                            Else
                                intTime = caculate_middle_time(row.Item("DEPTIME"),
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("ARRTIME"),
                                                              intPreviousKM,
                                                              _line_table.Select("TERMINAL = 'S'")(0).Item("KM"),
                                                              _line_table.Select("ORDER = '" & intHighestOrder & "'")(0).Item("KM"))
                            End If
                        End If


                        _line_table.Select("TERMINAL = 'S'")(0).Item("ORDER") = intFindOrder
                        _line_table.Select("TERMINAL = 'S'")(0).Item("ARRTIME") = intTime
                        _line_table.Select("TERMINAL = 'S'")(0).Item("DEPTIME") = intTime
                    End If
                End If
            End If
        End If

        Return _line_table

    End Function

    ''' <summary>
    ''' 計算營運線終點車站經過時間
    ''' </summary>
    ''' <param name="_line_table"></param>
    ''' <param name="_train_table"></param>
    ''' <param name="_line_dir"></param>
    ''' <returns></returns>
    Private Function check_Terminals_End(ByVal _line_table As DataTable,
                                         ByVal _train_table As DataTable,
                                         ByVal _line_dir As String,
                                         ByVal _Over_Night_Stn As String) As DataTable

        Dim bolTrans As Boolean = False

        Dim intEndOrder As Integer = _train_table.Rows.Count

        '確認終點車站沒有資料，才進行計算
        If _line_table.Select("TERMINAL = 'E'")(0).Item("ARRTIME").ToString = "" AndAlso checkChengzhuiLine(_train_table) = False Then
            Dim intLowestOrder As Integer
            Dim aryList As ArrayList = New ArrayList

            For Each row In _line_table.Rows
                If row.item("ORDER").ToString <> "" Then
                    aryList.Add(row.item("ORDER").ToString)
                End If
            Next


            'Dim i As Integer = _line_table.Rows.Count - 1

            'Do Until _line_table.Rows(i).Item("ORDER").ToString <> "" Or i = 0
            '    i -= 1
            'Loop

            '一行行查詢繪製表的順序碼，找出繪製表中最下方有值的順序
            'intLowestOrder = CInt(_line_table.Rows(i).Item("ORDER"))
            intLowestOrder = aryList(aryList.Count - 1)

            '該值如果與此車次最後停靠車站相同，則要另外依照繪製表車站順序與順逆行，決定是否要找出上一個停靠的車站
            If _line_dir = 0 Then
                If intLowestOrder <> intEndOrder Then
                    bolTrans = True
                ElseIf intLowestOrder = intEndOrder Then
                    bolTrans = False
                End If
            ElseIf _line_dir = 1 Then
                If intLowestOrder <> 1 Then
                    bolTrans = True
                ElseIf intLowestOrder = 1 Then
                    bolTrans = False
                End If
            End If

            'If intLowestOrder.ToString <> _line_table.Select("TERMINAL = 'S'")(0).Item("ORDER").ToString Then
            '    If intLowestOrder <> 1 Then
            '        If intLowestOrder <> _train_no_last_order Then
            '            bolTrans = True
            '        ElseIf intLowestOrder = _train_no_last_order Then
            '            If _line_dir = 0 Then
            '                bolTrans = True
            '            End If
            '        End If
            '    End If
            'ElseIf intLowestOrder.ToString = _line_table.Select("TERMINAL = 'S'")(0).Item("ORDER").ToString Then
            '    If _line_dir = 1 Then
            '        bolTrans = True
            '    End If
            'End If


            If bolTrans = True Then

                Dim intFindOrder As Integer

                If _line_dir = "0" Then
                    intFindOrder = intLowestOrder - 1
                ElseIf _line_dir = "1" Then
                    intFindOrder = intLowestOrder + 1
                End If

                '找出下一個停靠車站(不在要繪製的營運路線中)
                If intFindOrder <= _train_table.Rows.Count And intFindOrder <> 0 Then
                    Dim row As DataRow = _train_table.Select("ORDER = '" & intFindOrder & "'")(0)

                    '檢查下一個停靠車站是不是在里程表內，沒有代表為支線
                    If Stations.dtStations_KM.Select("ID = '" & row.Item("STATION") & "'").Length > 0 Then

                        '轉換該停靠車站的時間座標值
                        If row.Item("ARRTIME").ToString <> "" And row.Item("DEPTIME").ToString <> "" Then
                            row.Item("ARRTIME") = dtLocate.Select("DSC = '" & row.Item("ARRTIME") & "'")(0).Item("ID")
                            row.Item("DEPTIME") = dtLocate.Select("DSC = '" & row.Item("DEPTIME") & "'")(0).Item("ID")
                        End If

                        '推算下一個停靠車站的座標值
                        Dim intPreviousKM As Single = getSation_KM_not_in_line(row.Item("STATION"),
                                                                               _line_table.Rows(_line_table.Rows.Count - 1).Item("STATION"),
                                                                                Me.LineName,
                                                                               "E")

                        '推算下一個停靠車站座標值
                        Dim intTime As Single
                        '如果下一個停靠車站為過夜車站，另外將離站時間增加1441，處理跨午夜車次問題
                        If _line_dir = "0" Then
                            If _Over_Night_Stn = row.Item("STATION") Then
                                intTime = caculate_middle_time(row.Item("ARRTIME") + 1441,
                                                                         _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("DEPTIME"),
                                                                         intPreviousKM,
                                                                         _line_table.Select("TERMINAL = 'E'")(0).Item("KM"),
                                                                         _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("KM"))
                            Else
                                intTime = caculate_middle_time(row.Item("ARRTIME"),
                                                                          _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("DEPTIME"),
                                                                          intPreviousKM,
                                                                          _line_table.Select("TERMINAL = 'E'")(0).Item("KM"),
                                                                          _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("KM"))
                            End If
                        ElseIf _line_dir = "1" Then
                            If _Over_Night_Stn = row.Item("STATION") Then
                                intTime = caculate_middle_time(row.Item("DEPTIME") + 1441,
                                                                         _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("ARRTIME"),
                                                                         intPreviousKM,
                                                                         _line_table.Select("TERMINAL = 'E'")(0).Item("KM"),
                                                                         _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("KM"))
                            Else
                                intTime = caculate_middle_time(row.Item("DEPTIME"),
                                                                          _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("ARRTIME"),
                                                                          intPreviousKM,
                                                                          _line_table.Select("TERMINAL = 'E'")(0).Item("KM"),
                                                                          _line_table.Select("ORDER = '" & intLowestOrder & "'")(0).Item("KM"))
                            End If
                        End If



                        _line_table.Select("TERMINAL = 'E'")(0).Item("ORDER") = intFindOrder
                        _line_table.Select("TERMINAL = 'E'")(0).Item("ARRTIME") = intTime
                        _line_table.Select("TERMINAL = 'E'")(0).Item("DEPTIME") = intTime

                    End If
                End If
            End If
        End If

        Return _line_table

    End Function

    ''' <summary>
    ''' 特殊跨線車次處理，例：426車次，完全沒有停靠宜蘭線的車站
    ''' </summary>
    ''' <param name="_line_table"></param>
    ''' <param name="_train_table"></param>
    ''' <param name="_line_dir"></param>
    ''' <returns></returns>
    Private Function set_corss_line(ByVal _line_table As DataTable,
                                    ByVal _train_table As DataTable,
                                    ByVal _line_dir As String) As DataTable

        Dim strStartStation As String = ""
        Dim strEndStation As String = ""
        Dim i As Integer = 0

        If _line_dir = "0" Then
            For Each row As DataRow In _train_table.Rows
                If Me.Category.dtLine_WN.Select("ID = '" & row.Item("STATION") & "'").Length > 0 Then
                    strStartStation = row.Item("STATION")
                End If
            Next

            Do Until Me.Category.dtLine_N.Select("ID = '" & _train_table.Rows(i).Item("STATION") & "'").Length > 0
                i += 1
            Loop
            strEndStation = _train_table.Rows(i).Item("STATION")
        ElseIf _line_dir = "1" Then
            For Each row As DataRow In _train_table.Rows
                If Me.Category.dtLine_N.Select("ID = '" & row.Item("STATION") & "'").Length > 0 Then
                    strStartStation = row.Item("STATION")
                End If
            Next

            Do Until Me.Category.dtLine_WN.Select("ID = '" & _train_table.Rows(i).Item("STATION") & "'").Length > 0
                i += 1
            Loop
            strEndStation = _train_table.Rows(i).Item("STATION")
        End If

        Dim rowStartStation As DataRow = _train_table.Select("STATION = '" & strStartStation & "'")(0)

        rowStartStation.Item("ARRTIME") = dtLocate.Select("DSC = '" & rowStartStation.Item("ARRTIME") & "'")(0).Item("ID")
        rowStartStation.Item("DEPTIME") = dtLocate.Select("DSC = '" & rowStartStation.Item("DEPTIME") & "'")(0).Item("ID")

        Dim rowEndStation As DataRow = _train_table.Select("STATION = '" & strEndStation & "'")(0)

        rowEndStation.Item("ARRTIME") = dtLocate.Select("DSC = '" & rowEndStation.Item("ARRTIME") & "'")(0).Item("ID")
        rowEndStation.Item("DEPTIME") = dtLocate.Select("DSC = '" & rowEndStation.Item("DEPTIME") & "'")(0).Item("ID")


        Dim intStartKM As Single
        Dim intEndKM As Single

        If _line_dir = "0" Then
            intStartKM = getSation_KM_not_in_line(strStartStation, _line_table.Select("TERMINAL = 'E'")(0).Item("STATION"), Me.LineName)
            intEndKM = getSation_KM_not_in_line(strEndStation, _line_table.Select("TERMINAL = 'S'")(0).Item("STATION"), Me.LineName)
        ElseIf _line_dir = "1" Then
            intStartKM = getSation_KM_not_in_line(strStartStation, _line_table.Select("TERMINAL = 'S'")(0).Item("STATION"), Me.LineName)
            intEndKM = getSation_KM_not_in_line(strEndStation, _line_table.Select("TERMINAL = 'E'")(0).Item("STATION"), Me.LineName)
        End If

        Dim intTime1 As Single = caculate_middle_time(rowStartStation.Item("DEPTIME"),
                                                     rowEndStation.Item("ARRTIME"),
                                                     intStartKM,
                                                     _line_table.Select("TERMINAL = 'E'")(0).Item("KM"),
                                                     intEndKM)

        Dim intTime2 As Single = caculate_middle_time(rowStartStation.Item("DEPTIME"),
                                                     rowEndStation.Item("ARRTIME"),
                                                     intStartKM,
                                                     _line_table.Select("TERMINAL = 'S'")(0).Item("KM"),
                                                     intEndKM)

        _line_table.Select("TERMINAL = 'E'")(0).Item("ARRTIME") = intTime1
        _line_table.Select("TERMINAL = 'E'")(0).Item("DEPTIME") = intTime1

        _line_table.Select("TERMINAL = 'S'")(0).Item("ARRTIME") = intTime2
        _line_table.Select("TERMINAL = 'S'")(0).Item("DEPTIME") = intTime2

        Return _line_table

    End Function

    ''' <summary>
    ''' 推算上一個停靠車站的座標值
    ''' </summary>
    ''' <param name="_start_station"></param>
    ''' <param name="_end_station"></param>
    ''' <param name="_line"></param>
    ''' <returns></returns>
    Private Function getSation_KM_not_in_line(ByVal _start_station As String,
                                              ByVal _end_station As String,
                                              ByVal _line As String,
                                              Optional ByVal _kind As String = "") As Single

        '公式: 3000(繪製圖表最大的尺寸) * (終點車站里程數 - 起點車站里程數) / 營運路線總里程數

        Dim sinKM As Single
        Dim sinLineKM As Single = CSng(stations.dtLines_KM.Select("ID = '" & _line & "'")(0).Item("EXTRA1").ToString)
        Dim sinTargetSation As Single = CSng(stations.dtStations_KM.Select("ID = '" & _start_station & "'")(0).Item("EXTRA1").ToString)
        Dim sinStartSation As Single = CSng(stations.dtStations_KM.Select("ID = '" & _end_station & "'")(0).Item("EXTRA1").ToString)

        If _kind = "S" Then
            If _line = "WESTNORTH" Then
                sinKM = 3000 * (sinTargetSation - 878) / sinLineKM
            Else
                sinKM = 3000 * (sinTargetSation - sinStartSation) / sinLineKM
            End If
        ElseIf _kind = "E" Then
            If _line = "YILAN" Then
                sinKM = 3000 * (sinTargetSation + sinLineKM) / sinLineKM
            Else
                sinKM = 3000 * (sinTargetSation - sinStartSation + sinLineKM) / sinLineKM
            End If
        End If

        Return sinKM

    End Function

    ''' <summary>
    ''' 推算起點車站座標值
    ''' </summary>
    ''' <param name="_first_time"></param>
    ''' <param name="_end_time"></param>
    ''' <param name="_first_km"></param>
    ''' <param name="_middle_km"></param>
    ''' <param name="_end_km"></param>
    ''' <returns></returns>
    Private Function caculate_middle_time(ByVal _first_time As String,
                                          ByVal _end_time As String,
                                          ByVal _first_km As String,
                                          ByVal _middle_km As String,
                                          ByVal _end_km As String) As Single
        Dim sinNewX As Single

        'Dim p As Single = CSng(_middle_km) - CSng(_first_km)
        'Dim q As Single = CSng(_end_km) - CSng(_first_km)
        'Dim z As Single = CSng(_end_time) - CSng(_first_time)
        'sinNewX = (p * z + q * CSng(_first_time)) / q

        '內插法 y = [ (y2 -y1) / (x2 - x1) ] (x - x1) + y1
        sinNewX = ((CSng(_end_time) - CSng(_first_time)) / (CSng(_end_km) - CSng(_first_km))) *
                  (CSng(_middle_km) - CSng(_first_km)) + CSng(_first_time)

        Return sinNewX
    End Function

    ''' <summary>
    ''' 跨夜車次處理，午夜前
    ''' </summary>
    ''' <param name="_line_table"></param>
    ''' <param name="_over_night_stn"></param>
    ''' <param name="_line_dir"></param>
    ''' <returns></returns>
    Public Function setOverNightStn_before_midnight(ByVal _line_table As DataTable,
                                                    ByVal _over_night_stn As String,
                                                    ByVal _line_dir As String) As DataTable

        Try
            Dim intIndex As Integer = _line_table.Rows.IndexOf(_line_table.Select("Station = '" & _over_night_stn & "'")(0))

            Dim i As Integer

            If _line_dir = "0" And _line_table.Rows.Count <> intIndex Then

                i = intIndex + 1

                Do Until _line_table.Rows.Count = i OrElse (_line_table.Rows(i).Item("ARRTime").ToString <> "" And _line_table.Rows(i).Item("DEPTime").ToString <> "")
                    i += 1
                Loop

            ElseIf _line_dir = "1" And intIndex <> 0 Then

                i = intIndex - 1

                Do Until (_line_table.Rows(i).Item("ARRTime").ToString <> "" And _line_table.Rows(i).Item("DEPTime").ToString <> "") Or _line_table.Rows.Count - 1 = i
                    i -= 1
                Loop

            End If

            If _line_table.Rows.Count <> i Then

                Dim sinTime As Single = caculate_middle_time(_line_table.Rows(intIndex).Item("KM"),
                                                             _line_table.Rows(i).Item("KM"),
                                                             _line_table.Rows(intIndex).Item("ARRTime"),
                                                             0,
                                                             _line_table.Rows(i).Item("DEPTime") - 1440)

                _line_table.Rows(intIndex).Item("KM") = sinTime
                _line_table.Rows(intIndex).Item("ARRTime") = "1440"
                _line_table.Rows(intIndex).Item("DEPTime") = "1440"

                If _line_dir = "0" Then

                    Do Until _line_table.Rows(0).Item("Station") = _over_night_stn
                        _line_table.Rows.RemoveAt(0)
                    Loop

                ElseIf _line_dir = "1" Then

                    Do Until _line_table.Rows.Count - 1 = i
                        _line_table.Rows.RemoveAt(_line_table.Rows.Count - 1)
                    Loop

                End If
            End If

            'Do Until _line_table.Rows(0).Item("Station") = _over_night_stn
            '    _line_table.Rows.RemoveAt(0)
            'Loop

            'If _line_table.Rows.Count > 1 Then
            '    Dim sinTime As Single = caculate_middle_time(_line_table.Rows(0).Item("KM"),
            '                                              _line_table.Rows(1).Item("KM"),
            '                                              _line_table.Rows(0).Item("ARRTime"),
            '                                              0,
            '                                              _line_table.Rows(1).Item("DEPTime") - 1440)

            '    _line_table.Rows(0).Item("KM") = sinTime
            '    _line_table.Rows(0).Item("ARRTime") = "1440"
            '    _line_table.Rows(0).Item("DEPTime") = "1440"
            'End If
        Catch ex As Exception

        End Try
        Return _line_table

    End Function


    ''' <summary>
    ''' 跨夜車次處理，午夜後
    ''' </summary>
    ''' <param name="_line_table"></param>
    ''' <param name="_over_night_stn"></param>
    ''' <param name="_line_dir"></param>
    ''' <returns></returns>
    Public Function setOverNightStn_after_midnight(ByVal _line_table As DataTable,
                                                   ByVal _over_night_stn As String,
                                                   ByVal _line_dir As String) As DataTable

        Try



            Dim intIndex As Integer = _line_table.Rows.IndexOf(_line_table.Select("Station = '" & _over_night_stn & "'")(0))

            Dim i As Integer '= intIndex + 1

            'Dim intLast As Integer

            'Do Until (_line_table.Rows(i).Item("ARRTime").ToString <> "" And _line_table.Rows(i).Item("DEPTime").ToString <> "") Or _line_table.Rows.Count = i + 1
            '    'intLast = i
            '    i += 1
            'Loop

            If _line_dir = "0" Then

                i = intIndex + 1

                Do Until _line_table.Rows.Count = i OrElse (_line_table.Rows(i).Item("ARRTime").ToString <> "" And _line_table.Rows(i).Item("DEPTime").ToString <> "")
                    i += 1
                Loop

            ElseIf _line_dir = "1" Then

                i = intIndex - 1

                Do Until (_line_table.Rows(i).Item("ARRTime").ToString <> "" And _line_table.Rows(i).Item("DEPTime").ToString <> "") Or _line_table.Rows.Count - 1 = i
                    i -= 1
                Loop

            End If

            If _line_table.Rows.Count <> i Then

                Dim sinTime As Single = caculate_middle_time(_line_table.Rows(intIndex).Item("KM"),
                                                              _line_table.Rows(i).Item("KM"),
                                                              _line_table.Rows(intIndex).Item("ARRTime") + 1440,
                                                              1440,
                                                              _line_table.Rows(i).Item("DEPTime"))

                _line_table.Rows(i).Item("KM") = sinTime
                _line_table.Rows(i).Item("ARRTime") = "0"
                _line_table.Rows(i).Item("DEPTime") = "0"

                'Do Until _line_table.Rows.Count - 1 = i
                '    _line_table.Rows.RemoveAt(_line_table.Rows.Count - 1)
                'Loop


                If _line_dir = "1" Then

                    Dim strRow As String = _line_table.Rows(i).Item("Station")

                    Do Until _line_table.Rows(0).Item("Station") = strRow
                        _line_table.Rows.RemoveAt(0)
                    Loop

                ElseIf _line_dir = "0" Then

                    Do Until _line_table.Rows.Count - 1 = i
                        _line_table.Rows.RemoveAt(_line_table.Rows.Count - 1)
                    Loop

                End If
            End If

            'Dim intIndex As Integer = _line_table.Rows.IndexOf(_line_table.Select("Station = '" & _over_night_stn & "'")(0))
            'Dim intRows As Integer = _line_table.Rows.Count - 1

            'If intIndex <> intRows Then

            '    Do Until intRows = intIndex + 1
            '        _line_table.Rows.RemoveAt(intRows)
            '        intRows = _line_table.Rows.Count - 1
            '    Loop

            '    Dim sinTime As Single = caculate_middle_time(_line_table.Rows(intRows - 1).Item("KM"),
            '                                                  _line_table.Rows(intRows).Item("KM"),
            '                                                  _line_table.Rows(intRows - 1).Item("ARRTime") + 1441,
            '                                                  1441,
            '                                                  _line_table.Rows(intRows).Item("DEPTime"))

            '    _line_table.Rows(intRows).Item("KM") = sinTime
            '    _line_table.Rows(intRows).Item("ARRTime") = "0"
            '    _line_table.Rows(intRows).Item("DEPTime") = "0"
            'End If
        Catch ex As Exception

        End Try
        Return _line_table

    End Function


End Class
