#pragma once
#include "..\inc\LibWebFXScan\LibWebFXScan.h"

												
	HMODULE                   m_hLibWFX;
	LIBWFX_INITEX             m_pfnLibWFX_InitEx;
	LIBWFX_DEINIT             m_pfnLibWFX_DeInit;
	LIBWFX_CLOSEDEVICE        m_pfnLibWFX_CloseDevice;
	LIBWFX_GETLASTERRORCODE   m_pfnLibWFX_GetLastErrorCode;
	LIBWFX_SYNCHRONIZESCAN	  m_pfnLibWFX_SynchronizeScan;
	LIBWFX_SETPROPERTY        m_pfnLibWFX_SetProperty;
	LIBWFX_PAPERREADY         m_pfnLibWFX_PaperReady;
	
	bool InitLib(void);
	wchar_t* rtrim(wchar_t *str);
	int main();
	void AutoCapture();