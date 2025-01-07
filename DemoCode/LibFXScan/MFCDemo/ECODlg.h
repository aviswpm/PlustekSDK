#pragma once


// CECODlg dialog

class CECODlg : public CDialogEx
{
	DECLARE_DYNAMIC(CECODlg)

private:
    unsigned long* m_pulTime;
public:
	CECODlg(CWnd* pParent = NULL);   // standard constructor
    CECODlg(unsigned long& ulTime, CWnd* pParent = NULL);   // standard constructor
	virtual ~CECODlg();

// Dialog Data
	enum { IDD = IDD_ECO_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
    virtual BOOL OnInitDialog();
    afx_msg void OnDestroy();
};
