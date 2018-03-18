Imports XMLRead

Public Class frmMain

    Dim categoryBuild As categoryBuild = New categoryBuild(Application.StartupPath & "\Category.xml") '類別
    Dim stations As categoryBuild = New categoryBuild(Application.StartupPath & "\Stations.xml") '車站里程表(環島)

    Dim clsLinesTrans As Class_Lines_Trans
    'Dim clsLinesTrans As Class_Lines_Trans_IN

    Dim strMode As String = "" '參數

    Dim aryFileList As ArrayList = New ArrayList

    Dim xmlRead As XMLRead.xmlRead

    Dim _date_choose As String
    Public Property date_choose() As String
        Get
            Return _date_choose
        End Get
        Set(ByVal value As String)
            _date_choose = value
        End Set
    End Property

    Dim strOutputLoaction As String = ""

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Dim strFilePath As String = Application.StartupPath & "\xml\" & Now.Date.ToString("yyyyMMdd") & ".xml"

        'downloadTRAXML_single(strFilePath)


        xmlRead = New XMLRead.xmlRead("")

        clsLinesTrans = New Class_Lines_Trans(categoryBuild.dtLocate)

        '將目前所有的台鐵公開XML文件檔名抓進來
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(Application.StartupPath & "\xml\")
            aryFileList.Add(foundFile)
        Next

        getParameters()

        If strMode = "BATCH" Then
            'downloadTRAXML()

            batchProcess("WESTNORTH", Me.categoryBuild.dtLine_WN, "west_link_north")
            'batchProcess("WESTSOUTH", Me.categoryBuild.dtLine_WS, "西部幹線南段")
            'batchProcess("WESTMOUNTAIN", Me.categoryBuild.dtLine_WM, "西部幹線台中線")
            'batchProcess("WESTSEA", Me.categoryBuild.dtLine_WSEA, "西部幹線海岸線")
            'batchProcess("PINGTUNG", Me.categoryBuild.dtLine_P, "屏東線")
            'batchProcess("SOUTHLINK", Me.categoryBuild.dtLine_S, "南迴線")
            'batchProcess("TAITUNG", Me.categoryBuild.dtLine_T, "台東線")
            batchProcess("YILAN", Me.categoryBuild.dtLine_I, "yilan")
            'batchProcess("PINGXI", Me.categoryBuild.dtLine_PX, "平溪深澳線")
            'batchProcess("NEIWAN", Me.categoryBuild.dtLine_NW, "內灣線")
            'batchProcess("JIJI", Me.categoryBuild.dtLine_J, "集集線")
            'batchProcess("SHALUN", Me.categoryBuild.dtLine_SL, "沙崙線")

            Me.Close()
        End If



    End Sub



    ''' <summary>
    ''' 取得參數
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub getParameters()

        Dim inputArgument As String
        For Each s As String In My.Application.CommandLineArgs

            inputArgument = UCase("ADD_PARAM=")
            If s.ToUpper.StartsWith(inputArgument) Then
                Dim strParam As String = UCase(s.Remove(0, inputArgument.Length))
                Dim j1 As Integer = 0
                Dim m1 As Integer = 0
                Dim vx1 As String = ""
                Dim strParmNm As String = ""
                Dim strParmValue As String = ""

                Do
                    j1 = m1 + 1
                    m1 = InStr(j1, strParam & ";", ";")
                    If m1 > 0 Then
                        vx1 = Mid(strParam, j1, m1 - j1)
                        strParmNm = Mid(vx1, 1, InStr(1, vx1 & "/", "/") - 1)
                        strParmValue = Mid(vx1, InStr(1, vx1 & "/", "/") + 1)
                        If UCase(strParmNm) = UCase("mode") Then
                            strMode = strParmValue


                        End If
                    End If
                Loop While m1 > 0

            End If
            '---------------------------------------------------------------
        Next

    End Sub

    Private Sub dtpDateChoose_ValueChanged(sender As Object, e As EventArgs) Handles dtpDateChoose.ValueChanged
        Dim strFilePath As String = ""

        Me.date_choose = dtpDateChoose.Value.Date.ToString("yyyyMMdd")

        strFilePath = Application.StartupPath & "\xml\" & Me.date_choose & ".xml"

        downloadTRAXML_single(strFilePath)

    End Sub





#Region "不同參數的處理"

    ''' <summary>
    ''' 自動處理的程序
    ''' </summary>
    ''' <param name="_line_name">路線名稱</param>
    ''' <param name="_dt_line">路線資料</param>
    ''' <param name="_loc">路線中文名稱</param>
    Private Sub batchProcess(ByVal _line_name As String, ByVal _dt_line As DataTable, ByVal _loc As String)

        clsLinesTrans.Category = categoryBuild
        clsLinesTrans.Stations = stations

        clsLinesTrans.LineName = _line_name
        clsLinesTrans.LineTable = _dt_line

        strOutputLoaction = "C:\xampp\htdocs\tra\diagram\" & _loc

        Dim intLineKind As Integer = 0 '路線種類：0 一般路線、1 山海線、2 支線

        Select Case _line_name
            Case "WESTMOUNTAIN", "WESTSEA"
                intLineKind = 1
            Case "PINGXI", "NEIWAN", "JIJI"
                intLineKind = 2
            Case "SHALUN"
                intLineKind = 3
            Case Else
                intLineKind = 0
        End Select

        For Each files In aryFileList
            '只取副檔名為XML的檔案進行處理
            If Split(files, ".")(1) = "xml" Then
                drawDiagram(clsLinesTrans, files, intLineKind, _line_name, _dt_line)
            End If
        Next

    End Sub


    Private Sub tsm_Click(sender As Object, e As EventArgs) Handles tsm_WN.Click, tsm_WM.Click, tsm_WSEA.Click, tsm_WS.Click, tsm_S.Click, tsm_T.Click, tsm_I.Click, tsm_P.Click, tsm_PX.Click, tsm_NW.Click, tsm_J.Click, tsm_I.Click, tsm_SL.Click, tsm_N.Click, tsm_IN.Click

        Dim strLineName As String = ""

        Dim dtLine As DataTable = New DataTable
        Dim intLineKind As Integer = 0 '路線種類：0 一般路線、1 山海線、2 支線

        strOutputLoaction = txtFolderLocation.Text

        ToolStripStatusLabel1.Text = "鐵路運行圖繪製中"
        Me.Refresh()

        Select Case sender.name
            Case "tsm_WN"
                dtLine = Me.categoryBuild.dtLine_WN
                strLineName = "WESTNORTH"
            Case "tsm_WM"
                dtLine = Me.categoryBuild.dtLine_WM
                strLineName = "WESTMOUNTAIN"
                intLineKind = 1
            Case "tsm_WSEA"
                dtLine = Me.categoryBuild.dtLine_WSEA
                strLineName = "WESTSEA"
                intLineKind = 1
            Case "tsm_WS"
                dtLine = Me.categoryBuild.dtLine_WS
                strLineName = "WESTSOUTH"
            Case "tsm_S"
                dtLine = Me.categoryBuild.dtLine_S
                strLineName = "SOUTHLINK"
            Case "tsm_T"
                dtLine = Me.categoryBuild.dtLine_T
                strLineName = "TAITUNG"
            Case "tsm_I"
                dtLine = Me.categoryBuild.dtLine_I
                strLineName = "YILAN"
            Case "tsm_P"
                dtLine = Me.categoryBuild.dtLine_P
                strLineName = "PINGTUNG"
            Case "tsm_N"
                dtLine = Me.categoryBuild.dtLine_N
                strLineName = "NORTHLINK"
            Case "tsm_IN"
                dtLine = Me.categoryBuild.dtLine_IN
                strLineName = "NORTHYILAN"
            Case "tsm_PX"
                dtLine = Me.categoryBuild.dtLine_PX
                strLineName = "PINGXI"
                intLineKind = 2
            Case "tsm_NW"
                dtLine = Me.categoryBuild.dtLine_NW
                strLineName = "NEIWAN"
                intLineKind = 2
            Case "tsm_J"
                dtLine = Me.categoryBuild.dtLine_J
                strLineName = "JIJI"
                intLineKind = 2
            Case "tsm_SL"
                dtLine = Me.categoryBuild.dtLine_SL
                strLineName = "SHALUN"
                intLineKind = 3
        End Select

        clsLinesTrans.Category = categoryBuild
        clsLinesTrans.Stations = stations

        clsLinesTrans.LineName = strLineName
        clsLinesTrans.LineTable = dtLine

        For Each files In aryFileList

            drawDiagram(clsLinesTrans, files, intLineKind, strLineName, dtLine)

        Next

        ToolStripStatusLabel1.Text = sender.Text & " 鐵路運行圖繪製完成"

    End Sub

#End Region

#Region "繪製運行圖"

    ''' <summary>
    ''' 繪製運行圖主要程序
    ''' </summary>
    ''' <param name="_clsLinesTrans"></param>
    ''' <param name="_files"></param>
    ''' <param name="_line_kind"></param>
    ''' <param name="_line_name"></param>
    ''' <param name="_dt_line"></param>
    Private Sub drawDiagram(ByVal _clsLinesTrans As Class_Lines_Trans,
                            ByVal _files As String,
                            ByVal _line_kind As Integer,
                            ByVal _line_name As String,
                            ByVal _dt_line As DataTable)

        Dim strFileDate As String = Replace(Replace(_files, Application.StartupPath & "\xml\", ""), ".xml", "")
        Dim dtTrainsToDraw As DataTable = New DataTable

        xmlRead.readXML(_files, False)
        _clsLinesTrans.XMLRead = xmlRead

        Dim svgDrawDiagram As svgDrawDiagram = New svgDrawDiagram(strFileDate, strOutputLoaction, _line_name)

        '繪製基底圖(時間軸、時間標籤與車站軸)
        svgDrawDiagram.time_locate = categoryBuild.dtLocate
        svgDrawDiagram.drawDiagramBackground(_dt_line, _line_kind)

        If strMode = "BATCH" Then
            dtTrainsToDraw = _clsLinesTrans.getAllTrainNoInLines() '沒有選定特定車次
        Else
            If txtTrainNo.Text = "" Then
                dtTrainsToDraw = _clsLinesTrans.getAllTrainNoInLines() '沒有選定特定車次
            ElseIf txtTrainNo.Text <> "" Then
                dtTrainsToDraw = _clsLinesTrans.getAllTrainNoInLines(txtTrainNo.Text) '選定特定車次
            End If
        End If

        '運行線資料處理
        drawLineJobs(clsLinesTrans, svgDrawDiagram, dtTrainsToDraw, _dt_line, _line_name, _line_kind)

        svgDrawDiagram.endDrawJobs(strFileDate & ".html")
    End Sub


    ''' <summary>
    ''' 繪製車次線
    ''' </summary>
    ''' <param name="_clsLinesTrans"></param>
    ''' <param name="_svgDrawDiagram"></param>
    ''' <param name="_dt"></param>
    ''' <param name="_dtLine"></param>
    ''' <param name="_line_name"></param>
    ''' <param name="_line_kind"></param>
    Private Sub drawLineJobs(ByVal _clsLinesTrans As Class_Lines_Trans,
                             ByVal _svgDrawDiagram As svgDrawDiagram,
                             ByVal _dt As DataTable,
                             ByVal _dtLine As DataTable,
                             ByVal _line_name As String,
                             ByVal _line_kind As Integer)

        Dim bolSetToDraw As Boolean
        Dim dtDraw As DataTable = New DataTable

        '逐一繪製車次線
        For Each rows In _dt.Rows

            bolSetToDraw = False

            '山海線處理，避免某些通過竹南與彰化的車次，並非山線(海線)車次卻列入山線(海線)
            If _line_kind = 1 Then
                If _line_name = "WESTMOUNTAIN" And (rows.item("Line") = "1" Or rows.item("Line") = "0") Then
                    bolSetToDraw = True
                ElseIf _line_name = "WESTSEA" And (rows.item("Line") = "2" Or rows.item("Line") = "0") Then
                    bolSetToDraw = True
                End If
            Else
                bolSetToDraw = True
            End If

            '無誤才進行繪製
            If bolSetToDraw = True Then

                '重要的步驟：將繪製車次所停靠的時間填入營運路線暫存表
                dtDraw = _clsLinesTrans.setTrainNoTable(rows, rows.item("OverNightStn"), _line_name, _line_kind)

                If checkDrawDatatable(dtDraw) = False Then

                    '重要的步驟：過午夜車次處理，跨夜車站（OverNightStn）有資料，並且在營運路線內
                    If rows.item("OverNightStn") <> "0" And _dtLine.Select("ID = '" & rows.item("OverNightStn") & "'").Length > 0 Then
                        Dim dt1, dt2 As DataTable
                        dt1 = dtDraw.Copy
                        dt2 = dtDraw.Copy

                        '繪製邏輯：將營運路線暫存表拆成兩半，一半繪製午夜十二點前，另一半繪製午夜十二點後

                        '1. 午夜十二點前
                        '如果跨午夜十二點整，列車在車站內，將離站時間設為最大的時間軸座標值（1440）
                        If CSng(dt1.Select("STATION = '" & rows.item("OverNightStn") & "'")(0).Item("ARRTIME")) >
                       CSng(dt1.Select("STATION = '" & rows.item("OverNightStn") & "'")(0).Item("DEPTIME")) Then
                            dt1.Select("STATION = '" & rows.item("OverNightStn") & "'")(0).Item("DEPTIME") = 1440
                        Else
                            'dt1.Select("STATION = '" & rows.item("OverNightStn") & "'")(0).Item("ARRTIME") -= 1440
                        End If
                        'End If
                        dt1 = _clsLinesTrans.setOverNightStn_before_midnight(dt1, rows.item("OverNightStn"), rows.item("LineDir"))
                        'drawTrainLines(dt1, rows.Item("Train"), _dtLine, rows.item("LineDir"))

                        '2. 午夜十二點後
                        '如果跨午夜十二點整， 列車在車站內， 將到站時間設為最小的時間軸座標值（0）
                        If CSng(dt2.Select("STATION = '" & rows.item("OverNightStn") & "'")(0).Item("ARRTIME")) >
                       CSng(dt2.Select("STATION = '" & rows.item("OverNightStn") & "'")(0).Item("DEPTIME")) Then
                            dt2.Select("STATION = '" & rows.item("OverNightStn") & "'")(0).Item("ARRTIME") = 0
                        End If
                        'End If
                        dt2 = _clsLinesTrans.setOverNightStn_after_midnight(dt2, rows.item("OverNightStn"), rows.item("LineDir"))
                        'drawTrainLines(dt2, rows.Item("Train"), _dtLine, rows.item("LineDir"))

                    Else
                        drawTrainLines(_svgDrawDiagram, dtDraw, rows.Item("Train"), _dtLine, rows.item("LineDir"))
                    End If
                End If
            End If
        Next

    End Sub

    ''' <summary>
    ''' 檢核暫存營運路線表，至少要有一個車站有時間資料
    ''' </summary>
    ''' <param name="_dt"></param>
    ''' <returns></returns>
    Private Function checkDrawDatatable(ByVal _dt As DataTable) As Boolean
        Dim bolResults As Boolean = False
        Dim intRows As Integer = 0

        For Each row In _dt.Rows
            If row.item("ARRTIME").ToString <> "" And row.item("DEPTime").ToString <> "" Then
                intRows += 1
            End If
        Next

        If intRows <= 1 Then
            bolResults = True
        End If

        Return bolResults

    End Function

#End Region

#Region "繪製車次運行線"

    ''' <summary>
    ''' 繪製車次運行線
    ''' </summary>
    ''' <param name="_dt">運行線datatable</param>
    ''' <param name="_dtline"></param>
    ''' <remarks></remarks>
    Private Sub drawTrainLines(ByVal _svgDrawDiagram As svgDrawDiagram,
                               ByVal _dt As DataTable,
                               ByVal _train_no As String,
                               ByVal _dtline As DataTable,
                               ByVal _line_dir As String)

        Dim strCarClass As String = xmlRead.dtTrain.Select("Train = '" & _train_no & "'")(0).Item("CarClass") '繪製的車次所屬車種
        Dim strColor As String

        Select Case strCarClass '根據車次所屬車種設定畫線顏色
            Case "1102" '太魯閣號
                strColor = "#8c8c8c"
            Case "1107" '普悠瑪號
                strColor = "#ff0000"
            Case "1101", "1108" '自強號
                strColor = "#ff6600"
            Case "1111", "1110", "1114", "1115" '莒光號
                strColor = "#ffcc00"
            Case "1120" '復興快
                strColor = "#7094db"
            Case "1140" '普快
                strColor = "#000000"
            Case "1100", "1103" '柴油自強號
                '部分自強號電車車次由於也用1100這個類別，因此要用車次來判別顏色，目前有EMU1200、EMU300兩種
                strColor = "#ff9933"
            Case Else '區間車
                strColor = "#0000cc"
        End Select

        '繪製車次路線
        If _dt.Rows.Count > 1 Then
            Dim dictTrainPoints As New Dictionary(Of Integer, Array)

            Dim intOrder As Integer = check_over_night_in_station(_dt)

            If intOrder > 0 Then
                '在車站跨夜的處理(切兩半)
                dictTrainPoints = trans_trains_to_array(1, _dt, intOrder)
                _svgDrawDiagram.addLinePolyline(_line_dir, _train_no, dictTrainPoints, strColor, "1.5px")
                dictTrainPoints = trans_trains_to_array(2, _dt, intOrder)
                _svgDrawDiagram.addLinePolyline(_line_dir, _train_no, dictTrainPoints, strColor, "1.5px")
            Else
                dictTrainPoints = trans_trains_to_array(0, _dt)
                _svgDrawDiagram.addLinePolyline(_line_dir, _train_no, dictTrainPoints, strColor, "1.5px")
            End If

        End If


    End Sub

    ''' <summary>
    ''' 把時程表轉為圖片座標
    ''' </summary>
    ''' <param name="_kind"></param>
    ''' <param name="_dt"></param>
    ''' <param name="_order"></param>
    ''' <returns></returns>
    Private Function trans_trains_to_array(ByVal _kind As Integer,
                                           ByVal _dt As DataTable,
                                           Optional ByVal _order As Integer = 0) As Dictionary(Of Integer, Array)

        Dim dictOutput As New Dictionary(Of Integer, Array)
        Dim aryTime() As Single = {0.0, 0.0, 0.0}

        Select Case _kind
            Case 0
                For Each row In _dt.Rows
                    If row.Item("ORDER").ToString <> "" Then
                        aryTime = {CSng(row.Item("KM")), CSng(row.Item("ARRTime")), CSng(row.Item("DEPTime"))}
                        dictOutput.Add(CSng(row.Item("ORDER")), aryTime)
                    End If
                Next
            Case 1
                For Each row In _dt.Rows
                    If row.Item("ORDER").ToString <> "" Then
                        If CSng(row.Item("ORDER")) < _order Then
                            aryTime = {CSng(row.Item("KM")), CSng(row.Item("ARRTime")), CSng(row.Item("DEPTime"))}

                        ElseIf CSng(row.Item("ORDER")) = _order Then
                            aryTime = {CSng(row.Item("KM")), CSng(row.Item("ARRTime")), 14400}

                        End If
                        dictOutput.Add(CSng(row.Item("ORDER")), aryTime)
                    End If
                Next
            Case 2
                For Each row In _dt.Rows
                    If row.Item("ORDER").ToString <> "" Then
                        If CSng(row.Item("ORDER")) > _order Then
                            aryTime = {CSng(row.Item("KM")), CSng(row.Item("ARRTime")), CSng(row.Item("DEPTime"))}
                            dictOutput.Add(CSng(row.Item("ORDER")), aryTime)
                        ElseIf CSng(row.Item("ORDER")) = _order Then
                            aryTime = {CSng(row.Item("KM")), 0, CSng(row.Item("DEPTime"))}
                            dictOutput.Add(CSng(row.Item("ORDER")), aryTime)
                        End If
                    End If
                Next
        End Select

        '成追線處理，避免字典清單中，有出現順序不按照等比級數的問題
        Dim aryOrders As ArrayList = New ArrayList

        For Each item In dictOutput
            aryOrders.Add(item.Key)
        Next

        If aryOrders.Count > 1 Then
            If System.Math.Abs(CSng(aryOrders(aryOrders.Count - 1)) - CSng(aryOrders(aryOrders.Count - 2))) <> 1 Then
                dictOutput.Remove(aryOrders(aryOrders.Count - 1))
            End If
        End If

        Return dictOutput

    End Function


    ''' <summary>
    ''' 確認該車次是否有在車站跨夜
    ''' </summary>
    ''' <param name="_dt"></param>
    ''' <returns></returns>
    Private Function check_over_night_in_station(ByVal _dt As DataTable) As Integer

        Dim intOrder As Integer = 0
        Dim bolEnd As Boolean = False
        Dim i As Integer = 0

        Do
            If i <= _dt.Rows.Count - 1 Then
                If _dt.Rows(i).Item("ORDER").ToString <> "" AndAlso CInt(_dt.Rows(i).Item("ARRTime")) > CInt(_dt.Rows(i).Item("DEPTime")) Then
                    intOrder = CInt(_dt.Rows(i).Item("ORDER"))
                End If
            Else
                bolEnd = True
            End If
            i += 1
        Loop Until intOrder > 0 Or bolEnd = True

        Return intOrder

    End Function

#End Region

    ''' <summary>
    ''' 匯出圖片按鍵
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmPicOutput_Click(sender As Object, e As EventArgs)

        'SaveFileDialog1.FileName = "鐵路運行圖"

        'If Not Me.PictureBox1.Image Is Nothing Then
        '    If SaveFileDialog1.ShowDialog = DialogResult.OK Then
        '        If SaveFileDialog1.FileName <> "" Then

        '            Dim g As Graphics = Graphics.FromImage(Me.PictureBox1.Image)

        '            Dim strFileName As String = SaveFileDialog1.FileName '檔案名稱與儲存位置

        '            g.DrawImage(Me.PictureBox1.Image, New Point(0, 0))

        '            '檔案格式選擇
        '            Select Case SaveFileDialog1.FilterIndex
        '                Case 1
        '                    PictureBox1.Image.Save(strFileName, Imaging.ImageFormat.Png)
        '                Case 2
        '                    PictureBox1.Image.Save(strFileName, Imaging.ImageFormat.Gif)
        '            End Select

        '            If My.Computer.FileSystem.FileExists(strFileName) Then
        '                MessageBox.Show("檔案輸出成功", "輸出結果", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
        '            Else
        '                MessageBox.Show("檔案輸出錯誤，可能您已開啟要覆蓋的檔案，請試圖將該檔案關閉再重新儲存", "輸出結果",
        '                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
        '            End If
        '        End If
        '    End If
        'Else
        '    MsgBox("請先繪圖")
        'End If

    End Sub


#Region "下載檔案"

    Private Sub downloadTRAXML_single(ByVal _file_path As String) 'As Boolean
        'Dim bolOK As Boolean = False

        If My.Computer.FileSystem.FileExists(_file_path) = False Then
            MessageBox.Show("您選擇的日期尚無下載的台鐵XML檔案， 請點選確定後將會進行下載工作， 並請稍後。", "無今日之時刻表XML檔案",
                           MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1)
            downloadTRAXML("http: //163.29.3.98/xml/" & Me.date_choose & ".zip")
        End If

        'Return bolOK

    End Sub

    Private Sub downloadTRAXML(Optional ByVal _url As String = "")
        Dim DownloadManager As moduleDownload
        If _url = "" Then
            DownloadManager = New moduleDownload("http://163.29.3.98/xml/45Days.zip")
        Else
            DownloadManager = New moduleDownload(_url)
        End If

        DownloadManager.TimerGate(Timer1, 1000) '將Timer1實體傳入DownloadManager中，並設定Interval為1000毫秒
        DownloadManager.SaveFullPath = "\TrainXML.zip" '設定檔案的下載路徑
        DownloadManager.SaveFolderPath = ""
        DownloadManager.StartDownload() '開始進行下載

        While DownloadManager.CheckDownloading '如果正在下載中，就永遠執行迴圈
            Application.DoEvents() '停頓(多工)
        End While

        Select Case DownloadManager.GetStatus '取得狀態傳回值
            Case 0
                'MsgBox("下載失敗，可能主因為台鐵目前尚無該日期之XML，或網路服務不存在。", MsgBoxStyle.Critical)
            Case 1
                If unZip() = "Success" Then
                    'MsgBox("下載成功。使用時間：" & DownloadManager.FormatTime(DownloadManager.GetDownloadedTime), MsgBoxStyle.Information)
                Else
                    'MsgBox("解壓縮失敗", MsgBoxStyle.Critical)
                End If
        End Select

        deleteFile()

    End Sub

    Private Function unZip() As String
        Dim oZip As New MySharpZip.CZip
        Dim rc As String = oZip.UnZipFile(Application.StartupPath & "\TrainXML.zip", Application.StartupPath & "\xml")
        Return rc
    End Function

    Private Sub deleteFile()
        Dim FileExists As Boolean
        Dim filePath = Application.StartupPath & "\TrainXML.zip"
        FileExists = My.Computer.FileSystem.FileExists(filePath)
        If FileExists = True Then
            '檔案 存在則刪除檔案 
            My.Computer.FileSystem.DeleteFile(filePath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        End If
    End Sub

    Private Sub tsmDownloadXml_Click(sender As Object, e As EventArgs) Handles tsmDownloadXml.Click
        downloadTRAXML()
    End Sub


#End Region

End Class
