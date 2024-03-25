Option Explicit On
Imports System.Runtime.InteropServices

Public Class DeviceWrapper
    Public Const LIBWFX_DLLNAME As String = "LibWebFXScan.dll"

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

    <StructLayout(LayoutKind.Sequential)>
    Public Structure ST_IMAGE_INFO
        Public enColorMode As ENUM_LIBWFX_COLOR_MODE
        Public ulPixel As UInteger
        Public ulPerLawByte As UInteger
        Public ulLine As UInteger
        Public pRawDate As IntPtr
    End Structure

    Public Enum ENUM_PERMISSION_DATA_TYPE
        LIBWFX_DATA_TYPE_PERMISSION = 0
        LIBWFX_DATA_TYPE_REGINFO = 1
    End Enum

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Public Delegate Sub LIBWFXEVENTCB(ByVal enEventCode As ENUM_LIBWFX_EVENT_CODE, ByVal nParam As Integer, ByVal pUserDef As IntPtr)

    <UnmanagedFunctionPointer(CallingConvention.Cdecl)>
    Public Delegate Sub LIBWFXCB(ByVal enNotifyCode As ENUM_LIBWFX_NOTIFY_CODE, ByVal pUserDef As IntPtr, ByVal pParam1 As IntPtr, ByVal pParam2 As IntPtr)

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_Init", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_Init() As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_InitEx", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_InitEx(ByVal enInitMode As ENUM_LIBWFX_INIT_MODE) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_DeInit", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_DeInit() As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_GetDeviesList", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_GetDeviesList(ByRef szDevicesListOut As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_GetDeviesListWithSerial", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_GetDeviesListWithSerial(ByRef szDevicesListOut As IntPtr, ByRef szSerialListOut As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_GetFileList", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_GetFileList(ByRef szFileListOut As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, CharSet:=CharSet.Unicode, EntryPoint:="LibWFX_RemoveFile", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_RemoveFile(ByVal szFileNameIn As String) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, CharSet:=CharSet.Unicode, EntryPoint:="LibWFX_SetProperty", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_SetProperty(ByVal szRequestCmdIn As String, ByVal pfnLibWFXEVENTCBIn As LIBWFXEVENTCB, pUserDefIn As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_StartScan", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_StartScan(ByVal pfnLibWFXCBIn As LIBWFXCB, pUserDefIn As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_Calibrate", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_Calibrate() As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_ECOControl", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_ECOControl(ByRef pulTime As UInteger, ByVal nSetIn As Integer) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_PaperReady", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_PaperReady() As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_CloseDevice", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_CloseDevice() As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_EjectPaperControl", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_EjectPaperControl(ByVal enEjectDirectIn As ENUM_LIBWFX_EJECT_DIRECTION) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_EjectPaperControlWithMsg", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_EjectPaperControlWithMsg(ByVal enEjectDirectIn As ENUM_LIBWFX_EJECT_DIRECTION, ByRef szErrorMsg As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_GetPaperStatus", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_GetPaperStatus(ByRef penStatusOut As ENUM_LIBWFX_EVENT_CODE) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, CharSet:=CharSet.Unicode, EntryPoint:="LibWFX_ReadImage", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_ReadImage(ByVal szFilePathIn As String, ByVal pfnLibWFXCBIn As LIBWFXCB, pUserDefIn As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, CharSet:=CharSet.Unicode, EntryPoint:="LibWFX_IsWindowExist", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_IsWindowExist(ByVal szWindowNameIn As String) As Boolean
    End Function

    REM <DllImportAttribute(LIBWFX_DLLNAME, CharSet:=CharSet.Unicode, EntryPoint:="LibWFX_IsOCRVerMatch", CallingConvention:=CallingConvention.StdCall)>
    REM Public Shared Function LibWFX_IsOCRVerMatch() As Boolean
    REM End Function

    <DllImportAttribute(LIBWFX_DLLNAME, EntryPoint:="LibWFX_GetLastErrorCode", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_GetLastErrorCode(ByVal enErrorCode As ENUM_LIBWFX_ERRCODE, ByRef szErrorMsg As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, CharSet:=CharSet.Unicode, EntryPoint:="LibWFX_SynchronizeScan", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_SynchronizeScan(ByVal szRequestCmdIn As String, ByRef pScanImageList As IntPtr, ByRef pOCRResultList As IntPtr, ByRef pExceptionRet As IntPtr, ByRef pEventRet As IntPtr) As ENUM_LIBWFX_ERRCODE
    End Function

    <DllImportAttribute(LIBWFX_DLLNAME, CharSet:=CharSet.Unicode, EntryPoint:="LibWFX_GetCertificatePermission", CallingConvention:=CallingConvention.StdCall)>
    Public Shared Function LibWFX_GetCertificatePermission(ByRef szPermissionTypeList As IntPtr, ByVal enDataType As ENUM_PERMISSION_DATA_TYPE) As ENUM_LIBWFX_ERRCODE
    End Function
End Class