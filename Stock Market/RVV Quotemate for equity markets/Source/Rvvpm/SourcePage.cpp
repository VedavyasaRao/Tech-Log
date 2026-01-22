// SourcePage.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "SourcePage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSourcePage property page

IMPLEMENT_DYNCREATE(CSourcePage, CPropertyPage)

CSourcePage::CSourcePage() : CPropertyPage(CSourcePage::IDD)
{
	//{{AFX_DATA_INIT(CSourcePage)
	m_autorefresh = FALSE;
	m_datasrc = -1;
	m_news = _T("");
	m_quote = _T("");
	m_passwd = _T("");
	m_userid = _T("");
	//}}AFX_DATA_INIT
}

CSourcePage::~CSourcePage()
{
}

void CSourcePage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSourcePage)
	DDX_Check(pDX, IDC_AUTO, m_autorefresh);
	DDX_CBIndex(pDX, IDC_COMBO1, m_datasrc);
	DDX_Text(pDX, IDC_NEWS, m_news);
	DDX_Text(pDX, IDC_QUOTE, m_quote);
	DDX_Text(pDX, IDC_PASSWD, m_passwd);
	DDX_Text(pDX, IDC_USERID, m_userid);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CSourcePage, CPropertyPage)
	//{{AFX_MSG_MAP(CSourcePage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSourcePage message handlers

BOOL CSourcePage::OnInitDialog() 
{
	CPropertyPage::OnInitDialog();
	
	// TODO: Add extra initialization here
	m_szMasterFile = ((CRVVPMApp *)AfxGetApp())->m_szMasterFile;
	CComboBox	*pDataSrc = (CComboBox *)GetDlgItem(IDC_COMBO1);

	pDataSrc->AddString("WaterHouse");
	pDataSrc->AddString("RagingQuotes");
	pDataSrc->AddString("Yahoo");
	
	m_autorefresh = FALSE;
	m_datasrc = -1;
	m_news = _T("");
	m_quote = _T("");
	m_passwd = _T("");
	m_userid = _T("");

	char szBuf[100];
	int		ret;
	
	GetPrivateProfileString("Settings", "DataSource", "", szBuf, sizeof szBuf, m_szMasterFile);
	m_datasrc = pDataSrc->SelectString(0, szBuf);

	GetPrivateProfileString ("Settings", "UserId", "", szBuf, sizeof szBuf, m_szMasterFile);
	m_userid = szBuf;

	GetPrivateProfileString("Settings", "Passwd", "", szBuf, sizeof szBuf, m_szMasterFile);
	m_passwd = szBuf;

	ret = GetPrivateProfileInt("Settings", "QuoteRefresh", 5,  m_szMasterFile);
	itoa(ret, szBuf, 10);
	m_quote  =szBuf;

	ret = GetPrivateProfileInt("Settings", "NewsRefresh", 15,  m_szMasterFile);
	itoa(ret, szBuf, 10);
	m_news  =szBuf;

	m_autorefresh = GetPrivateProfileInt("Settings", "AutoRefresh", 1,  m_szMasterFile);

	UpdateData(FALSE);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CSourcePage::OnOK() 
{
	// TODO: Add your specialized code here and/or call the base class
	CComboBox	*pDataSrc = (CComboBox *)GetDlgItem(IDC_COMBO1);
	char szBuf[100];
	int ret;

	pDataSrc->GetLBText(m_datasrc , szBuf);
	WritePrivateProfileString("Settings", "DataSource", szBuf,  m_szMasterFile);

	WritePrivateProfileString ("Settings", "UserId", m_userid, m_szMasterFile);

	WritePrivateProfileString("Settings", "Passwd", m_passwd, m_szMasterFile);

	ret = WritePrivateProfileString("Settings", "QuoteRefresh", m_quote,  m_szMasterFile);

	ret = WritePrivateProfileString("Settings", "NewsRefresh", m_news,  m_szMasterFile);

	itoa (m_autorefresh , szBuf, 10);
	WritePrivateProfileString("Settings", "AutoRefresh", szBuf,  m_szMasterFile);

	
	CPropertyPage::OnOK();
}
