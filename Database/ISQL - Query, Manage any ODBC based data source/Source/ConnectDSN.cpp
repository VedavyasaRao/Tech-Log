// ConnectDSN.cpp : implementation file
//

#include "stdafx.h"
#include "isql.h"
#include "ConnectDSN.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CConnectDSN dialog


CConnectDSN::CConnectDSN(CWnd* pParent /*=NULL*/)
	: CDialog(CConnectDSN::IDD, pParent)
{
	//{{AFX_DATA_INIT(CConnectDSN)
	m_dsn = _T("");
	m_passwd = _T("");
	m_userid = _T("");
	m_chkpwd = FALSE;
	//}}AFX_DATA_INIT
}


void CConnectDSN::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CConnectDSN)
	DDX_Text(pDX, IDC_DSN, m_dsn);
	DDX_Text(pDX, IDC_PASSWD, m_passwd);
	DDX_Text(pDX, IDC_USERID, m_userid);
	DDX_Check(pDX, IDC_CHKPSW, m_chkpwd);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CConnectDSN, CDialog)
	//{{AFX_MSG_MAP(CConnectDSN)
	ON_BN_CLICKED(IDC_DSNBTN, OnDsnbtn)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CConnectDSN message handlers

void CConnectDSN::OnOK() 
{
	// TODO: Add extra validation here
	
	RETCODE	retcode;

	BeginWaitCursor();
	((CMDIFrameWnd*)AfxGetMainWnd())->SetMessageText( "Connecting, please wait ...");
	UpdateData( TRUE );
	retcode = SQLConnect(m_hdbc, (UCHAR FAR *)(LPCSTR)m_dsn, SQL_NTS, (UCHAR FAR *)(LPCSTR)m_userid, SQL_NTS, (UCHAR FAR *)(LPCSTR)m_passwd, SQL_NTS); 
	EndWaitCursor();
	if 	(retcode != SQL_SUCCESS  && retcode != SQL_SUCCESS_WITH_INFO)
	{
		((CIsqlApp*)AfxGetApp())->ShowSQLError();
		return;
	}
	
	CDialog::OnOK();
}

void CConnectDSN::OnDsnbtn() 
{
	// TODO: Add your control notification handler code here

	RETCODE	retcode;

	unsigned char szConnStrOut[100];
	short  cbConnStrOut;
	((CMDIFrameWnd*)AfxGetMainWnd())->SetMessageText( "Getting DSN information, please wait ...");
	BeginWaitCursor();
	retcode = SQLDriverConnect(m_hdbc, ::GetActiveWindow(), NULL, 0, szConnStrOut, 100, &cbConnStrOut, SQL_DRIVER_PROMPT);
	EndWaitCursor();
	if 	(retcode == SQL_SUCCESS  || retcode == SQL_SUCCESS_WITH_INFO)
	{
		CDialog::OnOK();
		
		CString s(szConnStrOut);
		int		pos, pos1;

		pos = s.Find("DSN=");
		if (pos != -1)
		{
			pos +=4;
			pos1 = s.Mid(pos).Find(";");
			if (pos1 != -1)
			{
				m_dsn  = s.Mid(pos, pos1);
			}
			else
				return;

		}
		else
			return;

		pos = s.Find("UID=");
		if (pos != -1)
		{
			pos +=4;
			pos1 = s.Mid(pos).Find(";");
			if (pos1 != -1)
			{
				m_userid  = s.Mid(pos, pos1);
			}
			else
				return;

		}
		else
			return;

		pos = s.Find("PWD=");
		if (pos != -1)
		{
			pos +=4;
			pos1 = s.Mid(pos).Find(";");
			if (pos1 != -1)
			{
				m_passwd  = s.Mid(pos, pos1);
			}
			else
				return;

		}
		else
			return;

	}
	else
		((CIsqlApp*)AfxGetApp())->ShowSQLError();
}

void CConnectDSN::OnCancel() 
{
	// TODO: Add extra cleanup here
	if (AfxMessageBox("This is going to terminate the application.\nDo you want to continue?", MB_YESNO | MB_ICONSTOP ) == IDNO) 
		return;

	CDialog::OnCancel();
}
