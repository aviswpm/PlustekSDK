using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

public enum ENUM_LIBWFX_ERRCODE
{
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
    LIBWFX_ERRCODE_SCANNING,                    /**< The scanning process is not over yet */
    LIBWFX_ERRCODE_FILE_OCCUPIED,               /**< When the RecycleSaveFolder is executed, the file or folder is occupied */
    LIBWFX_ERRCODE_SAVEPATH_ERROR,              /**< When the RecycleSaveFolder is executed, the save path format error */
    LIBWFX_ERRCODE_TIMEOUT,                     /**< Timeout error */
    LIBWFX_ERRCODE_SERVER_OCCUPIED,             /**< Server has been occupied by other connections */
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
    LIBWFX_EVENT_CAMERA_RGB_DISLOCATION,
    LIBWFX_EVENT_CAMERA_TIMEOUT
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
    public IntPtr hLibModule = IntPtr.Zero;
    public IntPtr hCommandModule = IntPtr.Zero;
    public const String LIBWFX_DLLNAME = @"LibWebFXScan.dll";
    public const String COMMANDEDITOR_DLLNAME = @"CommandEditor.dll";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LIBWFXEVENTCB(ENUM_LIBWFX_EVENT_CODE enEventCode, int nParam, IntPtr pUserDef);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LIBWFXCB(ENUM_LIBWFX_NOTIFY_CODE enNotifyCode, IntPtr pUserDef, IntPtr pParam1, IntPtr pParam2);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr LoadLibrary(string dllToLoad);


    [DllImport("kernel32.dll")]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_Init();

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_InitEx(ENUM_LIBWFX_INIT_MODE enInitMode);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_DeInit();

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_GetDeviesList(out IntPtr szDevicesListOut);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_GetDeviesListWithSerial(out IntPtr szDevicesListOut, out IntPtr szSerialListOut);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_SetProperty(String szRequestCmdIn, [MarshalAs(UnmanagedType.FunctionPtr)] LIBWFXEVENTCB pfnLibWFXEVENTCBIn, IntPtr pUserDefIn);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_StartScan([MarshalAs(UnmanagedType.FunctionPtr)] LIBWFXCB pfnLibWFXCBIn, IntPtr pUserDefIn);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_Calibrate();

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_ECOControl(out uint pulTime, int nSetIn);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_PaperReady();

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_CloseDevice();

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_EjectPaperControl(ENUM_LIBWFX_EJECT_DIRECTION enEjectDirectIn);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_EjectPaperControlWithMsg(ENUM_LIBWFX_EJECT_DIRECTION enEjectDirectIn, out IntPtr szErrorMsg);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_GetPaperStatus(out ENUM_LIBWFX_EVENT_CODE penStatusOut);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_MergeToPdf(String szFileListIn);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate bool LibWFX_IsWindowExist(String szWindowNameIn);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_GetLastErrorCode(ENUM_LIBWFX_ERRCODE enErrorCode, out IntPtr szErrorMsg);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_SynchronizeScan(String szRequestCmdIn, out IntPtr szScanImageList, out IntPtr szOCRResultList, out IntPtr szExceptionRet, out IntPtr szEventRet);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate void EditCommand(String szCommand, out IntPtr szCommandOut);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_GetCertificatePermission(out IntPtr szPermissionTypeList, ENUM_PERMISSION_DATA_TYPE enDataType);

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_RecycleSaveFolder();

    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
    public delegate ENUM_LIBWFX_ERRCODE LibWFX_WriteAPLog(String szMsg);

    public LibWFX_Init m_pfnLibWFX_Init;
    public LibWFX_InitEx m_pfnLibWFX_InitEx;
    public LibWFX_DeInit m_pfnLibWFX_DeInit;
    public LibWFX_GetDeviesList m_pfnLibWFX_GetDeviesList;
    public LibWFX_GetDeviesListWithSerial m_pfnLibWFX_GetDeviesListWithSerial;
    public LibWFX_SetProperty m_pfnLibWFX_SetProperty;
    public LibWFX_StartScan m_pfnLibWFX_StartScan;
    public LibWFX_Calibrate m_pfnLibWFX_Calibrate;
    public LibWFX_ECOControl m_pfnLibWFX_ECOControl;
    public LibWFX_PaperReady m_pfnLibWFX_PaperReady;
    public LibWFX_CloseDevice m_pfnLibWFX_CloseDevice;
    public LibWFX_EjectPaperControl m_pfnLibWFX_EjectPaperControl;
    public LibWFX_EjectPaperControlWithMsg m_pfnLibWFX_EjectPaperControlWithMsg;
    public LibWFX_GetPaperStatus m_pfnLibWFX_GetPaperStatus;
    public LibWFX_MergeToPdf m_pfnLibWFX_MergeToPdf;
    public LibWFX_IsWindowExist m_pfnLibWFX_IsWindowExist;
    public LibWFX_GetLastErrorCode m_pfnLibWFX_GetLastErrorCode;
    public LibWFX_SynchronizeScan m_pfnLibWFX_SynchronizeScan;
    public EditCommand m_pfnLibWFX_EditCommand;
    public LibWFX_GetCertificatePermission m_pfnLibWFX_GetCertificatePermission;
    public LibWFX_RecycleSaveFolder m_pfnLibWFX_RecycleSaveFolder;
    public LibWFX_WriteAPLog m_pfnLibWFX_WriteAPLog;

    public DeviceWrapper()
    {
        string szPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        szPath = szPath.Substring(0, szPath.LastIndexOf('\\'));
        string szLibDLLPath = szPath + LIBWFX_DLLNAME;
        string szCommandDLLPath = szPath + COMMANDEDITOR_DLLNAME;
        hLibModule = LoadLibrary(szLibDLLPath);
        hCommandModule = LoadLibrary(szCommandDLLPath);
        if (hLibModule == IntPtr.Zero || hCommandModule == IntPtr.Zero)
        {
            //find sdk install path
#if WIN32
            szPath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1", "InstallLocation", "");
#else
            szPath = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1", "InstallLocation", "");
#endif
            System.IO.Directory.SetCurrentDirectory(szPath);

            if (szPath.LastIndexOf('\\') != (szPath.Length - 1))
                szPath += "\\";

            szLibDLLPath = szPath + LIBWFX_DLLNAME;
            szCommandDLLPath = szPath + COMMANDEDITOR_DLLNAME;

            if (hLibModule == IntPtr.Zero)
                hLibModule = LoadLibrary(szLibDLLPath);

            if (hCommandModule == IntPtr.Zero)
                hCommandModule = LoadLibrary(szCommandDLLPath);
        }
        if (hLibModule != IntPtr.Zero && hCommandModule != IntPtr.Zero)
        {
            IntPtr pFun = GetProcAddress(hLibModule, "LibWFX_Init");
            m_pfnLibWFX_Init = (LibWFX_Init)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_Init));

            pFun = GetProcAddress(hLibModule, "LibWFX_InitEx");
            m_pfnLibWFX_InitEx = (LibWFX_InitEx)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_InitEx));

            pFun = GetProcAddress(hLibModule, "LibWFX_DeInit");
            m_pfnLibWFX_DeInit = (LibWFX_DeInit)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_DeInit));

            pFun = GetProcAddress(hLibModule, "LibWFX_GetDeviesList");
            m_pfnLibWFX_GetDeviesList = (LibWFX_GetDeviesList)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_GetDeviesList));

            pFun = GetProcAddress(hLibModule, "LibWFX_GetDeviesListWithSerial");
            m_pfnLibWFX_GetDeviesListWithSerial = (LibWFX_GetDeviesListWithSerial)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_GetDeviesListWithSerial));

            pFun = GetProcAddress(hLibModule, "LibWFX_SetProperty");
            m_pfnLibWFX_SetProperty = (LibWFX_SetProperty)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_SetProperty));

            pFun = GetProcAddress(hLibModule, "LibWFX_StartScan");
            m_pfnLibWFX_StartScan = (LibWFX_StartScan)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_StartScan));

            pFun = GetProcAddress(hLibModule, "LibWFX_Calibrate");
            m_pfnLibWFX_Calibrate = (LibWFX_Calibrate)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_Calibrate));

            pFun = GetProcAddress(hLibModule, "LibWFX_ECOControl");
            m_pfnLibWFX_ECOControl = (LibWFX_ECOControl)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_ECOControl));

            pFun = GetProcAddress(hLibModule, "LibWFX_PaperReady");
            m_pfnLibWFX_PaperReady = (LibWFX_PaperReady)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_PaperReady));

            pFun = GetProcAddress(hLibModule, "LibWFX_CloseDevice");
            m_pfnLibWFX_CloseDevice = (LibWFX_CloseDevice)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_CloseDevice));

            pFun = GetProcAddress(hLibModule, "LibWFX_EjectPaperControl");
            m_pfnLibWFX_EjectPaperControl = (LibWFX_EjectPaperControl)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_EjectPaperControl));

            pFun = GetProcAddress(hLibModule, "LibWFX_EjectPaperControlWithMsg");
            m_pfnLibWFX_EjectPaperControlWithMsg = (LibWFX_EjectPaperControlWithMsg)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_EjectPaperControlWithMsg));

            pFun = GetProcAddress(hLibModule, "LibWFX_GetPaperStatus");
            m_pfnLibWFX_GetPaperStatus = (LibWFX_GetPaperStatus)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_GetPaperStatus));

            pFun = GetProcAddress(hLibModule, "LibWFX_MergeToPdf");
            m_pfnLibWFX_MergeToPdf = (LibWFX_MergeToPdf)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_MergeToPdf));

            pFun = GetProcAddress(hLibModule, "LibWFX_IsWindowExist");
            m_pfnLibWFX_IsWindowExist = (LibWFX_IsWindowExist)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_IsWindowExist));

            pFun = GetProcAddress(hLibModule, "LibWFX_GetLastErrorCode");
            m_pfnLibWFX_GetLastErrorCode = (LibWFX_GetLastErrorCode)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_GetLastErrorCode));

            pFun = GetProcAddress(hLibModule, "LibWFX_SynchronizeScan");
            m_pfnLibWFX_SynchronizeScan = (LibWFX_SynchronizeScan)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_SynchronizeScan));

            pFun = GetProcAddress(hCommandModule, "EditCommand");
            m_pfnLibWFX_EditCommand = (EditCommand)Marshal.GetDelegateForFunctionPointer(pFun, typeof(EditCommand));

            pFun = GetProcAddress(hLibModule, "LibWFX_GetCertificatePermission");
            m_pfnLibWFX_GetCertificatePermission = (LibWFX_GetCertificatePermission)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_GetCertificatePermission));

            pFun = GetProcAddress(hLibModule, "LibWFX_RecycleSaveFolder");
            m_pfnLibWFX_RecycleSaveFolder = (LibWFX_RecycleSaveFolder)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_RecycleSaveFolder));

            pFun = GetProcAddress(hLibModule, "LibWFX_WriteAPLog");
            m_pfnLibWFX_WriteAPLog = (LibWFX_WriteAPLog)Marshal.GetDelegateForFunctionPointer(pFun, typeof(LibWFX_WriteAPLog));
        }       
    }   
}

