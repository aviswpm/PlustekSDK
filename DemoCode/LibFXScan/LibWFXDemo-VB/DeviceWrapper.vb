Option Explicit On
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.Reflection


Public Class DeviceWrapper
    Implements IDisposable
    Dim hLibModule As IntPtr
    Dim hCommandModule As IntPtr
    Public Const LIBWFX_DLLNAME As String = "LibWebFXScan.dll"
    Public Const COMMANDEDITOR_DLLNAME As String = "CommandEditor.dll"

    Public Enum ENUM_LIBWFX_ERRCODE
        REM LIBWFX_ERRCODE_SUCCESS = 0
        REM LIBWFX_ERRCODE_FAIL = 1
        REM LIBWFX_ERRCODE_NO_INIT = 2
        REM LIBWFX_ERRCODE_NO_AVI_OCR = 3
        REM LIBWFX_ERRCODE_NO_DOC_OCR = 4
        REM LIBWFX_ERRCODE_NO_OCR = 5
        REM LIBWFX_ERRCODE_NO_DEVICES = 6
        REM LIBWFX_ERRCODE_FORMAT_ERROR = 7
        REM LIBWFX_ERRCODE_NO_DEVICE_NAME = 8
        REM LIBWFX_ERRCODE_NO_SOURCE = 9
        REM LIBWFX_ERRCODE_FILE_NO_EXIST = 10
        REM LIBWFX_ERRCODE_PAPER_NOT_READY = 11
        REM LIBWFX_ERRCODE_INVALID_SERIALNUM = 12

        LIBWFX_ERRCODE_SUCCESS = 0                      REM *< The function is performed successfully
        LIBWFX_ERRCODE_FAIL = 1                         REM *< The function is failed
        LIBWFX_ERRCODE_NO_INIT = 2                      REM *< Not do AVISP_INIT
        LIBWFX_ERRCODE_NOT_YET_OPEN_DEVICE = 3          REM *< Not do AVISP_OPEN_DEVICE 
        LIBWFX_ERRCODE_DEVICE_ALREADY_OPEN = 4          REM *< The device has opened by AVISP_OPEN_DEVICE
        LIBWFX_ERRCODE_INVALID_SOURCE = 5               REM *< Input invalid source
        LIBWFX_ERRCODE_NO_ENABLE_THRESHOLD = 6          REM *< In BW mode, the threshold config does not enalbe
        LIBWFX_ERRCODE_NO_SUPPORT_THRESHOLD = 7         REM *< In Auto mode, the threshold not support
        LIBWFX_ERRCODE_NOT_YET_SET_SCAN_PROPERTY = 8    REM *< Not yet set scan property
        LIBWFX_ERRCODE_NO_SET_RECOGNIZE_TOOL = 9        REM *< Not yet set Recognize tool
        LIBWFX_ERRCODE_OCR_NOT_SUPPORT_BOTTOMUP = 10    REM *< OCR can't recognize bottom-up source
        LIBWFX_ERRCODE_READ_IMAGE_FAILED = 11           REM *< Reading Image file Failed
        LIBWFX_ERRCODE_ONLY_SUPPORT_COLOR_MODE = 12     REM *< Only support color mode
        LIBWFX_ERRCODE_ICM_PROFILE_NOT_EXIST = 13       REM *< Icm Profile is not exist
        LIBWFX_ERRCODE_NO_SUPPORT_EJECT = 14            REM *< No support eject direction control
        LIBWFX_ERRCODE_NO_SUPPORT_JPEGXFER = 15         REM *< No support jpeg output form source
        LIBWFX_ERRCODE_PAPER_NOT_READY = 16             REM *< No paper
        LIBWFX_ERRCODE_INVALID_SERIALNUM = 17           REM *< The Serial number is invailid
        LIBWFX_ERRCODE_DISCONNECT = 18                  REM *< The internet has problem in Remote mode
        LIBWFX_ERRCODE_FORMAT_NOT_SUPPORT = 19          REM *< The recognizetextoupt format is not supported
        LIBWFX_ERRCODE_NO_CALIBRATION_DATA = 20         REM *< Not yet calibration
        LIBWFX_ERRCODE_OCR_TOOL_NOT_SUPPORT = 21        REM *< No support OCR tool
        LIBWFX_ERRCODE_RECOGNIZE_TYPE_NOT_SUPPORT = 22  REM *< No support table recognize
        LIBWFX_ERRCODE_INVALID_CERTIFICATE = 23         REM *< The Certificate or Recognize Type is invailid
        LIBWFX_ERRCODE_AP_ALREADY_EXISIT = 24           REM *< Ap has already exisited
        LIBWFX_ERRCODE_OPEN_REGISTRY_KEY_FAILED = 25    REM *< Open registry key failed
        LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL = 26          REM *< Load MRTD process failed
        LIBWFX_ERRCODE_COVER_OPENED = 27                REM *< Device Cover Opened
        LIBWFX_ERRCODE_CERTIFICATE_EXPIRED = 28         REM *< certificate expired
        LIBWFX_ERRCODE_ALREADY_INIT = 29                REM *< Already init
        LIBWFX_ERRCODE_NO_SUPPORT_DUPLEX = 30           REM *< No support duplex form source
        LIBWFX_ERRCODE_NO_AVI_OCR = 1001                REM *< AVIOCR is not installed
        LIBWFX_ERRCODE_NO_DOC_OCR = 1002                REM *< DOCOCR is not installed
        LIBWFX_ERRCODE_NO_OCR = 1003                    REM *< AVIOCR & DOCOCR are not installed
        LIBWFX_ERRCODE_NO_DEVICES = 1004                REM *< No device detected
        LIBWFX_ERRCODE_NO_DEVICE_NAME = 1005            REM *< Command has no device-name field
        LIBWFX_ERRCODE_NO_SOURCE = 1006                 REM *< Command has no source field
        LIBWFX_ERRCODE_FILE_NO_EXIST = 1007             REM *< When the RemoveFile is executed, the file does not exist
        LIBWFX_ERRCODE_PATH_TOO_LONG = 1008             REM *< Execution file address is too long
        LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH = 1009      REM *< There is a unsatisfied type in the command
        LIBWFX_ERRCODE_SCANNING = 1010                  REM *< The scanning process Is Not over yet
        LIBWFX_ERRCODE_FILE_OCCUPIED = 1011             REM *< When the RecycleSaveFolder Is executed, the file Or folder Is occupied
        LIBWFX_ERRCODE_SAVEPATH_ERROR = 1012            REM *< When the RecycleSaveFolder Is executed, the save path format Error
        LIBWFX_ERRCODE_TIMEOUT = 1013                   REM *< Timeout Error
        LIBWFX_ERRCODE_SERVER_OCCUPIED = 1014           REM *< Server has been occupied by other connections
    End Enum

    Public Enum ENUM_LIBWFX_EVENT_CODE
        LIBWFX_EVENT_PAPER_DETECTED = 0
        LIBWFX_EVENT_NO_PAPER = 1
        LIBWFX_EVENT_PAPER_JAM = 2
        LIBWFX_EVENT_MULTIFEED = 3
        LIBWFX_EVENT_NO_CALIBRATION_DATA = 4
        LIBWFX_EVENT_WARMUP_COUNTDOWN = 5
        LIBWFX_EVENT_SCAN_PROGRESS = 6
        LIBWFX_EVENT_BUTTON_DETECTED = 7
        LIBWFX_EVENT_SCANNING = 8
        LIBWFX_EVENT_PAPER_FEEDING_ERROR = 9
        LIBWFX_EVENT_COVER_OPEN = 10
        LIBWFX_EVENT_LEFT_SENSOR_DETECTED = 11
        LIBWFX_EVENT_RIGHT_SENSOR_DETECTED = 12
        LIBWFX_EVENT_ALL_SENSOR_DETECTED = 13
        LIBWFX_EVENT_UVSECURITY_DETECTED = 14
        LIBWFX_EVENT_PLUG_UNPLUG = 15
        LIBWFX_EVENT_OVER_TIME_SCAN = 16
        LIBWFX_EVENT_CANCEL_SCAN = 17
        LIBWFX_EVENT_CAMERA_RGB_DISLOCATION = 18
        LIBWFX_EVENT_CAMERA_TIMEOUT = 19
    End Enum

    Public Enum ENUM_LIBWFX_EXCEPTION_CODE
        LIBWFX_EXC_OTHER = 0
        LIBWFX_EXC_TIFF_SAVE_FINSIHED = 1
        LIBWFX_EXC_PDF_SAVE_FINSIHED = 2
        LIBWFX_EXC_IP_EXCEPTION = 3
    End Enum

    Public Enum ENUM_LIBWFX_NOTIFY_CODE
        LIBWFX_NOTIFY_IMAGE_DONE = 0
        LIBWFX_NOTIFY_END = 1
        LIBWFX_NOTIFY_EXCEPTION = 2
        LIBWFX_NOTIFY_SHOWPATHONLY = 3
    End Enum

    Public Enum ENUM_LIBWFX_EJECT_DIRECTION
        LIBWFX_EJECT_FORWARDING = 1
        LIBWFX_EJECT_BACKWARDING = 2
    End Enum

    Public Enum ENUM_LIBWFX_COLOR_MODE
        LIBWFX_COLOR_MODE_BW = 0
        LIBWFX_COLOR_MODE_GRAY = 1
        LIBWFX_COLOR_MODE_COLOR = 2
    End Enum

    Public Enum ENUM_LIBWFX_INIT_MODE
        LIBWFX_INIT_MODE_NORMAL = 0
        LIBWFX_INIT_MODE_NOOCR = 1
    End Enum

    Public Enum ENUM_PERMISSION_DATA_TYPE
        LIBWFX_DATA_TYPE_PERMISSION = 0
        LIBWFX_DATA_TYPE_REGINFO = 1
    End Enum

    <StructLayout(LayoutKind.Sequential)>
    Public Structure ST_IMAGE_INFO
        Public enColorMode As ENUM_LIBWFX_COLOR_MODE
        Public ulPixel As UInteger
        Public ulPerLawByte As UInteger
        Public ulLine As UInteger
        Public pRawDate As IntPtr
    End Structure

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Public Delegate Sub LIBWFXEVENTCB(ByVal enEventCode As ENUM_LIBWFX_EVENT_CODE, ByVal nParam As Integer, ByVal pUserDef As IntPtr)

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Public Delegate Sub LIBWFXCB(ByVal enNotifyCode As ENUM_LIBWFX_NOTIFY_CODE, ByVal pUserDef As IntPtr, ByVal pParam1 As IntPtr, ByVal pParam2 As IntPtr)


    <DllImport("kernel32.dll")>
    Public Shared Function LoadLibrary(<MarshalAs(UnmanagedType.LPStr)> ByVal lpLibFileName As String) As IntPtr
    End Function
    'Notice a very important detail here, which is, we MUST marshal lpProcName as
    'an ANSI String. There is no wide version of GetProcAddress for some reason.
    <DllImport("kernel32.dll")>
    Public Shared Function GetProcAddress(ByVal hModule As IntPtr, <MarshalAs(UnmanagedType.LPStr)> ByVal lpProcName As String) As IntPtr
    End Function
    <DllImport("Kernel32.dll")>
    Public Shared Function FreeLibrary(ByVal hLibModule As IntPtr) As Boolean
    End Function

    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_Init() As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_InitEx(ByVal enInitMode As ENUM_LIBWFX_INIT_MODE) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_DeInit() As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_GetDeviesList(ByRef szDevicesListOut As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_GetDeviesListWithSerial(ByRef szDevicesListOut As IntPtr, ByRef szSerialListOut As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_SetProperty(ByVal szRequestCmdIn As String, ByVal pfnLibWFXEVENTCBIn As LIBWFXEVENTCB, pUserDefIn As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_StartScan(ByVal pfnLibWFXCBIn As LIBWFXCB, pUserDefIn As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_SynchronizeScan(ByVal szRequestCmdIn As String, ByRef pScanImageList As IntPtr, ByRef pOCRResultList As IntPtr, ByRef pExceptionRet As IntPtr, ByRef pEventRet As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_Calibrate() As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_ECOControl(ByRef pulTime As UInteger, ByVal nSetIn As Integer) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_PaperReady() As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_CloseDevice() As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_EjectPaperControl(ByVal enEjectDirectIn As ENUM_LIBWFX_EJECT_DIRECTION) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_EjectPaperControlWithMsg(ByVal enEjectDirectIn As ENUM_LIBWFX_EJECT_DIRECTION, ByRef szErrorMsg As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_GetPaperStatus(ByRef penStatusOut As ENUM_LIBWFX_EVENT_CODE) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_MergeToPdf(ByVal szFileListIn As String) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_IsWindowExist(ByVal szWindowNameIn As String) As Boolean
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_GetLastErrorCode(ByVal enErrorCode As ENUM_LIBWFX_ERRCODE, ByRef szErrorMsg As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Sub EditCommand(ByVal szCommand As String, ByRef pCommandOut As IntPtr)
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_GetCertificatePermission(ByRef szPermissionTypeList As IntPtr, ByVal enDataType As ENUM_PERMISSION_DATA_TYPE) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_RecycleSaveFolder() As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_AsynchronizeReadImage(ByVal szFilePathIn As String, ByVal pfnLibWFXCBIn As LIBWFXCB, pUserDefIn As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_SynchronizeReadImage(ByVal szRequestCmdIn As String, ByVal szFilePathIn As String, ByRef szScanImageList As IntPtr, ByRef szOCRResultList As IntPtr, ByRef szExceptionRet As IntPtr) As ENUM_LIBWFX_ERRCODE
    <UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet:=CharSet.Unicode)>
    Public Delegate Function LibWFX_WriteAPLog(ByVal szMsg As String) As ENUM_LIBWFX_ERRCODE

    Public m_pfnLibWFX_Init As LibWFX_Init
    Public m_pfnLibWFX_InitEx As LibWFX_InitEx
    Public m_pfnLibWFX_DeInit As LibWFX_DeInit
    Public m_pfnLibWFX_GetDeviesList As LibWFX_GetDeviesList
    Public m_pfnLibWFX_GetDeviesListWithSerial As LibWFX_GetDeviesListWithSerial
    Public m_pfnLibWFX_SetProperty As LibWFX_SetProperty
    Public m_pfnLibWFX_StartScan As LibWFX_StartScan
    Public m_pfnLibWFX_Calibrate As LibWFX_Calibrate
    Public m_pfnLibWFX_ECOControl As LibWFX_ECOControl
    Public m_pfnLibWFX_PaperReady As LibWFX_PaperReady
    Public m_pfnLibWFX_CloseDevice As LibWFX_CloseDevice
    Public m_pfnLibWFX_EjectPaperControl As LibWFX_EjectPaperControl
    Public m_pfnLibWFX_EjectPaperControlWithMsg As LibWFX_EjectPaperControlWithMsg
    Public m_pfnLibWFX_GetPaperStatus As LibWFX_GetPaperStatus
    Public m_pfnLibWFX_MergeToPdf As LibWFX_MergeToPdf
    Public m_pfnLibWFX_IsWindowExist As LibWFX_IsWindowExist
    Public m_pfnLibWFX_GetLastErrorCode As LibWFX_GetLastErrorCode
    Public m_pfnLibWFX_SynchronizeScan As LibWFX_SynchronizeScan
    Public m_pfnLibWFX_EditCommand As EditCommand
    Public m_pfnLibWFX_GetCertificatePermission As LibWFX_GetCertificatePermission
    Public m_pfnLibWFX_RecycleSaveFolder As LibWFX_RecycleSaveFolder
    Public m_pfnLibWFX_WriteAPLog As LibWFX_WriteAPLog

    Private Const LOAD_LIBRARY_SEARCH_USER_DIRS As Integer = &H400
    Private Const LOAD_LIBRARY_SEARCH_SYSTEM32 As Integer = &H800

    <DllImportAttribute("kernel32.dll", EntryPoint:="SetDefaultDllDirectories", CallingConvention:=CallingConvention.StdCall)>
    Private Shared Function SetDefaultDllDirectories(ByVal directoryFlags As Integer) As Boolean
    End Function

    Public Sub New()

        Dim szPath As String = Assembly.GetExecutingAssembly().Location.ToString()
        szPath = szPath.Substring(0, szPath.LastIndexOf("\"))
        Dim szLibDLLPath As String = szPath + "\" + LIBWFX_DLLNAME
        Dim szCommandDLLPath As String = szPath + "\" + COMMANDEDITOR_DLLNAME

        Dim result2 As Boolean = SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_SYSTEM32 Or LOAD_LIBRARY_SEARCH_USER_DIRS)
        If Not result2 Then
            Return
        End If

        hLibModule = LoadLibrary(szLibDLLPath)
        hCommandModule = LoadLibrary(szCommandDLLPath)

        If hLibModule = IntPtr.Zero Or hCommandModule = IntPtr.Zero Then
            Dim keyName As String
#If WIN32 Then
        keyName = "HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"
#Else
            keyName = "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"
#End If
            Dim value As Object = Registry.GetValue(keyName, "InstallLocation", "")

            If value IsNot Nothing Then
                szPath = value.ToString()
                System.IO.Directory.SetCurrentDirectory(value.ToString())

                If (szPath.LastIndexOf("\") <> (szPath.Length - 1)) Then
                    szPath += "\"
                End If
                szLibDLLPath = szPath + LIBWFX_DLLNAME
                szCommandDLLPath = szPath + COMMANDEDITOR_DLLNAME

                If (hLibModule = IntPtr.Zero) Then
                    hLibModule = LoadLibrary(szLibDLLPath)
                End If
                If (hCommandModule = IntPtr.Zero) Then
                    hCommandModule = LoadLibrary(szCommandDLLPath)
                End If
            End If
        End If

        If hLibModule <> IntPtr.Zero And hCommandModule <> IntPtr.Zero Then
            Dim pFun As IntPtr = GetProcAddress(hLibModule, "LibWFX_Init")
            m_pfnLibWFX_Init = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_Init)), LibWFX_Init)

            pFun = GetProcAddress(hLibModule, "LibWFX_InitEx")
            m_pfnLibWFX_InitEx = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_InitEx)), LibWFX_InitEx)

            pFun = GetProcAddress(hLibModule, "LibWFX_DeInit")
            m_pfnLibWFX_DeInit = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_DeInit)), LibWFX_DeInit)

            pFun = GetProcAddress(hLibModule, "LibWFX_GetDeviesList")
            m_pfnLibWFX_GetDeviesList = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_GetDeviesList)), LibWFX_GetDeviesList)

            pFun = GetProcAddress(hLibModule, "LibWFX_GetDeviesListWithSerial")
            m_pfnLibWFX_GetDeviesListWithSerial = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_GetDeviesListWithSerial)), LibWFX_GetDeviesListWithSerial)

            pFun = GetProcAddress(hLibModule, "LibWFX_SetProperty")
            m_pfnLibWFX_SetProperty = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_SetProperty)), LibWFX_SetProperty)

            pFun = GetProcAddress(hLibModule, "LibWFX_StartScan")
            m_pfnLibWFX_StartScan = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_StartScan)), LibWFX_StartScan)

            pFun = GetProcAddress(hLibModule, "LibWFX_Calibrate")
            m_pfnLibWFX_Calibrate = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_Calibrate)), LibWFX_Calibrate)

            pFun = GetProcAddress(hLibModule, "LibWFX_ECOControl")
            m_pfnLibWFX_ECOControl = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_ECOControl)), LibWFX_ECOControl)

            pFun = GetProcAddress(hLibModule, "LibWFX_PaperReady")
            m_pfnLibWFX_PaperReady = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_PaperReady)), LibWFX_PaperReady)

            pFun = GetProcAddress(hLibModule, "LibWFX_CloseDevice")
            m_pfnLibWFX_CloseDevice = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_CloseDevice)), LibWFX_CloseDevice)

            pFun = GetProcAddress(hLibModule, "LibWFX_EjectPaperControl")
            m_pfnLibWFX_EjectPaperControl = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_EjectPaperControl)), LibWFX_EjectPaperControl)

            pFun = GetProcAddress(hLibModule, "LibWFX_EjectPaperControlWithMsg")
            m_pfnLibWFX_EjectPaperControlWithMsg = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_EjectPaperControlWithMsg)), LibWFX_EjectPaperControlWithMsg)

            pFun = GetProcAddress(hLibModule, "LibWFX_GetPaperStatus")
            m_pfnLibWFX_GetPaperStatus = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_GetPaperStatus)), LibWFX_GetPaperStatus)

            pFun = GetProcAddress(hLibModule, "LibWFX_MergeToPdf")
            m_pfnLibWFX_MergeToPdf = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_MergeToPdf)), LibWFX_MergeToPdf)

            pFun = GetProcAddress(hLibModule, "LibWFX_IsWindowExist")
            m_pfnLibWFX_IsWindowExist = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_IsWindowExist)), LibWFX_IsWindowExist)

            pFun = GetProcAddress(hLibModule, "LibWFX_GetLastErrorCode")
            m_pfnLibWFX_GetLastErrorCode = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_GetLastErrorCode)), LibWFX_GetLastErrorCode)

            pFun = GetProcAddress(hLibModule, "LibWFX_SynchronizeScan")
            m_pfnLibWFX_SynchronizeScan = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_SynchronizeScan)), LibWFX_SynchronizeScan)

            pFun = GetProcAddress(hCommandModule, "EditCommand")
            Dim m_pfnLibWFX_EditCommand As EditCommand = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(EditCommand)), EditCommand)

            pFun = GetProcAddress(hLibModule, "LibWFX_GetCertificatePermission")
            m_pfnLibWFX_GetCertificatePermission = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_GetCertificatePermission)), LibWFX_GetCertificatePermission)

            pFun = GetProcAddress(hLibModule, "LibWFX_RecycleSaveFolder")
            m_pfnLibWFX_RecycleSaveFolder = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_RecycleSaveFolder)), LibWFX_RecycleSaveFolder)

            pFun = GetProcAddress(hLibModule, "LibWFX_WriteAPLog")
            m_pfnLibWFX_WriteAPLog = DirectCast(Marshal.GetDelegateForFunctionPointer(pFun, GetType(LibWFX_WriteAPLog)), LibWFX_WriteAPLog)

        Else
            Console.WriteLine("Load library fail!")

        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        If (hLibModule <> IntPtr.Zero) Then
            FreeLibrary(hLibModule)
        End If
        If (hCommandModule <> IntPtr.Zero) Then
            FreeLibrary(hCommandModule)
        End If
    End Sub
End Class

