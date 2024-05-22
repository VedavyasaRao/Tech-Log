// APITesterPrintDlg.cpp : implementation file
//

#include "stdafx.h"
#include "APITester.h"
#include "APITesterPrintDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAPITesterPrintDlg dialog


CAPITesterPrintDlg::CAPITesterPrintDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CAPITesterPrintDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAPITesterPrintDlg)
	m_funcdef = FALSE;
	m_logfile = FALSE;
	//}}AFX_DATA_INIT
}


void CAPITesterPrintDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAPITesterPrintDlg)
	DDX_Check(pDX, IDC_CHECK1, m_funcdef);
	DDX_Check(pDX, IDC_CHECK2, m_logfile);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CAPITesterPrintDlg, CDialog)
	//{{AFX_MSG_MAP(CAPITesterPrintDlg)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAPITesterPrintDlg message handlers

