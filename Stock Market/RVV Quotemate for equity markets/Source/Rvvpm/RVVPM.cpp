// RVVPM.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include <strstream>
#include <iomanip>
#include "RVVPM.h"
#include "About.h"
#include "MainFrm.h"
#include "direct.h"
#include "io.h"
#include "include/cef_command_line.h"
#include "include/cef_sandbox_win.h"
#include "include/cef_browser.h"
#include "include/cef_command_line.h"
#include "include/views/cef_browser_view.h"
#include "include/views/cef_window.h"
#include "include/wrapper/cef_helpers.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

int GetColWidth(int colid);

/////////////////////////////////////////////////////////////////////////////
// CRVVPMApp

BEGIN_MESSAGE_MAP(CRVVPMApp, CWinApp)
	//{{AFX_MSG_MAP(CRVVPMApp)
	ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRVVPMApp construction

CRVVPMApp::CRVVPMApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CRVVPMApp object

CRVVPMApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CRVVPMApp initialization
BOOL CRVVPMApp::InitInstance()
{
	AfxEnableControlContainer();
	char szAppPath[1000];
	DWORD length = GetModuleFileName(NULL, szAppPath, sizeof szAppPath);
	PathRemoveFileSpec(szAppPath);
	m_bNewsOndemand = false;
	m_bQuoteOndemand = false;
	m_szTemp = szAppPath + CString("\\..\\Temp");
	if (access(m_szTemp, 0) != 0)
	{
		mkdir(m_szTemp);
	}
	
	m_szCache = szAppPath + CString("\\..\\cache");
	if (access(m_szCache, 0) != 0)
	{
		mkdir(m_szCache);
	}

	m_szData = szAppPath + CString("\\..\\Data");
	if (access(m_szData, 0) != 0)
	{
		mkdir(m_szData);
	}

	m_szOutput = szAppPath + CString("\\..");

	m_szMasterFile = m_szData + CString("\\MASTER.RVP");
	m_szSymbdataFile = m_szData + CString("\\SYMBDATA.RVP");
	m_szDataSrcsFile = m_szData + CString("\\DATASRCS.RVP");
	m_szPortfoliosFile = m_szData + CString("\\PORTFOLIOS.RVP");

	m_imMaster.Serialize(m_szMasterFile.GetBuffer());
	m_imSymbdata.Serialize(m_szSymbdataFile.GetBuffer());
	m_imDataSrcs.Serialize(m_szDataSrcsFile.GetBuffer());
	m_imPortfolios.Serialize(m_szPortfoliosFile.GetBuffer());

	m_szYahooFinance = m_szOutput + CString("\\YahooFinance\\YahooFinance.exe");
	m_szGoogleNews = m_szOutput + CString("\\GoogleNews\\GoogleNews.exe");

	auto hMutex = OpenMutex(MUTEX_ALL_ACCESS,FALSE,TEXT("VedavyasRao")); 
	if (hMutex == nullptr)
	{
		hMutex = CreateMutex(NULL,FALSE,TEXT("VedavyasRao"));
		StopYahooFinanceGoogleNews();
		StartYahooFinance();
		StartGoogleNews();
		Sleep(10000);
	}

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif

	// Change the registry key under which our settings are stored.
	// TODO: You should modify this string to be something appropriate
	// such as the name of your company or organization.
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));

	// To create the main window, this code creates a new frame window
	// object and then sets it as the application's main window object.
	CMainFrame* pFrame = new CMainFrame;
	m_pMainWnd = pFrame;

	// create and load the frame with its resources
	pFrame->LoadFrame(IDR_MAINFRAME,
		WS_OVERLAPPEDWINDOW | FWS_ADDTOTITLE, NULL,
		NULL);

	// The one and only window has been initialized, so show and update it.
	SetWindowTitle();
	pFrame->ShowWindow(SW_SHOW);
	pFrame->UpdateWindow();


	m_IconData.cbSize = sizeof m_IconData;
	m_IconData.hWnd = pFrame->m_hWnd;
	m_IconData.uID = 101;
	m_IconData.uFlags = NIF_ICON | NIF_MESSAGE | NIF_TIP;
	m_IconData.uCallbackMessage = WM_PM_TASKMSGS;
	m_IconData.hIcon = LoadIcon(IDR_MAINFRAME);
	lstrcpy(m_IconData.szTip, "QuoteMate");

	Shell_NotifyIcon(NIM_ADD, &m_IconData);
	

	pFrame->LoadPortfolios();
	pFrame->LoadBookmarks();
	m_pquotesrc = new CYahooSource();
	SetStatusMsg("Starting Threads");
	AfxBeginThread(PMQuotesNewsThread, NULL);
	AfxBeginThread(PMClockIndicesThread, NULL);
	AfxBeginThread(PMAlaramThread, NULL);
	LoadAgain();

	return TRUE;
}

void CRVVPMApp::StartYahooFinance()
{
	STARTUPINFO StartupInfo;
	char Args[4096];
	ULONG rc;

	sprintf(Args, "%s", m_szYahooFinance);
	ZeroMemory(&StartupInfo, sizeof(StartupInfo));
	ZeroMemory(&m_yahooproc, sizeof(m_yahooproc));
	StartupInfo.cb = sizeof(STARTUPINFO);
	StartupInfo.dwFlags = STARTF_USESHOWWINDOW;
	StartupInfo.wShowWindow = SW_HIDE;

	if (!CreateProcess(NULL, Args, NULL, NULL, FALSE,
		CREATE_NEW_CONSOLE | CREATE_NO_WINDOW,
		NULL,
		m_szOutput + CString("\\YahooFinance"),
		&StartupInfo,
		&m_yahooproc))
	{
		return;
	}
}

void	CRVVPMApp::StopYahooFinanceGoogleNews()
{
	WinExec("taskkill /f /t /im googlenews.exe", SW_HIDE);
	Sleep(5000);
	WaitForSingleObject(m_gnewsproc.hProcess, INFINITE);

	WinExec("taskkill /f /t /im yahoofinance.exe", SW_HIDE);
	WaitForSingleObject(m_yahooproc.hProcess, INFINITE);
	Sleep(5000);
}


void CRVVPMApp::StartGoogleNews()
{
	STARTUPINFO StartupInfo;
	char Args[4096];
	ULONG rc;

	sprintf(Args, "%s", m_szGoogleNews);
	ZeroMemory(&StartupInfo, sizeof(StartupInfo));
	ZeroMemory(&m_gnewsproc, sizeof(m_gnewsproc));
	StartupInfo.cb = sizeof(STARTUPINFO);
	StartupInfo.dwFlags = STARTF_USESHOWWINDOW;
	StartupInfo.wShowWindow = SW_HIDE;

	if (!CreateProcess(NULL, Args, NULL, NULL, FALSE,
		CREATE_NEW_CONSOLE | CREATE_NO_WINDOW,
		NULL,
		m_szOutput + CString("\\GoogleNews"),
		&StartupInfo,
		&m_gnewsproc))
	{
		return;
	}
}


/////////////////////////////////////////////////////////////////////////////
// CRVVPMApp message handlers

// App command to run the dialog
void CRVVPMApp::OnAppAbout()
{
	CAboutDlg aboutDlg;
	aboutDlg.DoModal();

}

/////////////////////////////////////////////////////////////////////////////
// CRVVPMApp message handlers

template <class T> int Compare( T& a, T& b, int order )
{
	return (((order == 1) && (a > b) ) || ((order == 2) && (a < b) ));
}

void CRVVPMApp::AddJob(CQMJob::Job_type jt)
{
	m_qmjobmanager.AddJob(jt);
}

void CRVVPMApp::ProcessJob()
{
	ShowBusy(1);
	m_qmjobmanager.ProcessJob();
	ShowBusy(0);
}

void CRVVPMApp::LoadAgain()
{
	AddJob(CQMJob::LoadPortfolio);
}

void CRVVPMApp::SortList()
{
	int		i, j, ret, col;

	for ( i=0; i<m_header.GetSize(); ++i)
	{
		if (m_header[i].m_sort != 0)
			break;
	}

	if (i == m_header.GetSize())
		return;

	col = i;



	for (i=0; i<m_currentlist.GetSize(); ++i)
	{
		for (j=i; j<m_currentlist.GetSize(); ++j)
		{
			if (m_abort)
				return;

			ret = 0;
			switch(m_header[col].m_id)
			{
				case PM_EXCHANGE:
					ret = Compare(CString(m_currentlist[i]->m_exchange), CString(m_currentlist[j]->m_exchange), m_header[col].m_sort);
					break;
			
				case PM_SYMBOL:
					ret = Compare(CString(m_currentlist[i]->m_symbol), CString(m_currentlist[j]->m_symbol) , m_header[col].m_sort );
					break;

				case PM_OPEN:
					ret = Compare(m_currentlist[i]->m_open, m_currentlist[j]->m_open , m_header[col].m_sort );
					break;

				case PM_ASK:
					ret = Compare(m_currentlist[i]->m_ask, m_currentlist[j]->m_ask , m_header[col].m_sort );
					break;

				case PM_BID:
					ret = Compare(m_currentlist[i]->m_bid, m_currentlist[j]->m_bid , m_header[col].m_sort );
					break;

				case PM_CLOSEDAY1:
					ret = Compare(m_currentlist[i]->m_close[0], m_currentlist[j]->m_close[0] , m_header[col].m_sort );
					break;

				case PM_CLOSEDAY2:
					ret = Compare(m_currentlist[i]->m_close[1], m_currentlist[j]->m_close[1] , m_header[col].m_sort );
					break;

				case PM_CLOSEDAY3:
					ret = Compare(m_currentlist[i]->m_close[2], m_currentlist[j]->m_close[2] , m_header[col].m_sort );
					break;

				case PM_CLOSEDAY4:
					ret = Compare(m_currentlist[i]->m_close[3], m_currentlist[j]->m_close[3] , m_header[col].m_sort );
					break;

				case PM_CLOSEDAY5:
					ret = Compare(m_currentlist[i]->m_close[4], m_currentlist[j]->m_close[4] , m_header[col].m_sort );
					break;

				case PM_LASTTRADE:
					ret = Compare(m_currentlist[i]->m_lasttrade, m_currentlist[j]->m_lasttrade , m_header[col].m_sort );
					break;

				case PM_CHANGE:
					ret = Compare(m_currentlist[i]->m_change[0], m_currentlist[j]->m_change[0] , m_header[col].m_sort );
					break;

				case PM_CHANGE2:
					ret = Compare(m_currentlist[i]->m_change[1], m_currentlist[j]->m_change[1] , m_header[col].m_sort );
					break;

				case PM_CHANGE3:
					ret = Compare(m_currentlist[i]->m_change[2], m_currentlist[j]->m_change[2] , m_header[col].m_sort );
					break;

				case PM_CHANGE4:
					ret = Compare(m_currentlist[i]->m_change[3], m_currentlist[j]->m_change[3] , m_header[col].m_sort );
					break;

				case PM_CHANGE5:
					ret = Compare(m_currentlist[i]->m_change[4], m_currentlist[j]->m_change[4] , m_header[col].m_sort );
					break;
			}

			if (ret)
			{
				CPMScrip *p;
				p = m_currentlist[i];
				m_currentlist[i] = m_currentlist[j];
				m_currentlist[j] = p;
			}

		}
	}

	for (i = 0; i < m_currentlist.GetSize(); ++i)
	{
		auto temp = CString(m_currentlist[i]->m_symbol);
		if (temp == "^DJI")
		{
			if (i == 0)
				continue;
			CPMScrip *dow = m_currentlist[i];
			for (j = i-1; j >= 0 ; --j)
			{
				CPMScrip *P = m_currentlist[j+1];
				CPMScrip *P2 = m_currentlist[j];
				m_currentlist[j+1] = m_currentlist[j];
			}
			m_currentlist[0] = dow;
			break;
		}
	}

	for (i = 1; i < m_currentlist.GetSize(); ++i)
	{
		auto temp = CString(m_currentlist[i]->m_symbol);
		if (temp == "^IXIC")
		{
			if (i == 1)
				continue;
			CPMScrip *nasdaq = m_currentlist[i];
			for (j = i - 1; j >= 1; --j)
			{
				CPMScrip *P3 = m_currentlist[j + 1];
				CPMScrip *P4 = m_currentlist[j];
				m_currentlist[j + 1] = m_currentlist[j];
			}
			m_currentlist[1] = nasdaq;
			break;
		}
	}

	for (i = 1; i < m_currentlist.GetSize(); ++i)
	{
		auto temp = CString(m_currentlist[i]->m_symbol);
		if (temp == "^GSPC")
		{
			if (i == 2)
				continue;
			CPMScrip *gspc = m_currentlist[i];
			for (j = i - 1; j >= 1; --j)
			{
				CPMScrip *P3 = m_currentlist[j + 1];
				CPMScrip *P4 = m_currentlist[j];
				m_currentlist[j + 1] = m_currentlist[j];
			}
			m_currentlist[2] = gspc;
			break;
		}
	}
}

void CRVVPMApp::LoadPortfolio()
{
	SetStatusMsg("Loading data...");

	COLLIST			m_header;
	SCRIPLIST		m_currentlist;

	m_header.RemoveAll();
	auto cols = m_imMaster.GetKeys( "Columns");
	for (auto itr=cols.begin(); itr != cols.end(); ++itr)
	{
		m_header.Add(CPMColumn(GetColumnId(itr->c_str()), itr->c_str(), m_imMaster.GetPrivateProfileInt("Columns",*itr,0)));
	}

	string szCurrent;
	m_currentlist.RemoveAll();

	m_imPortfolios.GetPrivateProfileString("Settings", "Current", "", szCurrent);
	auto pf = m_imPortfolios.GetKeys(szCurrent);
	int alramflag=0;
	for (auto pfitr = pf.begin(); pfitr != pf.end(); ++pfitr)
	{
		if (m_abort)
			return;

		CPMScrip*	pscrip;

		int i=0;
		for (i=0; i<m_scriplist.GetSize(); ++i)
			if ((CString(m_scriplist[i]->m_exchange)+","+ CString(m_scriplist[i]->m_symbol)) == CString(pfitr->c_str()))
				break;

		if (i == m_scriplist.GetSize())
		{
			pscrip = new CPMScrip((char*)pfitr->c_str());
			m_scriplist.Add( pscrip );
			
			m_closesrc.QueryClose(pscrip);
			m_pquotesrc->QueryQuotes(pscrip);
			pscrip->Calucalate();
			if (pscrip->m_alramflag)
				alramflag = 1;

		}
		else
		{
			pscrip = m_scriplist[i];
		}

		m_currentlist.Add( pscrip );

	}

	SetStatusMsg("Done");

	this->m_header.RemoveAll();
	for (int i=0; i<m_header.GetSize(); ++i)
		this->m_header.Add(m_header[i]); 
	
	this->m_currentlist.RemoveAll();
	for (int i=0; i<m_currentlist.GetSize(); ++i)
		this->m_currentlist.Add(m_currentlist[i]); 

	SortList();

	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_RESETLISTCTRL, 0, 0 );
	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_FILLLISTCTRL, 0, 0 );
	
	if (alramflag)
		m_alertevent.PulseEvent();
}



void CRVVPMApp::FillData()
{
	int alramflg = 0;


	SetStatusMsg("Quering quotes...");

	for (int i=0; i<m_currentlist.GetSize(); ++i)
	{
		if (m_abort)
			return;

		m_pquotesrc->QueryQuotes(m_currentlist[i]);
		m_currentlist[i]->Calucalate();
		if (m_currentlist[i]->m_alramflag)
			alramflg = 1;

	}
	m_bQuoteOndemand = false;
	SortList();
	SetStatusMsg("Done");
	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_FILLLISTCTRL, 0, 0 );
	if (alramflg)
		m_alertevent.PulseEvent();

}

void CRVVPMApp::SortData()
{
	SortList();
	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_FILLLISTCTRL, 0, 0 );

}

std::string truncate(std::string str, size_t width)
{
	if (str.length() > width)
			return str.substr(0, width) + "...";
		else
			return str.substr(0, width);
	return str;
}

void CRVVPMApp::PrintData()
{

	CDC printerdc;

	//CPrintDialog dlg(TRUE, PD_RETURNDEFAULT, this->m_pMainWnd );
	//if (!dlg.GetDefaults())
	//	return;
	//LPDEVMODE   pDevInfo = dlg.GetDevMode();
	//pDevInfo->dmOrientation = 2;

	typedef tuple<int, string, int, int, int> PCData;

	map<int, tuple<string, int, int>>  colidname =
	{
		{PM_EXCHANGE      ,{ "Exchange",10,1}},
		{PM_SYMBOL       ,{ "Symbol",10 ,0}},
		{PM_ALARAM       ,{ "Alarm",15 ,1}},
		{PM_LASTTRADE    ,{ "Price",15 ,0}},
		{PM_CHANGE       ,{ "Change",20 ,0}},
		{PM_CHANGE2      ,{ "Change 2",20 ,0}},
		{PM_CHANGE3      ,{ "Change 3",20 ,0}},
		{PM_CHANGE4      ,{ "Change 4",20 ,0}},
		{PM_CHANGE5      ,{ "Change 5",20 ,0}},
		{PM_OPEN         ,{ "Open",10 ,0}},
		{PM_VOLUME       ,{ "Volume",10 ,0}},
		{PM_ASK          ,{ "Ask",10 ,0}},
		{PM_BID          ,{ "Bid",10 ,0}},
		{PM_CLOSEDAY1    ,{ "Close",10 ,0}},
		{PM_CLOSEDAY2    ,{ "Close 2",10 ,0}},
		{PM_CLOSEDAY3    ,{ "Close 3",10 ,0}},
		{PM_CLOSEDAY4    ,{ "Close 4",10 ,0}},
		{PM_CLOSEDAY5    ,{ "Close 5",10 ,0}},
		{PM_DAYRANGE     ,{ "Day Range",20 ,0}},
		{PM_52WKRANGE    ,{ "52Wk Range",20 ,0}},
		{PM_BOUGHT       ,{ "Bought",05 ,0}},
		{PM_PAID         ,{ "Paid",10 ,0}},
		{PM_GAIN        ,{ "Gain",15 ,0}},
		{PM_HGHALRM      ,{ "High Alarm",10 ,0}},
		{PM_LOWALRM      ,{ "Low Alarm",10 ,0}},
		{PM_ACTVALUE     ,{ "Value",10 ,0}},
	};

	CPrintDialog dlg(FALSE);
	if (dlg.DoModal() != IDOK)
		return;
	LPDEVMODE   pDevInfo = dlg.GetDevMode();

	HDC hdc = dlg.CreatePrinterDC();
	printerdc.Attach(hdc);

	int nHorz = printerdc.GetDeviceCaps(HORZRES);
	int nVert = printerdc.GetDeviceCaps(VERTRES);

	int fontmulti = nHorz / 640;

	CFont   courierfnt;
	courierfnt.CreateFont(14 * fontmulti, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");

	CFont   courierfnt1;
	courierfnt1.CreateFont(10 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");

	CFont   courierfnt2;
	courierfnt2.CreateFont(10 * fontmulti, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");

	CFont   arialfnt;
	arialfnt.CreateFont(18 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Arial");

	CFont   arialfnt1;
	arialfnt1.CreateFont(14 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Arial");

	printerdc.SelectObject(&courierfnt1);

	map<string, PCData> PrintCols;
	vector<string> PrintColNames;
	auto keys = m_imMaster.GetKeys("Columns");
	CString str;
	for (auto itr=keys.begin(); itr != keys.end(); ++itr)
	{
		if (m_imMaster.GetPrivateProfileInt("Columns", itr->c_str(), 0) == 0)
			continue;

		for (int i = PM_FIRSTCOLUMN + 1; i < PM_LASTCOLUMN; ++i)
		{
			str.LoadString(i);
			char *colname = strtok(str.GetBuffer(), "\t");
			char *colwidth = strtok(NULL, "\t");
			if (strcmpi(itr->c_str(), colname)==0)
			{
				PrintCols[colname] = make_tuple(i, get<0>(colidname[i]), get<1>(colidname[i]), 0, get<2>(colidname[i]));
				PrintColNames.push_back(colname);
				break;
			}
		}
	}
    // Initialize the members of a DOCINFO structure. 
     DOCINFO di;
	di.cbSize = sizeof(DOCINFO); 
    di.lpszDocName = "RVV QuoteMate Report"; 
    di.lpszOutput = (LPTSTR) NULL; 
    di.lpszDatatype = (LPTSTR) NULL; 
    di.fwType = 0; 
 
    printerdc.StartDoc (&di); 
	

	int x, y;
	CString st, timestr;
	CSize sz;

	char tmpbuf[100];

	_strdate( tmpbuf );
	timestr = tmpbuf;
	timestr += " ";
	_strtime( tmpbuf );
	timestr += tmpbuf;

	((CYahooSource*)m_pquotesrc)->QueryDateTime(timestr);

	char dowstr[100], nazstr[100];
	sscanf(m_dow, "%s %s", tmpbuf, dowstr);
	sscanf(m_nasdaq, "%s %s", tmpbuf, nazstr);

	int pagecounter = 0, line=0;
	while (1)
	{
		if (m_abort)
			return;

		st.Format("Page %4d", ++pagecounter);
		st = "Printing "+ st;
		SetStatusMsg(st);
	    
		printerdc.StartPage(); 

		x = 10*fontmulti;
		y = 0;
		printerdc.SelectObject(&courierfnt2);
		printerdc.TextOut(x, y, timestr);
		
		st = "RVV QUOTEMATE";
		printerdc.SelectObject(&arialfnt);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x = (nHorz - sz.cx) / 2;
		y = 0;
		printerdc.TextOut(x, y, st);

		st.Format("Page %4d", pagecounter);
		printerdc.SelectObject(&courierfnt2);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x = (nHorz - sz.cx - 50) ;
		y = 0;
		printerdc.TextOut(x, y, st);


		x = 0;
		y = sz.cy + 12*fontmulti;
		st = "DOW:";
		printerdc.SelectObject(&arialfnt1);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		st = dowstr;
		printerdc.SelectObject(&courierfnt);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		st = "NASDAQ:";
		printerdc.SelectObject(&arialfnt1);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		st = nazstr;
		printerdc.SelectObject(&courierfnt);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		

		x = (nHorz - x)/2;
		st = "DOW:";
		printerdc.SelectObject(&arialfnt1);
		printerdc.TextOut(x, y, st);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		st = dowstr;
		printerdc.SelectObject(&courierfnt);
		printerdc.TextOut(x, y, st);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		st = "NASDAQ:";
		printerdc.SelectObject(&arialfnt1);
		printerdc.TextOut(x, y, st);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		st = nazstr;
		printerdc.SelectObject(&courierfnt);
		printerdc.TextOut(x, y, st);
		sz = printerdc.GetTextExtent( st, st.GetLength());

		int fontmultix = 10*fontmulti;
		while (1)
		{
			printerdc.SelectObject(&courierfnt2);
			int llen = 0;
			int colsz = 0;
			for (int k = 0; k < PrintColNames.size(); ++k)
			{
				PCData &pcd = PrintCols[PrintColNames[k]];
				get<3>(pcd) = colsz;
				string s(get<2>(pcd), ' ');
				colsz = printerdc.GetTextExtent(s.c_str(), s.length()).cx;
				llen += colsz;
			}
			if (llen < nHorz)
				break;
			--fontmultix;
			courierfnt1.DeleteObject();
			courierfnt1.CreateFont(fontmultix, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");
			courierfnt2.DeleteObject();
			courierfnt2.CreateFont(fontmultix, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");
		}

		x = 10;
		y += sz.cy + 8*fontmulti;
		printerdc.SelectObject(&courierfnt1);
		for (int k = 0; k<PrintColNames.size(); ++k)
		{
			PCData &pcd = PrintCols[PrintColNames[k]];
			x += get<3>(pcd);
			strstream sstr;
			sstr << ((get<4>(pcd)) ? left : internal) << setfill(' ') << setw(get<2>(pcd)) << get<1>(pcd) << ends;
			CString ss = sstr.str();
			printerdc.TextOut(x, y, ss.GetBuffer(), ss.GetLength());
		}

		printerdc.SelectObject(&courierfnt2);
		for (; line<m_currentlist.GetSize(); ++line)
		{
			x =10;
			y += sz.cy + 8 * fontmulti;
			CPMScrip*  pscrip = m_currentlist[line];
			for (int k=0; k<PrintCols.size(); ++k)
			{
				PCData &pcd = PrintCols[PrintColNames[k]];
				x += get<3>(pcd);
				strstream sstr;
				sstr << ((get<4>(pcd))?left:right) << setfill(' ') << setw(get<2>(pcd)) << pscrip->GetColumnValue(get<0>(pcd), 1).Trim().GetBuffer() << ends;
				CString ss = sstr.str();
				printerdc.TextOut(x, y, ss.GetBuffer(), ss.GetLength());
			}

			if (y +sz.cy >= nVert)
				break;
		}

		printerdc.EndPage(); 
		if (line == m_currentlist.GetSize())
			break;
	} 
	
	printerdc.EndDoc();
	SetStatusMsg("Done");
}

void CRVVPMApp::LoadNews()
{
	int alramflag = 0;
	SetStatusMsg("Querying news..");
	for (int i=0; i<m_currentlist.GetSize(); ++i)
	{
		if (m_abort)
			return;

		m_newssrc.QueryNews(m_currentlist[i]);
		m_currentlist[i]->Calucalate();
		if (m_currentlist[i]->m_alramflag)
			alramflag = 1;
	}
	m_bNewsOndemand = false;
	SetStatusMsg("Done");
	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_FILLLISTCTRL, 0, 0 );
	if (alramflag)
		m_alertevent.PulseEvent();

}


int CRVVPMApp::GetColumnId(CString col)
{
	CString str;
	for (int i=PM_FIRSTCOLUMN+1; i < PM_LASTCOLUMN; ++i)
	{
		str.LoadString(i);
		str = str.Mid(0, str.Find("\t"));
		if (str == col)
			return i;

	}

	return -1;
}

void CRVVPMApp::SetStatusMsg(CString msg)
{
	m_statusmsg	= msg;
	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_UPDATESTATUSBAR, 0, 0 );
	m_pMainWnd->UpdateWindow();
}

void CRVVPMApp::SetPaneInfo(CString& pane1, CString& pane2, CString& pane3)
{
	m_nasdaq = pane1;
	m_dow = pane2;
	m_curtime = pane3;
	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_UPDATEPANES, 0, 0 );
}
	
void	CRVVPMApp::SetWindowTitle()
{
	CString str;
	str.LoadString(IDR_MAINFRAME);
	string szCurrent;
	m_imMaster.GetPrivateProfileString("Settings", "DataSource", "", szCurrent);
	GetMainWnd()->SetWindowText(str+szCurrent.c_str());

}

void    CRVVPMApp::ShowBusy(bool bbusy)
{
	::PostMessage(m_pMainWnd->m_hWnd, WM_PM_UPDATEBUSYPANE, bbusy, 0);
}

void CRVVPMApp::KillThreads()
{
	HANDLE hs[]{ m_quit_evts[0],m_quit_evts[1],m_quit_evts[2]};
	m_quit = true;
	m_abort = true;
	PulseEvent(m_queryevent);
	PulseEvent(m_alertevent);
	auto ret = WaitForMultipleObjects(3, hs, TRUE, 30000);

}

int CRVVPMApp::ExitInstance() 
{
	// TODO: Add your specialized code here and/or call the base class
	StopYahooFinanceGoogleNews();
	m_imMaster.Persist();
	m_imSymbdata.Persist();
	m_imDataSrcs.Persist();
	m_imPortfolios.Persist();

	return CWinApp::ExitInstance();
}

void CRVVPMApp::OnContextInitialized()
{
	CEF_REQUIRE_UI_THREAD();
	
	// Specify CEF browser settings here.
	CefBrowserSettings browser_settings;
	CefRefPtr<CefCommandLine> commandLine = CefCommandLine::CreateCommandLine();
	commandLine->InitFromString(::GetCommandLineW());

	// Information used when creating the native window.
	CefWindowInfo window_info;

	// Alloy style will create a basic native window. Chrome style will create a
	// fully styled Chrome UI window.
	window_info.runtime_style = CEF_RUNTIME_STYLE_DEFAULT;

	HWND parentWnd = m_htmlview->GetSafeHwnd();
	auto browserHandler = ((CPMHTMLView*)m_htmlview)->m_browserHandler.get();
	RECT rect;
	m_htmlview->GetClientRect(&rect);

	window_info.SetAsChild(parentWnd, CefRect(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top));
	browserHandler->SetMainHwnd(parentWnd);
	CefBrowserHost::CreateBrowser(window_info, browserHandler, "", browser_settings, nullptr, nullptr);

}

CefRefPtr<CefClient> CRVVPMApp::GetDefaultClient()
{
	// Called when a new browser window is created via Chrome style UI.
	return m_browserHandler;
}

BOOL CRVVPMApp::PumpMessage()
{
	auto result = CWinApp::PumpMessage();

	CefDoMessageLoopWork();

	return result;
}
