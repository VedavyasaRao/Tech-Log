// RVVPM.h : main header file for the RVVPM application
//

#if !defined(AFX_RVVPM_H__4CE23C45_B257_11D3_854D_006097B6FED6__INCLUDED_)
#define AFX_RVVPM_H__4CE23C45_B257_11D3_854D_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CRVVPMApp:
// See RVVPM.cpp for the implementation of this class
//
#include "mainfrm.h"
#include "pmscrip.h"
#include "pmcolumn.h"
#include "DatasourceImp.h"
#include "qmjob.h"
#include "IniFile.h"

typedef	CArray<CPMColumn,CPMColumn&>	COLLIST;
typedef	CArray<CPMScrip*,CPMScrip*>		SCRIPLIST;

UINT PMClockIndicesThread( LPVOID pParam );
UINT PMQuotesNewsThread( LPVOID pParam );
UINT PMAlaramThread( LPVOID pParam );

class CRVVPMApp : public CWinApp
{
public:
	CRVVPMApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRVVPMApp)
	public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();
	//}}AFX_VIRTUAL

// Implementation
public:
	CString				m_szYahooFinance;
	CString				m_szGoogleNews;
	CString				m_szTemp;
	CString				m_szData;
	CString				m_szOutput;
	CEvent				m_queryevent;
	CEvent				m_alertevent;

	CString				m_nasdaq;
	CString				m_dow;
	CString				m_curtime;
	CString				m_statusmsg;

	
	CYahooSource	m_closesrc;
	CYahooSource	m_newssrc;
	CPMDataSource*	m_pquotesrc;

	IniFile         m_imMaster;
	IniFile         m_imSymbdata;
	IniFile         m_imDataSrcs;
	IniFile         m_imPortfolios;
	
	COLLIST			m_header;
	SCRIPLIST		m_currentlist;
	SCRIPLIST		m_scriplist;
	NOTIFYICONDATA	m_IconData;
	CQMJobManager	m_qmjobmanager;
	bool m_abort = false;

public:
	void	LoadPortfolio();
	void	LoadAgain();
	void	FillData();
	void	LoadNews();
	void	SortData();
	void	PrintData();
	void	SetStatusMsg(CString msg);
	void	SetPaneInfo(CString&, CString&, CString&);
	int		GetColumnId(CString col);
	void	SortList();
	void	SetWindowTitle();
	void	AddJob(CQMJob::Job_type jt);
	void	ProcessJob();
	void    ShowBusy(bool bbusy);
	void	StartYahooFinance();
	void	StartGoogleNews();
	void	StopYahooFinanceGoogleNews();

public:
	//{{AFX_MSG(CRVVPMApp)
	afx_msg void OnAppAbout();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

public:
	bool				m_bNewsOndemand;
	bool				m_bQuoteOndemand;

private:
	CString				m_szMasterFile;
	CString				m_szSymbdataFile;
	CString				m_szDataSrcsFile;
	CString				m_szPortfoliosFile;

};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RVVPM_H__4CE23C45_B257_11D3_854D_006097B6FED6__INCLUDED_)
