<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormWaitMsg
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
        Me.Label_Scan = New System.Windows.Forms.Label()
        Me.Label_Cal_Normal_1 = New System.Windows.Forms.Label()
        Me.Label_Cal_Normal_2 = New System.Windows.Forms.Label()
        Me.Label_Cal_Xmini = New System.Windows.Forms.Label()
        Me.Label_Init = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label_Scan
        '
        Me.Label_Scan.AutoSize = True
        Me.Label_Scan.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.25!)
        Me.Label_Scan.Location = New System.Drawing.Point(26, 37)
        Me.Label_Scan.Name = "Label_Scan"
        Me.Label_Scan.Size = New System.Drawing.Size(238, 26)
        Me.Label_Scan.TabIndex = 0
        Me.Label_Scan.Text = "Scanning...Please Wait"
        '
        'Label_Cal_Normal_1
        '
        Me.Label_Cal_Normal_1.AutoSize = True
        Me.Label_Cal_Normal_1.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.25!)
        Me.Label_Cal_Normal_1.Location = New System.Drawing.Point(28, 20)
        Me.Label_Cal_Normal_1.Name = "Label_Cal_Normal_1"
        Me.Label_Cal_Normal_1.Size = New System.Drawing.Size(247, 26)
        Me.Label_Cal_Normal_1.TabIndex = 1
        Me.Label_Cal_Normal_1.Text = "Calibration in progress..."
        '
        'Label_Cal_Normal_2
        '
        Me.Label_Cal_Normal_2.AutoSize = True
        Me.Label_Cal_Normal_2.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.25!)
        Me.Label_Cal_Normal_2.Location = New System.Drawing.Point(16, 53)
        Me.Label_Cal_Normal_2.Name = "Label_Cal_Normal_2"
        Me.Label_Cal_Normal_2.Size = New System.Drawing.Size(266, 26)
        Me.Label_Cal_Normal_2.TabIndex = 2
        Me.Label_Cal_Normal_2.Text = "Please wait 3~40 seconds"
        '
        'Label_Cal_Xmini
        '
        Me.Label_Cal_Xmini.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.25!)
        Me.Label_Cal_Xmini.Location = New System.Drawing.Point(7, 24)
        Me.Label_Cal_Xmini.Name = "Label_Cal_Xmini"
        Me.Label_Cal_Xmini.Size = New System.Drawing.Size(600, 66)
        Me.Label_Cal_Xmini.TabIndex = 3
        Me.Label_Cal_Xmini.Text = "Note: The calibration process may take up to 40 seconds to complete. During this " &
    "time, you may hear beeps."
        '
        'Label_Init
        '
        Me.Label_Init.AutoSize = True
        Me.Label_Init.Font = New System.Drawing.Font("Microsoft Sans Serif", 16.25!)
        Me.Label_Init.Location = New System.Drawing.Point(50, 40)
        Me.Label_Init.Name = "Label_Init"
        Me.Label_Init.Size = New System.Drawing.Size(176, 26)
        Me.Label_Init.TabIndex = 4
        Me.Label_Init.Text = "Init...Please Wait"
        '
        'FormWaitMsg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(294, 106)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label_Init)
        Me.Controls.Add(Me.Label_Cal_Xmini)
        Me.Controls.Add(Me.Label_Cal_Normal_2)
        Me.Controls.Add(Me.Label_Cal_Normal_1)
        Me.Controls.Add(Me.Label_Scan)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormWaitMsg"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label_Scan As Label
    Friend WithEvents Label_Cal_Normal_1 As Label
    Friend WithEvents Label_Cal_Normal_2 As Label
    Friend WithEvents Label_Cal_Xmini As Label
    Friend WithEvents Label_Init As Label
End Class
