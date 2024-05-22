// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "RVVPM.h"

#include "MainFrm.h"
#include "SymbolTriggerPage.h"
#include "OtherTriggerPage.h"
#include "bookmarkdlg.h"

#include "PortfolioDlg.h"
#include "datasourcepage.h"
#include "columnspage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMainFrame

IMPLEMENT_DYNAMIC(CMainFrame, CFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
	//{{AFX_MSG_MAP(CMainFrame)
	ON_WM_CREATE()
	ON_COMMAND(ID_PM_EDIT, OnPmEdit)
	ON_COMMAND(ID_PM_SEETINGS, OnPmSeetings)
	ON_COMMAND(ID_PM_TRIGGER, OnPmTrigger)
	ON_COMMAND(ID_QM_URLS, OnQmUrls)
	ON_COMMAND(ID_QM_SAVE, OnQmSave)
	ON_COMMAND(ID_QM_RUN, OnQmRun)
	ON_UPDATE_COMMAND_UI(ID_QM_RUN, OnUpdateQmRun)
	ON_COMMAND(ID_QM_BAK, OnQmBak)
	ON_UPDATE_COMMAND_UI(ID_QM_BAK, OnUpdateQmBak)
	ON_COMMAND(ID_QM_FWD, OnQmFwd)
	ON_UPDATE_COMMAND_UI(ID_QM_FWD, OnUpdateQmFwd)
	ON_COMMAND(ID_QM_REFRESH, OnQmRefresh)
	ON_UPDATE_COMMAND_UI(ID_QM_REFRESH, OnUpdateQmRefresh)
	ON_COMMAND(ID_PM_QREFRESH, OnPmQrefresh)
	ON_COMMAND(ID_PM_NREFRESH, OnPmNrefresh)
	ON_COMMAND(ID_PM_PRINT, OnPmPrint)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

static UINT indicators[] =
{
	ID_SEPARATOR,           // status line indicator
	ID_BUSY_PANE,
	ID_DOW_PANE,
	ID_NASDAQ_PANE,
	ID_TIME_PANE,
};

/////////////////////////////////////////////////////////////////////////////
// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
	// TODO: add member initialization code here
	m_htmlsel	= 0;
	m_pimmaster = &((CRVVPMApp *)AfxGetApp())->m_imMaster;
	m_piportfolips = &((CRVVPMApp *)AfxGetApp())->m_imPortfolios;
}

CMainFrame::~CMainFrame()
{
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	
	if (CFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC  ) ||
		!m_wndToolBar.LoadToolBar(IDR_MAINFRAME))
	{
		TRACE0("Failed to create toolbar\n");
		return -1;      // fail to create
	}
	m_wndToolBar.SetHeight(32);
	m_wndToolBar.SetButtonInfo(1, 1001, TBBS_SEPARATOR, 120);

	RECT rc;

	m_wndToolBar.GetItemRect(1, &rc);
	m_pflist.Create(
      WS_CHILD|WS_VISIBLE|WS_VSCROLL|CBS_DROPDOWNLIST|WS_HSCROLL|WS_VSCROLL,
      CRect(rc.left+3,rc.top, 150, 200), 
	  &m_wndToolBar, 
	  IDC_COMBO1);
	m_pflist.SendMessage(WM_SETFONT, (DWORD)::CreateFont( 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Arial"), MAKELPARAM(1,0));

	m_wndToolBar.SetButtonInfo(10, 1002, TBBS_SEPARATOR, 140);
	m_wndToolBar.GetItemRect(10, &rc);
	m_urllist.Create(
      WS_CHILD|WS_VISIBLE|CBS_DROPDOWNLIST|WS_HSCROLL|WS_VSCROLL,
      CRect(rc.left+3,rc.top, 450, 200), 
	  &m_wndToolBar, 
	  IDC_COMBO2);
	m_urllist.SendMessage(WM_SETFONT, (DWORD)::CreateFont( 18, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "MSSerif"), MAKELPARAM(1,0));
	
	if (!m_wndStatusBar.Create(this) ||
		!m_wndStatusBar.SetIndicators(indicators,
		  sizeof(indicators)/sizeof(UINT)))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}

	UINT	nID;
	UINT	nStyle;
	int		cxWidth;

	m_wndStatusBar.SendMessage(WM_SETFONT, (DWORD)::CreateFont(14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Arial"), MAKELPARAM(1, 0));
	m_wndStatusBar.GetPaneInfo(1, nID, nStyle, cxWidth);
	m_wndStatusBar.SetPaneInfo(1, nID, nStyle, 40);

	m_wndStatusBar.GetPaneInfo( 2, nID, nStyle, cxWidth );
	m_wndStatusBar.SetPaneInfo( 2, nID, nStyle, 60);

	m_wndStatusBar.GetPaneInfo( 3, nID, nStyle, cxWidth );
	m_wndStatusBar.SetPaneInfo( 3, nID, nStyle, 80);

	m_wndStatusBar.GetPaneInfo( 4, nID, nStyle, cxWidth );
	m_wndStatusBar.SetPaneInfo( 4, nID, nStyle, 50);



	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	cs.hMenu = NULL;
	cs.dwExStyle &= ~WS_EX_CLIENTEDGE;
	cs.lpszClass = AfxRegisterWndClass(0, 0, 0, AfxGetApp()->LoadIcon(IDR_MAINFRAME));
	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CFrameWnd::Dump(dc);
}

#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMainFrame message handlers


BOOL CMainFrame::OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext) 
{
	// TODO: Add your specialized code here and/or call the base class
	
//	return CFrameWnd::OnCreateClient(lpcs, pContext);

	BOOL rtn;
	rtn = m_wndSplitter.CreateStatic(this, 2, 1);
	rtn |= m_wndSplitter.CreateView(0, 0, RUNTIME_CLASS(CPMListView), CSize(1000, 1000), pContext);
	rtn |= m_wndSplitter.CreateView(1, 0, RUNTIME_CLASS(CPMHTMLView), CSize(0, 0), pContext);
	return rtn;

}

void CMainFrame::OnPmEdit() 
{
	// TODO: Add your command handler code here
	CPortfolioDlg	portDlg;
	CRVVPMApp *pApp = ((CRVVPMApp *)AfxGetApp());

	int nResponse = portDlg.DoModal();
	LoadPortfolios();
	pApp->LoadAgain();
}



void CMainFrame::OnPmSeetings() 
{
	// TODO: Add your command handler code here
	CRVVPMApp *pApp = ((CRVVPMApp *)AfxGetApp());
	CPropertySheet dlgSheet("Manage Preferences");

	CDatasourcePage srcPage;
	CColumnsPage colPage;

	dlgSheet.AddPage(&srcPage);
	dlgSheet.AddPage(&colPage);

	m_pimmaster->Backup();
	int nResponse = dlgSheet.DoModal();
	if (nResponse == IDOK)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with OK

		pApp->LoadAgain();

	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with Cancel
		m_pimmaster->Restore();
	}
}


LRESULT CMainFrame::WindowProc(UINT message, WPARAM wParam, LPARAM lParam) 
{
	// TODO: Add your specialized code here and/or call the base class
	CRVVPMApp*  pApp=  (CRVVPMApp*)AfxGetApp();

	if (message == WM_PM_UPDATEPANES)
	{
		m_wndStatusBar.m_szDow = pApp->m_dow;
		m_wndStatusBar.GetStatusBarCtrl().SetText("", 2, SBT_OWNERDRAW);
		m_wndStatusBar.m_szNasdaq = pApp->m_nasdaq;
		m_wndStatusBar.GetStatusBarCtrl().SetText("", 3, SBT_OWNERDRAW);
		m_wndStatusBar.SetPaneText (4, pApp->m_curtime, TRUE);
	}

	if (message == WM_PM_UPDATEBUSYPANE)
	{
		if (wParam != 0)
		{
			m_wndStatusBar.m_szBusy = "  Busy";
			m_wndStatusBar.GetStatusBarCtrl().SetText("", 1, SBT_OWNERDRAW);
		}
		else
		{
			m_wndStatusBar.m_szBusy = "";
			m_wndStatusBar.GetStatusBarCtrl().SetText("", 1, SBT_OWNERDRAW);
		}
	}

	if (message == WM_PM_UPDATESTATUSBAR)
	{
		SetMessageText (pApp->m_statusmsg);
	}
	
	if (message == WM_PM_RESETLISTCTRL)
	{
		((CPMListView*)m_wndSplitter.GetPane(0,0))->ResetControl();
		
	}

	if (message == WM_PM_FILLLISTCTRL)
	{
		((CPMListView*)m_wndSplitter.GetPane(0,0))->FillControl();
	}

	if (message == WM_PM_FLASHWINDOW)
	{
		FlashWindow(wParam);
	}

	if (message == WM_SYSCOMMAND)
	{
		if (SC_MINIMIZE == wParam)
		{
			ShowWindow(SW_HIDE);
			return TRUE;
		}

	}

	if (message == WM_PM_TASKMSGS)
	{
		
		if (lParam == WM_LBUTTONDOWN )
		{
			
			ShowWindow(SW_SHOWNORMAL);

			return TRUE;
		}

	}
	return CFrameWnd::WindowProc(message, wParam, lParam);
}


void CMainFrame::LoadHtmlPane() 
{
	// TODO: Add your control notification handler code here

	if (!m_htmlsel)
		return;

	int scroll=0;
	CPMListView*	pListView = (CPMListView*)m_wndSplitter.GetPane(0,0);
	CPMHTMLView*	pHtmlView = (CPMHTMLView*)m_wndSplitter.GetPane(1,0);
	CListCtrl *pList = &pListView->GetListCtrl();
	CString exchange = pListView->GetExchange();
	CString symbol = pListView->GetSymbol();

	CString str, str1;
	string str3;
	char szBuffer[1000];
	int pos1, pos2;

	if (m_urllist.GetCurSel() == CB_ERR)
		return;
	m_urllist.GetLBText(m_urllist.GetCurSel(), szBuffer);
	m_pimmaster->GetPrivateProfileString("Bookmarks", szBuffer, "", str3);
	
	str = str3.c_str();
	if (str == "")
		return;

	
	if ((pos1 = str.Find("-SCROLL=")) != -1)
	{
		scroll = 0;
		sscanf(str.Mid(pos1+8), "%d", &scroll);
		str = str.Mid(0, pos1);
	}
	

	while (true)
	{
		if (str.Find("<Symbol(0:1)>") != -1)
		{
			str.Replace("<Symbol(0:1)>", exchange + ":" + symbol);
			continue;
		}
		else if (str.Find("<Symbol(0-1)>") != -1)
		{
			str.Replace("<Symbol(0-1)>", exchange + "-" + symbol);
			continue;
		}
		else if (str.Find("<Symbol(1)>") != -1)
		{
			str.Replace("<Symbol(1)>", symbol);
			continue;
		}
		break;
	}

	m_wndSplitter.SetActivePane(1,0);
	pHtmlView->Navigate(str,scroll);
}



void CMainFrame::OnPmTrigger() 
{
	// TODO: Add your command handler code here
	CPMListView*	pListView = (CPMListView*)m_wndSplitter.GetPane(0,0);
	//trgDlg.m_symbol = pListView->GetSymbol();

	CPropertySheet dlgSheet("Triggers");

	CSymbolTriggerPage symPage;
	COtherTriggerPage otrPage;

	symPage.m_fullsymbol= pListView->GetSymbol(); 

	dlgSheet.AddPage(&symPage);
	dlgSheet.AddPage(&otrPage);



	CRVVPMApp *pApp = ((CRVVPMApp *)AfxGetApp());

	m_pimmaster->Backup();
	int nResponse = dlgSheet.DoModal();
	if (nResponse == IDOK)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with OK
		if (otrPage.m_clearnews)
		{
			auto& symbdata = ((CRVVPMApp *)AfxGetApp())->m_imSymbdata;
			auto sections = symbdata.GetSectionNames();
			for (auto itr = sections.begin(); itr != sections.end(); ++itr)
			{
				symbdata.WritePrivateProfileStringA(*itr, "LASTNEWS", "");
			}
		}

		pApp->LoadAgain();

	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with Cancel
		m_pimmaster->Restore();
	}


}


void CMainFrame::OnQmUrls() 
{
	// TODO: Add your command handler code here
	CBookmarkDlg	bmDlg;
	bmDlg.DoModal();
	LoadBookmarks();
}

void CMainFrame::OnQmSave() 
{
	// TODO: Add your command handler code here
	
}

void CMainFrame::LoadPortfolios() 
{

	m_pflist.ResetContent();
	auto portfolios = m_piportfolips->GetSectionNames();
	for (auto itr = portfolios.begin(); itr != portfolios.end(); ++itr)
	{
		if ( strstr(itr->c_str(), "Settings") == nullptr)
			m_pflist.AddString( itr->c_str() );
	}

	string szCurrent;
	m_piportfolips->GetPrivateProfileString("Settings", "Current", "", szCurrent);
  	
	int lp; 
	if ( (lp = m_pflist.FindStringExact(0, szCurrent.c_str())) == CB_ERR )
		lp = 0;
	m_pflist.SetCurSel(lp);
}


void CMainFrame::LoadBookmarks() 
{

	m_urllist.ResetContent();
	auto  bksec = m_pimmaster->GetKeys("Bookmarks");
	for (auto titr = bksec.begin(); titr != bksec.end(); ++titr)
	{
		m_urllist.AddString(titr->c_str());
	}
	m_urllist.SetCurSel(0);

}

void CMainFrame::OnQmRun() 
{
	// TODO: Add your command handler code here
	if (m_htmlsel)
	{
		m_htmlsel = 0;
		m_wndSplitter.SetRowInfo(0, 1000, 1000);
		m_wndSplitter.RecalcLayout();
	}
	else
	{
		m_htmlsel = 1;
		m_wndSplitter.SetRowInfo(0, 100, 100);
		m_wndSplitter.RecalcLayout();
		LoadHtmlPane();
	}
	
}

void CMainFrame::OnUpdateQmRun(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->SetCheck(m_htmlsel);
	
}

void CMainFrame::OnQmBak() 
{
	// TODO: Add your command handler code here
	
	CPMHTMLView*	pHtmlView = (CPMHTMLView*)m_wndSplitter.GetPane(1,0);

	pHtmlView->NavigateBack();


}

void CMainFrame::OnUpdateQmBak(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable(m_htmlsel);
}

void CMainFrame::OnQmFwd() 
{
	// TODO: Add your command handler code here
	CPMHTMLView*	pHtmlView = (CPMHTMLView*)m_wndSplitter.GetPane(1,0);

	pHtmlView->NavigateForward();
	
}

void CMainFrame::OnUpdateQmFwd(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable(m_htmlsel);
	
}

void CMainFrame::OnQmRefresh() 
{
	// TODO: Add your command handler code here
	if (m_htmlsel)
		LoadHtmlPane();
	
}

void CMainFrame::OnUpdateQmRefresh(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable(m_htmlsel);
	
}


BOOL CMainFrame::DestroyWindow() 
{
	// TODO: Add your specialized code here and/or call the base class
	Shell_NotifyIcon(NIM_DELETE, &((CRVVPMApp *)AfxGetApp())->m_IconData);

	return CFrameWnd::DestroyWindow();
}

void CMainFrame::OnPmQrefresh() 
{
	// TODO: Add your command handler code here
	CRVVPMApp *pApp = ((CRVVPMApp *)AfxGetApp());
	pApp->m_bQuoteOndemand = true;
	pApp->AddJob(CQMJob::QureyQuotes);
}

void CMainFrame::OnPmNrefresh() 
{
	// TODO: Add your command handler code here
	CRVVPMApp *pApp = ((CRVVPMApp *)AfxGetApp());
	pApp->m_bNewsOndemand = true;
	pApp->AddJob(CQMJob::QueryNews);

}

void CMainFrame::OnPmPrint() 
{
	// TODO: Add your command handler code here
	CRVVPMApp *pApp = ((CRVVPMApp *)AfxGetApp());

	pApp->AddJob(CQMJob::Print);
}
