// Login.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "Login.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CLogin dialog


CLogin::CLogin(CWnd* pParent /*=NULL*/)
	: CDialog(CLogin::IDD, pParent)
{
	//{{AFX_DATA_INIT(CLogin)
	m_passwd = _T("");
	m_save = FALSE;
	m_userid = _T("");
	//}}AFX_DATA_INIT
}


void CLogin::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CLogin)
	DDX_Text(pDX, IDC_PASSWD, m_passwd);
	DDX_Check(pDX, IDC_SAVE, m_save);
	DDX_Text(pDX, IDC_USERID, m_userid);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CLogin, CDialog)
	//{{AFX_MSG_MAP(CLogin)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CLogin message handlers

void CLogin::OnOK() 
{
	// TODO: Add extra validation here
	CRVVPMApp *pApp = ((CRVVPMApp *)AfxGetApp());

	UpdateData();

	pApp->m_userid = m_userid;
	pApp->m_passwd = m_passwd;
	
	if (m_save)
	{
		WritePrivateProfileString("Settings", "UserId", m_userid, pApp->m_szMasterFile);
		WritePrivateProfileString("Settings", "Passwd", m_passwd, pApp->m_szMasterFile);
	}

	CDialog::OnOK();
	
}
