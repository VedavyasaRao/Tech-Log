// ColumnsPage.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "ColumnsPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CColumnsPage property page

IMPLEMENT_DYNCREATE(CColumnsPage, CPropertyPage)

CColumnsPage::CColumnsPage() : CPropertyPage(CColumnsPage::IDD)
{
	//{{AFX_DATA_INIT(CColumnsPage)
		// NOTE: the ClassWizard will add member initialization here
	m_sizecols = FALSE;
	//}}AFX_DATA_INIT
}

CColumnsPage::~CColumnsPage()
{
}

void CColumnsPage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CColumnsPage)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	DDX_Check(pDX, IDC_CHECK1, m_sizecols);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CColumnsPage, CPropertyPage)
	//{{AFX_MSG_MAP(CColumnsPage)
	ON_BN_CLICKED(IDC_ADD, OnAdd)
	ON_BN_CLICKED(IDC_DOWN, OnDown)
	ON_BN_CLICKED(IDC_REMOVE, OnRemove)
	ON_BN_CLICKED(IDC_UP, OnUp)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CColumnsPage message handlers

BOOL CColumnsPage::OnInitDialog() 
{
	CPropertyPage::OnInitDialog();
	
	// TODO: Add extra initialization here
	m_pimmaster = &((CRVVPMApp *)AfxGetApp())->m_imMaster;
	LoadColumns();

	m_sizecols = m_pimmaster->GetPrivateProfileInt("Settings", "SizeCols", 0);

	::SendMessage(GetDlgItem(IDC_ADD)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_RIGHT1));
	::SendMessage(GetDlgItem(IDC_REMOVE)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_LEFT1));
	::SendMessage(GetDlgItem(IDC_UP)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_UP1));
	::SendMessage(GetDlgItem(IDC_DOWN)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_DOWN1));

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CColumnsPage::LoadColumns()
{

	CListBox *pList1 = (CListBox *)GetDlgItem(IDC_LIST2);	
	CListBox *pList2 = (CListBox *)GetDlgItem(IDC_LIST1);

	auto keys = m_pimmaster->GetKeys("Columns");
	
	for (auto titr = keys.begin(); titr != keys.end(); ++titr)
	{
		if (m_pimmaster->GetPrivateProfileInt("Columns", *titr, 0) == 0)
			pList1->AddString(titr->c_str());
		else
			pList2->AddString(titr->c_str());
	}
}

void CColumnsPage::OnAdd() 
{
	// TODO: Add your control notification handler code here
	CListBox *pList1 = (CListBox *)GetDlgItem(IDC_LIST2);	
	CListBox *pList2 = (CListBox *)GetDlgItem(IDC_LIST1);
	
	int pos = pList1->GetCurSel();
	if (pos == -1)
		return;

	CString s;
	pList1->GetText(pos, s);
	pList2->AddString( s );
	pList1->DeleteString(pos);


}

void CColumnsPage::OnRemove() 
{
	// TODO: Add your control notification handler code here
	CListBox *pList1 = (CListBox *)GetDlgItem(IDC_LIST2);	
	CListBox *pList2 = (CListBox *)GetDlgItem(IDC_LIST1);

	int pos = pList2->GetCurSel();
	if (pos == -1)
		return;


	CString s;
	pList2->GetText(pos, s);
	pList1->AddString( s );
	pList2->DeleteString(pos);
}

void CColumnsPage::OnDown() 
{
	// TODO: Add your control notification handler code here
	CListBox *pList2 = (CListBox *)GetDlgItem(IDC_LIST1);

	int nCount = pList2->GetSelCount();

	if (nCount > 1)
		return;

	int pos = pList2->GetCurSel();
	if (pos != -1 && pos < pList2->GetCount() - 1)
	{
		CString	s;

		pList2->GetText(pos, s);
		pList2->DeleteString(pos);
		pList2->InsertString(pos + 1, s);
		pList2->SetCurSel(pos+1);
	}
	pList2->SetSel(pos+1);


}

void CColumnsPage::OnUp() 
{
	// TODO: Add your control notification handler code here

	CListBox *pList2 = (CListBox *)GetDlgItem(IDC_LIST1);

	int nCount = pList2->GetSelCount();

	if (nCount > 1)
		return;

	int pos = pList2->GetCurSel();
	if (pos > 0)
	{
		CString	s;

		pList2->GetText(pos, s);
		pList2->DeleteString(pos);
		pList2->InsertString(pos - 1, s);
		pList2->SetCurSel(pos-1);
	
	}
	pList2->SetSel(pos-1);
	
}

void CColumnsPage::OnOK() 
{
	// TODO: Add your specialized code here and/or call the base class

	CListBox *pList1 = (CListBox *)GetDlgItem(IDC_LIST2);	
	CListBox *pList2 = (CListBox *)GetDlgItem(IDC_LIST1);
	int i;

	m_pimmaster->DeleteSection("Columns");
	for (i=0; i<pList2->GetCount(); ++i)
	{
		CString	s;
		pList2->GetText(i, s);
		m_pimmaster->WritePrivateProfileString("Columns", s.GetBuffer(), "1");

	}

	for (i=0; i<pList1->GetCount(); ++i)
	{
		CString	s;
		pList1->GetText(i, s);
		m_pimmaster->WritePrivateProfileString("Columns", s.GetBuffer(), "0");
	}

	char szBuf[10];
	sprintf(szBuf, "%d", m_sizecols);
	m_pimmaster->WritePrivateProfileString("Settings", "SizeCols", szBuf);
	CPropertyPage::OnOK();
}

