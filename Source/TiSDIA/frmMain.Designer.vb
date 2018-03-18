<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請勿使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.營運路線ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_WN = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_WM = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_WSEA = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_WS = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_P = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_S = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_T = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_N = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_I = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_PX = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_NW = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_J = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsm_SL = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmDownloadXml = New System.Windows.Forms.ToolStripMenuItem()
        Me.選項ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.關於ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.dtpDateChoose = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtTrainNo = New System.Windows.Forms.TextBox()
        Me.txtFolderLocation = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsm_IN = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.營運路線ToolStripMenuItem, Me.tsmDownloadXml, Me.選項ToolStripMenuItem, Me.關於ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(457, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        '營運路線ToolStripMenuItem
        '
        Me.營運路線ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsm_WN, Me.tsm_WM, Me.tsm_WSEA, Me.tsm_WS, Me.tsm_P, Me.tsm_S, Me.tsm_T, Me.tsm_N, Me.tsm_I, Me.tsm_PX, Me.tsm_NW, Me.tsm_J, Me.tsm_SL, Me.tsm_IN})
        Me.營運路線ToolStripMenuItem.Name = "營運路線ToolStripMenuItem"
        Me.營運路線ToolStripMenuItem.Size = New System.Drawing.Size(67, 20)
        Me.營運路線ToolStripMenuItem.Text = "營運路線"
        '
        'tsm_WN
        '
        Me.tsm_WN.Name = "tsm_WN"
        Me.tsm_WN.Size = New System.Drawing.Size(152, 22)
        Me.tsm_WN.Text = "西部幹線北段"
        '
        'tsm_WM
        '
        Me.tsm_WM.Name = "tsm_WM"
        Me.tsm_WM.Size = New System.Drawing.Size(152, 22)
        Me.tsm_WM.Text = "山線"
        '
        'tsm_WSEA
        '
        Me.tsm_WSEA.Name = "tsm_WSEA"
        Me.tsm_WSEA.Size = New System.Drawing.Size(152, 22)
        Me.tsm_WSEA.Text = "海線"
        '
        'tsm_WS
        '
        Me.tsm_WS.Name = "tsm_WS"
        Me.tsm_WS.Size = New System.Drawing.Size(152, 22)
        Me.tsm_WS.Text = "西部幹線南段"
        '
        'tsm_P
        '
        Me.tsm_P.Name = "tsm_P"
        Me.tsm_P.Size = New System.Drawing.Size(152, 22)
        Me.tsm_P.Text = "屏東線"
        '
        'tsm_S
        '
        Me.tsm_S.Name = "tsm_S"
        Me.tsm_S.Size = New System.Drawing.Size(152, 22)
        Me.tsm_S.Text = "南迴線"
        '
        'tsm_T
        '
        Me.tsm_T.Name = "tsm_T"
        Me.tsm_T.Size = New System.Drawing.Size(152, 22)
        Me.tsm_T.Text = "台東線"
        '
        'tsm_N
        '
        Me.tsm_N.Name = "tsm_N"
        Me.tsm_N.Size = New System.Drawing.Size(152, 22)
        Me.tsm_N.Text = "北迴線"
        '
        'tsm_I
        '
        Me.tsm_I.Name = "tsm_I"
        Me.tsm_I.Size = New System.Drawing.Size(152, 22)
        Me.tsm_I.Text = "宜蘭線"
        '
        'tsm_PX
        '
        Me.tsm_PX.Name = "tsm_PX"
        Me.tsm_PX.Size = New System.Drawing.Size(152, 22)
        Me.tsm_PX.Text = "平溪線"
        '
        'tsm_NW
        '
        Me.tsm_NW.Name = "tsm_NW"
        Me.tsm_NW.Size = New System.Drawing.Size(152, 22)
        Me.tsm_NW.Text = "內灣線"
        '
        'tsm_J
        '
        Me.tsm_J.Name = "tsm_J"
        Me.tsm_J.Size = New System.Drawing.Size(152, 22)
        Me.tsm_J.Text = "集集線"
        '
        'tsm_SL
        '
        Me.tsm_SL.Name = "tsm_SL"
        Me.tsm_SL.Size = New System.Drawing.Size(152, 22)
        Me.tsm_SL.Text = "沙崙線"
        '
        'tsmDownloadXml
        '
        Me.tsmDownloadXml.Name = "tsmDownloadXml"
        Me.tsmDownloadXml.Size = New System.Drawing.Size(129, 20)
        Me.tsmDownloadXml.Text = "更新台鐵時刻表XML"
        '
        '選項ToolStripMenuItem
        '
        Me.選項ToolStripMenuItem.Name = "選項ToolStripMenuItem"
        Me.選項ToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.選項ToolStripMenuItem.Text = "選項"
        '
        '關於ToolStripMenuItem
        '
        Me.關於ToolStripMenuItem.Name = "關於ToolStripMenuItem"
        Me.關於ToolStripMenuItem.Size = New System.Drawing.Size(43, 20)
        Me.關於ToolStripMenuItem.Text = "關於"
        '
        'dtpDateChoose
        '
        Me.dtpDateChoose.Font = New System.Drawing.Font("微軟正黑體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.dtpDateChoose.Location = New System.Drawing.Point(59, 27)
        Me.dtpDateChoose.Name = "dtpDateChoose"
        Me.dtpDateChoose.Size = New System.Drawing.Size(160, 29)
        Me.dtpDateChoose.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("微軟正黑體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "日期"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "PNG|*.png|GIF|*.gif"
        Me.SaveFileDialog1.InitialDirectory = "%USERPROFILE%\Deskto"
        Me.SaveFileDialog1.RestoreDirectory = True
        Me.SaveFileDialog1.Title = "儲存檔案"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("微軟正黑體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label2.Location = New System.Drawing.Point(225, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "繪製特定車次"
        '
        'txtTrainNo
        '
        Me.txtTrainNo.Font = New System.Drawing.Font("微軟正黑體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtTrainNo.Location = New System.Drawing.Point(336, 27)
        Me.txtTrainNo.Name = "txtTrainNo"
        Me.txtTrainNo.Size = New System.Drawing.Size(100, 29)
        Me.txtTrainNo.TabIndex = 6
        '
        'txtFolderLocation
        '
        Me.txtFolderLocation.Font = New System.Drawing.Font("微軟正黑體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtFolderLocation.Location = New System.Drawing.Point(91, 71)
        Me.txtFolderLocation.Name = "txtFolderLocation"
        Me.txtFolderLocation.Size = New System.Drawing.Size(268, 29)
        Me.txtFolderLocation.TabIndex = 8
        Me.txtFolderLocation.Text = "C:\temp"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("微軟正黑體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 20)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "匯出位置"
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("微軟正黑體", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Button1.Location = New System.Drawing.Point(365, 70)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 29)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "瀏覽"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 166)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(457, 22)
        Me.StatusStrip1.TabIndex = 10
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(31, 17)
        Me.ToolStripStatusLabel1.Text = "就緒"
        '
        'tsm_IN
        '
        Me.tsm_IN.Name = "tsm_IN"
        Me.tsm_IN.Size = New System.Drawing.Size(152, 22)
        Me.tsm_IN.Text = "宜蘭北迴"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(457, 188)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtFolderLocation)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtTrainNo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtpDateChoose)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "frmMain"
        Me.Text = "台灣鐵路局鐵路運行圖繪製程式"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents 營運路線ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsm_WN As ToolStripMenuItem
    Friend WithEvents tsm_WM As ToolStripMenuItem
    Friend WithEvents tsm_WSEA As ToolStripMenuItem
    Friend WithEvents tsm_WS As ToolStripMenuItem
    Friend WithEvents tsm_P As ToolStripMenuItem
    Friend WithEvents tsm_S As ToolStripMenuItem
    Friend WithEvents tsm_T As ToolStripMenuItem
    Friend WithEvents tsm_PX As ToolStripMenuItem
    Friend WithEvents tsm_NW As ToolStripMenuItem
    Friend WithEvents tsm_J As ToolStripMenuItem
    Friend WithEvents 關於ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsmDownloadXml As ToolStripMenuItem
    Friend WithEvents 選項ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents dtpDateChoose As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents Label2 As Label
    Friend WithEvents txtTrainNo As TextBox
    Friend WithEvents tsm_I As ToolStripMenuItem
    Friend WithEvents txtFolderLocation As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents tsm_SL As ToolStripMenuItem
    Friend WithEvents tsm_N As ToolStripMenuItem
    Friend WithEvents tsm_IN As ToolStripMenuItem
End Class
