// LibWFXDemoDlg_BlockMode.cpp : implementation file
//

#include "stdafx.h"
#include "LibWFXDemo.h"
#include "LibWFXDemoDlg_NonBlockMode.h"
#include "afxdialogex.h"
#include "ECODlg.h"
#include "..\inc\json\jansson.h"
#include <string>
#include "tchar.h"
#include <windows.h>
#include <Dbt.h>
#include <iostream>
#include <fstream> 
#include <vector>
#include <thread>
#include <algorithm>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

#define DoResetIfExcept 0   //0:not do set+scan  1:do set+scan   when ip error
#define BC_USDL_FIXFIELDVALUE 0  //option
using namespace std;


// CLibWFXDemoDlg_NonBlockMode dialog

IMPLEMENT_DYNAMIC(CLibWFXDemoDlg_NonBlockMode, CDialogEx)

CLibWFXDemoDlg_NonBlockMode::CLibWFXDemoDlg_NonBlockMode(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_LIBWFXDEMO_DIALOG1, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_dlgWarmup = NULL;
	m_CalibrationDlg = NULL;
	m_CalibrationXminiDlg = NULL;
	m_InitDlg = NULL;
	m_szEventMsg.Empty();
	InitializeCriticalSection(&m_CriticalSecion);
}

CLibWFXDemoDlg_NonBlockMode::~CLibWFXDemoDlg_NonBlockMode()
{
	DeleteCriticalSection(&m_CriticalSecion);
}

void CLibWFXDemoDlg_NonBlockMode::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_REGISTER, m_bt_register);
}


BEGIN_MESSAGE_MAP(CLibWFXDemoDlg_NonBlockMode, CDialogEx)
	ON_BN_CLICKED(IDC_BUTTON_SET_PROPERTY, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonSetProperty)
	ON_CBN_SELCHANGE(IDC_COMBO_DEVICE_NAME, &CLibWFXDemoDlg_NonBlockMode::OnCbnSelchangeComboDeviceName)
	ON_BN_CLICKED(IDC_BUTTON_REFRESH, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonRefresh)
	ON_BN_CLICKED(IDC_BUTTON_EDIT_CMD, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonEditCmd)
	ON_BN_CLICKED(IDC_BUTTON_ECO, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonEco)
	ON_BN_CLICKED(IDC_BUTTON_EJECT, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonEject)
	ON_BN_CLICKED(IDC_BUTTON_PAPER_READY, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonPaperReady)
	ON_BN_CLICKED(IDC_BUTTON_PAPERSTATUS, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonPaperstatus)
	ON_BN_CLICKED(IDC_BUTTON_CALIBRATE, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonCalibrate)
	ON_BN_CLICKED(IDCANCEL, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedCancel)
	ON_WM_QUERYDRAGICON()
	ON_WM_PAINT()
	ON_WM_DESTROY()
	ON_CBN_DROPDOWN(IDC_COMBO_COMMAND, &CLibWFXDemoDlg_NonBlockMode::OnCbnDropdownComboCommand)
	ON_WM_WINDOWPOSCHANGING()
	ON_MESSAGE(WM_LIBWFX_STARTSCAN, OnStartScan)
	ON_MESSAGE(WM_LIBWFX_SETPROPERTY, OnSetProperty)
	ON_MESSAGE(WM_LIBWFX_WRITELOG, OnWriteLog)
	ON_MESSAGE(WM_LIBWFX_SHOW_WARMUP, OnHandleWarmupProgress)
	ON_MESSAGE(WM_LIBWFX_SCAN_PROGRESS, OnHandleScanProgress)
	ON_MESSAGE(WM_LIBWFX_CLEAN_DRAW, OnHandleCleanDraw)
	ON_MESSAGE(WM_LIBWFX_CLOSEDEVICE, OnCloseDevice)
	ON_MESSAGE(WM_LIBWFX_EJECTPAPER, OnEjectPaper)
	ON_MESSAGE(WM_LIBWFX_WRITENOTIFYLOG, OnWriteNotifyLog)
	ON_MESSAGE(WM_LIBWFX_SHOWIMAGE, OnShowImage)
	ON_MESSAGE(WM_LIBWFX_SETAUTOSCANOFF, OnSetAutoScanOff)
	ON_MESSAGE(WM_LIBWFX_FIXUSDLFIELDVALUE, OnFixUSDLFieldValueToJsonFile)
	ON_BN_CLICKED(IDC_BUTTON_SCAN, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonScan)
	ON_BN_CLICKED(IDC_BUTTON_RECYCLESAVEFOLDER, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonRecyclesavefolder)
	ON_BN_CLICKED(IDC_BUTTON_MERGEPDF, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonMergepdf)
	ON_BN_CLICKED(IDC_BUTTON_REGISTER, &CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonRegister)
END_MESSAGE_MAP()


// CLibWFXDemoDlg_NonBlockMode message handlers
void CLibWFXDemoDlg_NonBlockMode::LibWFXEVENTCB(ENUM_LIBWFX_EVENT_CODE enEventCode, int nParam, void* pUserDef)
{
	CLibWFXDemoDlg_NonBlockMode* pLibWFXDemoDlg = (CLibWFXDemoDlg_NonBlockMode *)pUserDef;
#if 0
	if (enEventCode == LIBWFX_EVENT_PAPER_DETECTED)
	{
		if (::MessageBox(NULL, _T("Scan ?"), _T("Message"), MB_YESNO) == IDYES)
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_STARTSCAN, NULL, NULL);
	}
#endif
	switch (enEventCode)
	{
	case LIBWFX_EVENT_PAPER_DETECTED:
		//pLibWFXDemoDlg->WriteLog(_T("LIBWFX_EVENT_PAPER_DETECTED"));
		if (nParam == 3)
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_PAPER_DETECTED"), NULL);
		else if (nParam == 1)
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_PASSPORT_DETECTED"), NULL);
		else if (nParam == 2)
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_CARD_DETECTED"), NULL);
		break;
	case LIBWFX_EVENT_NO_PAPER:
		//pLibWFXDemoDlg->WriteLog(_T("LIBWFX_EVENT_NO_PAPER"));
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_NO_PAPER", LIBWFX_EVENT_NO_PAPER);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_PAPER_JAM:
		//pLibWFXDemoDlg->WriteLog(_T("LIBWFX_EVENT_PAPER_JAM"));		
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_PAPER_JAM", LIBWFX_EVENT_PAPER_JAM);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_MULTIFEED:
		//pLibWFXDemoDlg->WriteLog(_T("LIBWFX_EVENT_MULTIFEED"));		
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_MULTIFEED", LIBWFX_EVENT_MULTIFEED);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_NO_CALIBRATION_DATA:
		//pLibWFXDemoDlg->WriteLog(_T("LIBWFX_EVENT_NO_CALIBRATION_DATA"));	
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_NO_CALIBRATION_DATA", LIBWFX_EVENT_NO_CALIBRATION_DATA);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_WARMUP_COUNTDOWN:
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_SHOW_WARMUP, (WPARAM)nParam, NULL);
		break;
	case LIBWFX_EVENT_SCAN_PROGRESS:
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_SCAN_PROGRESS, (WPARAM)nParam, NULL);
		break;
	case LIBWFX_EVENT_BUTTON_DETECTED:
		//when press the scan button on machine, it's callback the number of control panel
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("%d"), nParam);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		theApp.m_pMainWnd->PostMessage(WM_LIBWFX_STARTSCAN, NULL, NULL);
		break;
	case LIBWFX_EVENT_PAPER_FEEDING_ERROR:
		//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_PAPER_FEEDING_ERROR"), NULL);	
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_PAPER_FEEDING_ERROR", LIBWFX_EVENT_PAPER_FEEDING_ERROR);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_SETAUTOSCANOFF, NULL, NULL);
		break;
	case LIBWFX_EVENT_UVSECURITY_DETECTED:
		if (nParam == 0)
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_UVSECURITY_DETECTED[0]"), NULL);
		else
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_UVSECURITY_DETECTED[1]"), NULL);
		break;
	case LIBWFX_EVENT_LEFT_SENSOR_DETECTED:
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_LEFT_SENSOR_DETECTED"), NULL);
		break;
	case LIBWFX_EVENT_RIGHT_SENSOR_DETECTED:
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_RIGHT_SENSOR_DETECTED"), NULL);
		break;
	case LIBWFX_EVENT_ALL_SENSOR_DETECTED:
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("LIBWFX_EVENT_ALL_SENSOR_DETECTED"), NULL);
		break;
	case LIBWFX_EVENT_PLUG_UNPLUG:
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_PLUG_UNPLUG", LIBWFX_EVENT_PLUG_UNPLUG);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_COVER_OPEN:
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_COVER_OPEN", LIBWFX_EVENT_COVER_OPEN);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_OVER_TIME_SCAN:
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_OVER_TIME_SCAN", LIBWFX_EVENT_OVER_TIME_SCAN);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_CANCEL_SCAN:
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_CANCEL_SCAN", LIBWFX_EVENT_CANCEL_SCAN);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_CAMERA_RGB_DISLOCATION:
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_CAMERA_RGB_DISLOCATION", LIBWFX_EVENT_CAMERA_RGB_DISLOCATION);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	case LIBWFX_EVENT_CAMERA_TIMEOUT:
		pLibWFXDemoDlg->m_szEventMsg.Format(_T("[ Notice ] %s - [%d]"), L"LIBWFX_EVENT_CAMERA_TIMEOUT", LIBWFX_EVENT_CAMERA_TIMEOUT);
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(pLibWFXDemoDlg->m_szEventMsg.GetString()), NULL);
		break;
	default:
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)_T("[ Notice ] Undefined Event"), NULL);
		break;
	}
}

void CLibWFXDemoDlg_NonBlockMode::LibWFXCB(ENUM_LIBWFX_NOTIFY_CODE enNotifyCode, void* pUserDef, void* pParam1, void* pParam2)
{
	CLibWFXDemoDlg_NonBlockMode* pLibWFXDemoDlg = (CLibWFXDemoDlg_NonBlockMode *)pUserDef;
	EnterCriticalSection(&pLibWFXDemoDlg->m_CriticalSecion);

	if (enNotifyCode == LIBWFX_NOTIFY_IMAGE_DONE)
	{
		pLibWFXDemoDlg->SendMessage(WM_LIBWFX_CLEAN_DRAW, NULL, NULL);
		if (pParam1)
		{
			CString szCommand;
			(pLibWFXDemoDlg->GetDlgItem(IDC_COMBO_COMMAND))->GetWindowText(szCommand);

			if (szCommand.Find(_T("\"rawdata\":true")) == -1)
			{
				//pLibWFXDemoDlg->WriteLog((wchar_t *)pParam1);
				CString szFilePath;
				szFilePath.Format(_T("%s\r\n"), (wchar_t *)pParam1);
				//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)pParam1, NULL);
				pLibWFXDemoDlg->vecNotifyMsg.push_back(szFilePath.GetString());
				//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITENOTIFYLOG, NULL, NULL);
				if (!wcsstr((wchar_t *)pParam1, L".pdf") && !wcsstr((wchar_t *)pParam1, L".tif") && !wcsstr(CharUpper((wchar_t *)pParam1), L"_PHOTO"))
				{
					CString* pPath = new CString((wchar_t*)pParam1);
					pLibWFXDemoDlg->PostMessage(WM_LIBWFX_SHOWIMAGE, 0, (LPARAM)pPath);
				}
			}
			else
			{
				pLibWFXDemoDlg->DrawImage((ST_IMAGE_INFO *)pParam1);
			}
		}

		if (pParam2 != NULL)
		{
			//pLibWFXDemoDlg->WriteLog((wchar_t *)pParam2);
			//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)pParam2, NULL);
			CString szOCRResult;
			szOCRResult.Format(_T("%s\r\n"), (wchar_t *)pParam2);
			//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(szOCRResult.GetString()), NULL);
			pLibWFXDemoDlg->vecNotifyMsg.push_back(szOCRResult.GetString());
			//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITENOTIFYLOG, NULL, NULL);
		}
	}
	else if (enNotifyCode == LIBWFX_NOTIFY_SHOWPATHONLY)
	{
		if (pParam1)
		{
			CString szFilePath;
			szFilePath.Format(_T("%s\r\n"), (wchar_t *)pParam1);
			//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(szFilePath.GetString()), NULL);
			pLibWFXDemoDlg->vecNotifyMsg.push_back(szFilePath.GetString());
			//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITENOTIFYLOG, NULL, NULL);
		}
	}
	else if (enNotifyCode == LIBWFX_NOTIFY_END)
	{
#if	BC_USDL_FIXFIELDVALUE
		bool bIsBCUSDL = std::any_of(pLibWFXDemoDlg->vecNotifyMsg.begin(), pLibWFXDemoDlg->vecNotifyMsg.end(), [&](const std::wstring& s) {
			return s.find(L"\"IIN\":\"636028\"") != std::wstring::npos;
		});
		if (bIsBCUSDL)
		{
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_FIXUSDLFIELDVALUE, (WPARAM)pLibWFXDemoDlg->vecNotifyMsg[0].c_str(), (LPARAM)pLibWFXDemoDlg->vecNotifyMsg[1].c_str());
		}
		else
		{
			pLibWFXDemoDlg->vecNotifyMsg.push_back(L"Status:[Scan End]\r\n");
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITENOTIFYLOG, NULL, NULL);
		}

#else
		pLibWFXDemoDlg->vecNotifyMsg.push_back(L"Status:[Scan End]\r\n");
		pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITENOTIFYLOG, NULL, NULL);
#endif	
#if DoResetIfExcept
		if (pLibWFXDemoDlg->m_bIPexception)
		{
			theApp.m_pMainWnd->PostMessage(WM_LIBWFX_SETPROPERTY, NULL, NULL);
			theApp.m_pMainWnd->PostMessage(WM_LIBWFX_STARTSCAN, NULL, NULL);
		}
#endif
	}
	else if (enNotifyCode == LIBWFX_NOTIFY_EXCEPTION)
	{
		pLibWFXDemoDlg->m_bIPexception = false;
		ENUM_LIBWFX_EXCEPTION_CODE enCode = (ENUM_LIBWFX_EXCEPTION_CODE)((int)pParam1);
		if (enCode == LIBWFX_EXC_TIFF_SAVE_FINSIHED || enCode == LIBWFX_EXC_PDF_SAVE_FINSIHED)
		{
			static CString szLog;
			szLog.Format(_T("%s[SAVE_FINISHED]"), (wchar_t *)pParam2);
			//pLibWFXDemoDlg->WriteLog(const_cast<TCHAR *>(szLog.GetString()));
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(szLog.GetString()), NULL);
		}
		else if (pParam2 != NULL)
		{
			//pLibWFXDemoDlg->WriteLog((wchar_t *)pParam2);
			//pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)pParam2, NULL);
			static CString szExceptionlog;
			szExceptionlog.Format(_T("%s"), (wchar_t *)pParam2);
			pLibWFXDemoDlg->PostMessage(WM_LIBWFX_WRITELOG, (WPARAM)const_cast<TCHAR *>(szExceptionlog.GetString()), NULL);
			if (enCode == LIBWFX_EXC_IP_EXCEPTION)   //already do StopScan			
				pLibWFXDemoDlg->m_bIPexception = true;
		}
	}
	LeaveCriticalSection(&pLibWFXDemoDlg->m_CriticalSecion);
}

BOOL CLibWFXDemoDlg_NonBlockMode::WriteNotifyLog()
{
	CString szContent;

	GetDlgItem(IDC_EDIT_LOG)->GetWindowText(szContent);

	for (int i = 0; i < vecNotifyMsg.size(); i++)
	{
		szContent.Append((wchar_t *)vecNotifyMsg.at(i).c_str());
	}
	GetDlgItem(IDC_EDIT_LOG)->SetWindowText(szContent);

	((CEdit *)GetDlgItem(IDC_EDIT_LOG))->LineScroll(((CEdit *)GetDlgItem(IDC_EDIT_LOG))->GetLineCount());

	CString szContent2;
	for (int i = 0; i < vecNotifyMsg.size(); i++)
	{
		m_pfnLibWFX_WriteAPLog((wchar_t *)vecNotifyMsg.at(i).c_str());
		szContent2.Append((wchar_t *)vecNotifyMsg.at(i).c_str());
	}
	vecNotifyMsg.clear();
	return TRUE;
}

BOOL CLibWFXDemoDlg_NonBlockMode::InitLib(VOID)
{	
	TCHAR szExeDirPath[AVI_MAXPATH_LEN] = { 0 };
	DWORD dwLen = GetModuleFileName(NULL, szExeDirPath, AVI_MAXPATH_LEN);
	while (dwLen-- > 0) {
		if (szExeDirPath[dwLen] == _T('\\')) {
			szExeDirPath[dwLen + 1] = 0;
			break;
		}
	}
	TCHAR szDLLPath[AVI_MAXPATH_LEN + 1];
	_stprintf_s(szDLLPath, _T("%s%s"), szExeDirPath, LIBWFX_DLLNAME);
	m_hLibWFX = LoadLibraryEx(szDLLPath, NULL, LOAD_WITH_ALTERED_SEARCH_PATH);

	if (m_hLibWFX == NULL)
	{
		TCHAR szSDKDLLPath[AVI_MAXPATH_LEN] = {0};
		if (GetSDKInstallPath(szSDKDLLPath, false))
		{
			if (szDLLPath[_tcslen(szSDKDLLPath) - 1] != _T('\\'))
			{
				_tcscat_s(szSDKDLLPath, _T("\\"));
			}
			_tcscat_s(szSDKDLLPath, LIBWFX_DLLNAME);
			m_hLibWFX = ::LoadLibrary(szSDKDLLPath);
		}
	}

	if (m_hLibWFX == NULL)
	{	
		::MessageBoxW(NULL, L"Library loading failed. Please ensure that the SDK installation package is correctly installed!", L"Warning", MB_OK | MB_ICONEXCLAMATION);
		SendMessage(WM_CLOSE, NULL, NULL);
		return FALSE;
	}

	m_pfnLibWFX_Init = (LIBWFX_INIT)::GetProcAddress(m_hLibWFX, LIBWFX_API_INIT);
	if (m_pfnLibWFX_Init == NULL)
	{
		WriteLog(_T("Status:[Load m_pfnLibWFX_Init Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_InitEx = (LIBWFX_INITEX)::GetProcAddress(m_hLibWFX, LIBWFX_API_INITEX);
	if (m_pfnLibWFX_InitEx == NULL)
	{
		WriteLog(_T("Status:[Load m_pfnLibWFX_InitEx Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_DeInit = (LIBWFX_DEINIT)::GetProcAddress(m_hLibWFX, LIBWFX_API_DEINIT);
	if (m_pfnLibWFX_DeInit == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_DEINIT Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_GetDevicesList = (LIBWFX_GETDEVICESLIST)::GetProcAddress(m_hLibWFX, LIBWFX_API_GET_DEVICESLIST);
	if (m_pfnLibWFX_GetDevicesList == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_GET_DEVICESLIST Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_GetDevicesListWithSerial = (LIBWFX_GETDEVICESLIST_WITHSERIAL)::GetProcAddress(m_hLibWFX, LIBWFX_API_GET_DEVICESLIST_WITHSERIAL);
	if (m_pfnLibWFX_GetDevicesListWithSerial == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_GET_DEVICESLIST_WITHSERIAL Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_SetProperty = (LIBWFX_SETPROPERTY)::GetProcAddress(m_hLibWFX, LIBWFX_API_SET_PROPERTY);
	if (m_pfnLibWFX_SetProperty == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_SET_PROPERTY Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_StartScan = (LIBWFX_STARTSCAN)::GetProcAddress(m_hLibWFX, LIBWFX_API_START_SCAN);
	if (m_pfnLibWFX_StartScan == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_START_SCAN Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_Calibrate = (LIBWFX_CALIBRATE)::GetProcAddress(m_hLibWFX, LIBWFX_API_CALIBRATE);
	if (m_pfnLibWFX_Calibrate == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_CALIBRATE Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_ECOControl = (LIBWFX_ECOCONTROL)::GetProcAddress(m_hLibWFX, LIBWFX_API_ECOCONTROL);
	if (m_pfnLibWFX_ECOControl == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_ECOCONTROL Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_PaperReady = (LIBWFX_PAPERREADY)::GetProcAddress(m_hLibWFX, LIBWFX_API_PAPER_READY);
	if (m_pfnLibWFX_PaperReady == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_PAPERREADY Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_CloseDevice = (LIBWFX_CLOSEDEVICE)::GetProcAddress(m_hLibWFX, LIBWFX_API_CLOSE_DEVICE);
	if (m_pfnLibWFX_CloseDevice == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_CLOSE_DEVICE Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_EjectPaperControl = (LIBWFX_EJECTPAPER_CONTORL)::GetProcAddress(m_hLibWFX, LIBWFX_API_EJECTPAPER_CONTORL);
	if (m_pfnLibWFX_EjectPaperControl == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_EJECTPAPER_CONTORL Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_EjectPaperControlWithMsg = (LIBWFX_EJECTPAPER_CONTORL_WITHMSG)::GetProcAddress(m_hLibWFX, LIBWFX_API_EJECTPAPER_CONTORL_WITHMSG);
	if (m_pfnLibWFX_EjectPaperControlWithMsg == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_EJECTPAPER_CONTORL_WITHMSG Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_GetPaperStatus = (LIBWFX_GETPAPERSTATUS)::GetProcAddress(m_hLibWFX, LIBWFX_API_GET_PAPERSTATUS);
	if (m_pfnLibWFX_GetPaperStatus == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_GETPAPERSTATUS Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_IsWindowExist = (LIBWFX_ISWINDOWEXIST)::GetProcAddress(m_hLibWFX, LIBWFX_API_ISWINDOWEXIST);
	if (m_pfnLibWFX_IsWindowExist == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_ISWINDOWEXIST Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_GetLastErrorCode = (LIBWFX_GETLASTERRORCODE)::GetProcAddress(m_hLibWFX, LIBWFX_API_GETLASTERRORCODE);
	if (m_pfnLibWFX_GetLastErrorCode == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_GETLASTERRORCODE Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_SynchronizeScan = (LIBWFX_SYNCHRONIZESCAN)::GetProcAddress(m_hLibWFX, LIBWFX_API_SYNCHRONIZESCAN);
	if (m_pfnLibWFX_SynchronizeScan == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_SYNCHRONIZESCAN Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_GetCertificatePermission = (LIBWFX_GETCERTIFICATEPERMISSION)::GetProcAddress(m_hLibWFX, LIBWFX_API_GETCERTIFICATEPERMISSION);
	if (m_pfnLibWFX_GetCertificatePermission == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_GETCERTIFICATEPERMISSION Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_RecycleSaveFolder = (LIBWFX_RECYCLESAVEFOLDER)::GetProcAddress(m_hLibWFX, LIBWFX_API_RECYCLESAVEFOLDER);
	if (m_pfnLibWFX_RecycleSaveFolder == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_RECYCLESAVEFOLDER Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_MergeToPdf = (LIBWFX_MERGETOPDF)::GetProcAddress(m_hLibWFX, LIBWFX_API_MERGETOPDF);
	if (m_pfnLibWFX_MergeToPdf == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_MERGETOPDF Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_WriteAPLog = (LIBWFX_WRITEAPLOG)::GetProcAddress(m_hLibWFX, LIBWFX_API_WRITEAPLOG);
	if (m_pfnLibWFX_WriteAPLog == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_WRITEAPLOG Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_GetDeviceCapability = (LIBWFX_GETDEVICECAPABILITY)::GetProcAddress(m_hLibWFX, LIBWFX_API_GETDEVICECAPABILITY);
	if (m_pfnLibWFX_GetDeviceCapability == NULL)
	{
		WriteLog(_T("Status:[Get LIBWFX_API_GETDEVICECAPABILITY Fail]"));
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	WriteLog(_T("Status:[Load LibWebFXScan Success]"));
	m_nCount = 0;
	return TRUE;
}

BOOL CLibWFXDemoDlg_NonBlockMode::WriteLog(TCHAR* szMsg)
{
	CString szContent;

	GetDlgItem(IDC_EDIT_LOG)->GetWindowText(szContent);
	szContent.Append(szMsg);
	szContent.Append(_T("\r\n"));
	GetDlgItem(IDC_EDIT_LOG)->SetWindowText(szContent);
	((CEdit *)GetDlgItem(IDC_EDIT_LOG))->LineScroll(((CEdit *)GetDlgItem(IDC_EDIT_LOG))->GetLineCount());
	if (m_pfnLibWFX_WriteAPLog)
	{
		CString szContent2;
		szContent2.Append(szMsg);
		szContent2.Append(_T("\r\n"));
		m_pfnLibWFX_WriteAPLog((wchar_t *)szContent2.GetString());
	}
	return TRUE;
}

char* CLibWFXDemoDlg_NonBlockMode::FixUSDLFieldValueToJsonFile(const char* szFilePath, const char* szOCRData)
{
	const char* targetKey = "\"SecurityFunction\":";
	const char* pTarget = strstr(szOCRData, targetKey);
	const char* pInsertAfter = nullptr;

	if (pTarget != NULL) {
		const char* pValueStart = strchr(pTarget + strlen(targetKey), '\"');
		if (pValueStart) {
			const char* pValueEnd = strchr(pValueStart + 1, '\"');
			if (pValueEnd) {
				pInsertAfter = pValueEnd + 1;
			}
		}
	}

	if (pInsertAfter == NULL) {
		const char* fallbackKey = "\"CardExpiryDate\":";
		const char* pFallback = strstr(szOCRData, fallbackKey);
		if (pFallback != NULL) {
			const char* pValueStart = strchr(pFallback + strlen(fallbackKey), '\"');
			if (pValueStart) {
				const char* pValueEnd = strchr(pValueStart + 1, '\"');
				if (pValueEnd) {
					pInsertAfter = pValueEnd + 1;
				}
			}
		}
	}

	if (pInsertAfter == NULL) {
		return _strdup(szOCRData);
	}

	const char* key = "\"CardExpiryDate\":";
	const char* pKey = strstr(szOCRData, key);
	char cardExpiry[32] = { 0 };

	if (pKey != NULL) {
		const char* pValStart = strchr(pKey + strlen(key), '\"');
		if (pValStart) {
			pValStart++;
			const char* pValEnd = strchr(pValStart, '\"');
			if (pValEnd) {
				int valLen = pValEnd - pValStart;
				if (valLen < sizeof(cardExpiry)) {
					strncpy_s(cardExpiry, sizeof(cardExpiry), pValStart, valLen);
				}
			}
		}
	}

	char newField[128] = { 0 };
	if (pKey != NULL && cardExpiry[0] != '\0') {
		sprintf_s(newField, sizeof(newField), ",\"ExpiryDateExt\":\"20%s\"", cardExpiry);
	}
	else {
		sprintf_s(newField, sizeof(newField), ",\"ExpiryDateExt\":\"\"");
	}

	size_t prefixLen = pInsertAfter - szOCRData;
	size_t suffixLen = strlen(pInsertAfter);

	char* szFinalJson = (char*)malloc(prefixLen + strlen(newField) + suffixLen + 1);

	if (!szFinalJson) {
		return _strdup(szOCRData);
	}

	memcpy(szFinalJson, szOCRData, prefixLen);
	memcpy(szFinalJson + prefixLen, newField, strlen(newField));
	memcpy(szFinalJson + prefixLen + strlen(newField), pInsertAfter, suffixLen);
	szFinalJson[prefixLen + strlen(newField) + suffixLen] = '\0';

	char szJsonPath[512];
	strcpy_s(szJsonPath, sizeof(szJsonPath), szFilePath);
	char* dot = strrchr(szJsonPath, '.');
	if (dot) *dot = '\0';
	strcat_s(szJsonPath, sizeof(szJsonPath), "_OCR.json");

	FILE* fp = NULL;
	errno_t err = fopen_s(&fp, szJsonPath, "w");
	if (err == 0 && fp != NULL) {
		fputs(szFinalJson, fp);
		fclose(fp);
		return szFinalJson;
	}

	return _strdup(szOCRData);
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnFixUSDLFieldValueToJsonFile(WPARAM wParam, LPARAM lParam)
{
	LPCTSTR pWPath = (LPCTSTR)wParam;
	LPCTSTR pWData = (LPCTSTR)lParam;

	CStringA szFilePathA(pWPath);
	CStringA szOcrDataA(pWData);
	char* szfinalJson = FixUSDLFieldValueToJsonFile(szFilePathA.GetString(), szOcrDataA.GetString());
	CString szOcrData(szfinalJson);
	if (szfinalJson != NULL) {
		CString szOcrData(szfinalJson);
		vecNotifyMsg.clear();
		vecNotifyMsg.push_back(pWPath);
		vecNotifyMsg.push_back(szOcrData.GetString());
		vecNotifyMsg.push_back(L"Status:[Scan End]\r\n");
		WriteNotifyLog();
		free(szfinalJson);
	}
	return 0;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnShowImage(WPARAM, LPARAM lParam)
{
	std::unique_ptr<CString> path((CString*)lParam);
	ShowImage((TCHAR*)(LPCTSTR)(*path));
	return 0;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnSetAutoScanOff(WPARAM wPararm, LPARAM lParam)
{
	CString szCommand;
	GetDlgItem(IDC_COMBO_COMMAND)->GetWindowText(szCommand);
	//exclude VTM300
	if ((szCommand.Find(_T("\"device-name\":\"776U\"")) != -1 || szCommand.Find(_T("\"device-name\":\"778U\"")) != -1) && szCommand.Find(_T("\"fastscan\":true")) == -1)
		return 0;
	szCommand.Replace(L"\"autoscan\":true", L"\"autoscan\":false");
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_SetProperty((wchar_t *)szCommand.GetString(), LibWFXEVENTCB, this);
	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		return 0;
	}
	return 0;
}



BOOL CLibWFXDemoDlg_NonBlockMode::ShowImage(TCHAR* szFilePath)
{
	CImage image;
	if (image.Load(szFilePath) != S_OK) return FALSE;
	if (image.GetWidth() == 0 || image.GetHeight() == 0) return FALSE;

	CWnd* pWnd = GetDlgItem((m_nCount++ % 2) ? IDC_STATIC_IMG_2 : IDC_STATIC_IMG_1);
	if (!pWnd) return FALSE;

	CRect rcImage;
	pWnd->GetClientRect(&rcImage);

	int nRatioWidth = rcImage.Height() * image.GetWidth() / image.GetHeight();
	int nHRatioHeight = rcImage.Width() * image.GetHeight() / image.GetWidth();

	if (rcImage.Width() > nRatioWidth) {
		rcImage.left = (rcImage.Width() - nRatioWidth) / 2;
		rcImage.right = rcImage.left + nRatioWidth;
	}
	if (rcImage.Height() > nHRatioHeight) {
		rcImage.top = (rcImage.Height() - nHRatioHeight) / 2;
		rcImage.bottom = rcImage.top + nHRatioHeight;
	}

	rcImage.DeflateRect(5, 5);
	if (rcImage.Width() <= 0 || rcImage.Height() <= 0) return FALSE;

	CDC* pDC = pWnd->GetWindowDC();
	if (!pDC) return FALSE;

	pDC->SetStretchBltMode(COLORONCOLOR);
	image.Draw(pDC->m_hDC, rcImage);
	pWnd->ReleaseDC(pDC);
	return TRUE;
}

BOOL CLibWFXDemoDlg_NonBlockMode::DrawImage(ST_IMAGE_INFO* pstImgInfo)
{
	BITMAPINFO* pstBitmapInfo = NULL;

	if (pstImgInfo->enColorMode == LIBWFX_COLOR_MODE_BW)
		pstBitmapInfo = (BITMAPINFO *)malloc(sizeof(BITMAPINFOHEADER) + sizeof(RGBQUAD) * (1 << 1));
	else if (pstImgInfo->enColorMode == LIBWFX_COLOR_MODE_GRAY)
		pstBitmapInfo = (BITMAPINFO *)malloc(sizeof(BITMAPINFOHEADER) + sizeof(RGBQUAD) * (1 << 8));
	else
		pstBitmapInfo = (BITMAPINFO *)malloc(sizeof(BITMAPINFO));

	pstBitmapInfo->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	pstBitmapInfo->bmiHeader.biWidth = pstImgInfo->ulPixel;
	pstBitmapInfo->bmiHeader.biHeight = -(int)pstImgInfo->ulLine;
	pstBitmapInfo->bmiHeader.biPlanes = 1;

	if (pstImgInfo->enColorMode == LIBWFX_COLOR_MODE_BW) pstBitmapInfo->bmiHeader.biBitCount = 1;
	else if (pstImgInfo->enColorMode == LIBWFX_COLOR_MODE_GRAY) pstBitmapInfo->bmiHeader.biBitCount = 8;
	else pstBitmapInfo->bmiHeader.biBitCount = 24;

	pstBitmapInfo->bmiHeader.biCompression = BI_RGB;
	pstBitmapInfo->bmiHeader.biSizeImage = 0;
	pstBitmapInfo->bmiHeader.biXPelsPerMeter = 0;
	pstBitmapInfo->bmiHeader.biYPelsPerMeter = 0;
	pstBitmapInfo->bmiHeader.biClrUsed = 0;
	pstBitmapInfo->bmiHeader.biClrImportant = 0;

	if (pstBitmapInfo->bmiHeader.biBitCount == 1)
	{
		pstBitmapInfo->bmiColors[0].rgbRed = 0;
		pstBitmapInfo->bmiColors[0].rgbGreen = 0;
		pstBitmapInfo->bmiColors[0].rgbBlue = 0;
		pstBitmapInfo->bmiColors[0].rgbReserved = 0;
		pstBitmapInfo->bmiColors[1].rgbRed = 255;
		pstBitmapInfo->bmiColors[1].rgbGreen = 255;
		pstBitmapInfo->bmiColors[1].rgbBlue = 255;
		pstBitmapInfo->bmiColors[1].rgbReserved = 0;
	}

	if (pstBitmapInfo->bmiHeader.biBitCount == 8)
	{
		for (int nColor = 0; nColor < (1 << pstBitmapInfo->bmiHeader.biBitCount); nColor++)
		{
			pstBitmapInfo->bmiColors[nColor].rgbRed = nColor;
			pstBitmapInfo->bmiColors[nColor].rgbGreen = nColor;
			pstBitmapInfo->bmiColors[nColor].rgbBlue = nColor;
			pstBitmapInfo->bmiColors[nColor].rgbReserved = 0;
		}
	}

	CWnd* wndItem = GetDlgItem((m_nCount++ % 2) ? IDC_STATIC_IMG_2 : IDC_STATIC_IMG_1);

	//wndItem->Invalidate(false);

	HDC hdc = ::GetDC(wndItem->GetSafeHwnd());
	RECT rect;
	ULONG nLeft, nTop;
	ULONG nWidth, nHeight;
	ULONG nRatioWidth, nHRatioHeight;
	::GetWindowRect(wndItem->GetSafeHwnd(), &rect);
	nLeft = 0;
	nTop = 0;
	nWidth = rect.right - rect.left - 5;
	nHeight = rect.bottom - rect.top - 5;
	nRatioWidth = nHeight * pstImgInfo->ulPixel / pstImgInfo->ulLine;
	nHRatioHeight = nWidth * pstImgInfo->ulLine / pstImgInfo->ulPixel;
	if (nWidth > nRatioWidth)
	{
		nWidth = nRatioWidth;
		nLeft = (rect.right - rect.left - nWidth) / 2;
	}
	if (nHeight > nHRatioHeight)
	{
		nHeight = nHRatioHeight;
		nTop = (rect.bottom - rect.top - nHeight) / 2;
	}
	SetStretchBltMode(hdc, COLORONCOLOR);
	StretchDIBits(hdc, nLeft, nTop + 5, nWidth, nHeight - 5, 0, 0, pstImgInfo->ulPixel, pstImgInfo->ulLine, pstImgInfo->pRawDate, pstBitmapInfo, DIB_RGB_COLORS, SRCCOPY);

	free(pstBitmapInfo);
	::ReleaseDC(wndItem->GetSafeHwnd(), hdc);

	return TRUE;
}

BOOL CLibWFXDemoDlg_NonBlockMode::GetJsonString(CString szDevName)
{
	CString szDefJson;
	szDefJson.Empty();

	USES_CONVERSION;
	if (GetCommandString(T2W(szDevName.GetBuffer())))
	{
		return TRUE;
	}
	else
	{
		szDefJson.Append(_T("{\"device-name\":\""));
		szDefJson.Append(szDevName);
		ENUM_SOURCETYPE enSource = UNKNOWN;
		ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_GetDeviceCapability(T2W(szDevName.GetBuffer()), &enSource, NULL, NULL, NULL, NULL, NULL, NULL);

		if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
		{
			CString szErr;
			wchar_t szErrorMsg[MAX_PATH] = { 0 };
			m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
			szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));
			return TRUE;
		}
		else if (enSource == SHEETFED)
			szDefJson.Append(_T("\",\"source\":\"Sheetfed-Front\",\"autoscan\":true}"));
		else if (enSource == FLATBED)
			szDefJson.Append(_T("\",\"source\":\"Flatbed\"}"));
		else if (enSource == CAMERA)
			szDefJson.Append(_T("\",\"source\":\"Camera\",\"autoscan\":true,\"recognize-type\":\"passport\"}"));
		else if (enSource == ADF || enSource == ADF_SHEETFED || enSource == ADF_FLATBED)
			szDefJson.Append(_T("\",\"source\":\"ADF-Duplex\",\"autoscan\":true}"));
		else
			return TRUE;
	}
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->AddString(szDefJson);
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->SetCurSel(0);
	return TRUE;
}

BOOL CLibWFXDemoDlg_NonBlockMode::InitDevicesList(VOID)
{
	((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->ResetContent();

	const wchar_t* szDevicesList = NULL;
	const wchar_t* szSerialList = NULL;
	//ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_GetDevicesList(&szDevicesList);
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_GetDevicesListWithSerial(&szDevicesList, &szSerialList);
	if (enErrCode == LIBWFX_ERRCODE_SUCCESS)
	{
		json_t* root;
		json_t* root2;
		json_error_t error;
		USES_CONVERSION;
		root = json_loads(W2A(szDevicesList), 0, &error);
		root2 = json_loads(W2A(szSerialList), 0, &error);
		for (unsigned int nIdx = 0; nIdx < json_array_size(root); nIdx++)
		{
			json_t *data;
			data = json_array_get(root, nIdx);

			json_t *data2;
			data2 = json_array_get(root2, nIdx);

			USES_CONVERSION;
			((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->AddString(A2T(json_string_value(data)));


			CString szTmp;
			szTmp.Format(_T("Device: %s  Serial Number: %s"), A2T(json_string_value(data)), A2T(json_string_value(data2)));
			WriteLog(const_cast<TCHAR *>(szTmp.GetString()));
		}
		json_decref(root);

		((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->SetCurSel(0);

		CString szDefaultDev;
		CString szGetItemText;

		int nSelIdx = ((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetCurSel();
		((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetLBText(nSelIdx, szGetItemText);
		szDefaultDev.Append(szGetItemText);
		GetJsonString(szGetItemText);
		//GetDlgItem(IDC_EDIT_COMMAND)->SetWindowText(szDefaultDev);

		//TODO, handle szSerialList
	}
	else
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		if (LIBWFX_ERRCODE_NO_INIT == enErrCode || LIBWFX_ERRCODE_LOAD_MRTD_DLL_FAIL == enErrCode || LIBWFX_ERRCODE_SCANNING == enErrCode)
		{			
			m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
			szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		}
		else
		{
			m_pfnLibWFX_GetLastErrorCode(LIBWFX_ERRCODE_NO_DEVICES, szErrorMsg);
			szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, LIBWFX_ERRCODE_NO_DEVICES);		
		}
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}

	return TRUE;
}


//check command.txt
BOOL CLibWFXDemoDlg_NonBlockMode::GetCommandString(wchar_t* DevName)
{
	TCHAR szIniPath[MAX_PATH];
	//memset(Command, '\0', sizeof(Command));
	USES_CONVERSION;
	LPCTSTR   szDevName = W2T(DevName);   //   wchar   ->   tchar 	

	SHGetFolderPath(NULL, CSIDL_COMMON_APPDATA, NULL, 0, szIniPath);  //C:\ProgramData\Plustek\

	if (szIniPath[_tcslen(szIniPath) - 1] != _T('\\'))
		lstrcat(szIniPath, _T("\\"));
	lstrcat(szIniPath, _T("Plustek\\"));
	lstrcat(szIniPath, szDevName);

	if (GetFileAttributes(szIniPath) == INVALID_FILE_ATTRIBUTES)	//directory not exist
		::CreateDirectory(szIniPath, NULL);

	lstrcat(szIniPath, _T("\\Command.txt"));

	int   nUTF8Len = WideCharToMultiByte(CP_UTF8, 0, szIniPath, -1, NULL, 0, NULL, NULL);
	char* pszUTF8 = new char[nUTF8Len + 1];
	WideCharToMultiByte(CP_UTF8, 0, szIniPath, -1, pszUTF8, nUTF8Len, NULL, NULL);

	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->ResetContent();
	std::ifstream input(szIniPath);
	std::string line;
	int lineidx = 0;
	int current = 0;
	int next;
	vector<string> buf;

	if (input.bad())
		return false;

	while (std::getline(input, line))
	{
		current = 0;
		while (1)
		{
			lineidx++;
			//do something with the line
			next = line.find_first_of("\r", current);
			if (next != current)
			{
				string tmp = line.substr(current, next - current);
				if (tmp.size() != 0)
				{
					buf.push_back(tmp);
				}
			}
			if (next == string::npos) break;
			current = next + 1;
		}
	}
	if (buf.size() == 0)
		return false;

	for (vector<string>::size_type idx = 0; idx != buf.size() && idx < m_nCmdMaxNum; ++idx)
	{
		std::string wstbuffer(buf[idx].begin(), buf[idx].end());
		std::string retbuffer;
		retbuffer.clear();

		//if (wstbuffer.at(0) != '{')
		//	break;

		int pos = wstbuffer.find('}', 0);
		wstbuffer.erase(wstbuffer.begin() + pos + 1, wstbuffer.end());

		std::string subtoken;//, subtoken2;
		int nPos = 0;
		for (nPos = 0; nPos < wstbuffer.length(); nPos++)
		{
			if (wstbuffer.at(nPos) == '"')
				retbuffer.append("\"");
			else if (wstbuffer.at(nPos) == ':')
				retbuffer.append(":");
			else if (wstbuffer.at(nPos) == ' ')
				retbuffer.append(" ");
			else if (wstbuffer.at(nPos) == ',')
				retbuffer.append(",");
			else if (wstbuffer.at(nPos) == '-')
				retbuffer.append("-");
			else
			{
				subtoken = wstbuffer.substr(nPos, 1);
				retbuffer.append(subtoken);
			}
		}
		CString szDefJson(retbuffer.c_str());
		((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->AddString(szDefJson.GetString());
	}
	input.close();
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->SetCurSel(0);

	if (pszUTF8)
		delete[] pszUTF8;
	return true;
}

//write command to command.txt
BOOL CLibWFXDemoDlg_NonBlockMode::SetCommandString(wchar_t* DevName, CString Command)
{
	//Confirm whether the devicename in the command is correct
	int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)DevName, -1, NULL, 0, NULL, NULL);
	char* pszUTF8IP = new char[nUTF8LenIP + 1];
	WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)DevName, -1, pszUTF8IP, nUTF8LenIP, NULL, NULL);
	CString devname(pszUTF8IP);
	if (Command.Find(devname) == -1)
		return false;

	CString szGetCmd;
	char szPath[MAX_PATH];
	TCHAR szIniPath[MAX_PATH];
	USES_CONVERSION;
	LPCTSTR   szDevName = W2T(DevName);   //   wchar   ->   tchar 	
	SHGetFolderPath(NULL, CSIDL_COMMON_APPDATA, NULL, 0, szIniPath);  //C:\ProgramData\Plustek\

	if (szIniPath[_tcslen(szIniPath) - 1] != _T('\\'))
		lstrcat(szIniPath, _T("\\"));
	lstrcat(szIniPath, _T("Plustek\\"));
	lstrcat(szIniPath, szDevName);
	lstrcat(szIniPath, _T("\\Command.txt"));

	int   nUTF8Len = WideCharToMultiByte(CP_UTF8, 0, szIniPath, -1, NULL, 0, NULL, NULL);
	char* pszUTF8 = new char[nUTF8Len + 1];
	WideCharToMultiByte(CP_UTF8, 0, szIniPath, -1, pszUTF8, nUTF8Len, NULL, NULL);

	if (pszUTF8IP)
		delete[] pszUTF8IP;

	vector<CString> cmdtmp;	
	int cmdnum = ((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->GetCount();
	if (cmdnum < 1)
		return false;

	for (int nSelIdx = 0; nSelIdx < cmdnum; nSelIdx++)
	{
		if (nSelIdx == (m_nCmdMaxNum - 1) || (((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->GetCurSel() == nSelIdx && nSelIdx != 0))
			continue;
		CString tmp;
		((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->GetLBText(nSelIdx, tmp);
		szGetCmd.Append(tmp);
		szGetCmd.Append(L"\r");
		cmdtmp.push_back(tmp);
	}

	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->ResetContent();
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->AddString(Command);
	for (int i = 0; i < cmdtmp.size(); i++)
		((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->AddString(cmdtmp[i]);

	Command.Append(L"\r");
	Command.Append(szGetCmd);

	int size_needed = WideCharToMultiByte(CP_ACP, 0, Command.GetString(), -1, NULL, 0, NULL, NULL);
	char* buffer = new char[size_needed];
	WideCharToMultiByte(CP_ACP, 0, Command.GetString(), -1, buffer, size_needed, NULL, NULL);

	std::ofstream outFile(pszUTF8, std::ios::binary);
	if (outFile) {
		outFile.write(buffer, size_needed - 1);
		outFile.close();
	}
	if (buffer)
		delete[] buffer;
	if (pszUTF8)
		delete[] pszUTF8;
	if (!outFile) {  //Error: Unable to open file for writing
		return false;
	}
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->SetCurSel(0);
	return true;
}

wchar_t* CLibWFXDemoDlg_NonBlockMode::rtrim(wchar_t *str)
{
	if (str == NULL || *str == '\0' || wcscmp(str, L" ") == 0)
	{
		return str;
	}
	int len = wcslen(str);
	wchar_t *p = str + len - 1;
	while (isspace(*p) && p >= str)
	{
		*p = '\0';
		--p;
	}
	return str;
}

void CLibWFXDemoDlg_NonBlockMode::GetCertificatePermission()
{
	wchar_t szPermissionTypeList[MAX_PATH] = { 0 };
	CString szErr;
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_GetCertificatePermission(szPermissionTypeList, LIBWFX_DATA_TYPE_REGINFO);
	if (enErrCode == LIBWFX_ERRCODE_SUCCESS)
	{
		int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szPermissionTypeList, -1, NULL, 0, NULL, NULL);
		char* pszUTF8IP = new char[nUTF8LenIP + 1];
		WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szPermissionTypeList, -1, pszUTF8IP, nUTF8LenIP, NULL, NULL);

		CString szErr2(pszUTF8IP);
		if (szErr2 != "")
			szErr.Format(_T("License: %s"), szErr2);
		else
			szErr.Format(_T("License: none"));
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));

		if (pszUTF8IP)
			delete[] pszUTF8IP;
	}
	else
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
}

BOOL CLibWFXDemoDlg_NonBlockMode::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	HWND hwnd = NULL;
	m_nCmdMaxNum = 5;
	ShowDlg(true, L"Init");
	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
								// TODO: Add extra initialization here
	HICON hicon = AfxGetApp()->LoadIconW(IDI_LICENSE);
	m_bt_register.SetIcon(hicon);
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject backwarding- force"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject forwarding- force"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject backwarding stop- force"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject forwarding stop- force"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject backwarding"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject forwarding"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject backwarding stop"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject forwarding stop"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject backwarding by steps"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->AddString(_T("eject forwarding by steps"));
	((CComboBox *)GetDlgItem(IDC_COMBO_EJECT_DIRECTION))->SetCurSel(0);
#ifdef _DEBUG
	AllocConsole();
#endif

	if (InitLib())
	{
		if (m_pfnLibWFX_IsWindowExist(L"") == true)
		{
			ShowDlg(false, L"Init");
			::MessageBoxW(hwnd, L"Please confirm whether the \"CheckWindowTitle\" parameter content in LibWebFxScan.ini are all closed!!", L"Warning", MB_OK | MB_ICONEXCLAMATION);
			CString szErr;
			szErr.Format(_T("[ Warning ] LIBWFX_ERRCODE_SPECIFIC_AP_OPENING - [%d]"), LIBWFX_ERRCODE_SPECIFIC_AP_OPENING);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));
			szErr.Format(_T("[ Warning ] LIBWFX_ERRCODE_NO_INIT - [%d]"), LIBWFX_ERRCODE_NO_INIT);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));			
			return FALSE;
		}
		//ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_Init();
		//since we can't debug OCR engine, for debuging UI flow, use LIBWFX_INIT_MODE_NOOCR
		//OCR will not work, but easier to debug UI while developing
#ifdef _DEBUG
		ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_InitEx(LIBWFX_INIT_MODE_NOOCR);
#else
		ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_InitEx(LIBWFX_INIT_MODE_NORMAL);
#endif

		if (enErrCode == LIBWFX_ERRCODE_SUCCESS)
		{
			InitDevicesList();
			GetCertificatePermission();
		}
		else if (enErrCode == LIBWFX_ERRCODE_NO_OCR)
		{
			WriteLog(_T("Status:[No Recognize Tool]"));
			InitDevicesList();
			GetCertificatePermission();
		}
		else if (enErrCode == LIBWFX_ERRCODE_NO_AVI_OCR)
		{
			WriteLog(_T("Status:[No AVI Recognize Tool]"));
			InitDevicesList();
			GetCertificatePermission();
		}
		else if (enErrCode == LIBWFX_ERRCODE_NO_DOC_OCR)
		{
			WriteLog(_T("Status:[No DOC Recognize Tool]"));
			InitDevicesList();
			GetCertificatePermission();
		}
		else
		{
			CString szErr;
			wchar_t szErrorMsg[MAX_PATH] = { 0 };
			m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
			szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);

			if (enErrCode == LIBWFX_ERRCODE_PATH_TOO_LONG)
				WriteLog(_T("Status:[Path Is Too Long (max limit: 130 bits)]"));
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		}
	}
	static_cast<CButton *>(GetDlgItem(IDC_BUTTON_SET_PROPERTY))->SetFocus();
	ShowDlg(false, L"Init");
	return TRUE; //  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.


LRESULT CLibWFXDemoDlg_NonBlockMode::OnStartScan(WPARAM wPararm, LPARAM lParam)
{
	CString szCommand;
	GetDlgItem(IDC_COMBO_COMMAND)->GetWindowText(szCommand);
	if (szCommand.Find(_T("\"recognize-type\":\"DocumentExtract\"")) != -1)
	{
		::MessageBox(NULL, _T("\"DocumentExtract\" is not supported in Non-Block Mode. Please modify LibWebFxScan.ini to enable Block Mode for this function."), _T("Message"), MB_YESNO);
		return 0;
	}

	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_StartScan(LibWFXCB, this);

	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}

	return 0;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnSetProperty(WPARAM wPararm, LPARAM lParam)
{
	OnBnClickedButtonSetProperty();

	return 0;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnWriteLog(WPARAM wPararm, LPARAM lParam)
{
	TCHAR* szMsg = (TCHAR *)wPararm;

	WriteLog(szMsg);

	return 0;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnWriteNotifyLog(WPARAM wPararm, LPARAM lParam)
{
	WriteNotifyLog();
	return 0;
}


LRESULT CLibWFXDemoDlg_NonBlockMode::OnHandleWarmupProgress(WPARAM wPararm, LPARAM lParam)
{
	if (!m_dlgWarmup && wPararm != 0)
	{
		m_dlgWarmup = new WarmupDlg();
		m_dlgWarmup->Create(WarmupDlg::IDD, this);
		m_dlgWarmup->ShowWindow(SW_SHOW);

		m_nWarmupTotalTime = wPararm;
	}

	if (m_dlgWarmup)
		m_dlgWarmup->SetWarmupProgressPos((100 - ((wPararm * 100) / m_nWarmupTotalTime)));

	if (wPararm == 0)
	{
		if (m_dlgWarmup)
		{
			int nResult = 0;
			m_dlgWarmup->EndDialog(nResult);
		}
		delete m_dlgWarmup;
		m_dlgWarmup = NULL;
	}

	return 1;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnHandleScanProgress(WPARAM wPararm, LPARAM lParam)
{
	CString szProgress;
	szProgress.Format(_T("%d%%"), wPararm);

	((CProgressCtrl *)GetDlgItem(IDC_PROGRESS_SCAN))->SetPos(wPararm);
	GetDlgItem(IDC_STATIC_PROGRESS)->SetWindowText(szProgress.GetString());
	return 1;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnHandleCleanDraw(WPARAM wPararm, LPARAM lParam)
{
	CWnd* wndItem = GetDlgItem((m_nCount % 2) ? IDC_STATIC_IMG_2 : IDC_STATIC_IMG_1);
	wndItem->Invalidate(FALSE);

	return 1;
}


LRESULT CLibWFXDemoDlg_NonBlockMode::OnCloseDevice(WPARAM wPararm, LPARAM lParam)
{
	if (m_hLibWFX)
	{
		m_pfnLibWFX_CloseDevice();
	}
	return 1;
}

LRESULT CLibWFXDemoDlg_NonBlockMode::OnEjectPaper(WPARAM wPararm, LPARAM lParam)
{
	if (m_hLibWFX)
	{
		const wchar_t* szErrorMsg = NULL;
		ENUM_LIBWFX_EJECT_DIRECTION enEjectDirect = (ENUM_LIBWFX_EJECT_DIRECTION)wPararm;
		ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_EjectPaperControlWithMsg(enEjectDirect, &szErrorMsg);

		int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, NULL, 0, NULL, NULL);

		if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
		{
			CString szErr;
			wchar_t szErrorMsg[MAX_PATH] = { 0 };
			m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
			szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		}
		else if (nUTF8LenIP > 1) //event happen
		{
			char* pszUTF8IP = new char[nUTF8LenIP + 1];
			WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, pszUTF8IP, nUTF8LenIP, NULL, NULL);
			CString szErr(pszUTF8IP);
			CString szErr2(pszUTF8IP);
			szErr.Format(_T("%s"), szErr2);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));

			if (pszUTF8IP)
				delete[] pszUTF8IP;
		}
		else
			WriteLog(_T("Status:[LibWFX_EjectPaperControl Success]"));
	}
	return 1;
}

void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonSetProperty()
{
	// TODO: Add your control notification handler code here
	CString szCommand;
	GetDlgItem(IDC_COMBO_COMMAND)->GetWindowText(szCommand);

	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_SetProperty((wchar_t *)szCommand.GetString(), LibWFXEVENTCB, this);

	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
	else
	{
		CString szGetItemText;
		//CString szCommand;

		int nSelIdx = ((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetCurSel();
		if (nSelIdx > -1)
			((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetLBText(nSelIdx, szGetItemText);
		//GetDlgItem(IDC_EDIT_COMMAND)->GetWindowText(szCommand);

		//if(((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->GetCurSel() == -1)
		SetCommandString(T2W(szGetItemText.GetBuffer()), T2W(szCommand.GetBuffer()));
		WriteLog(_T("Status:[Device Ready!]"));
	}
}


void CLibWFXDemoDlg_NonBlockMode::OnCbnSelchangeComboDeviceName()
{
	// TODO: Add your control notification handler code here
	CString szDefaultDev(_T("device_name="));
	CString szGetItemText;

	int nSelIdx = ((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetCurSel();
	((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetLBText(nSelIdx, szGetItemText);
	GetJsonString(szGetItemText);
	//GetDlgItem(IDC_EDIT_COMMAND)->SetWindowText(szDefaultDev);
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonRefresh()
{
	// TODO: Add your control notification handler code here
	InitDevicesList();
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonEditCmd()
{
	// TODO: Add your control notification handler code here
	typedef void(__stdcall* API_EDIT_COMMAND)(wchar_t*, wchar_t**);
	TCHAR szExeDirPath[AVI_MAXPATH_LEN] = { 0 };
	DWORD dwLen = GetModuleFileName(NULL, szExeDirPath, AVI_MAXPATH_LEN);
	while (dwLen-- > 0) {
		if (szExeDirPath[dwLen] == _T('\\')) {
			szExeDirPath[dwLen + 1] = 0;
			break;
		}
	}
	TCHAR szDLLPath[AVI_MAXPATH_LEN + 1];
	_stprintf_s(szDLLPath, _T("%s%s"), szExeDirPath, _T("CommandEditor.dll"));
	HMODULE hLib = LoadLibraryEx(szDLLPath, NULL, LOAD_WITH_ALTERED_SEARCH_PATH);

	if (hLib == NULL)
	{
		TCHAR szSDKDLLPath[AVI_MAXPATH_LEN] = { 0 };
		if (GetSDKInstallPath(szSDKDLLPath, false))
		{
			if (szDLLPath[_tcslen(szSDKDLLPath) - 1] != _T('\\'))
			{
				_tcscat_s(szSDKDLLPath, _T("\\"));
			}
			_tcscat_s(szSDKDLLPath, _T("CommandEditor.dll"));
			hLib = ::LoadLibrary(szSDKDLLPath);
		}
	}

	if (hLib == NULL)
	{
		WriteLog(_T("Status:[Load CommandEditor Fail]"));
		return;
	}
	API_EDIT_COMMAND pfn = (API_EDIT_COMMAND)::GetProcAddress(hLib, "EditCommand");
	if (pfn == NULL)
	{
		WriteLog(_T("Status:[Get CommandEditor API Fail]"));
		::FreeLibrary(hLib);
		return;
	}
	CString szCommand;
	GetDlgItem(IDC_COMBO_COMMAND)->GetWindowText(szCommand);


	wchar_t* szRtn = NULL;
	pfn((wchar_t *)szCommand.GetString(), &szRtn);
	if (szRtn != nullptr)
	{
		if (!wcscmp(L"Invalid JSON format", szRtn))
			::MessageBox(NULL, _T("Invalid JSON format"), _T("Message"), MB_YESNO);
		else
			GetDlgItem(IDC_COMBO_COMMAND)->SetWindowText(szRtn);
	}
	::FreeLibrary(hLib);

}

void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonEco()
{
	// TODO: Add your control notification handler code here
	unsigned long ulTime = 0;

	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_ECOControl(&ulTime, 0);

	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		return;
	}

	CECODlg ECODlg(ulTime);
	ECODlg.DoModal();

	m_pfnLibWFX_ECOControl(&ulTime, 1);
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonEject()
{
	const wchar_t* szErrorMsg = NULL;
	CString szCommand;
	GetDlgItem(IDC_COMBO_COMMAND)->GetWindowText(szCommand);
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_SetProperty((wchar_t *)szCommand.GetString(), NULL, this);
	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		return;
	}

	ENUM_LIBWFX_EJECT_DIRECTION enEjectDirect = LIBWFX_EJECT_BACKWARDING;
	CString szEjectDirection;
	GetDlgItem(IDC_COMBO_EJECT_DIRECTION)->GetWindowText(szEjectDirection);
	if (szEjectDirection == _T("eject backwarding- force")) enEjectDirect = LIBWFX_EJECT_BACKWARDING;
	else if (szEjectDirection == _T("eject forwarding- force")) enEjectDirect = LIBWFX_EJECT_FORWARDING;
	else if (szEjectDirection == _T("eject backwarding stop- force")) enEjectDirect = LIBWFX_EJECT_BACKWARDINGS;
	else if (szEjectDirection == _T("eject forwarding stop- force")) enEjectDirect = LIBWFX_EJECT_FORWARDINGS;
	else if (szEjectDirection == _T("eject backwarding")) enEjectDirect = LIBWFX_EJECT_BACKWARDINGD;
	else if (szEjectDirection == _T("eject forwarding")) enEjectDirect = LIBWFX_EJECT_FORWARDINGD;
	else if (szEjectDirection == _T("eject backwarding stop")) enEjectDirect = LIBWFX_EJECT_BACKWARDINGSD;
	else if (szEjectDirection == _T("eject forwarding stop")) enEjectDirect = LIBWFX_EJECT_FORWARDINGSD;
	else if (szEjectDirection == _T("eject backwarding by steps")) enEjectDirect = LIBWFX_EJECT_BACKWARDING_BY_STEPS;
	else if (szEjectDirection == _T("eject forwarding by steps")) enEjectDirect = LIBWFX_EJECT_FORWARDING_BY_STEPS;

	enErrCode = m_pfnLibWFX_EjectPaperControlWithMsg(enEjectDirect, &szErrorMsg);

	int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, NULL, 0, NULL, NULL);

	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
	else if (nUTF8LenIP > 1) //event happen
	{
		char* pszUTF8IP = new char[nUTF8LenIP + 1];
		WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, pszUTF8IP, nUTF8LenIP, NULL, NULL);
		CString szErr(pszUTF8IP);
		CString szErr2(pszUTF8IP);
		szErr.Format(_T("%s"), szErr2);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));

		if (pszUTF8IP)
			delete[] pszUTF8IP;
	}
	else
		WriteLog(_T("Status:[LibWFX_EjectPaperControl Success]"));
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonPaperReady()
{
	// TODO: Add your control notification handler code here
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_PaperReady();
	if (enErrCode == LIBWFX_ERRCODE_SUCCESS)
	{
		WriteLog(_T("Paper is ready!"));
	}
	else if (enErrCode == LIBWFX_ERRCODE_PAPER_NOT_READY)
	{
		WriteLog(_T("Paper is NOT ready!"));
	}
	else
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonPaperstatus()
{
	// TODO: Add your control notification handler code here
	ENUM_LIBWFX_EVENT_CODE enPaperStatus;
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_GetPaperStatus(&enPaperStatus);
	if (enErrCode == LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		szErr.Format(_T("Status:[LibWFX_GetPaperStatus Success [%d]]"), enPaperStatus);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
	else
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonCalibrate()
{
	CString szGetItemText;
	CString szCommand = _T("");

	int nSelIdx = ((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetCurSel();
	if (nSelIdx == -1)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(LIBWFX_ERRCODE_NO_DEVICES, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, LIBWFX_ERRCODE_NO_DEVICES);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		return;
	}

	((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetLBText(nSelIdx, szGetItemText);
	if (szGetItemText == _T("A61") || szGetItemText == _T("A62") || szGetItemText == _T("A63") || szGetItemText == _T("A64") || szGetItemText == _T("A65") || szGetItemText == _T("A66") || szGetItemText == _T("J6102") || szGetItemText == _T("J1204"))
		szCommand.Format(_T("{\"device-name\":\"%s\",\"source\":\"Camera\",\"ext-capturetype\":\"g\"}"), szGetItemText);
	else
		GetDlgItem(IDC_COMBO_COMMAND)->GetWindowText(szCommand);

	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_SetProperty((wchar_t *)szCommand.GetString(), NULL, this);
	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		return;
	}
	if (szGetItemText == _T("A64"))
		ShowDlg(true, L"Calibrate_xmini");
	else
		ShowDlg(true, L"Calibrate");

	enErrCode = m_pfnLibWFX_Calibrate();

	if (szGetItemText == _T("A64"))
		ShowDlg(false, L"Calibrate_xmini");
	else
		ShowDlg(false, L"Calibrate");

	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
	else
	{
		WriteLog(_T("Status:[LibWFX_Calibrate Success]"));
	}
}

void CLibWFXDemoDlg_NonBlockMode::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	CDialogEx::OnCancel();
}


HCURSOR CLibWFXDemoDlg_NonBlockMode::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CLibWFXDemoDlg_NonBlockMode::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}


void CLibWFXDemoDlg_NonBlockMode::OnDestroy()
{
	CDialogEx::OnDestroy();
	if (m_hLibWFX)
	{
		m_pfnLibWFX_CloseDevice();
		m_pfnLibWFX_DeInit();
		::FreeLibrary(m_hLibWFX);
	}

#ifdef _DEBUG
	FreeConsole();
#endif
}


void CLibWFXDemoDlg_NonBlockMode::OnCbnDropdownComboCommand()
{
	CClientDC dc(this);
	int nWitdh = 10;
	int nSaveDC = dc.SaveDC();

	dc.SelectObject(GetFont());
	for (int i = 0; i < ((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->GetCount(); i++)
	{
		CString strLable = _T("");
		((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->GetLBText(i, strLable);

		nWitdh = max(nWitdh, dc.GetTextExtent(strLable).cx);
	}
	nWitdh += 10;
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->SetDroppedWidth(nWitdh);
}


void CLibWFXDemoDlg_NonBlockMode::OnWindowPosChanging(WINDOWPOS* lpwndpos)
{
	CDialogEx::OnWindowPosChanging(lpwndpos);
}



void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonScan()
{
	OnStartScan(NULL, NULL);
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonRecyclesavefolder()
{
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_RecycleSaveFolder();

	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));
	}
	else
		WriteLog(_T("Status:[LibWFX_RecycleSaveFolder Success]"));
}


void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonMergepdf()
{
	TCHAR szFilters[] = _T("Image Files (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png||");
	CFileDialog fileDlg(TRUE, NULL, NULL, OFN_FILEMUSTEXIST | OFN_HIDEREADONLY, szFilters);
	fileDlg.GetOFN().Flags |= OFN_ALLOWMULTISELECT;

	CString data;
	fileDlg.m_pOFN->nMaxFile = (MAX_FILE_NAMES*(MAX_PATH + 1)) + 1;
	fileDlg.m_pOFN->lpstrFile = data.GetBuffer((MAX_FILE_NAMES*(MAX_PATH + 1)) + 1);

	if (fileDlg.DoModal() == IDOK)
	{
		CString filelist = L"";  //i.e.L"D:\\testpdf\\D:\\testpdf\\IMG_113692078_00002.jpg*D:\\testpdf\\IMG_114006609_00002.jpg";
		POSITION pos(fileDlg.GetStartPosition());
		while (pos)
		{
			CString filename = fileDlg.GetNextPathName(pos);
			filelist.Append(filename);
			filelist.Append(_T("*"));
		}

		ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_MergeToPdf((wchar_t *)filelist.GetString());
		if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
		{
			CString szErr;
			wchar_t szErrorMsg[MAX_PATH] = { 0 };
			m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
			szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));
		}
		else
			WriteLog(_T("Status:[LibWFX_MergeToPdf Success]"));
	}
	data.ReleaseBuffer();
}

BOOL CLibWFXDemoDlg_NonBlockMode::GetSDKInstallPath(TCHAR* szInstallPath, bool bIsSDKInstallPath)
{
	HKEY  key = NULL;
	TCHAR szRegPath[MAX_PATH] = { 0 };
	bool bIsWow64Process = IsWow64Process();
	if (bIsWow64Process && bIsSDKInstallPath)
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{E96A9957-0A5A-40C3-8358-75A3FA6D9CC7}_is1"));
	else if (bIsWow64Process && !bIsSDKInstallPath)
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"));
	else if (!bIsWow64Process && bIsSDKInstallPath)
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{E96A9957-0A5A-40C3-8358-75A3FA6D9CC7}_is1"));
	else
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"));

	if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, szRegPath, 0, KEY_QUERY_VALUE, &key) == ERROR_SUCCESS)
	{
		if (key)
		{
			DWORD type, size;
			size = MAX_PATH;
			if (RegQueryValueEx(key, _T("InstallLocation"), NULL, &type, (LPBYTE)szInstallPath, &size) == ERROR_SUCCESS)
				return TRUE;
		}
	}
	return FALSE;
}

void CLibWFXDemoDlg_NonBlockMode::OnBnClickedButtonRegister()
{
	TCHAR szRegisterEXEPath[AVI_MAXPATH_LEN] = { 0 };
	if (GetSDKInstallPath(szRegisterEXEPath, true))
	{
		if (szRegisterEXEPath[_tcslen(szRegisterEXEPath) - 1] != _T('\\'))
		{
			_tcscat_s(szRegisterEXEPath, _T("\\"));
		}
		_tcscat_s(szRegisterEXEPath, REGISTER_EXENAME);
	}
	ShellExecute(NULL, _T("open"), szRegisterEXEPath, L"", NULL, SW_NORMAL);
}


void CLibWFXDemoDlg_NonBlockMode::ShowDlg(bool enableDlg, wchar_t* szAction)
{
	if (enableDlg)
	{
		if (!wcscmp(szAction, L"Calibrate_xmini"))
		{
			m_CalibrationXminiDlg = new CalibrationXminiDlg();
			m_CalibrationXminiDlg->Create(CalibrationXminiDlg::IDD, this);
			m_CalibrationXminiDlg->ShowWindow(SW_SHOW);		
		}
		else if (!wcscmp(szAction, L"Calibrate"))
		{
			m_CalibrationDlg = new CalibrationDlg();
			m_CalibrationDlg->Create(CalibrationDlg::IDD, this);
			m_CalibrationDlg->ShowWindow(SW_SHOW);
		}
		else
		{
			m_InitDlg = new InitDlg();
			m_InitDlg->Create(InitDlg::IDD, this);
			m_InitDlg->ShowWindow(SW_SHOW);
		}
		this->ShowWindow(SW_HIDE);
	}
	else
	{
		if (!wcscmp(szAction, L"Calibrate_xmini"))
		{
			m_CalibrationXminiDlg->EndDialog(0);
			delete m_CalibrationXminiDlg;
			m_CalibrationXminiDlg = NULL;		
		}
		else if (!wcscmp(szAction, L"Calibrate"))
		{
			m_CalibrationDlg->EndDialog(0);
			delete m_CalibrationDlg;
			m_CalibrationDlg = NULL;
		}
		else
		{
			m_InitDlg->EndDialog(0);
			delete m_InitDlg;
			m_InitDlg = NULL;
		}
		this->ShowWindow(SW_SHOW);
	}
}

BOOL CLibWFXDemoDlg_NonBlockMode::IsWow64Process()
{
	BOOL bIsWow64 = FALSE;
	fnIsWow64Process = (LPFN_ISWOW64PROCESS)GetProcAddress(
		GetModuleHandle(TEXT("kernel32")), "IsWow64Process");

	if (NULL != fnIsWow64Process)
	{
		if (!fnIsWow64Process(GetCurrentProcess(), &bIsWow64))
		{
			//handle error
		}
	}
	return bIsWow64;
}
