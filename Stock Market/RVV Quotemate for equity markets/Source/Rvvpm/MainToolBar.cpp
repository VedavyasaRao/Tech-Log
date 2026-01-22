// MainToolBar.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "MainToolBar.h"
#include "MainFrm.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMainToolBar

CMainToolBar::CMainToolBar()
{
}

CMainToolBar::~CMainToolBar()
{
}


BEGIN_MESSAGE_MAP(CMainToolBar, CToolBar)
	//{{AFX_MSG_MAP(CMainToolBar)
		// NOTE - the ClassWizard will add and remove mapping macros here.
	ON_CBN_SELCHANGE(IDC_COMBO1, OnSelchangeCombo1)
	ON_CBN_SELCHANGE(IDC_COMBO2, OnSelchangeCombo2)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMainToolBar message handlers
void CMainToolBar::OnSelchangeCombo1() 
{
	// TODO: Add your control notification handler code here

	CRVVPMApp *pApp =  ((CRVVPMApp *)AfxGetApp());

	CComboBox *pCmbList  = (CComboBox *)GetDlgItem(IDC_COMBO1);
	char szBuffer[200];
	pCmbList->GetLBText(pCmbList->GetCurSel(), szBuffer);

	((CRVVPMApp *)AfxGetApp())->m_imPortfolios.WritePrivateProfileString("Settings", "Current", szBuffer);

	pApp->LoadAgain();

}

void CMainToolBar::OnSelchangeCombo2() 
{
	// TODO: Add your control notification handler code here
	((CMainFrame*)AfxGetMainWnd())->LoadHtmlPane();
}
