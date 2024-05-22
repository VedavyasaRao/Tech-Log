// BookmarkDlg.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "BookmarkDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CBookmarkDlg dialog


CBookmarkDlg::CBookmarkDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CBookmarkDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CBookmarkDlg)
	m_bookmark = _T("");
	m_address = _T("");
	//}}AFX_DATA_INIT
}


void CBookmarkDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CBookmarkDlg)
	DDX_CBString(pDX, IDC_COMBO1, m_bookmark);
	DDX_Text(pDX, IDC_EDIT2, m_address);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CBookmarkDlg, CDialog)
	//{{AFX_MSG_MAP(CBookmarkDlg)
	ON_CBN_SELCHANGE(IDC_COMBO1, OnSelchangeCombo1)
	ON_COMMAND(ID_QM_NEWBM, OnQmNewbm)
	ON_COMMAND(ID_QM_SAVEBM, OnQmSavebm)
	ON_COMMAND(ID_QM_DELETEBM, OnQmDeletebm)
	//}}AFX_MSG_MAP
	ON_NOTIFY_EX_RANGE(TTN_NEEDTEXTW, 0, 0xFFFF, OnToolTipText)
	ON_NOTIFY_EX_RANGE(TTN_NEEDTEXTA, 0, 0xFFFF, OnToolTipText)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CBookmarkDlg message handlers

BOOL CBookmarkDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	m_pimmaster = &((CRVVPMApp *)AfxGetApp())->m_imMaster;
 	LoadURLs();
	SetCurrent();

	// Create toolbar at the top of the dialog window
	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC  ) ||
		!m_wndToolBar.LoadToolBar(IDR_URL))
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

void CBookmarkDlg::LoadURLs()
{
	// TODO: Add extra initialization here

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);
	auto  bksec = m_pimmaster->GetKeys("Bookmarks");
	for (auto titr = bksec.begin(); titr != bksec.end(); ++titr)
	{
		pPortfs->AddString(titr->c_str());
	}
	
}

void CBookmarkDlg::SetCurrent()
{
	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	
	pPortfs->SetCurSel(0);
	OnSelchangeCombo1();
}

void CBookmarkDlg::OnSelchangeCombo1() 
{
	// TODO: Add your control notification handler code here

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	

	if (pPortfs->GetCurSel() == CB_ERR)
		return;

	pPortfs->GetLBText(pPortfs->GetCurSel(), m_bookmark);
	
	string sReturnBuffer;

	m_pimmaster->GetPrivateProfileString("Bookmarks", m_bookmark.GetBuffer(), "", sReturnBuffer);

	m_address = sReturnBuffer.c_str();
	UpdateData(FALSE);

}

void CBookmarkDlg::saveURL()
{

	m_pimmaster->WritePrivateProfileString("Bookmarks", m_bookmark.GetBuffer(), m_address.GetBuffer());
}

void CBookmarkDlg::OnQmNewbm() 
{
	// TODO: Add your command handler code here
	m_bookmark = _T("");
	m_address = _T("");
	UpdateData(FALSE);
	
}

void CBookmarkDlg::OnQmDeletebm() 
{
	// TODO: Add your command handler code here
	UpdateData();
	if ( !m_bookmark.GetLength()) 
	{
		MessageBox("Please enter a name for the bookmark", "Delete Bookmark", 0);
		return;
	}

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);

	int lp;
	if ( (lp = pPortfs->FindStringExact(0, m_bookmark)) == CB_ERR )
	{
		MessageBox("This bookmark does not exists.", "Delete Bookmark", 0);
		return;
	}

	m_pimmaster->DeleteKey( "Bookmarks", m_bookmark.GetBuffer());

	pPortfs->DeleteString(lp);

	OnQmNewbm();
}

void CBookmarkDlg::OnQmSavebm() 
{
	// TODO: Add your command handler code here
	UpdateData();
	if ( !m_bookmark.GetLength()) 
	{
		MessageBox("Please enter a name for the Bookmark", "Save Bookmark", 0);
		return;
	}

	if ( !m_address.GetLength()) 
	{
		MessageBox("Please enter an adress", "Save Bookmark", 0);
		return;
	}

	CComboBox	*pPortfs = (CComboBox *)GetDlgItem(IDC_COMBO1);	

	int lp;
	if ( (lp = pPortfs->FindStringExact(0, m_bookmark)) == CB_ERR )
	{
		pPortfs->AddString( m_bookmark );

	}

	saveURL();
	
}

BOOL CBookmarkDlg::OnToolTipText(UINT, NMHDR* pNMHDR, LRESULT* pResult)
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

