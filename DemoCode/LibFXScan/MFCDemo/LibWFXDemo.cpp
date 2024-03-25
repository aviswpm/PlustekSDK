
// LibWFXDemo.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "LibWFXDemo.h"
#include "LibWFXDemoDlg_BlockMode.h"
#include "LibWFXDemoDlg_NonBlockMode.h"
#include <windows.h>
#include <tlhelp32.h>


#include <string.h>
#include <io.h>
#include <direct.h>
#include <errno.h>
#include <codecvt> 

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CLibWFXDemoApp

BEGIN_MESSAGE_MAP(CLibWFXDemoApp, CWinApp)
	ON_COMMAND(ID_HELP, &CWinApp::OnHelp)
END_MESSAGE_MAP()


// CLibWFXDemoApp construction

CLibWFXDemoApp::CLibWFXDemoApp()
{
	// support Restart Manager
	m_dwRestartManagerSupportFlags = AFX_RESTART_MANAGER_SUPPORT_RESTART;

	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CLibWFXDemoApp object

CLibWFXDemoApp theApp;


// CLibWFXDemoApp initialization

BOOL CLibWFXDemoApp::InitInstance()
{
	// InitCommonControlsEx() is required on Windows XP if an application
	// manifest specifies use of ComCtl32.dll version 6 or later to enable
	// visual styles.  Otherwise, any window creation will fail.
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwSize = sizeof(InitCtrls);
	// Set this to include all the common control classes you want to use
	// in your application.
	InitCtrls.dwICC = ICC_WIN95_CLASSES;
	InitCommonControlsEx(&InitCtrls);

	CWinApp::InitInstance();


	AfxEnableControlContainer();

	// Create the shell manager, in case the dialog contains
	// any shell tree view or shell list view controls.
	CShellManager *pShellManager = new CShellManager;

	// Activate "Windows Native" visual manager for enabling themes in MFC controls
	CMFCVisualManager::SetDefaultManager(RUNTIME_CLASS(CMFCVisualManagerWindows));

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	// of your final executable, you should remove from the following
	// the specific initialization routines you do not need
	// Change the registry key under which our settings are stored
	// TODO: You should modify this string to be something appropriate
	// such as the name of your company or organization
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));

	//
	//for "/q" command to close the process
	// Just get default class for the dialogs
	WNDCLASS wndcls;
	::GetClassInfo(NULL,MAKEINTRESOURCE(32770),&wndcls);

	// Set our own class name
	wndcls.lpszClassName = _T("LibWFXDemo");

	// Just register the class
	if (!::RegisterClass(&wndcls))
	{
		_ASSERTE(! __FUNCTION__ " Failed to register window class");
		return FALSE;
	}

	BOOL bQuit = FALSE;
	BOOL bCancelWindow = FALSE;
	BOOL bAskDone = FALSE;
	LPWSTR *szArg = NULL;
	int argCount = 0;
	szArg = CommandLineToArgvW(GetCommandLine(), &argCount);

	for (int nIndex=1;nIndex<argCount;nIndex++)
	{
		if (!lstrcmpi (szArg[nIndex], _T("/q")))
		{
			bQuit = TRUE;
		}
	}

	bQuit = TRUE;
	while (1)
	{
		HWND hWnd = ::FindWindow(_T("LibWFXDemo"), NULL);

		if (hWnd)
		{
			//if (!bAskDone)
			//{
			//	if (::MessageBox(NULL, _T("LibWFXDemo has been opened, do you want to close the others?"), _T("Warning"), MB_YESNO) == IDYES)
			//	{
			//		bAskDone = TRUE;
			//		bCancelWindow = TRUE;
			//		::PostMessage(hWnd, WM_CLOSE, NULL, NULL);
			//		//kill process
			//		HANDLE hProcessSnap;
			//		hProcessSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
			//		if (hProcessSnap == INVALID_HANDLE_VALUE) {
			//			printf("create snapshot fail.\n");
			//			return EXIT_FAILURE;
			//		}
			//
			//		PROCESSENTRY32 pe32;
			//		pe32.dwSize = sizeof(PROCESSENTRY32);
			//		if (!Process32First(hProcessSnap, &pe32)) {
			//			printf("get process fail.\n");
			//			CloseHandle(hProcessSnap);
			//			return EXIT_FAILURE;
			//		}
			//
			//		do {
			//
			//			if (!lstrcmp(L"LibWFXDemo.exe", pe32.szExeFile)) {
			//				// find excel.exe, kill it.
			//				HANDLE hProcess;
			//				hProcess = OpenProcess(PROCESS_ALL_ACCESS,
			//					FALSE,
			//					pe32.th32ProcessID);
			//				if (hProcess == NULL) printf("open process fail.\n");
			//				TerminateProcess(hProcess, 0);
			//				WaitForSingleObject(hProcess, INFINITE);
			//				CloseHandle(hProcess);
			//
			//				break; // after close, then break
			//			}
			//		} while (Process32Next(hProcessSnap, &pe32));
			//		CloseHandle(hProcessSnap);
			//	}
			//	else
			//		return FALSE;
			//}
			//else if (bCancelWindow)
			//{
			//	::PostMessage(hWnd, WM_CLOSE, NULL, NULL);
			//}
			return FALSE;
		}
		else if (bQuit)
		{
			//if (pShellManager != NULL)
			//{
			//	delete pShellManager;
			//}
			break;
		}
		else
			break;
	}


	//CLibWFXDemoDlg dlg;
	//m_pMainWnd = &dlg;

	//get LibWebFxScan.ini 
	HANDLE hFind;
	WIN32_FIND_DATAW FindFileData;
	TCHAR szIniPath[MAX_PATH] = { 0 };
	int dwlin = 0;
	
	dwlin = ::GetModuleFileName(NULL, szIniPath, _MAX_PATH);

	while (dwlin-- >0)
	{
		if (szIniPath[dwlin] == _T('\\')) {
			szIniPath[dwlin + 1] = 0;
			break;
		}
	}

	lstrcat(szIniPath, _T("LibWebFxScan.ini"));
	USES_CONVERSION;
	hFind = FindFirstFileW(T2W(szIniPath), &FindFileData);

	if (hFind == INVALID_HANDLE_VALUE)
	{
		if (GetSDKInstallPath(szIniPath))
		{
			if (szIniPath[_tcslen(szIniPath) - 1] != _T('\\'))
			{
				_tcscat_s(szIniPath, _T("\\"));
			}
		}
		lstrcat(szIniPath, _T("LibWebFxScan.ini"));
	}

	//m_NonUIAutoStart:  0-block mode  1-non-block mode
	int nStartMode = GetPrivateProfileInt(_T("Style"), _T("UseModeBlock "), 1, szIniPath);	

	INT_PTR nResponse;
	if (nStartMode)
	{
		CLibWFXDemoDlg_BlockMode dlg;
		m_pMainWnd = &dlg;
		nResponse = dlg.DoModal();
	}
	else
	{
		CLibWFXDemoDlg_NonBlockMode dlg;
		m_pMainWnd = &dlg;
		nResponse = dlg.DoModal();
	}

	//INT_PTR nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with OK
	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with Cancel
	}
	else if (nResponse == -1)
	{
		TRACE(traceAppMsg, 0, "Warning: dialog creation failed, so application is terminating unexpectedly.\n");
		TRACE(traceAppMsg, 0, "Warning: if you are using MFC controls on the dialog, you cannot #define _AFX_NO_MFC_CONTROLS_IN_DIALOGS.\n");
	}

	// Delete the shell manager created above.
	if (pShellManager != NULL)
	{
		delete pShellManager;
	}

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}

BOOL CLibWFXDemoApp::GetSDKInstallPath(TCHAR* szSDKInstallPath)
{
	HKEY  key = NULL;
	TCHAR szRegPath[MAX_PATH] = { 0 };

	for (int iRegType = 0; iRegType<REG_TYPE_COUNT; iRegType++)
	{
		switch (iRegType)
		{
		case REG_TYPE_W64NODE32:
			_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"));
			break;
		case REG_TYPE_COMMON:
			_stprintf_s(szRegPath, MAX_PATH, _T("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{02232A38-5FF5-47F3-A3C9-268F4588BEE8}_is1"));
			break;
		default:
			break;
		}

		if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, szRegPath, 0, KEY_QUERY_VALUE, &key) != ERROR_SUCCESS)
		{
			continue;
		}

		if (key)
		{
			DWORD type, size;
			size = MAX_PATH;
			if (!RegQueryValueEx(key, _T("InstallLocation"), NULL, &type, (LPBYTE)szSDKInstallPath, &size) == ERROR_SUCCESS)
				continue;
			else {
				return TRUE;
			}
		}
		else
		{
			continue;
		}
	}

	return FALSE;
}

