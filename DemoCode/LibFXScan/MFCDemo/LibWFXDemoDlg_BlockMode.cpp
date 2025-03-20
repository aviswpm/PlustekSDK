// LibWFXDemoDlg_BlockMode.cpp : implementation file
//

#include "stdafx.h"
#include "LibWFXDemo.h"
#include "LibWFXDemoDlg_BlockMode.h"
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

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

using namespace std;
IMPLEMENT_DYNAMIC(CLibWFXDemoDlg_BlockMode, CDialogEx)
std::vector<std::wstring> vecImagePath;

CLibWFXDemoDlg_BlockMode::CLibWFXDemoDlg_BlockMode(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_LIBWFXDEMO_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_ScanningDlg = NULL;
	m_CalibrationDlg = NULL;
	vecImagePath.clear();
}

CLibWFXDemoDlg_BlockMode::~CLibWFXDemoDlg_BlockMode()
{
}

void CLibWFXDemoDlg_BlockMode::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BUTTON_REGISTER, m_bt_register);
}

BEGIN_MESSAGE_MAP(CLibWFXDemoDlg_BlockMode, CDialogEx)
	ON_CBN_SELCHANGE(IDC_COMBO_DEVICE_NAME, &CLibWFXDemoDlg_BlockMode::OnCbnSelchangeComboDeviceName)
	ON_BN_CLICKED(IDC_BUTTON_REFRESH, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonRefresh)
	ON_BN_CLICKED(IDC_BUTTON_EDIT_CMD, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonEditCmd)
	ON_BN_CLICKED(IDC_BUTTON_ECO, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonEco)
	ON_BN_CLICKED(IDC_BUTTON_EJECT, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonEject)
	ON_BN_CLICKED(IDC_BUTTON_PAPER_READY, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonPaperReady)
	ON_BN_CLICKED(IDC_BUTTON_PAPERSTATUS, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonPaperstatus)
	ON_BN_CLICKED(IDC_BUTTON_CALIBRATE, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonCalibrate)
	ON_BN_CLICKED(IDCANCEL, &CLibWFXDemoDlg_BlockMode::OnBnClickedCancel)
	ON_WM_QUERYDRAGICON()
	ON_WM_PAINT()
	ON_WM_DESTROY()
	ON_CBN_DROPDOWN(IDC_COMBO_COMMAND, &CLibWFXDemoDlg_BlockMode::OnCbnDropdownComboCommand)
	ON_WM_WINDOWPOSCHANGING()	
	ON_BN_CLICKED(IDC_BUTTON_SCAN, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonScan)	
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_BUTTON_MERGEPDF, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonMergepdf)
	ON_BN_CLICKED(IDC_BUTTON_RECYCLESAVEFOLDER, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonRecyclesavefolder)
	ON_BN_CLICKED(IDC_BUTTON_REGISTER, &CLibWFXDemoDlg_BlockMode::OnBnClickedButtonRegister)
END_MESSAGE_MAP()

BOOL CLibWFXDemoDlg_BlockMode::InitLib(VOID)
{
	m_hLibWFX = ::LoadLibrary(LIBWFX_DLLNAME);
	if (m_hLibWFX == NULL)
	{
		TCHAR szDLLPath[AVI_MAXPATH_LEN] = { 0 };
		if (GetSDKInstallPath(szDLLPath, false))
		{
			if (szDLLPath[_tcslen(szDLLPath) - 1] != _T('\\'))
			{
				_tcscat_s(szDLLPath, _T("\\"));
			}
			_tcscat_s(szDLLPath, LIBWFX_DLLNAME);
			m_hLibWFX = ::LoadLibrary(szDLLPath);
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

	WriteLog(_T("Status:[Load LibWebFXScan Success]"));
	m_nCount = 0;
	return TRUE;
}

BOOL CLibWFXDemoDlg_BlockMode::WriteLog(TCHAR* szMsg)
{
	CString szContent;

	GetDlgItem(IDC_EDIT_LOG)->GetWindowText(szContent);
	szContent.Append(szMsg);
	szContent.Append(_T("\r\n"));
	GetDlgItem(IDC_EDIT_LOG)->SetWindowText(szContent);

	((CEdit *)GetDlgItem(IDC_EDIT_LOG))->LineScroll(((CEdit *)GetDlgItem(IDC_EDIT_LOG))->GetLineCount());
	CString szContent2;
	szContent2.Append(szMsg);
	szContent2.Append(_T("\r\n"));
	m_pfnLibWFX_WriteAPLog((wchar_t *)szContent2.GetString());
	return TRUE;
}

BOOL CLibWFXDemoDlg_BlockMode::ShowImage(int controlID, std::wstring filepath)
{
	m_muxMap.lock();
	CRect rcImage;
	static_cast<CStatic *>(GetDlgItem(controlID))->GetClientRect(&rcImage);

	CImage Image;
	HRESULT hRlt = Image.Load(filepath.c_str());

	if (hRlt != S_OK)
	{
		vecImagePath.erase(vecImagePath.begin());
		return FALSE;
	}
	if ((Image.GetWidth() == 0) || (Image.GetHeight() == 0))
	{
		vecImagePath.erase(vecImagePath.begin());
		return FALSE;
	}

	int nRatioWidth = rcImage.Height() * Image.GetWidth() / Image.GetHeight();
	int nHRatioHeight = rcImage.Width() * Image.GetHeight() / Image.GetWidth();

	if (rcImage.Width() > nRatioWidth)
	{
		rcImage.left = (rcImage.right - rcImage.left - nRatioWidth) / 2;
		rcImage.right = rcImage.left + nRatioWidth;
	}

	if (rcImage.Height() > nHRatioHeight)
	{
		rcImage.top = (rcImage.bottom - rcImage.top - nHRatioHeight) / 2;
		rcImage.bottom = rcImage.top + nHRatioHeight;
	}

	CDC* pDC = GetDlgItem(controlID)->GetWindowDC();
	pDC->SetStretchBltMode(COLORONCOLOR);

	rcImage.left += 5;
	rcImage.right -= 5;
	rcImage.top += 5;
	rcImage.bottom -= 5;
	Image.Draw(pDC->m_hDC, rcImage);
	ReleaseDC(pDC);
	InvalidateRect(rcImage, false);
	m_muxMap.unlock();
	return TRUE;
}

BOOL CLibWFXDemoDlg_BlockMode::GetJsonString(CString szDevName)
{
	CString szDefJson;
	szDefJson.Empty();

	USES_CONVERSION;
	if (GetCommandString(T2W(szDevName.GetBuffer())))
	{
		return TRUE;
	}
	else if (szDevName == _T("A61") || szDevName == _T("A62") || szDevName == _T("A63") || szDevName == _T("A64") || szDevName == _T("A65") || szDevName == _T("A66") || szDevName == _T("J6102"))
	{
		szDefJson.Append(_T("{\"device-name\":\""));
		szDefJson.Append(szDevName);
		szDefJson.Append(_T("\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}"));
	}
	else if (szDevName == _T("7C1U") || szDevName == _T("7C8U") || szDevName == _T("7C9U") || szDevName == _T("7CAU") || szDevName == _T("773U") || szDevName == _T("7CCU"))
	{
		szDefJson.Append(_T("{\"device-name\":\""));
		szDefJson.Append(szDevName);
		szDefJson.Append(_T("\",\"source\":\"Sheetfed-Duplex\",\"recognize-type\":\"passport\"}"));
	}
	else if (szDevName == _T("776U") || szDevName == _T("777U") || szDevName == _T("778U") || szDevName == _T("FE7010_FE7011"))
	{
		szDefJson.Append(_T("{\"device-name\":\""));
		szDefJson.Append(szDevName);
		szDefJson.Append(_T("\",\"source\":\"Sheetfed-Duplex\"}"));
	}
	else if (szDevName == _T("74RU") || szDevName == _T("74BU") || szDevName == _T("7P1U") || szDevName == _T("M11U") || szDevName == _T("7B3U") || szDevName == _T("M12U"))
	{
		szDefJson.Append(_T("{\"device-name\":\""));
		szDefJson.Append(szDevName);
		szDefJson.Append(_T("\",\"source\":\"Sheetfed-Front\"}"));
	}
	else if (szDevName == _T("256U") ||
		szDevName == _T("258U") ||
		szDevName == _T("258U_259U") ||
		szDevName == _T("25AU") ||
		szDevName == _T("271U") ||
		szDevName == _T("273U") ||
		szDevName == _T("273U_274U") ||
		szDevName == _T("275U") ||
		szDevName == _T("276U") ||
		szDevName == _T("261U") ||
		szDevName == _T("BAG") ||
		szDevName == _T("7K1U") ||
		szDevName == _T("6C6U") ||
		szDevName == _T("BB1U") ||
		szDevName == _T("BAGU") ||
		szDevName == _T("2B2U") ||
		szDevName == _T("2B3U") ||
		szDevName == _T("7N1U") ||
		szDevName == _T("2D1U") ||
		szDevName == _T("2C1U") ||
		szDevName == _T("797U") ||
		szDevName == _T("7K7U") ||
		szDevName == _T("2G1U") ||
		szDevName == _T("2G2U") ||
		szDevName == _T("678U") ||
		szDevName == _T("7K8U") ||
		szDevName == _T("B85U") ||
		szDevName == _T("2D3U"))
	{
		szDefJson.Append(_T("{\"device-name\":\""));
		szDefJson.Append(szDevName);
		szDefJson.Append(_T("\",\"source\":\"Flatbed\"}"));
	}
	else
	{
		szDefJson.Append(_T("{\"device-name\":\""));
		szDefJson.Append(szDevName);
		szDefJson.Append(_T("\",\"source\":\"ADF-Duplex\"}"));
	}
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->AddString(szDefJson);
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->SetCurSel(0);
	return TRUE;
}

BOOL CLibWFXDemoDlg_BlockMode::InitDevicesList(VOID)
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
BOOL CLibWFXDemoDlg_BlockMode::GetCommandString(wchar_t* DevName)
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
		std::wstring wstbuffer(buf[idx].begin(), buf[idx].end());
		std::wstring retbuffer;
		retbuffer.clear();

		//if (wstbuffer.at(0) != '{')
		//	break;

		int pos = wstbuffer.find('}', 0);
		wstbuffer.erase(wstbuffer.begin() + pos + 1, wstbuffer.end());

		std::wstring subtoken;//, subtoken2;
		int nPos = 0;
		for (nPos = 0; nPos < wstbuffer.length(); nPos++)
		{
			if (wstbuffer.at(nPos) == '"')
				retbuffer.append(L"\"");
			else if (wstbuffer.at(nPos) == ':')
				retbuffer.append(L":");
			else if (wstbuffer.at(nPos) == ' ')
				retbuffer.append(L" ");
			else if (wstbuffer.at(nPos) == ',')
				retbuffer.append(L",");
			else if (wstbuffer.at(nPos) == '-')
				retbuffer.append(L"-");
			else
			{
				subtoken = wstbuffer.substr(nPos, 1);
				retbuffer.append(subtoken);
			}
		}
		USES_CONVERSION;
		((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->AddString(retbuffer.c_str());
	}
	input.close();
	((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->SetCurSel(0);
	return true;
}

//write command to command.txt
BOOL CLibWFXDemoDlg_BlockMode::SetCommandString(wchar_t* DevName, CString Command)
{
	//Confirm whether the devicename in the command is correct
	int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)DevName, -1, NULL, 0, NULL, NULL);
	char* pszUTF8IP = new char[nUTF8LenIP + 1];
	WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)DevName, -1, pszUTF8IP, nUTF8LenIP, NULL, NULL);
	CString devname(pszUTF8IP);
	if (Command.Compare(devname) == -1)
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

	if (pszUTF8IP)
		delete[] pszUTF8IP;

	int   nUTF8Len = WideCharToMultiByte(CP_UTF8, 0, szIniPath, -1, NULL, 0, NULL, NULL);
	char* pszUTF8 = new char[nUTF8Len + 1];
	WideCharToMultiByte(CP_UTF8, 0, szIniPath, -1, pszUTF8, nUTF8Len, NULL, NULL);

	FILE *file;
	vector<CString> cmdtmp;

	if (!fopen_s(&file, pszUTF8, "w+"))
	{
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

		char szCommand[2048];
		sprintf_s(szCommand, "%S", Command);

		fwrite(szCommand, 1, strlen(szCommand), file);
		fclose(file);
		((CComboBox *)GetDlgItem(IDC_COMBO_COMMAND))->SetCurSel(0);

		if (pszUTF8)
			delete[] pszUTF8;
		return true;
	}
	if (pszUTF8)
		delete[] pszUTF8;
	return false;
}

wchar_t* CLibWFXDemoDlg_BlockMode::rtrim(wchar_t* str) {
	wchar_t* end;
	end = str + wcslen(str) - 1;

	while (end >= str && iswspace(*end)) {
		end--;
	}
	*(end + 1) = L'\0';
	return str;
}

void CLibWFXDemoDlg_BlockMode::GetCertificatePermission()
{
	const wchar_t* szPermissionTypeList = NULL;
	CString szErr;
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_GetCertificatePermission(&szPermissionTypeList, LIBWFX_DATA_TYPE_REGINFO);
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

BOOL CLibWFXDemoDlg_BlockMode::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	HWND hwnd = NULL;
	m_nCmdMaxNum = 5;
	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
									// TODO: Add extra initialization here

	HICON hicon = AfxGetApp()->LoadIconW(IDI_LICENSE);
	m_bt_register.SetIcon(hicon);
#ifdef _DEBUG
	AllocConsole();
#endif

	if (InitLib())
	{		
		if (m_pfnLibWFX_IsWindowExist(L"") == true)
		{
			::MessageBoxW(hwnd, L"Please confirm whether the \"CheckWindowTitle\" parameter content in LibWebFxScan.ini are all closed!!", L"Warning", MB_OK | MB_ICONEXCLAMATION);
			CString szErr;
			szErr.Format(_T("[ Warning ]LIBWFX_ERRCODE_SPECIFIC_AP_OPENING - [%d]"), LIBWFX_ERRCODE_SPECIFIC_AP_OPENING);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));
			szErr.Format(_T("[ Warning ]LIBWFX_ERRCODE_NO_INIT - [%d]"), LIBWFX_ERRCODE_NO_INIT);
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
	return FALSE;  // return TRUE  unless you set the focus to a control
}

void CLibWFXDemoDlg_BlockMode::OnCbnSelchangeComboDeviceName()
{
	CString szDefaultDev(_T("device_name="));
	CString szGetItemText;

	int nSelIdx = ((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetCurSel();
	((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetLBText(nSelIdx, szGetItemText);
	GetJsonString(szGetItemText);
	//GetDlgItem(IDC_EDIT_COMMAND)->SetWindowText(szDefaultDev);
}

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonRefresh()
{
	InitDevicesList();
}

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonEditCmd()
{
	typedef void(__stdcall* API_EDIT_COMMAND)(wchar_t*, wchar_t**);

	HMODULE hLib = ::LoadLibrary(_T("CommandEditor.dll"));
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

	GetDlgItem(IDC_COMBO_COMMAND)->SetWindowText(szRtn);

	::FreeLibrary(hLib);
}

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonEco()
{
	unsigned long ulTime = 0;
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

	enErrCode = m_pfnLibWFX_ECOControl(&ulTime, 0);
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

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonEject()
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
	ENUM_LIBWFX_EJECT_DIRECTION enEjectDirect = ((CButton *)GetDlgItem(IDC_CHECK_BACKWARD))->GetCheck() == BST_CHECKED ? LIBWFX_EJECT_BACKWARDING : LIBWFX_EJECT_FORWARDING;
	enErrCode = m_pfnLibWFX_EjectPaperControlWithMsg(enEjectDirect, &szErrorMsg);

	int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, NULL, 0, NULL, NULL);

	if (nUTF8LenIP > 1) //event happen
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
	else if (enErrCode == LIBWFX_ERRCODE_SUCCESS)
	{
		WriteLog(_T("Status:[LibWFX_EjectPaperControl Success]"));
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

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonPaperReady()
{
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

	enErrCode = m_pfnLibWFX_PaperReady();
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

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonPaperstatus()
{
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
	ENUM_LIBWFX_EVENT_CODE enPaperStatus;
	enErrCode = m_pfnLibWFX_GetPaperStatus(&enPaperStatus);
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

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonCalibrate()
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

	if (szGetItemText == _T("A61") || szGetItemText == _T("A62") || szGetItemText == _T("A63") || szGetItemText == _T("A64") || szGetItemText == _T("A65") || szGetItemText == _T("A66") || szGetItemText == _T("J6102"))
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

	ShowDlg(true, L"Calibrate");
	enErrCode = m_pfnLibWFX_Calibrate();
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

void CLibWFXDemoDlg_BlockMode::OnBnClickedCancel()
{
	CDialogEx::OnCancel();
}

HCURSOR CLibWFXDemoDlg_BlockMode::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.
void CLibWFXDemoDlg_BlockMode::OnPaint()
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

void CLibWFXDemoDlg_BlockMode::OnDestroy()
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

void CLibWFXDemoDlg_BlockMode::OnCbnDropdownComboCommand()
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

void CLibWFXDemoDlg_BlockMode::OnWindowPosChanging(WINDOWPOS* lpwndpos)
{
	CDialogEx::OnWindowPosChanging(lpwndpos);
}

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonScan()
{
	const wchar_t* szScanImageList = NULL;
	const wchar_t* szOCRResultList = NULL;
	const wchar_t* szExceptionRet = NULL;
	const wchar_t* szEventRet = NULL;
	CString szCommand;
	GetDlgItem(IDC_COMBO_COMMAND)->GetWindowText(szCommand);
	CString szGetItemText;
	int nSelIdx = ((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetCurSel();
	if (nSelIdx > -1)
		((CComboBox *)GetDlgItem(IDC_COMBO_DEVICE_NAME))->GetLBText(nSelIdx, szGetItemText);

	if (szCommand.Find(_T("\"autoscan\":true")) != -1)
	{
		::MessageBox(NULL, _T("BlockScan do not support autoscan!! If you want to implement autoscan, please refer to the AutoCaptureDemo-C++."), _T("Message"), MB_YESNO);
		return;
	}
	//ShowScanningDlg(true);
	ShowDlg(true, L"Scan");
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_SynchronizeScan((wchar_t *)szCommand.GetString(), &szScanImageList, &szOCRResultList, &szExceptionRet, &szEventRet);
	//ShowScanningDlg(false);
	ShowDlg(false, L"Scan");

	int nUTF8Ecxeption = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szExceptionRet, -1, NULL, 0, NULL, NULL);
	int nUTF8Event = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szEventRet, -1, NULL, 0, NULL, NULL);

	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		wchar_t szErrorMsg[MAX_PATH] = { 0 };
		m_pfnLibWFX_GetLastErrorCode(enErrCode, szErrorMsg);
		szErr.Format(_T("[ Warning ] %s - [%d]"), szErrorMsg, enErrCode);
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));  //get fail message
	}
	else if (nUTF8Event > 1) //event happen
	{
		CString szErr;
		szErr.Format(_T("%s"), szEventRet);
		SetCommandString(T2W(szGetItemText.GetBuffer()), T2W(szCommand.GetBuffer()));
		WriteLog(_T("Status:[Device Ready!]"));
		WriteLog(const_cast<TCHAR *>(szErr.GetString()));   //get event message

		if (szErr != "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" && szErr != "LIBWFX_EVENT_UVSECURITY_DETECTED[1]")
		{
			WriteLog(_T("Status:[Scan End]"));
			return;
		}

		wchar_t* token;
		wchar_t* token2;
		const wchar_t delim[] = L"|&|";

		if (szCommand.Find(_T("\"rawdata\":true")) != -1)
		{
			wchar_t* position = _wcsdup((wchar_t*)szOCRResultList);
			while ((token = wcsstr(position, delim)) != nullptr)
			{
				*token = L'\0';		
				WriteLog(rtrim(position));
			}
		}
		else
		{
			wchar_t* position = _wcsdup((wchar_t*)szScanImageList);
			wchar_t* position2 = _wcsdup((wchar_t*)szOCRResultList);

			while ((token = wcsstr(position, delim)) != nullptr)
			{
				token2 = wcsstr(position2, delim);
				*token = L'\0';
				*token2 = L'\0';
				WriteLog(rtrim(position));
				WriteLog(rtrim(position2));

				if (!wcsstr((wchar_t *)rtrim(position), L".pdf") && !wcsstr((wchar_t *)rtrim(position), L".tif") && !wcsstr(CharUpper((wchar_t *)rtrim(position)), L"_PHOTO") && wcscmp((wchar_t *)rtrim(position), L""))
				{
					vecImagePath.push_back(rtrim(position));
					SetTimer(vecImagePath.size(), 1, NULL);
				}
				position = token + wcslen(delim);
				position2 = token2 + wcslen(delim);
			}
		}
	}
	else
	{
		SetCommandString(T2W(szGetItemText.GetBuffer()), T2W(szCommand.GetBuffer()));
		WriteLog(_T("Status:[Device Ready!]"));

		if (nUTF8Ecxeption > 1) //exception happen
		{
			CString szErr;
			szErr.Format(_T("%s"), szExceptionRet);
			WriteLog(const_cast<TCHAR *>(szErr.GetString()));   //get exception message
		}

		wchar_t* token;
		wchar_t* token2;
		const wchar_t delim[] = L"|&|";

		if (szCommand.Find(_T("\"rawdata\":true")) != -1)
		{
			wchar_t* position = _wcsdup((wchar_t*)szOCRResultList);
			while ((token = wcsstr(position, delim)) != nullptr)
			{
				*token = L'\0';			
				WriteLog(rtrim(position));
			}
		}
		else
		{
			wchar_t* position = _wcsdup((wchar_t*)szScanImageList);
			wchar_t* position2 = _wcsdup((wchar_t*)szOCRResultList);
			while ((token = wcsstr(position, delim)) != nullptr)
			{
				token2 = wcsstr(position2, delim);
				*token = L'\0';
				*token2 = L'\0';
				WriteLog(rtrim(position));	
				WriteLog(rtrim(position2));

				if (!wcsstr((wchar_t *)rtrim(position), L".pdf") && !wcsstr((wchar_t *)rtrim(position), L".tif") && !wcsstr(CharUpper((wchar_t *)rtrim(position)), L"_PHOTO") && wcscmp((wchar_t *)rtrim(position), L""))
				{
					vecImagePath.push_back(rtrim(position));
					SetTimer(vecImagePath.size(), 1, NULL);
				}
				position = token + wcslen(delim);
				position2 = token2 + wcslen(delim);
			}
		}
	}
	WriteLog(_T("Status:[Scan End]"));
	return;
}

void CLibWFXDemoDlg_BlockMode::ShowScanningDlg(bool enableDlg)
{
	if (enableDlg)
	{
		m_ScanningDlg = new ScanningDlg();
		m_ScanningDlg->Create(ScanningDlg::IDD, this);
		m_ScanningDlg->ShowWindow(SW_SHOW);
		this->ShowWindow(SW_HIDE);
	}
	else
	{
		m_ScanningDlg->EndDialog(0);
		delete m_ScanningDlg;
		m_ScanningDlg = NULL;
		this->ShowWindow(SW_SHOW);
	}
}

void CLibWFXDemoDlg_BlockMode::ShowDlg(bool enableDlg, wchar_t* szAction)
{
	if (enableDlg)
	{
		if (!wcscmp(szAction, L"Scan"))
		{
			m_ScanningDlg = new ScanningDlg();
			m_ScanningDlg->Create(ScanningDlg::IDD, this);
			m_ScanningDlg->ShowWindow(SW_SHOW);
		}
		else if (!wcscmp(szAction, L"Calibrate"))
		{
			m_CalibrationDlg = new CalibrationDlg();
			m_CalibrationDlg->Create(CalibrationDlg::IDD, this);
			m_CalibrationDlg->ShowWindow(SW_SHOW);
		}
		this->ShowWindow(SW_HIDE);
	}
	else
	{
		if (!wcscmp(szAction, L"Scan"))
		{
			m_ScanningDlg->EndDialog(0);
			delete m_ScanningDlg;
			m_ScanningDlg = NULL;
		}
		else if (!wcscmp(szAction, L"Calibrate"))
		{
			//m_CalibrationDlg->EndDialog(0);
			m_CalibrationDlg->DestroyWindow();
			delete m_CalibrationDlg;
			m_CalibrationDlg = NULL;
		}
		this->ShowWindow(SW_SHOW);
	}
}

void CLibWFXDemoDlg_BlockMode::OnTimer(UINT_PTR nIDEvent)
{
	KillTimer(nIDEvent);
	int currentControlId = (m_nCount % 2) ? IDC_STATIC_IMG_2 : IDC_STATIC_IMG_1;
	int currentControlId_backup = (currentControlId == IDC_STATIC_IMG_2) ? IDC_STATIC_IMG_1 : IDC_STATIC_IMG_2;

	if (vecImagePath.size() > 1)
	{
		ShowImage(currentControlId_backup, vecImagePath.at(vecImagePath.size() - 2));
		ShowImage(currentControlId, vecImagePath.at(vecImagePath.size() - 1));
	}
	else if (vecImagePath.size() == 1)
	{
		ShowImage(currentControlId, vecImagePath.at(0));
	}


	if (vecImagePath.size() > 2)
		vecImagePath.erase(vecImagePath.begin());
	m_nCount++;
}


void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonMergepdf()
{
	// TODO: Add your control notification handler code here
	ENUM_LIBWFX_EVENT_CODE enPaperStatus;
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

	CFileDialog fileDlg(true);
	fileDlg.m_ofn.Flags |= OFN_ALLOWMULTISELECT;
	fileDlg.m_ofn.lpstrFilter = _T("Images (*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG| All files (*.*)|*.*");

	CString data;
	fileDlg.m_pOFN->nMaxFile = (MAX_FILE_NAMES*(MAX_PATH + 1)) + 1;
	fileDlg.m_pOFN->lpstrFile = data.GetBuffer((MAX_FILE_NAMES*(MAX_PATH + 1)) + 1);

	if (fileDlg.DoModal() == IDOK)
	{
		CString filelist;  //i.e.L"D:\\testpdf\\D:\\testpdf\\IMG_113692078_00002.jpg*D:\\testpdf\\IMG_114006609_00002.jpg";
		POSITION pos(fileDlg.GetStartPosition());
		while (pos)
		{
			CString filename = fileDlg.GetNextPathName(pos);
			filelist.Append(filename);
			filelist.Append(_T("*"));
		}

		enErrCode = m_pfnLibWFX_MergeToPdf((wchar_t *)filelist.GetString());
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

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonRecyclesavefolder()
{
	// TODO: Add your control notification handler code here
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

BOOL CLibWFXDemoDlg_BlockMode::GetSDKInstallPath(TCHAR* szInstallPath, bool bIsSDKInstallPath)
{
	HKEY  key = NULL;
	TCHAR szRegPath[MAX_PATH] = { 0 };

#if ((defined(__i386__) || defined(_M_IX86)) && defined(_WIN64))  //OS:X64  EXE:X86
	if (bIsSDKInstallPath)
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{E96A9957-0A5A-40C3-8358-75A3FA6D9CC7}_is1"));
	else
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"));

#else
	if (bIsSDKInstallPath)
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{E96A9957-0A5A-40C3-8358-75A3FA6D9CC7}_is1"));
	else
		_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"));
#endif


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

void CLibWFXDemoDlg_BlockMode::OnBnClickedButtonRegister()
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
