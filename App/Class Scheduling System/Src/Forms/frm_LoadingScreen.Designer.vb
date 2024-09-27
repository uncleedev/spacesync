<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_LoadingScreen
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim CustomizableEdges1 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges2 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges3 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim CustomizableEdges4 As Guna.UI2.WinForms.Suite.CustomizableEdges = New Guna.UI2.WinForms.Suite.CustomizableEdges()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_LoadingScreen))
        Label11 = New Label()
        Label2 = New Label()
        Guna2PictureBox1 = New Guna.UI2.WinForms.Guna2PictureBox()
        loadingBar = New Guna.UI2.WinForms.Guna2ProgressBar()
        Timer1 = New Timer(components)
        Guna2BorderlessForm1 = New Guna.UI2.WinForms.Guna2BorderlessForm(components)
        Guna2ShadowForm1 = New Guna.UI2.WinForms.Guna2ShadowForm(components)
        CType(Guna2PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label11
        ' 
        Label11.AutoSize = True
        Label11.BackColor = Color.Transparent
        Label11.Font = New Font("Franklin Gothic Medium", 48F, FontStyle.Regular, GraphicsUnit.Pixel, CByte(0))
        Label11.ForeColor = Color.FromArgb(CByte(151), CByte(254), CByte(237))
        Label11.Location = New Point(384, 138)
        Label11.Name = "Label11"
        Label11.Size = New Size(249, 61)
        Label11.TabIndex = 5
        Label11.Text = "SpaceSync"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.BackColor = Color.Transparent
        Label2.Font = New Font("Franklin Gothic Medium", 20F, FontStyle.Regular, GraphicsUnit.Pixel, CByte(0))
        Label2.ForeColor = Color.FromArgb(CByte(151), CByte(254), CByte(237))
        Label2.Location = New Point(384, 205)
        Label2.Name = "Label2"
        Label2.Size = New Size(224, 25)
        Label2.TabIndex = 7
        Label2.Text = "Class Scheduling System"
        ' 
        ' Guna2PictureBox1
        ' 
        Guna2PictureBox1.CustomizableEdges = CustomizableEdges1
        Guna2PictureBox1.Image = My.Resources.Resources.management
        Guna2PictureBox1.ImageRotate = 0F
        Guna2PictureBox1.Location = New Point(174, 74)
        Guna2PictureBox1.Name = "Guna2PictureBox1"
        Guna2PictureBox1.ShadowDecoration.CustomizableEdges = CustomizableEdges2
        Guna2PictureBox1.Size = New Size(200, 200)
        Guna2PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        Guna2PictureBox1.TabIndex = 8
        Guna2PictureBox1.TabStop = False
        ' 
        ' loadingBar
        ' 
        loadingBar.BackColor = Color.Transparent
        loadingBar.BorderRadius = 10
        loadingBar.CustomizableEdges = CustomizableEdges3
        loadingBar.Location = New Point(109, 297)
        loadingBar.Name = "loadingBar"
        loadingBar.ProgressColor = Color.FromArgb(CByte(151), CByte(254), CByte(237))
        loadingBar.ProgressColor2 = Color.FromArgb(CByte(151), CByte(254), CByte(237))
        loadingBar.ShadowDecoration.BorderRadius = 10
        loadingBar.ShadowDecoration.CustomizableEdges = CustomizableEdges4
        loadingBar.ShadowDecoration.Depth = 64
        loadingBar.ShadowDecoration.Enabled = True
        loadingBar.ShadowDecoration.Shadow = New Padding(0, 0, 5, 5)
        loadingBar.Size = New Size(582, 30)
        loadingBar.Style = ProgressBarStyle.Continuous
        loadingBar.TabIndex = 9
        loadingBar.Text = "Guna2ProgressBar2"
        loadingBar.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
        ' 
        ' Timer1
        ' 
        ' 
        ' Guna2BorderlessForm1
        ' 
        Guna2BorderlessForm1.BorderRadius = 20
        Guna2BorderlessForm1.ContainerControl = Me
        Guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6R
        Guna2BorderlessForm1.ResizeForm = False
        Guna2BorderlessForm1.TransparentWhileDrag = True
        ' 
        ' Guna2ShadowForm1
        ' 
        Guna2ShadowForm1.BorderRadius = 20
        Guna2ShadowForm1.TargetForm = Me
        ' 
        ' frm_LoadingScreen
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(11), CByte(102), CByte(106))
        ClientSize = New Size(800, 400)
        Controls.Add(loadingBar)
        Controls.Add(Guna2PictureBox1)
        Controls.Add(Label2)
        Controls.Add(Label11)
        FormBorderStyle = FormBorderStyle.None
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Name = "frm_LoadingScreen"
        StartPosition = FormStartPosition.CenterScreen
        Text = "frm_LoadingScreen"
        CType(Guna2PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents Label11 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Guna2PictureBox1 As Guna.UI2.WinForms.Guna2PictureBox
    Friend WithEvents loadingBar As Guna.UI2.WinForms.Guna2ProgressBar
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Guna2BorderlessForm1 As Guna.UI2.WinForms.Guna2BorderlessForm
    Friend WithEvents Guna2ShadowForm1 As Guna.UI2.WinForms.Guna2ShadowForm
End Class
