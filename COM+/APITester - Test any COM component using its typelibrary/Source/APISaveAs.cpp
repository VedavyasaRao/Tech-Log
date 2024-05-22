// APISaveAs.cpp : implementation file
//

#include "stdafx.h"
#include "APITester.h"
#include "APISaveAs.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAPISaveAs dialog


CAPISaveAs::CAPISaveAs(CWnd* pParent /*=NULL*/)
	: CDialog(CAPISaveAs::IDD, pParent)
{
	//{{AFX_DATA_INIT(CAPISaveAs)
	m_logfile = _T("");
	m_prototype = _T("");
	//}}AFX_DATA_INIT
}


void CAPISaveAs::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAPISaveAs)
	DDX_Text(pDX, IDC_EDIT1, m_logfile);
	DDX_Text(pDX, IDC_EDIT2, m_prototype);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CAPISaveAs, CDialog)
	//{{AFX_MSG_MAP(CAPISaveAs)
	ON_BN_CLICKED(IDC_FUNCFILEBTN, OnFuncfilebtn)
	ON_BN_CLICKED(IDC_LOGFILEBTN, OnLogfilebtn)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAPISaveAs message handlers

void CAPISaveAs::OnFuncfilebtn() 
{
	// TODO: Add your control notification handler code here
	CFileDialog dlgFile(FALSE, "INI", NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, "API Tester Function defintion Files (*.ini)|*.ini|");
	if (dlgFile.DoModal() == IDOK)
	{
		m_prototype = dlgFile.GetPathName();
		UpdateData(FALSE);
	}
	
}

void CAPISaveAs::OnLogfilebtn() 
{
	// TODO: Add your control notification handler code here
	CFileDialog dlgFile(FALSE, "LOG", NULL, OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, "API Tester Log Files (*.log)|*.log|");
	if (dlgFile.DoModal() == IDOK)
	{
		m_logfile = dlgFile.GetPathName();
		UpdateData(FALSE);
	}
}

void CAPISaveAs::OnOK() 
{
	// TODO: Add extra validation here
	
	UpdateData(TRUE);
	if (m_prototype != "")
	{
		char buf[1000];
		GetWindowsDirectory(buf, 1000);

		CString funcdeffile;
		funcdeffile  = CString(buf) + CString("\\") + funcdeffile;
		funcdeffile = funcdeffile  + ((CAPITesterApp*)AfxGetApp())->m_profilefile;
		
		CopyFile(funcdeffile, m_prototype, FALSE );
	}

	if (m_logfile != "")
	{
		CString logfilename;
		logfilename = ((CAPITesterApp*)AfxGetApp())->m_logfile;

		CopyFile(logfilename, m_logfile, FALSE );
	}

	CDialog::OnOK();
}
