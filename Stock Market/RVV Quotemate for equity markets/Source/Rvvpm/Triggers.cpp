// Triggers.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "Triggers.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTriggers dialog


CTriggers::CTriggers(CWnd* pParent /*=NULL*/)
	: CDialog(CTriggers::IDD, pParent)
{
	//{{AFX_DATA_INIT(CTriggers)
	m_highmvpts = _T("");
	m_highpc = _T("");
	m_highpr = _T("");
	m_highpts = _T("");
	m_lowmvpts = _T("");
	m_lowpc = _T("");
	m_lowpr = _T("");
	m_lowpts = _T("");
	m_bought = _T("");
	m_paid = _T("");
	m_highmvtds = _T("");
	m_lowmvtds = _T("");
	m_newspd = _T("");
	//}}AFX_DATA_INIT
}


void CTriggers::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CTriggers)
	DDX_Text(pDX, IDC_EDIT_HIGHMVPTS, m_highmvpts);
	DDX_Text(pDX, IDC_EDIT_HIGHPC, m_highpc);
	DDX_Text(pDX, IDC_EDIT_HIGHPR, m_highpr);
	DDX_Text(pDX, IDC_EDIT_HIGHPTS, m_highpts);
	DDX_Text(pDX, IDC_EDIT_LOW_MV_PTS, m_lowmvpts);
	DDX_Text(pDX, IDC_EDIT_LOWPC, m_lowpc);
	DDX_Text(pDX, IDC_EDIT_LOWPR, m_lowpr);
	DDX_Text(pDX, IDC_EDIT_LOWPTS, m_lowpts);
	DDX_Text(pDX, IDC_EDIT_BOUGHT, m_bought);
	DDX_Text(pDX, IDC_EDIT_PAID, m_paid);
	DDX_Text(pDX, IDC_EDIT_HIGHMV_TD, m_highmvtds);
	DDX_Text(pDX, IDC_EDIT_LOWMV_TD, m_lowmvtds);
	DDX_Text(pDX, IDC_EDIT_NEWPD, m_newspd);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CTriggers, CDialog)
	//{{AFX_MSG_MAP(CTriggers)
	ON_CBN_SELCHANGE(IDC_COMBO1, OnSelchangeCombo1)
	ON_COMMAND(ID_QM_RESET, OnQmReset)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTriggers message handlers

BOOL CTriggers::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	
	m_szMasterFile = ((CRVVPMApp *)AfxGetApp())->m_szMasterFile;
	
	char buf[50];
	GetPrivateProfileString("Settings", "Current", "", buf, sizeof buf, m_szMasterFile);
	m_portfolio = buf;
	LoadPortfolio();

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CTriggers::OnSelchangeCombo1() 
{
	// TODO: Add your control notification handler code here
	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	

	if (pPortfs->GetCurSel() == CB_ERR)
		return;

	SaveSymbol();
	char buf[20];
	pPortfs->GetLBText(pPortfs->GetCurSel(), buf);
	m_symbol = buf;
	LoadSymbol();


}

void CTriggers::LoadPortfolio()
{
	// TODO: Add your control notification handler code here

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	

	char	lpszReturnBuffer[1000], *pprtptr = lpszReturnBuffer;

	memset(lpszReturnBuffer, 0, sizeof lpszReturnBuffer);
	GetPrivateProfileSection( m_portfolio, lpszReturnBuffer,  sizeof lpszReturnBuffer, m_szMasterFile );

	while (strlen(pprtptr) )
	{
		pPortfs->AddString( pprtptr );
		pprtptr += strlen(pprtptr);
		pprtptr++;
	}
	int i = pPortfs->FindString(0, m_symbol);
	if ( i != CB_ERR)
	{
		pPortfs->SetCurSel(i);
		LoadSymbol();
	}

	UpdateData(FALSE);
}

void CTriggers::LoadSymbol()
{

	char buf[30];

	if (m_symbol == "")
		return;

	GetPrivateProfileString(m_symbol, "Bought", "", buf, sizeof buf, m_szSymbdataFile);
	m_bought = buf;

	GetPrivateProfileString(m_symbol, "Paid", "", buf, sizeof buf, m_szSymbdataFile);
	m_paid = buf;

	GetPrivateProfileString(m_symbol, "HIGHPRICE", "", buf, sizeof buf, m_szSymbdataFile);
	m_highpr = buf;

	GetPrivateProfileString(m_symbol, "LOWPRICE", "", buf, sizeof buf, m_szSymbdataFile);
	m_lowpr = buf;

	GetPrivateProfileString(m_symbol, "HIGHPOINTS", "", buf, sizeof buf, m_szSymbdataFile);
	m_highpts = buf;

	GetPrivateProfileString(m_symbol, "LOWPOINTS", "", buf, sizeof buf, m_szSymbdataFile);
	m_lowpts = buf;

	GetPrivateProfileString(m_symbol, "HIGHPERCENT", "", buf, sizeof buf, m_szSymbdataFile);
	m_highpc = buf;

	GetPrivateProfileString(m_symbol, "LOWPERCENT", "", buf, sizeof buf, m_szSymbdataFile);
	m_lowpc = buf;

	GetPrivateProfileString(m_symbol, "HIGHMOVEPOINTS", "", buf, sizeof buf, m_szSymbdataFile);
	m_highmvpts = buf;

	GetPrivateProfileString(m_symbol, "HIGHMOVETRADES", "", buf, sizeof buf, m_szSymbdataFile);
	m_highmvtds = buf;

	GetPrivateProfileString(m_symbol, "HIGHMOVEPOINTS", "", buf, sizeof buf, m_szSymbdataFile);
	m_lowmvpts = buf;

	GetPrivateProfileString(m_symbol, "LOWMOVETRADES", "", buf, sizeof buf, m_szSymbdataFile);
	m_lowmvtds = buf;

	GetPrivateProfileString(m_symbol, "NEWSPERIOD", "", buf, sizeof buf, m_szSymbdataFile);
	m_newspd = buf;

	UpdateData(FALSE);

}

void CTriggers::SaveSymbol()
{

	char buf[30];

	if (m_symbol == "")
		return;

	UpdateData(TRUE);
	lstrcpy(buf,  m_bought );
	WritePrivateProfileString(  m_symbol, "Bought", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_paid );
	WritePrivateProfileString(  m_symbol, "Paid", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_highpr );
	WritePrivateProfileString(  m_symbol, "HIGHPRICE", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_lowpr );
	WritePrivateProfileString(  m_symbol, "LOWPRICE", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_highpts );
	WritePrivateProfileString(  m_symbol, "HIGHPOINTS", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_lowpts );
	WritePrivateProfileString(  m_symbol, "LOWPOINTS", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_highpc );
	WritePrivateProfileString(  m_symbol, "HIGHPERCENT", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_lowpc );
	WritePrivateProfileString(  m_symbol, "LOWPERCENT", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_highmvpts );
	WritePrivateProfileString(  m_symbol, "HIGHMOVEPOINTS", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_highmvtds );
	WritePrivateProfileString(  m_symbol, "HIGHMOVETRADES", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_lowmvpts );
	WritePrivateProfileString(  m_symbol, "HIGHMOVEPOINTS", buf,   m_szSymbdataFile);

	lstrcpy(buf,  m_lowmvtds );
	WritePrivateProfileString(  m_symbol, "LOWMOVETRADES", buf,   m_szSymbdataFile);
 
	lstrcpy(buf,  m_newspd  );
	WritePrivateProfileString(  m_symbol, "NEWSPERIOD", buf,   m_szSymbdataFile);
	

}


void CTriggers::OnQmReset() 
{
	// TODO: Add your command handler code here
	m_highmvpts = _T("");
	m_highpc = _T("");
	m_highpr = _T("");
	m_highpts = _T("");
	m_lowmvpts = _T("");
	m_lowpc = _T("");
	m_lowpr = _T("");
	m_lowpts = _T("");
	m_bought = _T("");
	m_paid = _T("");
	m_highmvtds = _T("");
	m_lowmvtds = _T("");
	m_newspd = _T("");

	UpdateData(FALSE);
	SaveSymbol();
	
}
