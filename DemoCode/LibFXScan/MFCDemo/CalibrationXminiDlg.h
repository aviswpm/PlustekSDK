#pragma once


// CalibrationXminiDlg dialog

class CalibrationXminiDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CalibrationXminiDlg)

public:
	CalibrationXminiDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CalibrationXminiDlg();
	enum { IDD = IDD_CALIBRATION_XMINI_DIALOG };


protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
