// isql.h : main header file for the ISQL application
//

#if !defined(AFX_ISQL_H__D8B94E8C_C07A_11D2_846D_006097B6FED6__INCLUDED_)
#define AFX_ISQL_H__D8B94E8C_C07A_11D2_846D_006097B6FED6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CIsqlApp:
// See isql.cpp for the implementation of this class
//

class CIsqlApp : public CWinApp
{
public:
	int execSQL(CString& sqltxt);
	 ~CIsqlApp();
	HSTMT m_hstmt;
	HDBC m_hdbc;
	HENV m_henv;
	CIsqlApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CIsqlApp)
	public:
	virtual BOOL InitInstance();
	virtual CDocument* OpenDocumentFile(LPCTSTR lpszFileName);
	//}}AFX_VIRTUAL

// Implementation

	//{{AFX_MSG(CIsqlApp)
	afx_msg void OnAppAbout();
	afx_msg void OnExecsql();
	afx_msg void OnUpdateExecsql(CCmdUI* pCmdUI);
	afx_msg void OnConnect();
	afx_msg void OnTilehoriz();
	afx_msg void OnUpdateTilehoriz(CCmdUI* pCmdUI);
	afx_msg void OnTilevert();
	afx_msg void OnUpdateTilevert(CCmdUI* pCmdUI);
	afx_msg void OnShowsqlwindow();
	afx_msg void OnUpdateShowsqlwindow(CCmdUI* pCmdUI);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()


public:
	void GetMoreRows(CView*);
	void ShowSQLError(CString="");

private:
	CView* m_psqlvw;
	CView* m_poutvw;
	HWND	m_hwnd;
	int execQuery(CString& sqlstmt, int pipe);
	HFONT m_hfont;
	CView* getOutputView(int);
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ISQL_H__D8B94E8C_C07A_11D2_846D_006097B6FED6__INCLUDED_)
