Imports Microsoft.Win32
Public Class FormDemo
    Private Declare Unicode Function GetPrivateProfileString Lib "kernel32" _
    Alias "GetPrivateProfileStringW" (ByVal lpApplicationName As String,
    ByVal lpKeyName As String, ByVal lpDefault As String,
    ByVal lpReturnedString As String, ByVal nSize As Int32,
    ByVal lpFileName As String) As Int32

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ParamVal As String = Space$(5)
        Dim LenParamVal As Long = 1
        If System.IO.File.Exists(Application.StartupPath + "\\LibWebFxScan.ini") = True Then
            LenParamVal = GetPrivateProfileString("Style", "UseModeBlock", "1", ParamVal, Len(ParamVal), Application.StartupPath + "\\LibWebFxScan.ini")
        Else
            Dim keyName As String
            Dim sdkPath As String
#If WIN32 Then
        keyName = "HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"
#Else
            keyName = "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"
#End If
            Dim value As Object = Registry.GetValue(keyName, "InstallLocation", "")

            If value IsNot Nothing Then
                sdkPath = value.ToString()
                If (sdkPath.LastIndexOf("\") <> (sdkPath.Length - 1)) Then
                    sdkPath += "\"
                End If
                sdkPath = sdkPath + "LibWebFxScan.ini"

                If System.IO.File.Exists(sdkPath) = True Then
                    LenParamVal = GetPrivateProfileString("Style", "UseModeBlock", "1", ParamVal, Len(ParamVal), sdkPath)
                End If
            End If
        End If
        If ParamVal.Contains("0") Then
            Dim form As New FormDemo_NonBlockMode
            form.Show()
        Else
            Dim form As New FormDemo_BlockMode
            form.Show()
        End If
        Me.Close()
    End Sub
End Class
