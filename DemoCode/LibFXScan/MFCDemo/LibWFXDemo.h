
// LibWFXDemo.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CLibWFXDemoApp:
// See LibWFXDemo.cpp for the implementation of this class
//

class CLibWFXDemoApp : public CWinApp
{
public:
	CLibWFXDemoApp();

// Overrides
public:
	virtual BOOL InitInstance();
	virtual BOOL GetSDKInstallPath(TCHAR* szSDKInstallPath);
// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CLibWFXDemoApp theApp;