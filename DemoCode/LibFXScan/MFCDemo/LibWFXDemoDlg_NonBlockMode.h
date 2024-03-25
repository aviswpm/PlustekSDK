#pragma once
#include "..\inc\LibWebFXScan\LibWebFXScan.h"
#include "WarmupDlg.h"
#include <string>
#define WM_LIBWFX_BEGIN				 WM_USER + 1000
#define WM_LIBWFX_STARTSCAN          WM_LIBWFX_BEGIN + 1
#define WM_LIBWFX_WRITELOG			 WM_LIBWFX_BEGIN + 2
#define WM_LIBWFX_SHOW_WARMUP		 WM_LIBWFX_BEGIN + 3
#define WM_LIBWFX_SCAN_PROGRESS		 WM_LIBWFX_BEGIN + 4
#define WM_LIBWFX_CLEAN_DRAW		 WM_LIBWFX_BEGIN + 5
#define WM_LIBWFX_SETPROPERTY		 WM_LIBWFX_BEGIN + 6
#define WM_LIBWFX_CLOSEDEVICE		 WM_LIBWFX_BEGIN + 7
#define WM_LIBWFX_EJECTPAPER		 WM_LIBWFX_BEGIN + 8
#define MAX_FILE_NAMES 300

// CLibWFXDemoDlg_NonBlockMode dialog

class CLibWFXDemoDlg_NonBlockMode : public CDialogEx
{
	DECLARE_DYNAMIC(CLibWFXDemoDlg_NonBlockMode)

public:
	CLibWFXDemoDlg_NonBlockMode(CWnd* pParent = NULL);   // standard constructor
	virtual ~CLibWFXDemoDlg_NonBlockMode();

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_LIBWFXDEMO_DIALOG1 };
#endif

	bool m_bIPexception;

	

private:
	HMODULE                   m_hLibWFX;
	LIBWFX_INIT               m_pfnLibWFX_Init;
	LIBWFX_INITEX             m_pfnLibWFX_InitEx;
	LIBWFX_DEINIT             m_pfnLibWFX_DeInit;
	LIBWFX_GETDEVICESLIST     m_pfnLibWFX_GetDevicesList;
	LIBWFX_GETDEVICESLIST_WITHSERIAL     m_pfnLibWFX_GetDevicesListWithSerial;
	
	LIBWFX_SETPROPERTY        m_pfnLibWFX_SetProperty;
	LIBWFX_STARTSCAN          m_pfnLibWFX_StartScan;
	LIBWFX_CALIBRATE          m_pfnLibWFX_Calibrate;
	LIBWFX_ECOCONTROL         m_pfnLibWFX_ECOControl;
	LIBWFX_PAPERREADY         m_pfnLibWFX_PaperReady;
	LIBWFX_CLOSEDEVICE        m_pfnLibWFX_CloseDevice;
	LIBWFX_EJECTPAPER_CONTORL m_pfnLibWFX_EjectPaperControl;
	LIBWFX_EJECTPAPER_CONTORL_WITHMSG m_pfnLibWFX_EjectPaperControlWithMsg;
	LIBWFX_GETPAPERSTATUS     m_pfnLibWFX_GetPaperStatus;
	//LIBWFX_CAMERACALIBRATE    m_pfnLibWFX_CameraCalibrate;
	LIBWFX_ISWINDOWEXIST      m_pfnLibWFX_IsWindowExist;
	LIBWFX_GETLASTERRORCODE   m_pfnLibWFX_GetLastErrorCode;
	LIBWFX_SYNCHRONIZESCAN	  m_pfnLibWFX_SynchronizeScan;
	LIBWFX_GETCERTIFICATEPERMISSION	  m_pfnLibWFX_GetCertificatePermission;
	LIBWFX_RECYCLESAVEFOLDER    m_pfnLibWFX_RecycleSaveFolder;
	LIBWFX_MERGETOPDF		    m_pfnLibWFX_MergeToPdf;
	LIBWFX_WRITEAPLOG		  m_pfnLibWFX_WriteAPLog;

	int                       m_nCount;
	int                       m_nWarmupTotalTime;
	int						  m_nCmdMaxNum;
	WarmupDlg*                m_dlgWarmup;


	static void LibWFXEVENTCB(ENUM_LIBWFX_EVENT_CODE enEventCode, int nParam, void* pUserDef);
	static void LibWFXCB(ENUM_LIBWFX_NOTIFY_CODE enNotifyCode, void* pUserDef, void* pParam1, void* pParam2);
	BOOL InitLib(VOID);
	BOOL InitDevicesList(VOID);
	BOOL WriteLog(TCHAR* szMsg);
	BOOL ShowImage(TCHAR* szFilePath);
	BOOL DrawImage(ST_IMAGE_INFO* pstImgInfo);
	BOOL GetJsonString(CString szDevName);
	BOOL GetCommandString(wchar_t* DevName);
	BOOL SetCommandString(wchar_t* DevName, CString Command);
	wchar_t* rtrim(wchar_t *str);
	void GetCertificatePermission();
	BOOL GetSDKInstallPath(TCHAR* szSDKInstallPath);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	HICON m_hIcon;
	virtual BOOL OnInitDialog();
	afx_msg LRESULT OnStartScan(WPARAM wPararm, LPARAM lParam);
	afx_msg LRESULT OnSetProperty(WPARAM wPararm, LPARAM lParam);
	afx_msg LRESULT OnWriteLog(WPARAM wPararm, LPARAM lParam);
	afx_msg LRESULT OnHandleWarmupProgress(WPARAM wPararm, LPARAM lParam);
	afx_msg LRESULT OnHandleScanProgress(WPARAM wPararm, LPARAM lParam);
	afx_msg LRESULT OnHandleCleanDraw(WPARAM wPararm, LPARAM lParam);
	afx_msg LRESULT OnCloseDevice(WPARAM wPararm, LPARAM lParam);
	afx_msg LRESULT OnEjectPaper(WPARAM wPararm, LPARAM lParam);
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButtonSetProperty();
	afx_msg void OnCbnSelchangeComboDeviceName();
	afx_msg void OnBnClickedButtonRefresh();
	afx_msg void OnBnClickedButtonEditCmd();
	afx_msg void OnBnClickedButtonEco();
	afx_msg void OnBnClickedButtonEject();
	afx_msg void OnBnClickedButtonPaperReady();
	afx_msg void OnBnClickedButtonPaperstatus();
	afx_msg void OnBnClickedButtonCalibrate();
	afx_msg void OnBnClickedCancel();
	afx_msg void OnPaint();
	afx_msg void OnDestroy();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnWindowPosChanging(WINDOWPOS* lpwndpos);
	afx_msg void OnCbnDropdownComboCommand();
	afx_msg void OnBnClickedButtonScan();
	afx_msg void OnBnClickedButtonRecyclesavefolder();
	afx_msg void OnBnClickedButtonMergepdf();
};
