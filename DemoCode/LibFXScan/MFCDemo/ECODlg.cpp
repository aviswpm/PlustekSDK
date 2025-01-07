// ECODlg.cpp : implementation file
//

#include "stdafx.h"
#include "LibWFXDemo.h"
#include "ECODlg.h"
#include "afxdialogex.h"


// CECODlg dialog

IMPLEMENT_DYNAMIC(CECODlg, CDialogEx)

CECODlg::CECODlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CECODlg::IDD, pParent)
{

}

CECODlg::CECODlg(unsigned long& ulTime, CWnd* pParent/* = NULL*/)   // standard constructor
    : CDialogEx(CECODlg::IDD, pParent)
{
    m_pulTime = &ulTime;
}

CECODlg::~CECODlg()
{
}

void CECODlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CECODlg, CDialogEx)
    ON_WM_DESTROY()
END_MESSAGE_MAP()


// CECODlg message handlers


BOOL CECODlg::OnInitDialog()
{
    CDialogEx::OnInitDialog();

    // TODO:  Add extra initialization here
    CString szTime;
    szTime.Format(_T("%u"), *m_pulTime);
    GetDlgItem(IDC_EDIT_ECOTIME)->SetWindowText(szTime);

    return TRUE;  // return TRUE unless you set the focus to a control
    // EXCEPTION: OCX Property Pages should return FALSE
}


void CECODlg::OnDestroy()
{
    CDialogEx::OnDestroy();

    // TODO: Add your message handler code here
    
    CString szTime;
    GetDlgItem(IDC_EDIT_ECOTIME)->GetWindowText(szTime);
    *m_pulTime = _tstoi(szTime);
}
