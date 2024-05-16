// LibWebFXScan.cpp : Defines the exported functions for the DLL application.
//
#pragma once

#ifdef WIN32
    #define LIBWFX_API __stdcall
#else
    #define LIBWFX_API 
#endif
#define AVI_MAXPATH_LEN 512

#define LIBWFX_DLLNAME _T("LibWebFXScan.dll")

#define	LIBWFX_API_INIT							"LibWFX_Init"
#define	LIBWFX_API_INITEX						"LibWFX_InitEx"
#define	LIBWFX_API_DEINIT						"LibWFX_DeInit"
#define LIBWFX_API_GET_DEVICESLIST				"LibWFX_GetDeviesList"
#define LIBWFX_API_GET_DEVICESLIST_WITHSERIAL	"LibWFX_GetDeviesListWithSerial"
#define LIBWFX_API_SET_PROPERTY					"LibWFX_SetProperty"
#define	LIBWFX_API_START_SCAN					"LibWFX_StartScan"
#define	LIBWFX_API_CALIBRATE					"LibWFX_Calibrate"
#define	LIBWFX_API_ECOCONTROL					"LibWFX_ECOControl"
#define LIBWFX_API_PAPER_READY					"LibWFX_PaperReady"
#define	LIBWFX_API_CLOSE_DEVICE					"LibWFX_CloseDevice"
#define	LIBWFX_API_EJECTPAPER_CONTORL			"LibWFX_EjectPaperControl"
#define	LIBWFX_API_EJECTPAPER_CONTORL_WITHMSG   "LibWFX_EjectPaperControlWithMsg"
#define	LIBWFX_API_GET_PAPERSTATUS				"LibWFX_GetPaperStatus"
#define	LIBWFX_API_MERGETOPDF					"LibWFX_MergeToPdf"
#define	LIBWFX_API_ISWINDOWEXIST				"LibWFX_IsWindowExist"
#define	LIBWFX_API_GETPRODUCTNAMEDAT			"LibWFX_GetProductNameDat"
#define LIBWFX_API_GETLASTERRORCODE				"LibWFX_GetLastErrorCode"
#define LIBWFX_API_SYNCHRONIZESCAN				"LibWFX_SynchronizeScan"
#define LIBWFX_API_GETCERTIFICATEPERMISSION		"LibWFX_GetCertificatePermission"
#define	LIBWFX_API_RECYCLESAVEFOLDER			"LibWFX_RecycleSaveFolder"
#define LIBWFX_API_WRITEAPLOG					"LibWFX_WriteAPLog"

typedef void* WFXDevHandle; 

typedef enum _ENUM_LIBWFX_ERRCODE
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
    LIBWFX_ERRCODE_NO_SUPPORT_DUPLEX,           /**< No support duplex form source */
	LIBWFX_ERRCODE_NO_AVI_OCR = 1001,			/**< AVIOCR is not installed */
	LIBWFX_ERRCODE_NO_DOC_OCR,					/**< DOCOCR is not installed */
	LIBWFX_ERRCODE_NO_OCR,						/**< AVIOCR & DOCOCR are not installed */
	LIBWFX_ERRCODE_NO_DEVICES,					/**< No device detected */
	LIBWFX_ERRCODE_NO_DEVICE_NAME,				/**< Command has no device-name field */
	LIBWFX_ERRCODE_NO_SOURCE,					/**< Command has no source field */
	LIBWFX_ERRCODE_FILE_NO_EXIST,				/**< When the RemoveFile is executed, the file does not exist */
	LIBWFX_ERRCODE_PATH_TOO_LONG,				/**< Execution file address is too long */
	LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH,		/**< There is a unsatisfied type in the command */
	LIBWFX_ERRCODE_SCANNING,		            /**< The scanning process is not over yet */
	LIBWFX_ERRCODE_FILE_OCCUPIED,               /**< When the RecycleSaveFolder is executed, the file or folder is occupied */
	LIBWFX_ERRCODE_SAVEPATH_ERROR,              /**< When the RecycleSaveFolder is executed, the save path format error */
	LIBWFX_ERRCODE_TIMEOUT,                     /**< Timeout error */
	LIBWFX_ERRCODE_SERVER_OCCUPIED,             /**< Server has been occupied by other connections */
} ENUM_LIBWFX_ERRCODE;

typedef enum _ENUM_LIBWFX_EVENT_CODE
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
	LIBWFX_EVENT_CAMERA_TIMEOUT,
} ENUM_LIBWFX_EVENT_CODE;

typedef enum _ENUM_LIBWFX_EXCEPTION_CODE
{
    LIBWFX_EXC_OTHER = 0,
    LIBWFX_EXC_TIFF_SAVE_FINSIHED,
    LIBWFX_EXC_PDF_SAVE_FINSIHED,
	LIBWFX_EXC_IP_EXCEPTION,
} ENUM_LIBWFX_EXCEPTION_CODE;

typedef enum _ENUM_LIBWFX_NOTIFY_CODE
{
    LIBWFX_NOTIFY_IMAGE_DONE = 0,
    LIBWFX_NOTIFY_END,
    LIBWFX_NOTIFY_EXCEPTION,
	LIBWFX_NOTIFY_SHOWPATHONLY,
} ENUM_LIBWFX_NOTIFY_CODE;

typedef enum _ENUM_LIBWFX_EJECT_DIRECTION
{
    LIBWFX_EJECT_FORWARDING = 1,
    LIBWFX_EJECT_BACKWARDING,
} ENUM_LIBWFX_EJECT_DIRECTION;

typedef enum _ENUM_LIBWFX_COLOR_MODE
{
    LIBWFX_COLOR_MODE_BW = 0,
    LIBWFX_COLOR_MODE_GRAY,
    LIBWFX_COLOR_MODE_COLOR,
} ENUM_LIBWFX_COLOR_MODE;

typedef struct tagST_IMAGE_INFO
{
    ENUM_LIBWFX_COLOR_MODE enColorMode;
    unsigned long  ulPixel;
	unsigned long  ulPerLawByte;
	unsigned long  ulLine;
    unsigned char* pRawDate;
} ST_IMAGE_INFO;

typedef enum _ENUM_LIBWFX_INIT_MODE
{
    LIBWFX_INIT_MODE_NORMAL = 0x0,
    LIBWFX_INIT_MODE_NOOCR = 0x1,
} ENUM_LIBWFX_INIT_MODE;

typedef enum _ENUM_PERMISSION_DATA_TYPE
{
	LIBWFX_DATA_TYPE_PERMISSION = 0x0,
	LIBWFX_DATA_TYPE_REGINFO = 0x1,
} ENUM_PERMISSION_DATA_TYPE;

typedef enum _ENUM_REG_TYPE
{
	REG_TYPE_W64NODE32 = 0,
	REG_TYPE_COMMON,
	REG_TYPE_COUNT,
}ENUM_SPRINT_REG_TYPE;

typedef void (*LIBWFXEVENTCB)(ENUM_LIBWFX_EVENT_CODE enEventCode, int nParam, void* pUserDef);
typedef void (*LIBWFXCB)(ENUM_LIBWFX_NOTIFY_CODE enNotifyCode, void* pUserDef, void* pParam1, void* pParam2);

typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_INIT)(void);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_INITEX)(ENUM_LIBWFX_INIT_MODE enInitMode);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_DEINIT)(void);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_GETDEVICESLIST)(const wchar_t** szDevicesListOut);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_GETDEVICESLIST_WITHSERIAL)(const wchar_t** szDevicesListOut, const wchar_t** szSerialListOut);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_SETPROPERTY)(wchar_t* szRequestCmdIn, LIBWFXEVENTCB pfnLibWFXEVENTCBIn, void* pUserDefIn);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_STARTSCAN)(LIBWFXCB pfnLibWFXCBIn, void* pUserDefIn);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_CALIBRATE)(void);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_ECOCONTROL)(unsigned long* pulTime, int nSetIn);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_PAPERREADY)(void);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_CLOSEDEVICE)(void);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_EJECTPAPER_CONTORL)(ENUM_LIBWFX_EJECT_DIRECTION enEjectDirectIn);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_EJECTPAPER_CONTORL_WITHMSG)(ENUM_LIBWFX_EJECT_DIRECTION enEjectDirectIn, const wchar_t** szErrorMsg);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_GETPAPERSTATUS)(ENUM_LIBWFX_EVENT_CODE* penStatusOut);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_MERGETOPDF)(wchar_t* szFileListIn);
typedef BOOL (LIBWFX_API* LIBWFX_ISWINDOWEXIST)(wchar_t* szWindowNameIn);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_GETPRODUCTNAMEDAT)(char* ProductName,  char* id, char* module);
typedef ENUM_LIBWFX_ERRCODE (LIBWFX_API* LIBWFX_GETLASTERRORCODE)(ENUM_LIBWFX_ERRCODE enErrorCode, const wchar_t** szErrorMsg);
typedef ENUM_LIBWFX_ERRCODE(LIBWFX_API* LIBWFX_SYNCHRONIZESCAN)(wchar_t* szRequestCmdIn, const wchar_t** szScanImageList, const wchar_t** szOCRResultList, const wchar_t** szExceptionRet, const wchar_t** szEventRet);
typedef ENUM_LIBWFX_ERRCODE(LIBWFX_API* LIBWFX_GETCERTIFICATEPERMISSION)(const wchar_t** szPermissionTypeList, ENUM_PERMISSION_DATA_TYPE enDataType);
typedef ENUM_LIBWFX_ERRCODE(LIBWFX_API* LIBWFX_RECYCLESAVEFOLDER)(void);
typedef ENUM_LIBWFX_ERRCODE(LIBWFX_API* LIBWFX_WRITEAPLOG)(wchar_t* szMsg);