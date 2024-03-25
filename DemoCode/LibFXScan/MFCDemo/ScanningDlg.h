#pragma once


// ScanningDlg dialog

class ScanningDlg : public CDialogEx
{
	DECLARE_DYNAMIC(ScanningDlg)

public:
	ScanningDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~ScanningDlg();
	enum { IDD = IDD_SCANNING_DIALOG };
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
