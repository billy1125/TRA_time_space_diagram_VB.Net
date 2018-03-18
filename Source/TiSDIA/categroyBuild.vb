Imports System.Xml

Public Class categoryBuild

    Public dtCarClass As DataTable = New DataTable
    Public dtStations As DataTable = New DataTable
    Public dtArea As DataTable = New DataTable
    Public dtLocate As DataTable = New DataTable
    Public dtType As DataTable = New DataTable
    Public dtLine As DataTable = New DataTable
    Public dtLineDir As DataTable = New DataTable

    Public dtLine_WN As DataTable = New DataTable
    Public dtLine_WS As DataTable = New DataTable
    Public dtLine_T As DataTable = New DataTable
    Public dtLine_WSEA As DataTable = New DataTable
    Public dtLine_WM As DataTable = New DataTable
    Public dtLine_P As DataTable = New DataTable
    Public dtLine_S As DataTable = New DataTable
    Public dtLine_I As DataTable = New DataTable
    Public dtLine_IN As DataTable = New DataTable
    Public dtLine_N As DataTable = New DataTable
    Public dtLine_J As DataTable = New DataTable
    Public dtLine_PX As DataTable = New DataTable
    Public dtLine_NW As DataTable = New DataTable
    Public dtLine_SL As DataTable = New DataTable

    Public dtStations_KM As DataTable = New DataTable
    Public dtStations_KM1 As DataTable = New DataTable
    Public dtLines_KM As DataTable = New DataTable

    Public Sub New(ByVal _strFileLocation As String)
        Me.strFileLocation = _strFileLocation

        buildDatatable()

        dtCarClass = readXML(Me.strFileLocation, "CARCLASS", dtCarClass)
        'dtStations = readXML(Me.strFileLocation, "STATION", dtStations)
        dtArea = readXML(Me.strFileLocation, "AREA", dtArea)
        dtLocate = readXML(Me.strFileLocation, "TIMELOCAT", dtLocate)
        dtType = readXML(Me.strFileLocation, "TYPE", dtType)
        dtLine = readXML(Me.strFileLocation, "LINE", dtLine)
        dtLineDir = readXML(Me.strFileLocation, "LINEDIR", dtLineDir)

        dtLine_WN = readXML(Me.strFileLocation, "LINE_WN", dtLine_WN)
        dtLine_WS = readXML(Me.strFileLocation, "LINE_WS", dtLine_WS)
        dtLine_T = readXML(Me.strFileLocation, "LINE_T", dtLine_T)
        dtLine_WSEA = readXML(Me.strFileLocation, "LINE_WSEA", dtLine_WSEA)
        dtLine_WM = readXML(Me.strFileLocation, "LINE_WM", dtLine_WM)
        dtLine_P = readXML(Me.strFileLocation, "LINE_P", dtLine_P)
        dtLine_S = readXML(Me.strFileLocation, "LINE_S", dtLine_S)
        dtLine_I = readXML(Me.strFileLocation, "LINE_I", dtLine_I)
        dtLine_IN = readXML(Me.strFileLocation, "LINE_IN", dtLine_IN)
        dtLine_N = readXML(Me.strFileLocation, "LINE_N", dtLine_N)
        dtLine_J = readXML(Me.strFileLocation, "LINE_J", dtLine_J)
        dtLine_PX = readXML(Me.strFileLocation, "LINE_PX", dtLine_PX)
        dtLine_NW = readXML(Me.strFileLocation, "LINE_NW", dtLine_NW)
        dtLine_SL = readXML(Me.strFileLocation, "LINE_SL", dtLine_SL)

        dtStations_KM = readXML(Me.strFileLocation, "STATION_KM", dtStations_KM)
        dtStations_KM1 = readXML(Me.strFileLocation, "STATION_KM1", dtStations_KM1)
        dtLines_KM = readXML(Me.strFileLocation, "LINES", dtLines_KM)
    End Sub

    Dim _strFileLocation As String = ""
    Public Property strFileLocation() As String
        Get
            Return _strFileLocation
        End Get
        Set(ByVal value As String)
            _strFileLocation = value
        End Set
    End Property

    Private Sub buildDatatable()
        dtCarClass.Columns.Add("ID")
        dtCarClass.Columns.Add("DSC")
        dtCarClass.Columns.Add("EXTRA1")
        dtCarClass.Columns.Add("EXTRA2")
        dtCarClass.Columns.Add("EXTRA3")
        dtCarClass.Columns.Add("EXTRA4")

        dtStations.Columns.Add("ID")
        dtStations.Columns.Add("DSC")
        dtStations.Columns.Add("EXTRA1")
        dtStations.Columns.Add("EXTRA2")
        dtStations.Columns.Add("EXTRA3")
        dtStations.Columns.Add("EXTRA4")

        dtArea.Columns.Add("ID")
        dtArea.Columns.Add("DSC")
        dtArea.Columns.Add("EXTRA1")
        dtArea.Columns.Add("EXTRA2")
        dtArea.Columns.Add("EXTRA3")
        dtArea.Columns.Add("EXTRA4")

        dtLocate.Columns.Add("ID")
        dtLocate.Columns.Add("DSC")
        dtLocate.Columns.Add("EXTRA1")
        dtLocate.Columns.Add("EXTRA2")
        dtLocate.Columns.Add("EXTRA3")
        dtLocate.Columns.Add("EXTRA4")

        dtType.Columns.Add("ID")
        dtType.Columns.Add("DSC")
        dtType.Columns.Add("EXTRA1")
        dtType.Columns.Add("EXTRA2")
        dtType.Columns.Add("EXTRA3")
        dtType.Columns.Add("EXTRA4")

        dtLine.Columns.Add("ID")
        dtLine.Columns.Add("DSC")
        dtLine.Columns.Add("EXTRA1")
        dtLine.Columns.Add("EXTRA2")
        dtLine.Columns.Add("EXTRA3")
        dtLine.Columns.Add("EXTRA4")

        dtLineDir.Columns.Add("ID")
        dtLineDir.Columns.Add("DSC")
        dtLineDir.Columns.Add("EXTRA1")
        dtLineDir.Columns.Add("EXTRA2")
        dtLineDir.Columns.Add("EXTRA3")
        dtLineDir.Columns.Add("EXTRA4")


        dtLine_WN.Columns.Add("ID")
        dtLine_WN.Columns.Add("DSC")
        dtLine_WN.Columns.Add("EXTRA1")
        dtLine_WN.Columns.Add("EXTRA2")
        dtLine_WN.Columns.Add("EXTRA3")
        dtLine_WN.Columns.Add("EXTRA4")


        dtLine_WS.Columns.Add("ID")
        dtLine_WS.Columns.Add("DSC")
        dtLine_WS.Columns.Add("EXTRA1")
        dtLine_WS.Columns.Add("EXTRA2")
        dtLine_WS.Columns.Add("EXTRA3")
        dtLine_WS.Columns.Add("EXTRA4")

        dtLine_T.Columns.Add("ID")
        dtLine_T.Columns.Add("DSC")
        dtLine_T.Columns.Add("EXTRA1")
        dtLine_T.Columns.Add("EXTRA2")
        dtLine_T.Columns.Add("EXTRA3")
        dtLine_T.Columns.Add("EXTRA4")

        dtLine_WSEA.Columns.Add("ID")
        dtLine_WSEA.Columns.Add("DSC")
        dtLine_WSEA.Columns.Add("EXTRA1")
        dtLine_WSEA.Columns.Add("EXTRA2")
        dtLine_WSEA.Columns.Add("EXTRA3")
        dtLine_WSEA.Columns.Add("EXTRA4")

        dtLine_WM.Columns.Add("ID")
        dtLine_WM.Columns.Add("DSC")
        dtLine_WM.Columns.Add("EXTRA1")
        dtLine_WM.Columns.Add("EXTRA2")
        dtLine_WM.Columns.Add("EXTRA3")
        dtLine_WM.Columns.Add("EXTRA4")

        dtLine_P.Columns.Add("ID")
        dtLine_P.Columns.Add("DSC")
        dtLine_P.Columns.Add("EXTRA1")
        dtLine_P.Columns.Add("EXTRA2")
        dtLine_P.Columns.Add("EXTRA3")
        dtLine_P.Columns.Add("EXTRA4")

        dtLine_S.Columns.Add("ID")
        dtLine_S.Columns.Add("DSC")
        dtLine_S.Columns.Add("EXTRA1")
        dtLine_S.Columns.Add("EXTRA2")
        dtLine_S.Columns.Add("EXTRA3")
        dtLine_S.Columns.Add("EXTRA4")

        dtLine_I.Columns.Add("ID")
        dtLine_I.Columns.Add("DSC")
        dtLine_I.Columns.Add("EXTRA1")
        dtLine_I.Columns.Add("EXTRA2")
        dtLine_I.Columns.Add("EXTRA3")
        dtLine_I.Columns.Add("EXTRA4")

        dtLine_IN.Columns.Add("ID")
        dtLine_IN.Columns.Add("DSC")
        dtLine_IN.Columns.Add("EXTRA1")
        dtLine_IN.Columns.Add("EXTRA2")
        dtLine_IN.Columns.Add("EXTRA3")
        dtLine_IN.Columns.Add("EXTRA4")

        dtLine_N.Columns.Add("ID")
        dtLine_N.Columns.Add("DSC")
        dtLine_N.Columns.Add("EXTRA1")
        dtLine_N.Columns.Add("EXTRA2")
        dtLine_N.Columns.Add("EXTRA3")
        dtLine_N.Columns.Add("EXTRA4")

        dtLine_J.Columns.Add("ID")
        dtLine_J.Columns.Add("DSC")
        dtLine_J.Columns.Add("EXTRA1")
        dtLine_J.Columns.Add("EXTRA2")
        dtLine_J.Columns.Add("EXTRA3")
        dtLine_J.Columns.Add("EXTRA4")

        dtLine_PX.Columns.Add("ID")
        dtLine_PX.Columns.Add("DSC")
        dtLine_PX.Columns.Add("EXTRA1")
        dtLine_PX.Columns.Add("EXTRA2")
        dtLine_PX.Columns.Add("EXTRA3")
        dtLine_PX.Columns.Add("EXTRA4")

        dtLine_NW.Columns.Add("ID")
        dtLine_NW.Columns.Add("DSC")
        dtLine_NW.Columns.Add("EXTRA1")
        dtLine_NW.Columns.Add("EXTRA2")
        dtLine_NW.Columns.Add("EXTRA3")
        dtLine_NW.Columns.Add("EXTRA4")

        dtStations_KM.Columns.Add("ID")
        dtStations_KM.Columns.Add("DSC")
        dtStations_KM.Columns.Add("EXTRA1")
        dtStations_KM.Columns.Add("EXTRA2")
        dtStations_KM.Columns.Add("EXTRA3")
        dtStations_KM.Columns.Add("EXTRA4")

        dtStations_KM1.Columns.Add("ID")
        dtStations_KM1.Columns.Add("DSC")
        dtStations_KM1.Columns.Add("EXTRA1")
        dtStations_KM1.Columns.Add("EXTRA2")
        dtStations_KM1.Columns.Add("EXTRA3")
        dtStations_KM1.Columns.Add("EXTRA4")

        dtLines_KM.Columns.Add("ID")
        dtLines_KM.Columns.Add("DSC")
        dtLines_KM.Columns.Add("EXTRA1")
        dtLines_KM.Columns.Add("EXTRA2")
        dtLines_KM.Columns.Add("EXTRA3")
        dtLines_KM.Columns.Add("EXTRA4")

        dtLine_SL.Columns.Add("ID")
        dtLine_SL.Columns.Add("DSC")
        dtLine_SL.Columns.Add("EXTRA1")
        dtLine_SL.Columns.Add("EXTRA2")
        dtLine_SL.Columns.Add("EXTRA3")
        dtLine_SL.Columns.Add("EXTRA4")

    End Sub


    Private Function readXML(ByVal _fileLocation As String,
                            ByVal _kind As String,
                            ByVal _dt As DataTable) As DataTable

        Dim xmlFile As XmlDocument = New XmlDocument
        Dim xmlRoot As XmlNode
        Dim xNodeList_L1 As XmlNodeList
        Dim xNodeList_L2 As XmlNodeList
        Dim xmlNode_L1 As XmlNode
        Dim xElement_L2 As XmlElement

        Try
            '讀取XML
            xmlFile.Load(_fileLocation)
            xmlRoot = CType(xmlFile.DocumentElement, XmlNode)
            '選擇section
            xNodeList_L1 = xmlRoot.SelectNodes("CATEGORY[@KIND = '" & _kind & "']")
            For intI As Integer = 0 To xNodeList_L1.Count - 1
                _dt.Rows.Add()

                xmlNode_L1 = xNodeList_L1.Item(intI)
                xNodeList_L2 = xmlNode_L1.ChildNodes
                For j = 0 To xNodeList_L2.Count - 1
                    xElement_L2 = xNodeList_L2.Item(j)
                    _dt.Rows(_dt.Rows.Count - 1).Item(j) = xElement_L2.InnerText
                Next
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message & System.Environment.NewLine & ex.StackTrace)
        End Try

        Return _dt

    End Function



End Class
