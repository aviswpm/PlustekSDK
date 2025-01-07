Public Class FormECO
    Dim m_nTime As UInteger

    Public Sub New(ByVal nTime As UInteger)
        InitializeComponent()
        TEXT_ECOTIME.Text = nTime.ToString
    End Sub

    Public Overloads Function ShowDialog(ByRef nTime As UInteger) As DialogResult
        Dim dialogResult As DialogResult = Me.ShowDialog
        nTime = m_nTime
        Return dialogResult
    End Function

    Private Sub BTN_SETECO_Click(sender As Object, e As EventArgs) Handles BTN_SETECO.Click
        m_nTime = Convert.ToUInt32(TEXT_ECOTIME.Text)
        Close()
    End Sub
End Class