using System;
using System.Runtime.InteropServices;

public enum ENUM_LIBWFX_ERRCODE
{
    //LIBWFX_ERRCODE_SUCCESS = 0,
    //LIBWFX_ERRCODE_FAIL,       
    //LIBWFX_ERRCODE_NO_INIT,
    //LIBWFX_ERRCODE_NO_AVI_OCR,
    //LIBWFX_ERRCODE_NO_DOC_OCR,
    //LIBWFX_ERRCODE_NO_OCR,
    //LIBWFX_ERRCODE_NO_DEVICES,
    //LIBWFX_ERRCODE_FORMAT_ERROR,
    //LIBWFX_ERRCODE_NO_DEVICE_NAME,
    //LIBWFX_ERRCODE_NO_SOURCE,
    //LIBWFX_ERRCODE_FILE_NO_EXIST,
    //LIBWFX_ERRCODE_PAPER_NOT_READY,
    //LIBWFX_ERRCODE_INVALID_SERIALNUM,

    LIBWFX_ERRCODE_SUCCESS = 0,                 /**< The function is performed successfully */
    LIBWFX_ERRCODE_FAIL,                        /**< The function is failed */
    LIBWFX_ERRCODE_NO_INIT,                     /**< Not do AVISP_INIT */
    LIBWFX_ERRCODE_NOT_YET_OPEN_DEVICE,         /**< Not do AVISP_OPEN_DEVICE */
    LIBWFX_ERRCODE_DEVICE_ALREADY_OPEN,         /**< The device has opened by AVISP_OPEN_DEVICE */
    LIBWFX_ERRCODE_INVALID_SOURCE,              /**< Input invalid source */
    LIBWFX_ERRCODE_NO_ENABLE_THRESHOLD,         /**< In BW mode, the threshold config does not enalbe */
    LIBWFX_ERRCODE_NO_SUPPORT_THRESHOLD,        /**< In Auto mode, the threshold not support */
    LIBWFX_ERRCODE_NOT_YET_SET_SCAN_PROPERTY,   /**< Not yet set scan property */
    LIBWFX_ERRCODE_NO_SET_RECOGNIZE_TOOL,       /**< Not yet set Recognize tool */
    LIBWFX_ERRCODE_OCR_NOT_SUPPORT_BOTTOMUP,    /**< OCR can't recognize bottom-up source */
    LIBWFX_ERRCODE_READ_IMAGE_FAILED,           /**< Reading Image file Failed */
    LIBWFX_ERRCODE_ONLY_SUPPORT_COLOR_MODE,     /**< Only support color mode */
    LIBWFX_ERRCODE_ICM_PROFILE_NOT_EXIST,       /**< Icm Profile is not exist */
    LIBWFX_ERRCODE_NO_SUPPORT_EJECT,            /**< No support eject direction control */
    LIBWFX_ERRCODE_NO_SUPPORT_JPEGXFER,         /**< No support jpeg output form source */
    LIBWFX_ERRCODE_PAPER_NOT_READY,             /**< No paper */
    LIBWFX_ERRCODE_INVALID_SERIALNUM,	        /**< The Serial number is invailid */
    LIBWFX_ERRCODE_DISCONNECT,                  /**< The internet has problem in Remote mode */
    LIBWFX_ERRCODE_FORMAT_NOT_SUPPORT,          /**< The recognizetextoupt format is not supported */
    LIBWFX_ERRCODE_NO_CALIBRATION_DATA,         /**< Not yet calibration */
    LIBWFX_ERRCODE_OCR_TOOL_NOT_SUPPORT,        /**< No support OCR tool */
    LIBWFX_ERRCODE_RECOGNIZE_TYPE_NOT_SUPPORT,  /**< No support table recognize */
    LIBWFX_ERRCODE_INVALID_CERTIFICATE,         /**< The Certificate or Recognize Type is invailid */
    LIBWFX_ERRCODE_AP_ALREADY_EXISIT,           /**< Ap has already exisited */
    LIBWFX_ERRCODE_OPEN_REGISTRY_KEY_FAILED,    /**< Open registry key failed */
    LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL,          /**< Load MRTD process failed */
    LIBWFX_ERRCODE_COVER_OPENED,                /**< Device Cover Opened */
    LIBWFX_ERRCODE_CERTIFICATE_EXPIRED,         /**< certificate expired*/
    LIBWFX_ERRCODE_ALREADY_INIT,				/**< Already init*/
    LIBWFX_ERRCODE_NO_SUPPORT_DUPLEX,			/**< No support duplex form source */
    LIBWFX_ERRCODE_NO_AVI_OCR = 1001,           /**< AVIOCR is not installed */
    LIBWFX_ERRCODE_NO_DOC_OCR,                  /**< DOCOCR is not installed */
    LIBWFX_ERRCODE_NO_OCR,                      /**< AVIOCR & DOCOCR are not installed */
    LIBWFX_ERRCODE_NO_DEVICES,                  /**< No device detected */
    LIBWFX_ERRCODE_NO_DEVICE_NAME,              /**< Command has no device-name field */
    LIBWFX_ERRCODE_NO_SOURCE,                   /**< Command has no source field */
    LIBWFX_ERRCODE_FILE_NO_EXIST,               /**< When the RemoveFile is executed, the file does not exist */
    LIBWFX_ERRCODE_PATH_TOO_LONG,               /**< Execution file address is too long */
    LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH,        /**< There is a unsatisfied type in the command */
    LIBWFX_ERRCODE_SCANNING,		            /**< The scanning process is not over yet*/
}

public enum ENUM_LIBWFX_EVENT_CODE
{
    LIBWFX_EVENT_PAPER_DETECTED = 0,
    LIBWFX_EVENT_NO_PAPER,
    LIBWFX_EVENT_PAPER_JAM,
    LIBWFX_EVENT_MULTIFEED,
    LIBWFX_EVENT_NO_CALIBRATION_DATA,
    LIBWFX_EVENT_WARMUP_COUNTDOWN,
    LIBWFX_EVENT_SCAN_PROGRESS,
    LIBWFX_EVENT_BUTTON_DETECTED,
    LIBWFX_EVENT_SCANNING,
    LIBWFX_EVENT_PAPER_FEEDING_ERROR,
    LIBWFX_EVENT_COVER_OPEN,
    LIBWFX_EVENT_LEFT_SENSOR_DETECTED,
    LIBWFX_EVENT_RIGHT_SENSOR_DETECTED,
    LIBWFX_EVENT_ALL_SENSOR_DETECTED,
    LIBWFX_EVENT_UVSECURITY_DETECTED,
    LIBWFX_EVENT_PLUG_UNPLUG,
    LIBWFX_EVENT_OVER_TIME_SCAN,
    LIBWFX_EVENT_CANCEL_SCAN,
    LIBWFX_EVENT_CAMERA_RGB_DISLOCATION
}

public enum ENUM_LIBWFX_EXCEPTION_CODE
{
    LIBWFX_EXC_OTHER = 0,
    LIBWFX_EXC_TIFF_SAVE_FINSIHED,
    LIBWFX_EXC_PDF_SAVE_FINSIHED,
    LIBWFX_EXC_IP_EXCEPTION
}

public enum ENUM_LIBWFX_NOTIFY_CODE
{
    LIBWFX_NOTIFY_IMAGE_DONE = 0,
    LIBWFX_NOTIFY_END,
    LIBWFX_NOTIFY_EXCEPTION,
    LIBWFX_NOTIFY_SHOWPATHONLY,
}

public enum ENUM_LIBWFX_EJECT_DIRECTION
{
    LIBWFX_EJECT_FORWARDING = 1,
    LIBWFX_EJECT_BACKWARDING,
}

public enum ENUM_LIBWFX_COLOR_MODE
{
    LIBWFX_COLOR_MODE_BW = 0,
    LIBWFX_COLOR_MODE_GRAY,
    LIBWFX_COLOR_MODE_COLOR,
}

public enum ENUM_LIBWFX_INIT_MODE
{
    LIBWFX_INIT_MODE_NORMAL = 0x0,
    LIBWFX_INIT_MODE_NOOCR = 0x1,
}

[StructLayout(LayoutKind.Sequential)]
public struct ST_IMAGE_INFO
{
    public ENUM_LIBWFX_COLOR_MODE enColorMode;
    public uint ulPixel;
	public uint ulPerLawByte;
	public uint ulLine;
    public IntPtr pRawDate;
};

public enum ENUM_PERMISSION_DATA_TYPE
{
    LIBWFX_DATA_TYPE_PERMISSION,
    LIBWFX_DATA_TYPE_REGINFO,
}


[StructLayout(LayoutKind.Sequential)]
class DeviceWrapper
{
    public const String LIBWFX_DLLNAME = @"LibWebFXScan.dll";
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LIBWFXEVENTCB(ENUM_LIBWFX_EVENT_CODE enEventCode, int nParam, IntPtr pUserDef);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LIBWFXCB(ENUM_LIBWFX_NOTIFY_CODE enNotifyCode, IntPtr pUserDef, IntPtr pParam1, IntPtr pParam2);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_Init", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_Init();

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_InitEx", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_InitEx(ENUM_LIBWFX_INIT_MODE enInitMode);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_DeInit", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_DeInit();

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_GetDeviesList", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_GetDeviesList(out IntPtr szDevicesListOut);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_GetDeviesListWithSerial", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_GetDeviesListWithSerial(out IntPtr szDevicesListOut, out IntPtr szSerialListOut);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_GetFileList", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_GetFileList(out IntPtr szFileListOut);

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_RemoveFile", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_RemoveFile(String szFileNameIn);

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_SetProperty", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_SetProperty(String szRequestCmdIn, [MarshalAs(UnmanagedType.FunctionPtr)] LIBWFXEVENTCB pfnLibWFXEVENTCBIn, IntPtr pUserDefIn);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_StartScan", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_StartScan([MarshalAs(UnmanagedType.FunctionPtr)] LIBWFXCB pfnLibWFXCBIn, IntPtr pUserDefIn);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_Calibrate", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_Calibrate();


    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_ECOControl", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_ECOControl(out uint pulTime, int nSetIn);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_PaperReady", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_PaperReady();

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_CloseDevice", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_CloseDevice();

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_EjectPaperControl", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_EjectPaperControl(ENUM_LIBWFX_EJECT_DIRECTION enEjectDirectIn);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_EjectPaperControlWithMsg", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_EjectPaperControlWithMsg(ENUM_LIBWFX_EJECT_DIRECTION enEjectDirectIn, out IntPtr szErrorMsg);

    [DllImport(LIBWFX_DLLNAME, EntryPoint = "LibWFX_GetPaperStatus", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_GetPaperStatus(out ENUM_LIBWFX_EVENT_CODE penStatusOut);

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_ReadImage", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_ReadImage(String szFilePathIn, [MarshalAs(UnmanagedType.FunctionPtr)] LIBWFXCB pfnLibWFXCBIn, IntPtr pUserDefIn);

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_MergeToPdf", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_MergeToPdf(String szFileListIn);

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_IsWindowExist", CallingConvention = CallingConvention.StdCall)]
    public static extern bool LibWFX_IsWindowExist(String szWindowNameIn);

    //[DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_IsOCRVerMatch", CallingConvention = CallingConvention.StdCall)]
    //public static extern bool LibWFX_IsOCRVerMatch();

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_GetLastErrorCode", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_GetLastErrorCode(ENUM_LIBWFX_ERRCODE enErrorCode, out IntPtr szErrorMsg);

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_SynchronizeScan", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_SynchronizeScan(String szRequestCmdIn, out IntPtr szScanImageList, out IntPtr szOCRResultList, out IntPtr szExceptionRet, out IntPtr szEventRet);

    [DllImport(LIBWFX_DLLNAME, CharSet = CharSet.Unicode, EntryPoint = "LibWFX_GetCertificatePermission", CallingConvention = CallingConvention.StdCall)]
    public static extern ENUM_LIBWFX_ERRCODE LibWFX_GetCertificatePermission(out IntPtr szPermissionTypeList, ENUM_PERMISSION_DATA_TYPE enDataType);
}

