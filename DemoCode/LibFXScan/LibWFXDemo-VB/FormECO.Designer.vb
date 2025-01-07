<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormECO
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
        Me.LABEL_ECOTIME = New System.Windows.Forms.Label()
        Me.TEXT_ECOTIME = New System.Windows.Forms.TextBox()
        Me.LABEL_MINS = New System.Windows.Forms.Label()
        Me.BTN_SETECO = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LABEL_ECOTIME
        '
        Me.LABEL_ECOTIME.AutoSize = True
        Me.LABEL_ECOTIME.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LABEL_ECOTIME.Location = New System.Drawing.Point(21, 12)
        Me.LABEL_ECOTIME.Name = "LABEL_ECOTIME"
        Me.LABEL_ECOTIME.Size = New System.Drawing.Size(80, 18)
        Me.LABEL_ECOTIME.TabIndex = 0
        Me.LABEL_ECOTIME.Text = "ECO Time:"
        '
        'TEXT_ECOTIME
        '
        Me.TEXT_ECOTIME.Location = New System.Drawing.Point(107, 12)
        Me.TEXT_ECOTIME.Name = "TEXT_ECOTIME"
        Me.TEXT_ECOTIME.Size = New System.Drawing.Size(100, 22)
        Me.TEXT_ECOTIME.TabIndex = 1
        '
        'LABEL_MINS
        '
        Me.LABEL_MINS.AutoSize = True
        Me.LABEL_MINS.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LABEL_MINS.Location = New System.Drawing.Point(213, 12)
        Me.LABEL_MINS.Name = "LABEL_MINS"
        Me.LABEL_MINS.Size = New System.Drawing.Size(40, 18)
        Me.LABEL_MINS.TabIndex = 2
        Me.LABEL_MINS.Text = "mins"
        '
        'BTN_SETECO
        '
        Me.BTN_SETECO.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BTN_SETECO.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BTN_SETECO.Location = New System.Drawing.Point(296, 13)
        Me.BTN_SETECO.Name = "BTN_SETECO"
        Me.BTN_SETECO.Size = New System.Drawing.Size(75, 23)
        Me.BTN_SETECO.TabIndex = 3
        Me.BTN_SETECO.Text = "Set"
        Me.BTN_SETECO.UseVisualStyleBackColor = False
        '
        'FormECO
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(383, 48)
        Me.Controls.Add(Me.BTN_SETECO)
        Me.Controls.Add(Me.LABEL_MINS)
        Me.Controls.Add(Me.TEXT_ECOTIME)
        Me.Controls.Add(Me.LABEL_ECOTIME)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormECO"
        Me.Text = "FormECO"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LABEL_ECOTIME As System.Windows.Forms.Label
    Friend WithEvents TEXT_ECOTIME As System.Windows.Forms.TextBox
    Friend WithEvents LABEL_MINS As System.Windows.Forms.Label
    Friend WithEvents BTN_SETECO As System.Windows.Forms.Button
End Class
