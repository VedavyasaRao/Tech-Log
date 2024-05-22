// MainFrm.h : interface of the CMainFrame class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MAINFRM_H__4CE23C49_B257_11D3_854D_006097B6FED6__INCLUDED_)
#define AFX_MAINFRM_H__4CE23C49_B257_11D3_854D_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "pmlistView.h"
#include "pmhtmlView.h"
#include "MainToolBar.h"
#include "IniFile.h"

#define WM_PM_UPDATEPANES                   WM_USER + 0
#define WM_PM_UPDATESTATUSBAR               WM_USER + 1
#define WM_PM_RESETLISTCTRL                 WM_USER + 2
#define WM_PM_FILLLISTCTRL                  WM_USER + 3
#define WM_PM_FLASHWINDOW                   WM_USER + 4
#define WM_PM_TASKMSGS						WM_USER + 5
#define WM_PM_UPDATEBUSYPANE                WM_USER + 6

class CMainFrame : public CFrameWnd
{
	
public:
	CMainFrame();
protected: 
	DECLARE_DYNAMIC(CMainFrame)

// Attributes
public:

// Operations
public:
	void LoadHtmlPane();
	void LoadPortfolios();
	void LoadBookmarks();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainFrame)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual BOOL DestroyWindow();
	protected:
	virtual BOOL OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext);
	virtual LRESULT WindowProc(UINT message, WPARAM wParam, LPARAM lParam);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMainFrame();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:  // control bar embedded members
	//CStatusBar  m_wndStatusBar;
	CColoredStatusBarCtrl  m_wndStatusBar;
	CMainToolBar    m_wndToolBar;
	CSplitterWnd	m_wndSplitter;
	CComboBox	m_urllist;
	CComboBox	m_pflist;
	int			m_htmlsel;
	IniFile*  m_pimmaster;
	IniFile*  m_piportfolips;


// Generated message map functions
protected:

	//{{AFX_MSG(CMainFrame)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnPmEdit();
	afx_msg void OnPmSeetings();
	afx_msg void OnPmTrigger();
	afx_msg void OnQmUrls();
	afx_msg void OnQmSave();
	afx_msg void OnQmRun();
	afx_msg void OnUpdateQmRun(CCmdUI* pCmdUI);
	afx_msg void OnQmBak();
	afx_msg void OnUpdateQmBak(CCmdUI* pCmdUI);
	afx_msg void OnQmFwd();
	afx_msg void OnUpdateQmFwd(CCmdUI* pCmdUI);
	afx_msg void OnQmRefresh();
	afx_msg void OnUpdateQmRefresh(CCmdUI* pCmdUI);
	afx_msg void OnPmQrefresh();
	afx_msg void OnPmNrefresh();
	afx_msg void OnPmPrint();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINFRM_H__4CE23C49_B257_11D3_854D_006097B6FED6__INCLUDED_)
