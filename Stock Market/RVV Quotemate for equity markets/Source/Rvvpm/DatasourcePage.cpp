// DatasourcePage.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "DatasourcePage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDatasourcePage property page

IMPLEMENT_DYNCREATE(CDatasourcePage, CPropertyPage)

CDatasourcePage::CDatasourcePage() : CPropertyPage(CDatasourcePage::IDD)
{
	//{{AFX_DATA_INIT(CDatasourcePage)
	m_autorefresh = FALSE;
	m_datasrc = -1;
	m_quote = _T("");
	m_news = _T("");
	m_passwd = _T("");
	m_enablerigger = FALSE;
	//}}AFX_DATA_INIT
}

CDatasourcePage::~CDatasourcePage()
{
}

void CDatasourcePage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDatasourcePage)
	DDX_Check(pDX, IDC_AUTO, m_autorefresh);
	DDX_CBIndex(pDX, IDC_COMBO1, m_datasrc);
	DDX_Text(pDX, IDC_QUOTE, m_quote);
	DDX_Text(pDX, IDC_NEWS, m_news);
	DDX_Text(pDX, IDC_PASSWD, m_passwd);
	DDX_Check(pDX, IDC_AUTO3, m_enablerigger);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDatasourcePage, CPropertyPage)
	//{{AFX_MSG_MAP(CDatasourcePage)
	ON_CBN_SELCHANGE(IDC_COMBO1, OnSelchangeCombo1)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDatasourcePage message handlers

BOOL CDatasourcePage::OnInitDialog() 
{
	CPropertyPage::OnInitDialog();
	
	// TODO: Add extra initialization here
	CComboBox	*pDataSrc = (CComboBox *)GetDlgItem(IDC_COMBO1);
	m_pimDataSrcs = &((CRVVPMApp *)AfxGetApp())->m_imDataSrcs;
	
	bool bfirst = true;
	auto  datasrcs = m_pimDataSrcs->GetSectionNames();
	for (auto titr = datasrcs.begin(); titr != datasrcs.end(); ++titr)
	{
		if (!bfirst)
			pDataSrc->AddString(titr->c_str());
		bfirst = false;
	}

	m_autorefresh = FALSE;
	m_datasrc = -1;
	m_quote = _T("");
	m_news = _T(""); 
	m_passwd = _T("");

	char szBuf[2000];
	string sbuf;
	int		ret;
	
	m_pimDataSrcs->GetPrivateProfileString("Settings", "Current", "", sbuf);
	m_datasrc = pDataSrc->SelectString(0, sbuf.c_str());
	m_passwd = GetPasscode(sbuf.c_str());

	ret = m_pimDataSrcs->GetPrivateProfileInt("Settings", "QuoteRefresh", 5);
	itoa(ret, szBuf, 10);
	m_quote  =szBuf;

	ret = m_pimDataSrcs->GetPrivateProfileInt("Settings", "NewsRefresh", 15);
	itoa(ret, szBuf, 10);
	m_news = szBuf;

	m_autorefresh = m_pimDataSrcs->GetPrivateProfileInt("Settings", "AutoRefresh", 1);
	m_enablerigger = m_pimDataSrcs->GetPrivateProfileInt("Settings", "EnableTrigger", 1);


	UpdateData(FALSE);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CDatasourcePage::OnOK() 
{
	// TODO: Add your specialized code here and/or call the base class
	CComboBox	*pDataSrc = (CComboBox *)GetDlgItem(IDC_COMBO1);
	char szBuf[100];
	int ret;

	pDataSrc->GetLBText(m_datasrc , szBuf);
	m_pimDataSrcs->WritePrivateProfileString("Settings", "Current", szBuf);
	SetPasscode(szBuf, m_passwd);

	itoa (m_autorefresh , szBuf, 10);
	m_pimDataSrcs->WritePrivateProfileString("Settings", "AutoRefresh", szBuf);
	m_pimDataSrcs->WritePrivateProfileString("Settings", "QuoteRefresh", m_quote.GetBuffer());
	m_pimDataSrcs->WritePrivateProfileString("Settings", "NewsRefresh", m_news.GetBuffer());
	itoa(m_enablerigger, szBuf, 10);
	m_pimDataSrcs->WritePrivateProfileString("Settings", "EnableTrigger", szBuf);

	((CRVVPMApp *)AfxGetApp())->SetWindowTitle();
	CPropertyPage::OnOK();
}

void CDatasourcePage::OnSelchangeCombo1()
{
	// TODO: Add your control notification handler code here

	CComboBox	*pDataSrc = (CComboBox *)GetDlgItem(IDC_COMBO1);

	char szBuf[2000];
	pDataSrc->GetLBText(pDataSrc->GetCurSel(), szBuf);

	m_passwd = GetPasscode(szBuf);
	m_datasrc = pDataSrc->GetCurSel();
	UpdateData(FALSE);

}

CString CDatasourcePage::GetPasscode(CString szBuf)
{
	string szname, szpasswd;

	m_pimDataSrcs->GetPrivateProfileString(szBuf.GetBuffer(), "name", "", szname);
	m_pimDataSrcs->GetPrivateProfileString(szBuf.GetBuffer(), "value", "", szpasswd);
	return (strlen(szname.c_str()) == 0 && strlen(szpasswd.c_str()) == 0)?"":(CString(szname.c_str()) + "=" + CString(szpasswd.c_str()));
}

void CDatasourcePage::SetPasscode(CString szBuf, CString val)
{
	char szname[100]="";
	char szpasswd[2000]="";

	if (val.GetLength() != 0)
	{
		char *cptr = ( char*)strchr((const char*)val, '=');
		*cptr = 0;
		strcpy(szname, val);
		strcpy(szpasswd, ++cptr);
	}

	m_pimDataSrcs->WritePrivateProfileString(szBuf.GetBuffer(), "name", szname);
	m_pimDataSrcs->WritePrivateProfileString(szBuf.GetBuffer(), "value", szpasswd);
}
