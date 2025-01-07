<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormWarmup
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
        Me.LABEL_WARMUP = New System.Windows.Forms.Label()
        Me.PROGRESSBAR_WARMUP = New System.Windows.Forms.ProgressBar()
        Me.SuspendLayout()
        '
        'LABEL_WARMUP
        '
        Me.LABEL_WARMUP.AutoSize = True
        Me.LABEL_WARMUP.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LABEL_WARMUP.Location = New System.Drawing.Point(96, 9)
        Me.LABEL_WARMUP.Name = "LABEL_WARMUP"
        Me.LABEL_WARMUP.Size = New System.Drawing.Size(176, 18)
        Me.LABEL_WARMUP.TabIndex = 0
        Me.LABEL_WARMUP.Text = "Warm up...Please wait"
        '
        'PROGRESSBAR_WARMUP
        '
        Me.PROGRESSBAR_WARMUP.Location = New System.Drawing.Point(43, 52)
        Me.PROGRESSBAR_WARMUP.Name = "PROGRESSBAR_WARMUP"
        Me.PROGRESSBAR_WARMUP.Size = New System.Drawing.Size(287, 23)
        Me.PROGRESSBAR_WARMUP.TabIndex = 1
        '
        'FormWarmup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(379, 99)
        Me.Controls.Add(Me.PROGRESSBAR_WARMUP)
        Me.Controls.Add(Me.LABEL_WARMUP)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FormWarmup"
        Me.Text = "FormWarmup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LABEL_WARMUP As System.Windows.Forms.Label
    Friend WithEvents PROGRESSBAR_WARMUP As System.Windows.Forms.ProgressBar
End Class
