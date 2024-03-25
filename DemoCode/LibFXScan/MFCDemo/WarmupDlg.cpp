// WarmupDlg.cpp : implementation file
//

#include "stdafx.h"
#include "LibWFXDemo.h"
#include "WarmupDlg.h"
#include "afxdialogex.h"


// WarmupDlg dialog

IMPLEMENT_DYNAMIC(WarmupDlg, CDialogEx)

WarmupDlg::WarmupDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(WarmupDlg::IDD, pParent)
{

}

WarmupDlg::~WarmupDlg()
{
}

void WarmupDlg::SetWarmupProgressPos(int nPos)
{
    ((CProgressCtrl *)GetDlgItem(IDC_PROGRESS_WARMUP))->SetPos(nPos);
}

void WarmupDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(WarmupDlg, CDialogEx)
END_MESSAGE_MAP()


// WarmupDlg message handlers
