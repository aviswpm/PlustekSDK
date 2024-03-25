<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormDemo_BlockMode
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TEXT_LOG = New System.Windows.Forms.RichTextBox()
        Me.LABEL_DEVICE = New System.Windows.Forms.Label()
        Me.CMB_DEVICE_LIST = New System.Windows.Forms.ComboBox()
        Me.LABEL_COMMAND = New System.Windows.Forms.Label()
        Me.BTN_SCAN = New System.Windows.Forms.Button()
        Me.PIC_IMAGE1 = New System.Windows.Forms.PictureBox()
        Me.PIC_IMAGE2 = New System.Windows.Forms.PictureBox()
        Me.BTN_REFRESH = New System.Windows.Forms.Button()
        Me.BTN_RECYCLESAVEFOLDER = New System.Windows.Forms.Button()
        Me.BTN_MERGEPDF = New System.Windows.Forms.Button()
        Me.BTN_ECO = New System.Windows.Forms.Button()
        Me.BTN_PAPERREADY = New System.Windows.Forms.Button()
        Me.BTN_CALIBRATE = New System.Windows.Forms.Button()
        Me.BTN_EJECT_PAPER = New System.Windows.Forms.Button()
        Me.CHK_EJECT_DIRECT = New System.Windows.Forms.CheckBox()
        Me.BTN_PAPERSTATUS = New System.Windows.Forms.Button()
        Me.BTN_EDIT = New System.Windows.Forms.Button()
        Me.COMBO_COMMAND = New System.Windows.Forms.ComboBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        CType(Me.PIC_IMAGE1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PIC_IMAGE2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TEXT_LOG
        '
        Me.TEXT_LOG.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.TEXT_LOG.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TEXT_LOG.Location = New System.Drawing.Point(12, 13)
        Me.TEXT_LOG.Name = "TEXT_LOG"
        Me.TEXT_LOG.ReadOnly = True
        Me.TEXT_LOG.Size = New System.Drawing.Size(351, 144)
        Me.TEXT_LOG.TabIndex = 0
        Me.TEXT_LOG.Text = ""
        '
        'LABEL_DEVICE
        '
        Me.LABEL_DEVICE.AutoSize = True
        Me.LABEL_DEVICE.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LABEL_DEVICE.Location = New System.Drawing.Point(10, 179)
        Me.LABEL_DEVICE.Name = "LABEL_DEVICE"
        Me.LABEL_DEVICE.Size = New System.Drawing.Size(49, 15)
        Me.LABEL_DEVICE.TabIndex = 1
        Me.LABEL_DEVICE.Text = "Device"
        '
        'CMB_DEVICE_LIST
        '
        Me.CMB_DEVICE_LIST.FormattingEnabled = True
        Me.CMB_DEVICE_LIST.Location = New System.Drawing.Point(72, 179)
        Me.CMB_DEVICE_LIST.Name = "CMB_DEVICE_LIST"
        Me.CMB_DEVICE_LIST.Size = New System.Drawing.Size(210, 21)
        Me.CMB_DEVICE_LIST.TabIndex = 2
        '
        'LABEL_COMMAND
        '
        Me.LABEL_COMMAND.AutoSize = True
        Me.LABEL_COMMAND.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LABEL_COMMAND.Location = New System.Drawing.Point(10, 213)
        Me.LABEL_COMMAND.Name = "LABEL_COMMAND"
        Me.LABEL_COMMAND.Size = New System.Drawing.Size(56, 15)
        Me.LABEL_COMMAND.TabIndex = 3
        Me.LABEL_COMMAND.Text = "Command"
        '
        'BTN_SCAN
        '
        Me.BTN_SCAN.BackColor = System.Drawing.Color.PowderBlue
        Me.BTN_SCAN.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BTN_SCAN.Location = New System.Drawing.Point(8, 19)
        Me.BTN_SCAN.Name = "BTN_SCAN"
        Me.BTN_SCAN.Size = New System.Drawing.Size(168, 35)
        Me.BTN_SCAN.TabIndex = 6
        Me.BTN_SCAN.Text = "Scan"
        Me.BTN_SCAN.UseVisualStyleBackColor = False
        '
        'PIC_IMAGE1
        '
        Me.PIC_IMAGE1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PIC_IMAGE1.Location = New System.Drawing.Point(387, 13)
        Me.PIC_IMAGE1.Name = "PIC_IMAGE1"
        Me.PIC_IMAGE1.Size = New System.Drawing.Size(369, 144)
        Me.PIC_IMAGE1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PIC_IMAGE1.TabIndex = 7
        Me.PIC_IMAGE1.TabStop = False
        '
        'PIC_IMAGE2
        '
        Me.PIC_IMAGE2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PIC_IMAGE2.Location = New System.Drawing.Point(387, 164)
        Me.PIC_IMAGE2.Name = "PIC_IMAGE2"
        Me.PIC_IMAGE2.Size = New System.Drawing.Size(369, 144)
        Me.PIC_IMAGE2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PIC_IMAGE2.TabIndex = 8
        Me.PIC_IMAGE2.TabStop = False
        '
        'BTN_REFRESH
        '
        Me.BTN_REFRESH.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BTN_REFRESH.Location = New System.Drawing.Point(288, 179)
        Me.BTN_REFRESH.Name = "BTN_REFRESH"
        Me.BTN_REFRESH.Size = New System.Drawing.Size(75, 25)
        Me.BTN_REFRESH.TabIndex = 9
        Me.BTN_REFRESH.Text = "Refresh"
        Me.BTN_REFRESH.UseVisualStyleBackColor = True
        '
        'BTN_RECYCLESAVEFOLDER
        '
        Me.BTN_RECYCLESAVEFOLDER.Location = New System.Drawing.Point(190, 62)
        Me.BTN_RECYCLESAVEFOLDER.Name = "BTN_RECYCLESAVEFOLDER"
        Me.BTN_RECYCLESAVEFOLDER.Size = New System.Drawing.Size(167, 25)
        Me.BTN_RECYCLESAVEFOLDER.TabIndex = 10
        Me.BTN_RECYCLESAVEFOLDER.Text = "RecycleSaveFolder"
        Me.BTN_RECYCLESAVEFOLDER.UseVisualStyleBackColor = True
        '
        'BTN_MERGEPDF
        '
        Me.BTN_MERGEPDF.Location = New System.Drawing.Point(8, 96)
        Me.BTN_MERGEPDF.Name = "BTN_MERGEPDF"
        Me.BTN_MERGEPDF.Size = New System.Drawing.Size(75, 25)
        Me.BTN_MERGEPDF.TabIndex = 11
        Me.BTN_MERGEPDF.Text = "MergePdf"
        Me.BTN_MERGEPDF.UseVisualStyleBackColor = True
        '
        'BTN_ECO
        '
        Me.BTN_ECO.Location = New System.Drawing.Point(7, 62)
        Me.BTN_ECO.Name = "BTN_ECO"
        Me.BTN_ECO.Size = New System.Drawing.Size(75, 25)
        Me.BTN_ECO.TabIndex = 13
        Me.BTN_ECO.Text = "ECO"
        Me.BTN_ECO.UseVisualStyleBackColor = True
        '
        'BTN_PAPERREADY
        '
        Me.BTN_PAPERREADY.Location = New System.Drawing.Point(190, 24)
        Me.BTN_PAPERREADY.Name = "BTN_PAPERREADY"
        Me.BTN_PAPERREADY.Size = New System.Drawing.Size(75, 25)
        Me.BTN_PAPERREADY.TabIndex = 14
        Me.BTN_PAPERREADY.Text = "PaperReady"
        Me.BTN_PAPERREADY.UseVisualStyleBackColor = True
        '
        'BTN_CALIBRATE
        '
        Me.BTN_CALIBRATE.Location = New System.Drawing.Point(101, 62)
        Me.BTN_CALIBRATE.Name = "BTN_CALIBRATE"
        Me.BTN_CALIBRATE.Size = New System.Drawing.Size(75, 25)
        Me.BTN_CALIBRATE.TabIndex = 15
        Me.BTN_CALIBRATE.Text = "Calibrate"
        Me.BTN_CALIBRATE.UseVisualStyleBackColor = True
        '
        'BTN_EJECT_PAPER
        '
        Me.BTN_EJECT_PAPER.Location = New System.Drawing.Point(16, 18)
        Me.BTN_EJECT_PAPER.Name = "BTN_EJECT_PAPER"
        Me.BTN_EJECT_PAPER.Size = New System.Drawing.Size(75, 25)
        Me.BTN_EJECT_PAPER.TabIndex = 18
        Me.BTN_EJECT_PAPER.Text = "EjectPaper"
        Me.BTN_EJECT_PAPER.UseVisualStyleBackColor = True
        '
        'CHK_EJECT_DIRECT
        '
        Me.CHK_EJECT_DIRECT.AutoSize = True
        Me.CHK_EJECT_DIRECT.Location = New System.Drawing.Point(110, 23)
        Me.CHK_EJECT_DIRECT.Name = "CHK_EJECT_DIRECT"
        Me.CHK_EJECT_DIRECT.Size = New System.Drawing.Size(51, 17)
        Me.CHK_EJECT_DIRECT.TabIndex = 19
        Me.CHK_EJECT_DIRECT.Text = "Back"
        Me.CHK_EJECT_DIRECT.UseVisualStyleBackColor = True
        '
        'BTN_PAPERSTATUS
        '
        Me.BTN_PAPERSTATUS.Location = New System.Drawing.Point(283, 24)
        Me.BTN_PAPERSTATUS.Name = "BTN_PAPERSTATUS"
        Me.BTN_PAPERSTATUS.Size = New System.Drawing.Size(75, 25)
        Me.BTN_PAPERSTATUS.TabIndex = 14
        Me.BTN_PAPERSTATUS.Text = "PaperStatus"
        Me.BTN_PAPERSTATUS.UseVisualStyleBackColor = True
        '
        'BTN_EDIT
        '
        Me.BTN_EDIT.Location = New System.Drawing.Point(72, 210)
        Me.BTN_EDIT.Name = "BTN_EDIT"
        Me.BTN_EDIT.Size = New System.Drawing.Size(45, 23)
        Me.BTN_EDIT.TabIndex = 20
        Me.BTN_EDIT.Text = "Edit"
        Me.BTN_EDIT.UseVisualStyleBackColor = True
        '
        'COMBO_COMMAND
        '
        Me.COMBO_COMMAND.FormattingEnabled = True
        Me.COMBO_COMMAND.Location = New System.Drawing.Point(123, 212)
        Me.COMBO_COMMAND.Name = "COMBO_COMMAND"
        Me.COMBO_COMMAND.Size = New System.Drawing.Size(239, 21)
        Me.COMBO_COMMAND.TabIndex = 21
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.BTN_SCAN)
        Me.GroupBox1.Controls.Add(Me.BTN_PAPERREADY)
        Me.GroupBox1.Controls.Add(Me.BTN_PAPERSTATUS)
        Me.GroupBox1.Controls.Add(Me.BTN_ECO)
        Me.GroupBox1.Controls.Add(Me.BTN_CALIBRATE)
        Me.GroupBox1.Controls.Add(Me.BTN_RECYCLESAVEFOLDER)
        Me.GroupBox1.Controls.Add(Me.BTN_MERGEPDF)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 246)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(364, 132)
        Me.GroupBox1.TabIndex = 22
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Normal"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BTN_EJECT_PAPER)
        Me.GroupBox2.Controls.Add(Me.CHK_EJECT_DIRECT)
        Me.GroupBox2.Location = New System.Drawing.Point(387, 325)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(165, 64)
        Me.GroupBox2.TabIndex = 23
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "VTM300"
        '
        'FormDemo_BlockMode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(767, 398)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.COMBO_COMMAND)
        Me.Controls.Add(Me.BTN_EDIT)
        Me.Controls.Add(Me.BTN_REFRESH)
        Me.Controls.Add(Me.PIC_IMAGE2)
        Me.Controls.Add(Me.PIC_IMAGE1)
        Me.Controls.Add(Me.LABEL_COMMAND)
        Me.Controls.Add(Me.CMB_DEVICE_LIST)
        Me.Controls.Add(Me.LABEL_DEVICE)
        Me.Controls.Add(Me.TEXT_LOG)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormDemo_BlockMode"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Demo-VB"
        CType(Me.PIC_IMAGE1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PIC_IMAGE2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TEXT_LOG As System.Windows.Forms.RichTextBox
    Friend WithEvents LABEL_DEVICE As System.Windows.Forms.Label
    Friend WithEvents CMB_DEVICE_LIST As System.Windows.Forms.ComboBox
    Friend WithEvents LABEL_COMMAND As System.Windows.Forms.Label
    Friend WithEvents BTN_SCAN As System.Windows.Forms.Button
    Friend WithEvents PIC_IMAGE1 As System.Windows.Forms.PictureBox
    Friend WithEvents PIC_IMAGE2 As System.Windows.Forms.PictureBox
    Friend WithEvents BTN_REFRESH As System.Windows.Forms.Button
    Friend WithEvents BTN_RECYCLESAVEFOLDER As System.Windows.Forms.Button
    Friend WithEvents BTN_MERGEPDF As System.Windows.Forms.Button
    Friend WithEvents BTN_ECO As System.Windows.Forms.Button
    Friend WithEvents BTN_PAPERREADY As System.Windows.Forms.Button
    Friend WithEvents BTN_CALIBRATE As System.Windows.Forms.Button
    Friend WithEvents BTN_EJECT_PAPER As System.Windows.Forms.Button
    Friend WithEvents CHK_EJECT_DIRECT As System.Windows.Forms.CheckBox
    Friend WithEvents BTN_PAPERSTATUS As System.Windows.Forms.Button
    Friend WithEvents BTN_EDIT As Button
    Friend WithEvents COMBO_COMMAND As ComboBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
End Class
