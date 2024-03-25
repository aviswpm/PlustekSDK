Imports AutoCaptureDemo_VB.DeviceWrapper
Imports System.Runtime.InteropServices
Imports System.Threading


Module AutoCaptureDemo
    Dim WithEvents Timer As Timers.Timer = New Timers.Timer

    Dim interval As Integer = 1000  REM 1 seconds
    Dim totalTime As Integer = 60 * 1000 REM 60 seconds
    Dim elapsedTime As Integer = 0 REM Elapsed time In ms

    Sub Main(args As String())
        REM The window will automatically close after 60 seconds then do deinit
        Timer.Interval = 1000
        Timer.Enabled = True
        Timer.Start()

        REM do autocapture
        Dim ThreadBegin As New System.Threading.Thread(AddressOf AutoCapture)
        ThreadBegin.Start()
    End Sub

    Private Sub AutoCapture()
        Dim DoScan As Boolean = False
        Dim enRet As ENUM_LIBWFX_ERRCODE
        Dim pScanImageList As IntPtr
        Dim pOCRResultList As IntPtr
        Dim pExceptionRet As IntPtr
        Dim pEventRet As IntPtr
        Dim m_CBEvent As DeviceWrapper.LIBWFXEVENTCB
        Dim timer As Integer = 0
        Dim sum As Integer = 0
        Dim szCommand = "{""device-name"":""A64"",""source"":""Camera"",""recognize-type"":""passport""}"

        enRet = DeviceWrapper.LibWFX_InitEx(ENUM_LIBWFX_INIT_MODE.LIBWFX_INIT_MODE_NORMAL)

        If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_OCR Then
            Console.WriteLine("Status:[No Recognize Tool]")
        ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_AVI_OCR Then
            Console.WriteLine("Status:[No AVI Recognize Tool]")
        ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_NO_DOC_OCR Then
            Console.WriteLine("Status:[No DOC Recognize Tool]")
        ElseIf enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_PATH_TOO_LONG Then
            Console.WriteLine("Status:[Path Is Too Long (max limit: 130 bits)]")
            Console.WriteLine("Status:[LibWFX_InitEx Fail]")
        ElseIf enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            Console.WriteLine("Status:[LibWFX_InitEx Fail [" + Convert.ToDecimal(enRet).ToString + "]]")
            Return
        End If

        REM get command from bat file "AutoCaptureDemo-VB.bat"
        Dim arguments() As String = Microsoft.VisualBasic.Command().Split(" ")
        If arguments(0).Length > 1 Then
            szCommand = arguments(0)
        End If

        enRet = DeviceWrapper.LibWFX_SetProperty(szCommand, m_CBEvent, IntPtr.Zero)

        If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
            Dim pszErrorMsg As IntPtr
            Dim pszErrorMsg2 As String
            DeviceWrapper.LibWFX_GetLastErrorCode(enRet, pszErrorMsg)
            pszErrorMsg2 = Marshal.PtrToStringUni(pszErrorMsg)
            Console.WriteLine("Status:[LibWFX_SetProperty Fail [" + Convert.ToDecimal(enRet).ToString + "]]" + pszErrorMsg2.ToString)  REM get fail message
        End If

        While 1
            sum = 0
            timer = 0
            While timer < 3
                sum += 1
                Threading.Thread.Sleep(100)
                enRet = DeviceWrapper.LibWFX_PaperReady()
                If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS Then
                    timer += 1
                End If

                If sum = 4 Then
                    sum = 0
                    timer = 0
                    DoScan = False
                    Console.WriteLine("Please put the card.")
                    Threading.Thread.Sleep(1000)  REM option
                End If
            End While

            If DoScan Then
                Console.WriteLine("The card is continuously detected, please remove the card.")
                Threading.Thread.Sleep(1000)  REM  option
                Continue While
            End If

            enRet = DeviceWrapper.LibWFX_SynchronizeScan(szCommand, pScanImageList, pOCRResultList, pExceptionRet, pEventRet)

            Dim szExceptionRet As String = Marshal.PtrToStringUni(pExceptionRet)
            Dim szEventRet As String = Marshal.PtrToStringUni(pEventRet)

            If enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_SUCCESS And enRet <> ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH Then
                Dim pszErrorMsg As IntPtr
                Dim pszErrorMsg2 As String
                DeviceWrapper.LibWFX_GetLastErrorCode(enRet, pszErrorMsg)
                pszErrorMsg2 = Marshal.PtrToStringUni(pszErrorMsg)
                Console.WriteLine("Status:[LibWFX_SynchronizeScan Fail [" + Convert.ToDecimal(enRet).ToString + "]]" + pszErrorMsg2.ToString)  REM get fail message            
            ElseIf szEventRet.Length > 1 Then
                Console.WriteLine("Status:[Device Ready!]")
                Console.WriteLine(szEventRet)  REM get event message

                If szEventRet <> "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" And szEventRet <> "LIBWFX_EVENT_UVSECURITY_DETECTED[1]" Then
                    Console.WriteLine("Status:[Scan End]")
                    Return
                End If

                Dim szScanImageList As String = Marshal.PtrToStringUni(pScanImageList)
                Dim szOCRResultList As String = Marshal.PtrToStringUni(pOCRResultList)

                Dim ScanImageArray() As String = szScanImageList.TrimEnd("|").TrimEnd("&").TrimEnd("|").Split("|&|")
                Dim OCRResultArray() As String = szOCRResultList.TrimEnd("|").TrimEnd("&").TrimEnd("|").Split("|&|")

                If ScanImageArray.Count > 0 Then
                    For idx As Integer = 0 To ScanImageArray.Count - 1
                        Console.WriteLine(ScanImageArray(idx).Trim().Trim("|&|"))  REM get each image path
                        If OCRResultArray(idx) <> String.Empty Then
                            Console.WriteLine(OCRResultArray(idx).Trim().Trim("|&|"))  REM get each ocr result
                        End If
                    Next
                End If
            Else
                Console.WriteLine("Status:[Device Ready!]")

                If szExceptionRet.Length > 1 Then
                        Console.WriteLine("Status:[Device Ready!]")
                        Console.WriteLine(szExceptionRet)  REM get exception message
                    End If

                Dim szScanImageList As String = Marshal.PtrToStringUni(pScanImageList)
                Dim szOCRResultList As String = Marshal.PtrToStringUni(pOCRResultList)

                Dim ScanImageArray() As String = szScanImageList.TrimEnd("|").TrimEnd("&").TrimEnd("|").Split("|&|")
                Dim OCRResultArray() As String = szOCRResultList.TrimEnd("|").TrimEnd("&").TrimEnd("|").Split("|&|")

                If ScanImageArray.Count > 0 Then
                    For idx As Integer = 0 To ScanImageArray.Count - 1
                        Console.WriteLine(ScanImageArray(idx).Trim().Trim("|&|"))  REM get each image path
                        If OCRResultArray(idx) <> String.Empty Then
                            Console.WriteLine(OCRResultArray(idx).Trim().Trim("|&|"))  REM get each ocr result
                        End If
                    Next
                End If
            End If

            If enRet = ENUM_LIBWFX_ERRCODE.LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH Then
                Console.WriteLine("Status:[There are some mismatched key in command]")
            End If
            Console.WriteLine("Status:[Scan End]")
            DoScan = True
        End While
    End Sub

    Sub Timer_Timer(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles Timer.Elapsed
        If elapsedTime > totalTime Then
            REM Execute deinit when the window is closed
            DeviceWrapper.LibWFX_CloseDevice()
            DeviceWrapper.LibWFX_DeInit()
            Environment.Exit(0)
        End If
        elapsedTime += interval
    End Sub
End Module
