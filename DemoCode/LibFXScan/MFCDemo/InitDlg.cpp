// InitDlg.cpp : implementation file
//

#include "stdafx.h"
#include "LibWFXDemo.h"
#include "InitDlg.h"
#include "afxdialogex.h"


// InitDlg dialog

IMPLEMENT_DYNAMIC(InitDlg, CDialogEx)

InitDlg::InitDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(IDD_INIT_DIALOG, pParent)
{

}

InitDlg::~InitDlg()
{
}

void InitDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(InitDlg, CDialogEx)
END_MESSAGE_MAP()


// InitDlg message handlers
