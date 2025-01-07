#pragma once


// WarmupDlg dialog

class WarmupDlg : public CDialogEx
{
	DECLARE_DYNAMIC(WarmupDlg)

public:
    void SetWarmupProgressPos(int nPos);

public:
	WarmupDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~WarmupDlg();

// Dialog Data
	enum { IDD = IDD_WARMUP_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
