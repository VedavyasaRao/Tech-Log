// SymbolTriggerPage.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "SymbolTriggerPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSymbolTriggerPage property page

IMPLEMENT_DYNCREATE(CSymbolTriggerPage, CPropertyPage)

CSymbolTriggerPage::CSymbolTriggerPage() : CPropertyPage(CSymbolTriggerPage::IDD)
{
	//{{AFX_DATA_INIT(CSymbolTriggerPage)
	m_bought = _T("");
	m_highpr = _T("");
	m_lowpr = _T("");
	m_paid = _T("");
	//}}AFX_DATA_INIT
}

CSymbolTriggerPage::~CSymbolTriggerPage()
{
}

void CSymbolTriggerPage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CSymbolTriggerPage)
	DDX_Text(pDX, IDC_EDIT_BOUGHT, m_bought);
	DDX_Text(pDX, IDC_EDIT_HIGHPR, m_highpr);
	DDX_Text(pDX, IDC_EDIT_LOWPR, m_lowpr);
	DDX_Text(pDX, IDC_EDIT_PAID, m_paid);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CSymbolTriggerPage, CPropertyPage)
	//{{AFX_MSG_MAP(CSymbolTriggerPage)
	ON_BN_CLICKED(IDC_RESET, OnResetNew)
	ON_CBN_SELCHANGE(IDC_COMBO1, OnSelchangeCombo1)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSymbolTriggerPage message handlers

BOOL CSymbolTriggerPage::OnInitDialog() 
{
	CPropertyPage::OnInitDialog();
	
	// TODO: Add extra initialization here

	m_pimSymbdata = &((CRVVPMApp *)AfxGetApp())->m_imSymbdata;
	m_pimPortfolios = &((CRVVPMApp *)AfxGetApp())->m_imPortfolios;

	string buf;
	m_pimPortfolios->GetPrivateProfileString("Settings", "Current", "", buf);
	m_portfolio = buf.c_str();
	LoadPortfolio();
	::SendMessage(GetDlgItem(IDC_RESET)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_RESET));
	UpdateData(FALSE);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}


void CSymbolTriggerPage::OnSelchangeCombo1() 
{
	// TODO: Add your control notification handler code here
	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	

	if (pPortfs->GetCurSel() == CB_ERR)
		return;

	SaveSymbol();
	char buf[200];
	pPortfs->GetLBText(pPortfs->GetCurSel(), buf);
	m_fullsymbol = buf;
	LoadSymbol();


}

void CSymbolTriggerPage::LoadPortfolio()
{
	// TODO: Add your control notification handler code here

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	
	auto pf = m_pimPortfolios->GetKeys(m_portfolio.GetBuffer());
	for (auto pfitr = pf.begin(); pfitr != pf.end(); ++pfitr)
	{

		pPortfs->AddString(pfitr->c_str());
	}

	int i = pPortfs->FindString(0, m_fullsymbol);
	if ( i != CB_ERR)
	{
		pPortfs->SetCurSel(i);
		LoadSymbol();
	}

	UpdateData(FALSE);
}

void CSymbolTriggerPage::LoadSymbol()
{

	string buf;

	if (m_fullsymbol == "")
		return;

	m_pimSymbdata->GetPrivateProfileString(m_fullsymbol.GetBuffer(), "Bought", "", buf);
	m_bought = buf.c_str();

	m_pimSymbdata->GetPrivateProfileString(m_fullsymbol.GetBuffer(), "Paid", "", buf);
	m_paid = buf.c_str();

	m_pimSymbdata->GetPrivateProfileString(m_fullsymbol.GetBuffer(), "HIGHPRICE", "", buf);
	m_highpr = buf.c_str();

	m_pimSymbdata->GetPrivateProfileString(m_fullsymbol.GetBuffer(), "LOWPRICE", "", buf);
	m_lowpr = buf.c_str();


	UpdateData(FALSE);

}

void CSymbolTriggerPage::SaveSymbol()
{

	char buf[300];

	if (m_fullsymbol == "")
		return;

	UpdateData(TRUE);
	lstrcpy(buf,  m_bought );
	m_pimSymbdata->WritePrivateProfileString(m_fullsymbol.GetBuffer(), "Bought", buf);

	lstrcpy(buf,  m_paid );
	m_pimSymbdata->WritePrivateProfileString(m_fullsymbol.GetBuffer(), "Paid", buf);

	lstrcpy(buf,  m_highpr );
	m_pimSymbdata->WritePrivateProfileString(m_fullsymbol.GetBuffer(), "HIGHPRICE", buf);

	lstrcpy(buf,  m_lowpr );
	m_pimSymbdata->WritePrivateProfileString(m_fullsymbol.GetBuffer(), "LOWPRICE", buf);


}


void CSymbolTriggerPage::OnResetNew()
{
	// TODO: Add your command handler code here
	m_bought = _T("");
	m_highpr = _T("");
	m_lowpr = _T("");
	m_paid = _T("");

	UpdateData(FALSE);
	SaveSymbol();
	
}

void CSymbolTriggerPage::OnOK() 
{
	// TODO: Add your specialized code here and/or call the base class
	
	UpdateData(FALSE);
	SaveSymbol();
	CPropertyPage::OnOK();
}
