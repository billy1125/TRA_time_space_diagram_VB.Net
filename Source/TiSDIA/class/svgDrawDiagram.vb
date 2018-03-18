Imports System.Xml

Public Class svgDrawDiagram

    Dim _time_locate As DataTable
    Public Property time_locate() As DataTable
        Get
            Return _time_locate
        End Get
        Set(ByVal value As DataTable)
            _time_locate = value
        End Set
    End Property


    Dim xmlDoc As XmlDocument = New XmlDocument
    Dim xElement As XmlElement
    Dim xChildElement As XmlElement
    Dim xChildElement1 As XmlElement
    'Dim xChildElement2 As XmlElement
    'Dim xChildElement3 As XmlElement
    'xmlDoc.Load("C:\temp\TOTFA.xml")

    Dim strDate As String
    Dim strFileLocation As String
    Dim strLine As String
    Dim strLineName As String

    Public Sub New(ByVal _date As String, ByVal _file_Location As String, ByVal _line_name As String)
        strDate = _date
        strFileLocation = _file_Location
        strLine = _line_name

        Select Case strLine
            Case "WESTNORTH"
                strLineName = "西部幹線北段(基隆 - 竹南)"
            Case "WESTSOUTH"
                strLineName = "西部幹線南段(彰化 - 高雄)"
            Case "WESTMOUNTAIN"
                strLineName = "西部幹線台中線(竹南 - 彰化，經苗栗)"
            Case "WESTSEA"
                strLineName = "西部幹線海岸線(竹南 - 彰化，經大甲)"
            Case "PINGTUNG"
                strLineName = "屏東線(高雄 - 枋寮)"
            Case "SOUTHLINK"
                strLineName = "南迴線(枋寮 - 台東)"
            Case "TAITUNG"
                strLineName = "台東線(花蓮 - 台東)"
            Case "PINGXI"
                strLineName = "平溪線、深澳線(八斗子 - 菁桐)"
            Case "NEIWAN"
                strLineName = "內灣線(新竹 - 內灣)"
            Case "JIJI"
                strLineName = "集集線(二水 - 車埕)"
            Case "SHALUN"
                strLineName = "沙崙線(中洲 - 沙崙)"
        End Select
    End Sub

    ''' <summary>
    ''' 繪製背景
    ''' </summary>
    ''' <param name="_dt"></param>
    ''' <remarks></remarks>
    Public Sub drawDiagramBackground(ByVal _dt As DataTable, ByVal _line_kind As Integer)

        Try
            '共同宣告
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", Nothing))

            'svg根節點
            xElement = xmlDoc.CreateElement("svg")

            'svg基本設定
            xElement.SetAttribute("xmlns", "http://www.w3.org/2000/svg")
            xElement.SetAttribute("version", "2.0")
            xElement.SetAttribute("width", "15000")

            Select Case _line_kind
                Case 0, 1
                    xElement.SetAttribute("height", "3100")
                Case 2
                    xElement.SetAttribute("height", "1400")
                Case 3
                    xElement.SetAttribute("height", "750")
            End Select

            xElement.SetAttribute("style", "background: #b3fff0;font-family:Tahoma;")
            xmlDoc.AppendChild(xElement)


            Dim strArray01() As String = {"", ""} '起點
            Dim strArray02() As String = {"", ""} '終點

            '時間軸線
            For Each row In Me.time_locate.Rows

                If row.item("EXTRA1").ToString <> "" Then

                    strArray01(0) = row.item("ID") + 50
                    strArray01(1) = "50"
                    strArray02(0) = row.item("ID") + 50

                    Select Case _line_kind
                        Case 0, 1
                            strArray02(1) = "3050"
                        Case 2
                            strArray02(1) = "1300"
                        Case 3
                            strArray02(1) = "700"
                    End Select

                    addLine(xmlDoc, xElement, strArray01, strArray02, row.item("EXTRA1").ToString, "1px", "5,5")

                    '畫時間標籤
                    Dim strTimeLabel As String = ""
                    Dim strFontSize As String = ""

                    If row.item("EXTRA2").ToString <> "" Then
                        If row.item("EXTRA2").ToString = "14" Then
                            strTimeLabel = Split(row.item("DSC"), ":")(0) & Split(row.item("DSC"), ":")(1) '1時、2時
                            strFontSize = "14"
                        ElseIf row.item("EXTRA2").ToString = "12" Then
                            strTimeLabel = "30" '30分
                            strFontSize = "12"
                        End If

                        For i = 1 To 7
                            strArray01(1) = 49 + (i - 1) * 500
                            addText(xmlDoc, xElement, strArray01, "#999966", strFontSize, strTimeLabel)
                        Next
                    End If

                End If
            Next

            '車站軸(橫線與標籤)
            For Each row In _dt.Rows

                strArray01(0) = 50
                strArray01(1) = 50 + CInt(row.Item("EXTRA1"))
                strArray02(0) = 14450
                strArray02(1) = 50 + CInt(row.Item("EXTRA1"))

                addLine(xmlDoc, xElement, strArray01, strArray02, "#00334d", "0.5px", "5,5")


                For i As Integer = 0 To 20
                    strArray01(0) = 0 + i * 720
                    strArray01(1) = 63 + CInt(row.Item("EXTRA1"))
                    '車站標籤
                    If row.Item("ID") <> "NA" Then
                        addText(xmlDoc, xElement, strArray01, "#00334d", "18", row.Item("DSC"))
                    Else
                        addText(xmlDoc, xElement, strArray01, "#29a3a3", "18", row.Item("DSC"))
                    End If
                Next
            Next

            strArray01(0) = 0
            strArray01(1) = 25

            If strLineName = "TAITUNG" Then

                addText(xmlDoc, xElement, strArray01, "#000000", "18", strLineName & "  日期 " & strDate & "，運行圖均來自台鐵公開資料所分析，僅供參考，正確資料與實際運轉狀況請以台鐵網站或公告為主。程式設計：呂卓勳。請注意，由於舞鶴號誌站台鐵並未給予停車會車的時間，因此瑞穗與玉里間繪製的車次線僅供參考，可能有錯誤。")
            Else

                addText(xmlDoc, xElement, strArray01, "#000000", "18", strLineName & "  日期 " & strDate & "，運行圖均來自台鐵公開資料所分析，僅供參考，正確資料與實際運轉狀況請以台鐵網站或公告為主。程式設計：呂卓勳")
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' 完成繪製作業
    ''' </summary>
    ''' <param name="_file_name"></param>
    Public Sub endDrawJobs(ByVal _file_name As String)
        xmlDoc.Save(strFileLocation & "\" & strLine & "_" & _file_name)
    End Sub

    ''' <summary>
    ''' 畫直線(Line)
    ''' </summary>
    ''' <param name="_xmlDoc"></param>
    ''' <param name="_xElement"></param>
    ''' <param name="_start"></param>
    ''' <param name="_end"></param>
    ''' <param name="_stroke"></param>
    ''' <param name="_stroke_width"></param>
    ''' <param name="_stroke_dasharray"></param>
    Private Sub addLine(ByVal _xmlDoc As XmlDocument,
                        ByVal _xElement As XmlElement,
                        ByVal _start As Array, ByVal _end As Array,
                        ByVal _stroke As String, ByVal _stroke_width As String,
                        ByVal _stroke_dasharray As String)

        Dim xChildElement As XmlElement

        xChildElement = _xmlDoc.CreateElement("line")
        xChildElement.SetAttribute("x1", _start(0))
        xChildElement.SetAttribute("y1", _start(1))
        xChildElement.SetAttribute("x2", _end(0))
        xChildElement.SetAttribute("y2", _end(1))
        xChildElement.SetAttribute("style", "stroke:" & _stroke & ";stroke-width:" & _stroke_width & ";stroke-dasharray:" & _stroke_dasharray)

        _xElement.AppendChild(xChildElement)

    End Sub

    ''' <summary>
    ''' 畫折線(Polyline)
    ''' </summary>
    ''' <param name="_stroke"></param>
    ''' <param name="_stroke_width"></param>
    Public Sub addLinePolyline(ByVal _line_dir As String,
                               ByVal _train_no As String,
                               ByVal _dict As Dictionary(Of Integer, Array),
                               ByVal _stroke As String,
                               ByVal _stroke_width As String)

        Dim xChildElement As XmlElement
        Dim xChildElement1 As XmlElement
        Dim strPath As String = ""
        Dim i As Integer = 0

        '逐一填入折線折線值
        For Each pair As KeyValuePair(Of Integer, Array) In _dict
            strPath += setPolylineParamaters(pair.Value(0), pair.Value(1), pair.Value(2), _line_dir, i)
            i += 1
        Next

        '畫營運線
        xChildElement = xmlDoc.CreateElement("path")
        xChildElement.SetAttribute("id", _train_no)
        xChildElement.SetAttribute("d", strPath)
        xChildElement.SetAttribute("style", "fill:none;stroke:" & _stroke & ";stroke-width:" & _stroke_width)

        xElement.AppendChild(xChildElement)

        '標記車次標註
        Dim aryList() As String = {"50", "300", "600", "900", "1200", "1500", "1800", "2100", "2400", "2700"}

        For Each item In aryList
            xChildElement = xmlDoc.CreateElement("text")
            xChildElement.SetAttribute("x", item)
            xChildElement.SetAttribute("y", "0")
            xChildElement.SetAttribute("style", "stroke:" & _stroke & ";font-size:13px")
            'xChildElement.InnerText = "12"

            xElement.AppendChild(xChildElement)

            xChildElement1 = xmlDoc.CreateElement("textPath")
            xChildElement1.SetAttributeNode("href", "http://www.w3.org/1999/xlink")
            xChildElement1.Attributes("href").Value = "#" & _train_no
            xChildElement1.Attributes("href").Prefix = "xlink"
            xChildElement1.InnerText = _train_no

            xChildElement.AppendChild(xChildElement1)
        Next

    End Sub

    ''' <summary>
    ''' 折線折線值設定
    ''' </summary>
    ''' <param name="_km"></param>
    ''' <param name="_start_time"></param>
    ''' <param name="_end_time"></param>
    ''' <param name="_line_dir"></param>
    ''' <param name="_i"></param>
    ''' <returns></returns>
    Private Function setPolylineParamaters(ByVal _km As String,
                                           ByVal _start_time As String,
                                           ByVal _end_time As String,
                                           ByVal _line_dir As String,
                                           ByVal _i As Integer) As String

        Dim strOutput As String = ""
        Dim sngTime As Single = 50 + CSng(_km)
        Dim sngStartTime As Single = 50 + CSng(_start_time)
        Dim sngEndTime As Single = 50 + CSng(_end_time)
        Dim strDef, strDef1 As String

        If _i = 0 Then
            strDef = "M"
            strDef1 = "L"
            'ElseIf intCount = 1 Then
            '    strDef = "L"
        Else
            strDef = ""
            strDef1 = ""
        End If

        If _line_dir = "1" Then
            '逆行
            strOutput += strDef & sngStartTime & "," & sngTime & " " & strDef1 & sngEndTime & "," & sngTime & " "
        ElseIf _line_dir = "0" Then
            '順行
            strOutput += strDef & sngEndTime & "," & sngTime & " " & strDef1 & sngStartTime & "," & sngTime & " "
        End If


        'If strLine <> "TAITUNG" Then
        '    If _line_dir = "1" Then
        '        strOutput += strDef & intStartTime & "," & intTime & " " & strDef1 & intEndTime & "," & intTime & " "
        '    ElseIf _line_dir = "0" Then
        '        strOutput += strDef & intEndTime & "," & intTime & " " & strDef1 & intStartTime & "," & intTime & " "
        '    End If
        'Else
        '    If _line_dir = "0" Then
        '        strOutput += strDef & intStartTime & "," & intTime & " " & strDef1 & intEndTime & "," & intTime & " "
        '    ElseIf _line_dir = "1" Then
        '        strOutput += strDef & intEndTime & "," & intTime & " " & strDef1 & intStartTime & "," & intTime & " "
        '    End If
        'End If

        Return strOutput

    End Function

    ''' <summary>
    ''' 顯示字體
    ''' </summary>
    ''' <param name="_xmlDoc"></param>
    ''' <param name="_xElement"></param>
    ''' <param name="_start"></param>
    ''' <param name="_stroke"></param>
    Public Sub addText(ByVal _xmlDoc As XmlDocument,
                        ByVal _xElement As XmlElement,
                        ByVal _start As Array,
                        ByVal _stroke As String,
                        ByVal _font_size As String,
                        ByVal _text As String)

        Dim xChildElement As XmlElement

        xChildElement = _xmlDoc.CreateElement("text")
        xChildElement.SetAttribute("x", _start(0))
        xChildElement.SetAttribute("y", _start(1))
        xChildElement.SetAttribute("style", "stroke:" & _stroke & ";font-size:" & _font_size)
        xChildElement.InnerText = _text

        _xElement.AppendChild(xChildElement)

    End Sub

End Class
