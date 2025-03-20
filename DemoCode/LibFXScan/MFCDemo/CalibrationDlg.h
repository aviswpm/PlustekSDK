#pragma once


// CalibrationDlg dialog

class CalibrationDlg : public CDialogEx
{
	DECLARE_DYNAMIC(CalibrationDlg)

public:
	CalibrationDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CalibrationDlg();
	enum { IDD = IDD_CALIBRATION_DIALOG	};

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
