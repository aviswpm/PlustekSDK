Imports LibWFXDemo_VB.DeviceWrapper
Imports System.Runtime.InteropServices
Imports Newtonsoft.Json
Imports System.IO
Imports System.Boolean
Imports System.Threading


Public Class FormDemo_BlockMode
    Dim m_DeviceWrapper As DeviceWrapper = New DeviceWrapper()
    Dim m_scanningForm As FormScanning
    Dim m_nCount As Integer = 0
    Dim m_MaxCMDItems As Integer = 5

    Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (Destination As IntPtr, Source As IntPtr, ByVal Length As Integer)
    Private Function GetColorPalette(nColors As UInteger)
        Dim bitscolordepth As System.Drawing.Imaging.PixelFormat = System.Drawing.Imaging.PixelFormat.Format1bppIndexed
        Dim palatte As System.Drawing.Imaging.ColorPalette

        Dim bitmap As Bitmap
        If nColors > 2 Then
            bitscolordepth = System.Drawing.Imaging.PixelFormat.Format4bppIndexed
        End If

        If nColors > 16 Then
            bitscolordepth = System.Drawing.Imaging.PixelFormat.Format8bppIndexed
        End If

        bitmap = New Bitmap(1, 1, bitscolordepth)
        palatte = bitmap.Palette
        bitmap.Dispose()

        Return palatte
    End Function

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim enRet As ENUM_LIBWFX_ERRCODE
        m_scanningForm = New FormScanning


        REM Init LibWFXScan Library
        REM enRet = DeviceWrapper.LibWFX_Init()
        REM since we can't debug OCR engine, for debuging UI flow, use LIBWFX_INIT_MODE_NOOCR
        REM OCR will not work, but easier to debug UI while developing      

        If m_DeviceWrapper.m_pfnLibWFX_IsWindowExist("") = True Then
            MessageBox.Show("Status:[Please confirm whether the ""CheckWindowTitle"" parameter content in LibWebFxScan.ini are all closed!!]", "Warning")
            WriteLog("Status:[LibWFX_InitEx Fail]")
        Else
#If DEBUG Then
            enRet = m_DeviceWrapper.m_pfnLibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NOOCR)
#Else
            enRet = m_DeviceWrapper.m_pfnLibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL)
#End If

            If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
                BTN_REFRESH_Click(Nothing, Nothing)
                WriteLog("Status:[LibWFX_InitEx Success]")
                GetCertificatePermission()
            ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_OCR Then
                BTN_REFRESH_Click(Nothing, Nothing)
                WriteLog("Status:[No Recognize Tool]")
                GetCertificatePermission()
            ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_AVI_OCR Then
                BTN_REFRESH_Click(Nothing, Nothing)
                WriteLog("Status:[No AVI Recognize Tool]")
                GetCertificatePermission()
            ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DOC_OCR Then
                BTN_REFRESH_Click(Nothing, Nothing)
                WriteLog("Status:[No DOC Recognize Tool]")
                GetCertificatePermission()
            ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG Then
                WriteLog("Status:[Path Is Too Long (max limit: 130 bits)]")
                WriteLog("Status:[LibWFX_InitEx Fail]")
            Else
                WriteLog("Status:[LibWFX_InitEx Fail]")
            End If
        End If
    End Sub

    Private Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        REM Release LibWFXScan Resource
        m_scanningForm.Dispose()
        m_scanningForm = Nothing
        m_DeviceWrapper.m_pfnLibWFX_CloseDevice()
        m_DeviceWrapper.m_pfnLibWFX_DeInit()
        'Me.Close()
    End Sub

    Private Sub BTN_REFRESH_Click(sender As Object, e As EventArgs) Handles BTN_REFRESH.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim pszDeviceList As IntPtr
        Dim pszSerialNumberList As IntPtr
        Dim szDeviceList As String
        Dim szSerialNumberList As String
        Dim listDevice As New List(Of String)
        Dim listSerialNumber As New List(Of String)

        REM Init Get Devices List
        enRet = m_DeviceWrapper.m_pfnLibWFX_GetDeviesListWithSerial(pszDeviceList, pszSerialNumberList)
        If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            szDeviceList = Marshal.PtrToStringUni(pszDeviceList)
            szSerialNumberList = Marshal.PtrToStringUni(pszSerialNumberList)
            listDevice = JsonConvert.DeserializeObject(Of List(Of String))(szDeviceList)
            listSerialNumber = JsonConvert.DeserializeObject(Of List(Of String))(szSerialNumberList)

            CMB_DEVICE_LIST.Items.Clear()
            CMB_DEVICE_LIST.Items.AddRange(listDevice.ToArray)
            CMB_DEVICE_LIST.SelectedIndex = 0

            For idx As Integer = 0 To listDevice.Count - 1
                WriteLog("Device: " + listDevice(idx) + "   " + "Serial Number: " + listSerialNumber(idx))
            Next
        ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DEVICES Then
            WriteLog("Status:[No Device]")
        ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL Then
            WriteLog("Status:[Load MRTD DLL Fail]")
        ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SCANNING Then
            WriteLog("Status:[Scanning Fail]")
        Else
            WriteLog("Status:[LibWFX_GetDeviesList Fail]")
        End If
    End Sub

    Private Delegate Sub WriteLogCB(ByVal szMsg As String)
    Private Sub WriteLog(ByVal szMsg As String)
        If Me.TEXT_LOG.InvokeRequired Then
            Dim WriteLogCB As New WriteLogCB(AddressOf WriteLog)
            Me.BeginInvoke(WriteLogCB, szMsg)
        Else
            Me.TEXT_LOG.Text += szMsg
            Me.TEXT_LOG.Text += Environment.NewLine
            Me.TEXT_LOG.SelectionStart = Me.TEXT_LOG.Text.Length
            Me.TEXT_LOG.ScrollToCaret()
        End If
        m_DeviceWrapper.m_pfnLibWFX_WriteAPLog(szMsg)
    End Sub

    Private Sub CMB_DEVICE_LIST_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CMB_DEVICE_LIST.SelectedIndexChanged
        REM Dim szJsonCmd As String = ""
        SetDefaultJsonCommand(CMB_DEVICE_LIST.SelectedItem.ToString)
        REM TEXTBOX_COMMAND.Text = szJsonCmd
    End Sub

    Private Sub SetDefaultJsonCommand(ByVal szDevName As String)
        Dim szDefJson As String
        If GetCommandString(szDevName) = True Then
            Return
        ElseIf szDevName = "A61" Or szDevName = "A62" Or szDevName = "A63" Or szDevName = "A64" Or szDevName = "A65" Or szDevName = "A66" Or szDevName = "J6102R" Then
            szDefJson += "{""device-name"":"""
            szDefJson += szDevName
            szDefJson += """,""source"":""Camera"",""recognize-type"":""passport""}"
        ElseIf szDevName = "7C1U" Or szDevName = "7C8U" Or szDevName = "7C9U" Or szDevName = "7CAU" Or szDevName = "773U" Or szDevName = "7CCU" Then
            szDefJson += "{""device-name"":"""
            szDefJson += szDevName
            szDefJson += """,""source"":""Sheetfed-Duplex"",""recognize-type"":""passport""}"
        ElseIf szDevName = "776U" Or szDevName = "777U" Or szDevName = "778U" Then
            szDefJson += "{""device-name"":"""
            szDefJson += szDevName
            szDefJson += """,""source"":""Sheetfed-Duplex""}"
        ElseIf szDevName = "74RU" Or szDevName = "74BU" Or szDevName = "7P1U" Or szDevName = "M11U" Or szDevName = "7B3U" Or szDevName = "M12U" Then
            szDefJson += "{""device-name"":"""
            szDefJson += szDevName
            szDefJson += """,""source"":""Sheetfed-Front""}"
        ElseIf szDevName = "256U" Or
               szDevName = "258U" Or
               szDevName = "258U_259U" Or
               szDevName = "25AU" Or
               szDevName = "271U" Or
               szDevName = "273U" Or
               szDevName = "273U_274U" Or
               szDevName = "275U" Or
               szDevName = "276U" Or
               szDevName = "261U" Or
               szDevName = "BAG" Or
               szDevName = "7K1U" Or
               szDevName = "6C6U" Or
               szDevName = "BB1U" Or
               szDevName = "BAGU" Or
               szDevName = "2B2U" Or
               szDevName = "2B3U" Or
               szDevName = "2D1U" Or
               szDevName = "2C1U" Or
               szDevName = "797U" Or
               szDevName = "7K7U" Or
               szDevName = "2G1U" Or
               szDevName = "2G2U" Or
               szDevName = "678U" Or
               szDevName = "7K8U" Or
               szDevName = "B85U" Or
               szDevName = "2D3U" Then
            szDefJson += "{""device-name"":"""
            szDefJson += szDevName
            szDefJson += """,""source"":""Flatbed""}"
        Else
            szDefJson += "{""device-name"":"""
            szDefJson += szDevName
            szDefJson += """,""source"":""ADF-Duplex""}"
        End If
        COMBO_COMMAND.Items.Add(szDefJson)
        COMBO_COMMAND.SelectedIndex = 0
        FixCommandWidth()
    End Sub

    Private Sub BTN_ECO_Click(sender As Object, e As EventArgs) Handles BTN_ECO.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim nTime As UInteger
        Dim szCommand As String

        If COMBO_COMMAND.SelectedIndex = -1 Then
            szCommand = COMBO_COMMAND.Text
        Else
            szCommand = COMBO_COMMAND.SelectedItem.ToString()
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, Nothing, Handle)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_Setproperty Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
            Return
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_ECOControl(nTime, 0)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_ECOControl Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
        Else
            Dim formECO As New FormECO(nTime)
            formECO.ShowDialog(nTime)

            m_DeviceWrapper.m_pfnLibWFX_ECOControl(nTime, 1)
        End If

    End Sub

    Private Sub BTN_PAPERREADY_Click(sender As Object, e As EventArgs) Handles BTN_PAPERREADY.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim szCommand As String

        If COMBO_COMMAND.SelectedIndex = -1 Then
            szCommand = COMBO_COMMAND.Text
        Else
            szCommand = COMBO_COMMAND.SelectedItem.ToString()
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, Nothing, Handle)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_Setproperty Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
            Return
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_PaperReady()
        If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Paper is ready!")
        ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PAPER_NOT_READY Then
            WriteLog("Paper is NOT ready!")
        Else
            WriteLog("Status:[LibWFX_PaperReady Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
        End If
    End Sub

    Private Sub BTN_CALIBRATE_Click(sender As Object, e As EventArgs) Handles BTN_CALIBRATE.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE

        Dim szCommand As String

        If COMBO_COMMAND.SelectedIndex = -1 Then
            szCommand = COMBO_COMMAND.Text
        Else
            szCommand = COMBO_COMMAND.SelectedItem.ToString()
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, Nothing, Handle)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_Setproperty Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
            Return
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_Calibrate()
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_Calibrate Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
        Else
            WriteLog("Status:[LibWFX_Calibrate Success")
        End If
    End Sub

    Private Sub BTN_EJECT_PAPER_Click(sender As Object, e As EventArgs) Handles BTN_EJECT_PAPER.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim enDirection As ENUM_LIBWFX_EJECT_DIRECTION
        Dim szCommand As String

        If COMBO_COMMAND.SelectedIndex = -1 Then
            szCommand = COMBO_COMMAND.Text
        Else
            szCommand = COMBO_COMMAND.SelectedItem.ToString()
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, Nothing, Handle)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_Setproperty Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
            Return
        End If

        If CHK_EJECT_DIRECT.Checked = True Then
            enDirection = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_BACKWARDING
        Else
            enDirection = ENUM_LIBWFX_EJECT_DIRECTION.LIBWFX_EJECT_FORWARDING
        End If

        Dim pszErrorMsg As IntPtr
        enRet = m_DeviceWrapper.m_pfnLibWFX_EjectPaperControlWithMsg(enDirection, pszErrorMsg)
        If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SCANNING Then
            WriteLog("Status:[Scanning Fail]")
            Return
        End If

        Dim pszErrorMsg2 As String
        pszErrorMsg2 = Marshal.PtrToStringUni(pszErrorMsg)

        If pszErrorMsg2.Length > 0 Then
            WriteLog(pszErrorMsg2)
        ElseIf enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_EjectPaperControl Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
        Else
            WriteLog("Status:[LibWFX_EjectPaperControl Success")
        End If
    End Sub

    Private Sub BTN_PAPERSTATUS_Click(sender As Object, e As EventArgs) Handles BTN_PAPERSTATUS.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim enPaperStatus As ENUM_LIBWFX_EVENT_CODE
        Dim szCommand As String

        If COMBO_COMMAND.SelectedIndex = -1 Then
            szCommand = COMBO_COMMAND.Text
        Else
            szCommand = COMBO_COMMAND.SelectedItem.ToString()
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, Nothing, Handle)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_Setproperty Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
            Return
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_GetPaperStatus(enPaperStatus)

        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_GetPaperStatus Fail [" + Convert.ToDecimal(enRet).ToString() + "][" + Convert.ToDecimal(enPaperStatus).ToString() + "]]")
        Else
            WriteLog("Status:[LibWFX_GetPaperStatus Success [" + Convert.ToDecimal(enPaperStatus).ToString() + "]]")
        End If
    End Sub

    REM Private Sub BTN_CAMERACALIBRATE_Click(sender As Object, e As EventArgs)
    REM    Dim enRet As ENUM_LIBWFX_ERRCODE
    REM
    REM    enRet = DeviceWrapper.LibWFX_CameraCalibrate()
    REM    If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
    REM        WriteLog("Status:[LibWFX_CameraCalibrate() Fail [" + enRet.ToString() + "]]")
    REM    Else
    REM        WriteLog("Status:[LibWFX_CameraCalibrate() Success")
    REM    End If
    REM End Sub

    Private Function GetCommandString(ByVal szDevice As String) As Boolean
        If Not IO.Directory.Exists("C:\\ProgramData\\Plustek\\" + szDevice + "\\") Then
            IO.Directory.CreateDirectory("C:\\ProgramData\\Plustek\\" + szDevice + "\\")
        End If

        If System.IO.File.Exists("C:\\ProgramData\\Plustek\\" + szDevice + "\\Command.txt") = True Then
            Dim objStreamReader As StreamReader

            objStreamReader = New StreamReader("C:\\ProgramData\\Plustek\\" + szDevice + "\\Command.txt")
            COMBO_COMMAND.Items.Clear()
            Dim line As String = ""
            Dim idx As Integer = 0

            Do
                line = objStreamReader.ReadLine()
                If line Is Nothing And idx = 0 Then
                    objStreamReader.Close()
                    Return False
                End If
                If Not line Is Nothing Then
                    idx = idx + 1
                    COMBO_COMMAND.Items.Add(line)
                End If
            Loop Until line Is Nothing Or idx = m_MaxCMDItems

            objStreamReader.Close()
            GetCommandString = True
            FixCommandWidth()
            COMBO_COMMAND.SelectedIndex = 0
        Else
            GetCommandString = False
        End If
    End Function

    Private Function SetCommandString(ByVal szDevice As String, ByVal szCommand As String) As Boolean
        REM Confirm whether the devicename in the command is correct
        If szCommand.Contains(szDevice) <> True Then
            Return False
        End If

        If Not IO.Directory.Exists("C:\\ProgramData\\Plustek\\" + szDevice + "\\") Then
            IO.Directory.CreateDirectory("C:\\ProgramData\\Plustek\\" + szDevice + "\\")
        End If

        If System.IO.File.Exists("C:\\ProgramData\\Plustek\\" + szDevice + "\\Command.txt") = True Then
            Dim objStreamWriter As StreamWriter
            Dim commandLists As New ArrayList

            objStreamWriter = New StreamWriter("C:\\ProgramData\\Plustek\\" + szDevice + "\\Command.txt")
            commandLists.Add(szCommand)
            objStreamWriter.WriteLine(szCommand)
            COMBO_COMMAND.SelectedIndex = 0
            For idx As Integer = 0 To COMBO_COMMAND.Items.Count - 1
                If commandLists.Count = m_MaxCMDItems Or (szCommand = COMBO_COMMAND.SelectedItem.ToString() And idx = 0) Then Continue For
                COMBO_COMMAND.SelectedIndex = idx
                objStreamWriter.WriteLine(COMBO_COMMAND.SelectedItem.ToString())
                commandLists.Add(COMBO_COMMAND.SelectedItem.ToString())
            Next

            objStreamWriter.Close()
            COMBO_COMMAND.Items.Clear()
            For idx As Integer = 0 To commandLists.Count - 1
                COMBO_COMMAND.Items.Add(commandLists(idx))
            Next
            FixCommandWidth()
            COMBO_COMMAND.SelectedIndex = 0
            SetCommandString = True
        Else
            SetCommandString = False
        End If
    End Function

    Private Sub FixCommandWidth()
        Dim maxSize As Integer
        Dim selidx As Integer = COMBO_COMMAND.SelectedIndex
        Dim g As Graphics = CreateGraphics()

        For i As Integer = 0 To COMBO_COMMAND.Items.Count - 1
            COMBO_COMMAND.SelectedIndex = i
            Dim size As SizeF = g.MeasureString(COMBO_COMMAND.Text, COMBO_COMMAND.Font)
            If maxSize < size.Width Then
                maxSize = size.Width
            End If
        Next

        COMBO_COMMAND.DropDownWidth = COMBO_COMMAND.Width

        If COMBO_COMMAND.DropDownWidth < maxSize Then
            COMBO_COMMAND.DropDownWidth = maxSize
        End If

        g.Dispose()
    End Sub

    Private Sub GetCertificatePermission()
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim pstr As IntPtr
        Dim szPermission As String

        enRet = m_DeviceWrapper.m_pfnLibWFX_GetCertificatePermission(pstr, ENUM_PERMISSION_DATA_TYPE.LIBWFX_DATA_TYPE_REGINFO)

        If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            szPermission = Marshal.PtrToStringUni(pstr)
            If szPermission <> "" Then
                WriteLog("License: " + szPermission)
            Else
                WriteLog("License: none")
            End If
        Else
            WriteLog("Status:[LibWFX_GetCertificatePermission Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
        End If
    End Sub

    Private Sub BTN_EDIT_Click(sender As Object, e As EventArgs) Handles BTN_EDIT.Click
        Dim szCommand, szRtn As String
        Dim pCommandOut As IntPtr

        If COMBO_COMMAND.SelectedIndex = -1 Then
            szCommand = COMBO_COMMAND.Text
        Else
            szCommand = COMBO_COMMAND.SelectedItem.ToString()
        End If

        m_DeviceWrapper.m_pfnLibWFX_EditCommand(szCommand, pCommandOut)
        szRtn = Marshal.PtrToStringUni(pCommandOut)

        If (szRtn <> String.Empty) And (szRtn.Length > 0) Then
            Dim commandLists As New ArrayList
            commandLists.Add(szRtn)
            For idx As Integer = 0 To COMBO_COMMAND.Items.Count - 1
                If idx = (m_MaxCMDItems - 1) Then
                    Exit For
                End If
                COMBO_COMMAND.SelectedIndex = idx
                commandLists.Add(COMBO_COMMAND.SelectedItem.ToString())
            Next
            COMBO_COMMAND.Items.Clear()
            For idx As Integer = 0 To commandLists.Count - 1
                COMBO_COMMAND.Items.Add(commandLists(idx))
            Next
            COMBO_COMMAND.SelectedIndex = 0
        End If

    End Sub

    Private Sub BTN_SCAN_Click(sender As Object, e As EventArgs) Handles BTN_SCAN.Click
        Dim Command As String
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim pScanImageList As IntPtr
        Dim pOCRResultList As IntPtr
        Dim pExceptionRet As IntPtr
        Dim pEventRet As IntPtr

        If COMBO_COMMAND.SelectedIndex = -1 Then
            Command = COMBO_COMMAND.Text
        Else
            Command = COMBO_COMMAND.SelectedItem.ToString()
        End If

        If Command.IndexOf("""autoscan"":true") <> -1 Then
            MessageBox.Show("BlockScan do not support autoscan!! If you want to implement autoscan, please refer to the AutoCaptureDemo-VB.", "Warning")
            Return
        End If

        ShowScanningDlg(True)
        enRet = m_DeviceWrapper.m_pfnLibWFX_SynchronizeScan(Command, pScanImageList, pOCRResultList, pExceptionRet, pEventRet)
        ShowScanningDlg(False)

        Dim szExceptionRet As String = Marshal.PtrToStringUni(pExceptionRet)
        Dim szEventRet As String = Marshal.PtrToStringUni(pEventRet)

        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS And enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH Then
            Dim pszErrorMsg As IntPtr
            Dim pszErrorMsg2 As String
            m_DeviceWrapper.m_pfnLibWFX_GetLastErrorCode(enRet, pszErrorMsg)
            pszErrorMsg2 = Marshal.PtrToStringUni(pszErrorMsg)
            WriteLog("Status:[LibWFX_SynchronizeScan Fail [" + Convert.ToDecimal(enRet).ToString + "]]" + pszErrorMsg2.ToString)  REM get fail message
        ElseIf szEventRet.Length > 1 Then
            SetCommandString(CMB_DEVICE_LIST.SelectedItem.ToString(), COMBO_COMMAND.Text)
            WriteLog("Status:[Device Ready!]")
            WriteLog(szEventRet)  REM get event message

            If szEventRet <> "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" And szEventRet <> "LIBWFX_EVENT_UVSECURITY_DETECTED[1]" And szEventRet <> "LIBWFX_EVENT_NO_PAPER" Then
                WriteLog("Status:[Scan End]")
                Return
            End If

            Dim szScanImageList As String = Marshal.PtrToStringUni(pScanImageList)
            Dim szOCRResultList As String = Marshal.PtrToStringUni(pOCRResultList)

            REM Split String by "|&|"
            Dim myDelims As String() = New String() {"|&|"}
            Dim ScanImageArray() As String = szScanImageList.Trim().Split(myDelims, StringSplitOptions.None)
            Dim OCRResultArray() As String = szOCRResultList.Trim().Split(myDelims, StringSplitOptions.None)

            If ScanImageArray.Count > 0 Then
                For idx As Integer = 0 To ScanImageArray.Count - 1
                    WriteLog(ScanImageArray(idx).Trim())  REM get each image path

                    If (ScanImageArray(idx).Contains(".pdf") <> True And ScanImageArray(idx).Contains(".tif") <> True And ScanImageArray(idx).Equals("CustomPhotoZone") <> True And ScanImageArray(idx) <> String.Empty) Then
                        m_nCount += 1
                        If m_nCount Mod 2 = 1 Then
                            PIC_IMAGE1.Load(ScanImageArray(idx).Trim())
                        Else
                            PIC_IMAGE2.Load(ScanImageArray(idx).Trim())
                        End If
                    End If
                    If OCRResultArray(idx) <> String.Empty Then
                        WriteLog(OCRResultArray(idx).Trim())  REM get each ocr result               
                    End If
                Next
            End If
        Else
            SetCommandString(CMB_DEVICE_LIST.SelectedItem.ToString(), COMBO_COMMAND.Text)
            WriteLog("Status:[Device Ready!]")

            If szExceptionRet.Length > 1 Then
                WriteLog(szExceptionRet)  REM get exception message
            End If

            Dim szScanImageList As String = Marshal.PtrToStringUni(pScanImageList)
            Dim szOCRResultList As String = Marshal.PtrToStringUni(pOCRResultList)

            REM Split String by "|&|"
            Dim myDelims As String() = New String() {"|&|"}
            Dim ScanImageArray() As String = szScanImageList.Trim().Split(myDelims, StringSplitOptions.None)
            Dim OCRResultArray() As String = szOCRResultList.Trim().Split(myDelims, StringSplitOptions.None)

            If ScanImageArray.Count > 0 Then
                For idx As Integer = 0 To ScanImageArray.Count - 1
                    WriteLog(ScanImageArray(idx).Trim())  REM get each image path

                    If (ScanImageArray(idx).Contains(".pdf") <> True And ScanImageArray(idx).Contains(".tif") <> True And ScanImageArray(idx).Equals("CustomPhotoZone") <> True And ScanImageArray(idx) <> String.Empty) Then
                        m_nCount += 1
                        If m_nCount Mod 2 = 1 Then
                            PIC_IMAGE1.Load(ScanImageArray(idx).Trim())
                        Else
                            PIC_IMAGE2.Load(ScanImageArray(idx).Trim())
                        End If
                    End If
                    If OCRResultArray(idx) <> String.Empty Then
                        WriteLog(OCRResultArray(idx).Trim())  REM get each ocr result               
                    End If
                Next
            End If
        End If

        If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH Then
            WriteLog("Status:[There are some mismatched key in command]")
        End If
        WriteLog("Status:[Scan End]")
    End Sub

    Private Sub ShowScanningDlg(ByVal enableDlg As Boolean)
        If enableDlg Then
            m_scanningForm.Show()
            m_scanningForm.Refresh()
            Me.Hide()
        Else
            m_scanningForm.Hide()
            Me.Show()
        End If
    End Sub

    Private Sub BTN_RECYCLESAVEFOLDER_Click(sender As Object, e As EventArgs) Handles BTN_RECYCLESAVEFOLDER.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE
        enRet = m_DeviceWrapper.m_pfnLibWFX_RecycleSaveFolder()
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_RecycleSaveFolder Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
        Else
            WriteLog("Status:[LibWFX_RecycleSaveFolder Success]")
        End If
    End Sub

    Private Sub BTN_MERGEPDF_Click(sender As Object, e As EventArgs) Handles BTN_MERGEPDF.Click
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim OpenFileDialog1 As OpenFileDialog = New OpenFileDialog
        OpenFileDialog1.CheckFileExists = True
        OpenFileDialog1.Filter = "Images (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG|" + "All files (*.*)|*.*"
        OpenFileDialog1.Multiselect = True
        OpenFileDialog1.ShowDialog()

        Dim szFileList As String
        For Each item As String In OpenFileDialog1.FileNames
            szFileList += item
            szFileList += "*"
        Next


        REM Dim FilePath As String = OpenFileDialog1.FileName
        Dim szCommand As String

        If COMBO_COMMAND.SelectedIndex = -1 Then
            szCommand = COMBO_COMMAND.Text
        Else
            szCommand = COMBO_COMMAND.SelectedItem.ToString()
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_SetProperty(szCommand, Nothing, Handle)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_Setproperty Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
            Return
        End If

        enRet = m_DeviceWrapper.m_pfnLibWFX_MergeToPdf(szFileList)
        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            WriteLog("Status:[LibWFX_MergePdf Fail [" + Convert.ToDecimal(enRet).ToString() + "]]")
        Else
            WriteLog("Status:[LibWFX_MergePdf Success]")
        End If
    End Sub
End Class
