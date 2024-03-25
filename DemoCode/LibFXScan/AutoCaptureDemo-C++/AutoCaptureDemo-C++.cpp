#include "stdafx.h"
#include "AutoCaptureDemo-C++.h"
#include <string.h>
#include <thread>

#define LIBWFX_API __stdcall
using namespace std;

#define TIMEOUT 60000    //60sec

wchar_t* rtrim(wchar_t *str)
{
	if (str == NULL || *str == '\0')
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

bool InitLib(void)
{
	m_hLibWFX = ::LoadLibrary(LIBWFX_DLLNAME);
	if (m_hLibWFX == NULL)
	{
		printf_s("Status:[Load LibWebFXScan Fail]\n");
		return FALSE;
	}

	m_pfnLibWFX_InitEx = (LIBWFX_INITEX)::GetProcAddress(m_hLibWFX, LIBWFX_API_INITEX);
	if (m_pfnLibWFX_InitEx == NULL)
	{
		printf_s("Status:[Load m_pfnLibWFX_InitEx Fail]\n");
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_DeInit = (LIBWFX_DEINIT)::GetProcAddress(m_hLibWFX, LIBWFX_API_DEINIT);
	if (m_pfnLibWFX_DeInit == NULL)
	{
		printf_s("Status:[Get LIBWFX_API_DEINIT Fail]\n");
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_CloseDevice = (LIBWFX_CLOSEDEVICE)::GetProcAddress(m_hLibWFX, LIBWFX_API_CLOSE_DEVICE);
	if (m_pfnLibWFX_CloseDevice == NULL)
	{
		printf_s("Status:[Get LIBWFX_API_CLOSE_DEVICE Fail]\n");
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_SynchronizeScan = (LIBWFX_SYNCHRONIZESCAN)::GetProcAddress(m_hLibWFX, LIBWFX_API_SYNCHRONIZESCAN); //burgess 201215
	if (m_pfnLibWFX_SynchronizeScan == NULL)
	{
		printf_s("Status:[Get LIBWFX_API_SYNCHRONIZESCAN Fail]\n");
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_SetProperty = (LIBWFX_SETPROPERTY)::GetProcAddress(m_hLibWFX, LIBWFX_API_SET_PROPERTY);
	if (m_pfnLibWFX_SetProperty == NULL)
	{
		printf_s("Status:[Get LIBWFX_API_SET_PROPERTY Fail]");
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_PaperReady = (LIBWFX_PAPERREADY)::GetProcAddress(m_hLibWFX, LIBWFX_API_PAPER_READY);
	if (m_pfnLibWFX_PaperReady == NULL)
	{
		printf_s("Status:[Get LIBWFX_API_PAPERREADY Fail]");
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	m_pfnLibWFX_GetLastErrorCode = (LIBWFX_GETLASTERRORCODE)::GetProcAddress(m_hLibWFX, LIBWFX_API_GETLASTERRORCODE);
	if (m_pfnLibWFX_GetLastErrorCode == NULL)
	{
		printf_s("Status:[Get LIBWFX_API_GETLASTERRORCODE Fail]");
		::FreeLibrary(m_hLibWFX);
		return FALSE;
	}

	printf_s("Status:[Load LibWebFXScan Success]\n");
	return TRUE;
}

int main(void)
{
	std::thread t1(AutoCapture);
	t1.join();	
}

void AutoCapture()
{
	bool DoScan = false;	
	const wchar_t* szScanImageList = NULL;
	const wchar_t* szOCRResultList = NULL;
	const wchar_t* szExceptionRet = NULL;
	const wchar_t* szEventRet = NULL;
	wchar_t* command = L"{\"device-name\":\"A64\",\"source\":\"Camera\",\"recognize-type\":\"passport\"}";
	
	//get command from bat file "AutoCaptureDemo-C++.bat"
	LPWSTR *szArg = NULL;
	int argCount = 0;
	szArg = CommandLineToArgvW(GetCommandLine(), &argCount);
	if (argCount > 1)
		command = szArg[1];

	//do init
	InitLib();
	ENUM_LIBWFX_ERRCODE enErrCode = m_pfnLibWFX_InitEx(LIBWFX_INIT_MODE_NORMAL);
	
	if (enErrCode == LIBWFX_ERRCODE_NO_OCR)
		wprintf(_T("Status:[No Recognize Tool]\n"));
	else if (enErrCode == LIBWFX_ERRCODE_NO_AVI_OCR)
		wprintf(_T("Status:[No AVI Recognize Tool]\n"));
	else if (enErrCode == LIBWFX_ERRCODE_NO_DOC_OCR)
		wprintf(_T("Status:[No DOC Recognize Tool]\n"));
	else if (enErrCode == LIBWFX_ERRCODE_PATH_TOO_LONG)
	{
		wprintf(_T("Status:[Path Is Too Long (max limit: 130 bits)]\n"));
		wprintf(_T("Status:[LibWFX_InitEx Fail]\n"));
	}
	else if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		wprintf(L"Status:[LibWFX_InitEx Fail[%d]]\n", enErrCode);
		return;
	}
	
	enErrCode = m_pfnLibWFX_SetProperty(command, NULL, NULL);
	if (enErrCode != LIBWFX_ERRCODE_SUCCESS)
	{
		CString szErr;
		const wchar_t* szErrorMsg = NULL;		
		m_pfnLibWFX_GetLastErrorCode(enErrCode, &szErrorMsg);	

		int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, NULL, 0, NULL, NULL);		
		char* pszUTF8IP = new char[nUTF8LenIP + 1];		
		WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, pszUTF8IP, nUTF8LenIP, NULL, NULL);

		CString szErr2(pszUTF8IP);
		szErr.Format(_T("Status:[LibWFX_SetProperty Fail [%d]]  %s \n"), enErrCode, szErr2);
		wprintf(const_cast<TCHAR *>(szErr.GetString()));  //get fail message
	}

	int timer = 0, sum = 0;
	int totaltime = 0;

	while (totaltime < TIMEOUT)
	{	
		timer = 0;
		sum = 0;
		while (timer < 3)
		{
			std::this_thread::sleep_for(std::chrono::milliseconds(300));
			totaltime += 300;
			sum++;
			enErrCode = m_pfnLibWFX_PaperReady();
			if (enErrCode == LIBWFX_ERRCODE_SUCCESS)
				timer++;
			
			if (sum == 4)
			{				
				sum = 0;
				timer = 0;
				if (DoScan)
					DoScan = false;
				wprintf(L"Please put the card.\n");
				std::this_thread::sleep_for(std::chrono::milliseconds(1000));  //option
				totaltime += 1000;
			}
		}

		if (DoScan)
		{
			wprintf(L"The card is continuously detected, please remove the card.\n");
			std::this_thread::sleep_for(std::chrono::milliseconds(1000));  //option
			totaltime += 1000;
			continue;
		}

		enErrCode = m_pfnLibWFX_SynchronizeScan(command, &szScanImageList, &szOCRResultList, &szExceptionRet, &szEventRet);

		int nUTF8Ecxeption = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szExceptionRet, -1, NULL, 0, NULL, NULL);
		int nUTF8Event = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szEventRet, -1, NULL, 0, NULL, NULL);

		if (enErrCode != LIBWFX_ERRCODE_SUCCESS && enErrCode != LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
		{
			CString szErr;
			const wchar_t* szErrorMsg = NULL;
			m_pfnLibWFX_GetLastErrorCode(enErrCode, &szErrorMsg);

			int nUTF8LenIP = WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, NULL, 0, NULL, NULL);
			char* pszUTF8IP = new char[nUTF8LenIP + 1];
			WideCharToMultiByte(CP_UTF8, 0, (wchar_t*)szErrorMsg, -1, pszUTF8IP, nUTF8LenIP, NULL, NULL);

			CString szErr2(pszUTF8IP);
			szErr.Format(_T("Status:[LibWFX_SynchronizeScan Fail [%d]]  %s \n"), enErrCode, szErr2);
			wprintf(const_cast<TCHAR *>(szErr.GetString()));  //get fail message
		}		
		else if (nUTF8Event > 1) //event happen
		{
			CString szErr;
			szErr.Format(_T("%s"), szEventRet);
			wprintf(_T("Status:[Device Ready!] \n"));
			wprintf(const_cast<TCHAR *>(szErr.GetString()));   //get event message

			if (szErr != "LIBWFX_EVENT_UVSECURITY_DETECTED[0]" && szErr != "LIBWFX_EVENT_UVSECURITY_DETECTED[1]")
			{
				printf("Status:[Scan End]\n");
				continue;
			}

			if (enErrCode == LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
			{
				wprintf(_T("Status:[There are some mismatched key in command]\n"));
			}

			wchar_t* next_token, *next_token2;
			const wchar_t delim[] = L"|&|";
			wchar_t* token = wcstok_s((wchar_t*)szScanImageList, delim, &next_token);
			wchar_t* token2 = wcstok_s((wchar_t*)szOCRResultList, delim, &next_token2);

			while (token)
			{
				wprintf(rtrim(token)); //get each image path
				wprintf(L"\n");
				wprintf(rtrim(token2)); //get each ocr result
				wprintf(L"\n");				

				token = wcstok_s(NULL, delim, &next_token);
				token2 = wcstok_s(NULL, delim, &next_token2);
			}
		}
		else
		{
			if (enErrCode == LIBWFX_ERRCODE_COMMAND_KEY_MISMATCH)
			{
				wprintf(_T("Status:[There are some mismatched key in command]\n"));			
			}

			if (nUTF8Ecxeption > 1) //exception happen
			{
				CString szErr;
				szErr.Format(_T("%s"), szExceptionRet);
				wprintf(_T("Status:[Device Ready!] \n"));
				wprintf(const_cast<TCHAR *>(szErr.GetString()));   //get exception message															   
			}

			wchar_t* next_token, *next_token2;
			const wchar_t delim[] = L"|&|";
			wchar_t* token = wcstok_s((wchar_t*)szScanImageList, delim, &next_token);
			wchar_t* token2 = wcstok_s((wchar_t*)szOCRResultList, delim, &next_token2);

			while (token)
			{
				wprintf(rtrim(token)); //get each image path
				wprintf(L"\n");
				wprintf(rtrim(token2)); //get each ocr result
				wprintf(L"\n");

				token = wcstok_s(NULL, delim, &next_token);
				token2 = wcstok_s(NULL, delim, &next_token2);
			}
		}
		wprintf(_T("Status:[Scan End]\n"));
		DoScan = true;
	}

	//do de-init before close window
	m_pfnLibWFX_CloseDevice();
	m_pfnLibWFX_DeInit();
	exit(0);
}


