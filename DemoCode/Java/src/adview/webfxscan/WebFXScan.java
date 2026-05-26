package adview.webfxscan;
import java.io.File;

public class WebFXScan {
	
	public class ENUM_LIBWFX_ERRCODE
	{	
		public final static int LIBWFX_ERRCODE_SUCCESS = 0;                      /**< The function is performed successfully */
	    public final static int LIBWFX_ERRCODE_FAIL = 1;                         /**< The function is failed */
	    public final static int LIBWFX_ERRCODE_NO_INIT = 2;                      /**< Not do AVISP_INIT */
	    public final static int LIBWFX_ERRCODE_NOT_YET_OPEN_DEVICE = 3;          /**< Not do AVISP_OPEN_DEVICE */
	    public final static int LIBWFX_ERRCODE_DEVICE_ALREADY_OPEN = 4;          /**< The device has opened by AVISP_OPEN_DEVICE */
	    public final static int LIBWFX_ERRCODE_INVALID_SOURCE = 5;               /**< Input invalid source */
	    public final static int LIBWFX_ERRCODE_NO_ENABLE_THRESHOLD = 6;          /**< In BW mode, the threshold config does not enalbe */
	    public final static int LIBWFX_ERRCODE_NO_SUPPORT_THRESHOLD = 7;         /**< In Auto mode, the threshold not support */
	    public final static int LIBWFX_ERRCODE_NOT_YET_SET_SCAN_PROPERTY = 8;    /**< Not yet set scan property */
	    public final static int LIBWFX_ERRCODE_NO_SET_RECOGNIZE_TOOL = 9;        /**< Not yet set Recognize tool */
	    public final static int LIBWFX_ERRCODE_OCR_NOT_SUPPORT_BOTTOMUP = 10;    /**< OCR can't recognize bottom-up source */
	    public final static int LIBWFX_ERRCODE_READ_IMAGE_FAILED = 11;           /**< Reading Image file Failed */
	    public final static int LIBWFX_ERRCODE_ONLY_SUPPORT_COLOR_MODE = 12;     /**< Only support color mode */
	    public final static int LIBWFX_ERRCODE_ICM_PROFILE_NOT_EXIST = 13;       /**< Icm Profile is not exist */
	    public final static int LIBWFX_ERRCODE_NO_SUPPORT_EJECT = 14;            /**< No support eject direction control */
	    public final static int LIBWFX_ERRCODE_NO_SUPPORT_JPEGXFER = 15;         /**< No support jpeg output form source */
	    public final static int LIBWFX_ERRCODE_PAPER_NOT_READY = 16;             /**< No paper */
	    public final static int LIBWFX_ERRCODE_INVALID_SERIALNUM = 17;	         /**< The Serial number is invailid */
	    public final static int LIBWFX_ERRCODE_DISCONNECT = 18;                  /**< The internet has problem in Remote mode */
	    public final static int LIBWFX_ERRCODE_FORMAT_NOT_SUPPORT = 19;          /**< The recognizetextoupt format is not supported */
	    public final static int LIBWFX_ERRCODE_NO_CALIBRATION_DATA = 20;         /**< Not yet calibration */
	    public final static int LIBWFX_ERRCODE_OCR_TOOL_NOT_SUPPORT = 21;        /**< No support OCR tool */
	    public final static int LIBWFX_ERRCODE_RECOGNIZE_TYPE_NOT_SUPPORT = 22;  /**< No support table recognize */
	    public final static int LIBWFX_ERRCODE_INVALID_CERTICATE = 23;			 /**< The Certicate or Recognize Type is invailid */
	    public final static int LIBWFX_ERRCODE_AP_ALREADY_EXISIT = 24;			 /**< Ap has already exisited */
	    public final static int LIBWFX_ERRCODE_OPEN_REGISTRY_KEY_FAILED = 25;	 /**< Open registry key failed */
	    public final static int LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL = 26;          /**< Load MRTD process failed */
	    public final static int LIBWFX_ERRCODE_COVER_OPENED = 27;                /**< Device Cover Opened */
	    public final static int LIBWFX_ERRCODE_CERTIFICATE_EXPIRED = 28;         /**< Device Cover Opened */
	    public final static int LIBWFX_ERRCODE_ALREADY_INIT = 29;                /**< Device Cover Opened */
	    public final static int LIBWFX_ERRCODE_NO_SUPPORT_DUPLEX = 30;			 /**< No support duplex form source */
	    public final static int LIBWFX_ERRCODE_NO_AVI_OCR = 1001;				 /**< AVIOCR is not installed */
	    public final static int LIBWFX_ERRCODE_NO_DOC_OCR = 1002;				 /**< DOCOCR is not installed */
	    public final static int LIBWFX_ERRCODE_NO_OCR = 1003;					 /**< AVIOCR & DOCOCR are not installed */
	    public final static int LIBWFX_ERRCODE_NO_DEVICES = 1004;				 /**< No device detected */
	    public final static int LIBWFX_ERRCODE_NO_DEVICE_NAME = 1005;			 /**< Command has no device-name field */
	    public final static int LIBWFX_ERRCODE_NO_SOURCE = 1006;				 /**< Command has no source field */
	    public final static int LIBWFX_ERRCODE_FILE_NO_EXIST = 1007;			 /**< When the RemoveFile is executed, the file does not exist */
	    public final static int LIBWFX_ERRCODE_PATH_TOO_LONG = 1008;			 /**< Execution file address is too long */
	    public final static int LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH = 1009;		 /**< There is a unsatisfied type in the command */
	    public final static int LIBWFX_ERRCODE_SCANNING = 1010;	                 /**< The scanning process is not over yet */
	    public final static int LIBWFX_ERRCODE_FILE_OCCUPIED = 1011;			 /**< When the RecycleSaveFolder is executed, the file or folder is occupied*/
	    public final static int LIBWFX_ERRCODE_SAVEPATH_ERROR = 1012;            /**< When the RecycleSaveFolder is executed, the save path format error */
	    public final static int LIBWFX_ERRCODE_TIMEOUT = 1013;                   /**< Timeout error*/
	    public final static int LIBWFX_ERRCODE_SERVER_OCCUPIED = 1014;           /**< Server has been occupied by other connections */
	    public final static int LIBWFX_ERRCODE_SPECIFIC_AP_OPENING = 1015;       /**< The unauthorized program is running */
	    public final static int LIBWFX_ERRCODE_PARM_VALUE_MISMATCH = 1016;       /**< Undefined parameter value appears in command */
	    public final static int LIBWFX_ERRCODE_INVALID_FILE_FORMAT = 1017;       /**< Only image files(JPG, BMP, PNG) are allowed when using "MergePdf */
	    public final static int LIBWFX_ERRCODE_INIT_DLL_NOT_FOUND = 1018;        /**< Failed to load DLL because the file was not found */
	    public final static int LIBWFX_ERRCODE_INIT_DLL_LOAD_FAILED = 1019;      /**< DLL file exists but failed to load (may be corrupted or incompatible) */
	    public final static int LIBWFX_ERRCODE_API_BUSY = 1020;					 /**< API is busy processing the previous request */
	    public final static int LIBWFX_ERRCODE_ONLY_SUPPORT_X64 = 1021;			 /**< Only support X64 */
	}

	public class ENUM_LIBWFX_EVENT_CODE
	{
		public final static int LIBWFX_EVENT_PAPER_DETECTED = 0;
		public final static int LIBWFX_EVENT_NO_PAPER = 1;
		public final static int LIBWFX_EVENT_PAPER_JAM = 2;
		public final static int LIBWFX_EVENT_MULTIFEED = 3;
		public final static int LIBWFX_EVENT_NO_CALIBRATION_DATA = 4;
		public final static int LIBWFX_EVENT_WARMUP_COUNTDOWN = 5;
		public final static int LIBWFX_EVENT_SCAN_PROGRESS = 6;
		public final static int LIBWFX_EVENT_BUTTON_DETECTED = 7;
		public final static int LIBWFX_EVENT_SCANNING = 8;
		public final static int LIBWFX_EVENT_PAPER_FEEDING_ERROR = 9;
		public final static int LIBWFX_EVENT_COVER_OPEN = 10;
		public final static int LIBWFX_EVENT_LEFT_SENSOR_DETECTED = 11;
		public final static int LIBWFX_EVENT_RIGHT_SENSOR_DETECTED = 12;
		public final static int LIBWFX_EVENT_ALL_SENSOR_DETECTED = 13;
		public final static int LIBWFX_EVENT_UVSECURITY_DETECTED = 14;
		public final static int LIBWFX_EVENT_PLUG_UNPLUG = 15;
		public final static int LIBWFX_EVENT_OVER_TIME_SCAN = 16;
		public final static int LIBWFX_EVENT_CANCEL_SCAN = 17;
		public final static int LIBWFX_EVENT_CAMERA_RGB_DISLOCATION = 18;
		public final static int LIBWFX_EVENT_CAMERA_TIMEOUT = 19;
	}
	
	public class ENUM_LIBWFX_EXCEPTION_CODE
	{
	    public final static int LIBWFX_EXC_OTHER = 0;
        public final static int LIBWFX_EXC_TIFF_SAVE_FINSIHED = 1;
	    public final static int LIBWFX_EXC_PDF_SAVE_FINSIHED = 2;
	    public final static int LIBWFX_EXC_IP_EXCEPTION = 3;
	};
	
	public class ENUM_LIBWFX_NOTIFY_CODE
	{
		public final static int LIBWFX_NOTIFY_IMAGE_DONE = 0;
		public final static int LIBWFX_NOTIFY_END = 1;
		public final static int LIBWFX_NOTIFY_EXCEPTION = 2;
		public final static int LIBWFX_NOTIFY_SHOWPATHONLY = 3;
	};

	public class ENUM_LIBWFX_EJECT_DIRECTION
	{
	    public final static int LIBWFX_EJECT_FORWARDING = 1;
	    public final static int LIBWFX_EJECT_BACKWARDINGS = 2;
	    public final static int LIBWFX_EJECT_BACKWARDING = 3;
	    public final static int LIBWFX_EJECT_FORWARDINGS = 4;
	    public final static int LIBWFX_EJECT_FORWARDINGD = 5;
	    public final static int LIBWFX_EJECT_BACKWARDINGD = 6;
	    public final static int LIBWFX_EJECT_BACKWARDINGSD = 7;
	    public final static int LIBWFX_EJECT_FORWARDINGSD = 8;	    
	    public final static int LIBWFX_EJECT_FORWARDING_BY_STEPS = 10;
	    public final static int LIBWFX_EJECT_BACKWARDING_BY_STEPS = 11;
	};

	public class ENUM_LIBWFX_INIT_MODE
	{
		public final static int LIBWFX_INIT_MODE_NORMAL = 0;
		public final static int LIBWFX_INIT_MODE_NOOCR = 1;
	};
	
	public class ENUM_PERMISSION_DATA_TYPE
	{
		public final static int LIBWFX_DATA_TYPE_PERMISSION = 0;
	    public final static int LIBWFX_DATA_TYPE_REGINFO = 1;
	};
	
	public class ENUM_LIBWFX_INTERFACE
	{
		public final static int LIBWFX_INTERFACE_SDK = 0;
	    public final static int LIBWFX_INTERFACE_WEBSCAN = 1;
	};
	
	public class ENUM_SOURCETYPE
	{
		public final static int UNKNOWN = 0;
	    public final static int ADF = 1;
	    public final static int SHEETFED = 2;
	    public final static int ADF_SHEETFED = 3;
	    public final static int FLATBED = 4;
	    public final static int ADF_FLATBED = 5;
	    public final static int CAMERA = 6;
	};

	public class CImageInfo {
	    public int nColorMode; // 0:BW,1:GRAY,2:COLOR
	    public long ulPixel;
	    public long ulPerLawByte;
	    public long ulLine;
	    public byte[] byteRawDate;
	}
	
	public class CString {
		public String szValue;
		public CString(String szValueIn) {
			szValue = szValueIn;
		}
	}
	
	public class CLong {
		public long nValue;
		public CLong(long nValueIn) {
			nValue = nValueIn;
		}
	}
		
	public interface WFXScanEventCallBack {
		public abstract void DoCallBack(int enEventCode, int nParam);
	}
	
	public interface WFXScanCallBack {
		public abstract void DoCallBack(int enNotifyCode, int enExcCode, Object object, String szRecognizeTxt);
	}
	
	static {
		String szDllPath = System.getProperty("user.dir") + "\\JavaWFXScan.dll";
		 File file = new File(szDllPath);
	        if (file.exists()) {
	        	System.load(szDllPath);	    		
	        }
	        else {
	        	//debug mode
	        	System.loadLibrary("JavaWFXScan");
	        }
	}
	
	public native int WFXScan_Init();
	public native int WFXScan_InitEx(int nInitMode);
	public native int WFXScan_DeInit();
	public native int WFXScan_GetDeviceList(CString szDevicesListOut);
	public native int WFXScan_GetDevicesListWithSerial(CString szDevicesListOut, CString szSerialListOut);
	public native int WFXScan_GetDeviesListWithSerialAndFW(CString szDevicesListOut, CString szSerialListOut, CString szFW, CString szDriver);
	public native int WFXScan_GetFileList(CString szFileListOut);
	public native int WFXScan_RemoveFile(String szFileNameIn);
	public native int WFXScan_SetProperty(String szRequestCmdIn, WFXScanEventCallBack pfnLibWFXEVENTCBIn);
	public native int WFXScan_StartScan(WFXScanCallBack pfnLibWFXCBIn);
	public native int WFXScan_Calibrate();
	public native int WFXScan_ECOControl(CLong pulTime, int nSetIn);
	public native int WFXScan_PaperReady();
	public native int WFXScan_CloseDevice();
	public native int WFXScan_EjectPaperControl(int nEjectDirectIn);
	public native int WFXScan_EjectPaperControlWithMsg(int nEjectDirectIn, CString szErrorMsg);
	public native int WFXScan_GetPaperStatus(CLong nStatusOut);
	public native boolean WFXScan_IsWindowExist(String szWindowNameIn);
	public native int WFXScan_GetLastErrorCode(int enErrorCode, CString szErrorMsg);
	public native boolean WFXScan_EditCommand(String szCommand, CString szCommandOut);
	public native int WFXScan_SynchronizeScan(String szCommand, CString szScanImageListOut, CString szOCRResultListOut, CString szExceptionRetOut, CString szEventRetOut);
	public native int WFXScan_GetCertificatePermission(CString szPermissionTypeList, int enDataType);
	public native int WFXScan_RecycleSaveFolder();
	public native int WFXScan_MergeToPdf(String szFileNameIn);
	public native int WFXScan_WriteAPLog(String szMsg);
	public native void WFXScan_CallRegisterAP();
	public native int WFXScan_GetDeviceCapability(String szDeviceName, int[] outCaps); // [0]=enSource, [1]=bDuplex, [2]=bJpegTransfer, [3]=nDPI, [4]=nMaxX, [5]=nMaxY, [6]=bLongPaper
}
