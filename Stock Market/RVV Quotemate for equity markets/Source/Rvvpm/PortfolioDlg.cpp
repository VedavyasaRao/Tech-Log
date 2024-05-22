// PortfolioDlg.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "PortfolioDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CPortfolioDlg dialog


CPortfolioDlg::CPortfolioDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CPortfolioDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CPortfolioDlg)
	m_Portfolio = _T("");
	m_newTicker = _T("");
	m_dow = FALSE;
	m_nasdaq = FALSE;
	m_snp = FALSE;
	//}}AFX_DATA_INIT
}


void CPortfolioDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CPortfolioDlg)
	DDX_CBString(pDX, IDC_COMBO1, m_Portfolio);
	DDX_Check(pDX, IDC_CHKDOW, m_dow);
	DDX_Check(pDX, IDC_CHKDOW2, m_nasdaq);
	DDX_Check(pDX, IDC_CHKDOW3, m_snp);
	DDX_Check(pDX, IDC_CHKDOW3, m_snp);
	DDX_Text(pDX, IDC_EDIT1, m_newTicker);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CPortfolioDlg, CDialog)
	//{{AFX_MSG_MAP(CPortfolioDlg)
	ON_CBN_SELCHANGE(IDC_COMBO1, OnSelchangeCombo1)
	ON_CBN_SELENDOK(IDC_COMBO1, OnSelendokCombo1)
	ON_COMMAND(ID_QM_DELETEPF, OnQmDeletepf)
	ON_COMMAND(ID_QM_NEWPF, OnQmNewpf)
	ON_COMMAND(ID_QM_SAVEPF, OnQmSavepf)
	ON_BN_CLICKED(IDC_ADD, OnAdd)
	ON_BN_CLICKED(IDC_DOWN, OnDown)
	ON_BN_CLICKED(IDC_REMOVE, OnRemove)
	ON_BN_CLICKED(IDC_UP, OnUp)
	//}}AFX_MSG_MAP
	ON_NOTIFY_EX_RANGE(TTN_NEEDTEXTW, 0, 0xFFFF, OnToolTipText)
	ON_NOTIFY_EX_RANGE(TTN_NEEDTEXTA, 0, 0xFFFF, OnToolTipText)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CPortfolioDlg message handlers

BOOL CPortfolioDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	m_pimPortfolios = &((CRVVPMApp *)AfxGetApp())->m_imPortfolios;
	LoadPortfolios();
	SetCurrent();

	::SendMessage(GetDlgItem(IDC_ADD)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_ADD));
	::SendMessage(GetDlgItem(IDC_REMOVE)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_REMOVE));
	::SendMessage(GetDlgItem(IDC_UP)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_UP1));
	::SendMessage(GetDlgItem(IDC_DOWN)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_DOWN1));

	// Create toolbar at the top of the dialog window
	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC  ) ||
		!m_wndToolBar.LoadToolBar(IDR_PROFOLIO))
	{
		TRACE0("Failed to create toolbar\n");
		return -1;      // fail to create
	}

	// We need to resize the dialog to make room for control bars.
	// First, figure out how big the control bars are.
	CRect rcClientStart;
	CRect rcClientNow;
	GetClientRect(rcClientStart);
	RepositionBars(AFX_IDW_CONTROLBAR_FIRST, AFX_IDW_CONTROLBAR_LAST,
				   0, reposQuery, rcClientNow);


	// Now move all the controls so they are in the same relative
	// position within the remaining client area as they would be
	// with no control bars.
	CPoint ptOffset(rcClientNow.left - rcClientStart.left,
					rcClientNow.top - rcClientStart.top);

	CRect  rcChild;
	CWnd* pwndChild = GetWindow(GW_CHILD);
	while (pwndChild)
	{
		pwndChild->GetWindowRect(rcChild);
		ScreenToClient(rcChild);
		rcChild.OffsetRect(ptOffset);
		pwndChild->MoveWindow(rcChild, FALSE);
		pwndChild = pwndChild->GetNextWindow();
	}

	// Adjust the dialog window dimensions
	CRect rcWindow;
	GetWindowRect(rcWindow);
	rcWindow.right += rcClientStart.Width() - rcClientNow.Width();
	rcWindow.bottom += rcClientStart.Height() - rcClientNow.Height();
	MoveWindow(rcWindow, FALSE);

	// And position the control bars
	RepositionBars(AFX_IDW_CONTROLBAR_FIRST, AFX_IDW_CONTROLBAR_LAST, 0);


	// Finally, center the dialog on the screen
	CenterWindow();


	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CPortfolioDlg::LoadPortfolios()
{
	// TODO: Add extra initialization here
	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);
	auto  portfnames = m_pimPortfolios->GetSectionNames();
	for (auto itr = portfnames.begin(); itr != portfnames.end(); ++itr)
	{
		if (strstr(itr->c_str(), "Settings") == nullptr)
			pPortfs->AddString(itr->c_str());
	}
}

void CPortfolioDlg::SetCurrent()
{
	string s;
	m_pimPortfolios->GetPrivateProfileString("Settings", "Current", "", s);
  	
	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);
	int lp; 
	if ( (lp = pPortfs->FindStringExact(0, s.c_str())) != CB_ERR )
	{
		pPortfs->SetCurSel(lp);
		OnSelchangeCombo1();
	}
}

void CPortfolioDlg::OnSelchangeCombo1() 
{
	// TODO: Add your control notification handler code here
	m_dow = FALSE;
	m_nasdaq = FALSE;
	m_snp = FALSE;

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	
	CListBox *pList = (CListBox *)GetDlgItem(IDC_LIST1);

	if (pPortfs->GetCurSel() == CB_ERR)
		return;
	pList->ResetContent();

	pPortfs->GetLBText(pPortfs->GetCurSel(), m_Portfolio);
	auto keys = m_pimPortfolios->GetKeys(m_Portfolio.GetBuffer());
	for (auto itr=keys.begin(); itr != keys.end(); ++itr)
	{
		if (strcmpi(itr->c_str(), "NYSE,^DJI") == 0)
			m_dow = true;
		else if (strcmpi(itr->c_str(), "NASDAQ,^IXIC") == 0)
			m_nasdaq = true;
		else if (strcmpi(itr->c_str(), "INDEXSP,^GSPC") == 0)
			m_snp = true;
		else
			pList->AddString(itr->c_str());
	}
	UpdateData(FALSE);

}

void CPortfolioDlg::savePortfolio()
{
	m_pimPortfolios->DeleteSection(m_Portfolio.GetBuffer());
	if (m_dow)
		m_pimPortfolios->WritePrivateProfileString(m_Portfolio.GetBuffer(), "NYSE,^DJI", "Khri$ha");

	if (m_nasdaq)
		m_pimPortfolios->WritePrivateProfileString(m_Portfolio.GetBuffer(), "NASDAQ,^IXIC", "Khri$ha");

	if (m_snp)
		m_pimPortfolios->WritePrivateProfileString(m_Portfolio.GetBuffer(), "INDEXSP,^GSPC", "Khri$ha");

	CListBox *pList = (CListBox *)GetDlgItem(IDC_LIST1);
	for (int i = 0; i < pList->GetCount(); ++i)
	{
		CString s;
		pList->GetText(i, s);
		m_pimPortfolios->WritePrivateProfileString(m_Portfolio.GetBuffer(), s.GetBuffer(), "Khri$ha");
	}
}

void CPortfolioDlg::OnQmNewpf() 
{
	// TODO: Add your command handler code here
	m_Portfolio = _T("");
	m_Symbols = _T("");
	UpdateData(FALSE);
}

void CPortfolioDlg::OnQmDeletepf() 
{
	// TODO: Add your command handler code here
	UpdateData();
	if ( !m_Portfolio.GetLength()) 
	{
		MessageBox("Please enter a name for the portfolio", "Delete Portfolio", 0);
		return;
	}

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);

	int lp;
	if ( (lp = pPortfs->FindStringExact(0, m_Portfolio)) == CB_ERR )
	{
		MessageBox("This portfolio does not exists.", "Delete Portfolio", 0);
		return;
	}

	pPortfs->DeleteString(lp);
	m_pimPortfolios->WritePrivateProfileString("Settings", "Current", "");
	m_pimPortfolios->DeleteSection(m_Portfolio.GetBuffer());
	OnQmNewpf();
}

void CPortfolioDlg::OnQmSavepf() 
{
	// TODO: Add your command handler code here
	UpdateData();
	if ( !m_Portfolio.GetLength()) 
	{
		MessageBox("Please enter a name for the portfolio", "Save Portfolio", 0);
		return;
	}

	CListBox *pList = (CListBox *)GetDlgItem(IDC_LIST1);

	if (pList->GetCount() == 0 && !m_dow && !m_nasdaq && !m_snp)
	{
		MessageBox("Please enter atleast one symbol for this portfolio", "Save Portfolio", 0);
		return;
	}

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	

	int lp;
	if ( (lp = pPortfs->FindStringExact(0, m_Portfolio)) == CB_ERR )
	{
		pPortfs->AddString( m_Portfolio );
	}
	savePortfolio();
}


BOOL CPortfolioDlg::OnToolTipText(UINT, NMHDR* pNMHDR, LRESULT* pResult)
{
	ASSERT(pNMHDR->code == TTN_NEEDTEXTA || pNMHDR->code == TTN_NEEDTEXTW);

	// allow top level routing frame to handle the message

	// need to handle both ANSI and UNICODE versions of the message
	TOOLTIPTEXTA* pTTTA = (TOOLTIPTEXTA*)pNMHDR;
	TOOLTIPTEXTW* pTTTW = (TOOLTIPTEXTW*)pNMHDR;
	TCHAR szFullText[256];
	CString strTipText;
	UINT nID = pNMHDR->idFrom;
	if (pNMHDR->code == TTN_NEEDTEXTA && (pTTTA->uFlags & TTF_IDISHWND) ||
		pNMHDR->code == TTN_NEEDTEXTW && (pTTTW->uFlags & TTF_IDISHWND))
	{
		// idFrom is actually the HWND of the tool
		nID = ((UINT)(WORD)::GetDlgCtrlID((HWND)nID));
	}

	CString str;
	if (nID != 0) // will be zero on a separator
	{
		str.LoadString(nID);
		lstrcpy(szFullText, str);
			// this is the command id, not the button index
		strTipText =  str.Mid(str.Find("\n")+1);
	}
	if (pNMHDR->code == TTN_NEEDTEXTA)
		lstrcpyn(pTTTA->szText, strTipText,
			(sizeof(pTTTA->szText)/sizeof(pTTTA->szText[0])));
	else
		_mbstowcsz(pTTTW->szText, strTipText,
			(sizeof(pTTTW->szText)/sizeof(pTTTW->szText[0])));
	*pResult = 0;

	// bring the tooltip window above other popup windows
	::SetWindowPos(pNMHDR->hwndFrom, HWND_TOP, 0, 0, 0, 0,
		SWP_NOACTIVATE|SWP_NOSIZE|SWP_NOMOVE);

	return TRUE;    // message was handled
}

void CPortfolioDlg::OnSelendokCombo1() 
{
	// TODO: Add your control notification handler code here
	
}

void CPortfolioDlg::OnAdd()
{
	UpdateData();
	if (m_newTicker.Find(',') == -1)
	{
		MessageBox("Please enter as <exchange>,<ticker>. e.g., NYSE,A", "Save Portfolio", 0);
		return;
	}
	CListBox *pList = (CListBox *)GetDlgItem(IDC_LIST1);
	m_newTicker = m_newTicker.MakeUpper();
	if (pList->FindStringExact(0, m_newTicker.GetBuffer()) == -1)
		pList->AddString(m_newTicker.GetBuffer());

}

void CPortfolioDlg::OnDown()
{
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
		pList2->SetCurSel(pos + 1);
	}
	pList2->SetSel(pos + 1);

}

void CPortfolioDlg::OnRemove()
{
	CListBox *pList = (CListBox *)GetDlgItem(IDC_LIST1);

	int pos = pList->GetCurSel();
	if (pos != -1)
	{
		pList->DeleteString(pos);
	}
	pList->SetSel(pos - 1);

}

void CPortfolioDlg::OnUp()
{
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
		pList2->SetCurSel(pos - 1);

	}
	pList2->SetSel(pos - 1);


}

