Public Class FormWaitMsg
    Private labelToShow As String

    Public Sub New(value As String)
        InitializeComponent()
        labelToShow = value
    End Sub
    Private Sub FormWaitMsg_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowLabel(labelToShow)
    End Sub

    Private Sub ShowLabel(labelName As String)
        Select Case labelName
            Case "Scan"
                Me.Width = 300
                Me.Height = 112
                Label_Scan.Visible = True
                Label_Cal_Normal_1.Visible = False
                Label_Cal_Normal_2.Visible = False
                Label_Cal_Xmini.Visible = False
                Label_Init.Visible = False
            Case "Calibrate_normal"
                Me.Width = 300
                Me.Height = 112
                Label_Scan.Visible = False
                Label_Cal_Normal_1.Visible = True
                Label_Cal_Normal_2.Visible = True
                Label_Cal_Xmini.Visible = False
                Label_Init.Visible = False
            Case "Calibrate_xmini"
                Me.Width = 600
                Me.Height = 112
                Label_Scan.Visible = False
                Label_Cal_Normal_1.Visible = False
                Label_Cal_Normal_2.Visible = False
                Label_Cal_Xmini.Visible = True
                Label_Init.Visible = False
            Case "Init"
                Me.Width = 300
                Me.Height = 112
                Label_Scan.Visible = False
                Label_Cal_Normal_1.Visible = False
                Label_Cal_Normal_2.Visible = False
                Label_Cal_Xmini.Visible = False
                Label_Init.Visible = True
            Case Else
                Label_Scan.Visible = False
                Label_Cal_Normal_1.Visible = False
                Label_Cal_Normal_2.Visible = False
                Label_Cal_Xmini.Visible = False
                Label_Init.Visible = False
        End Select
    End Sub
End Class