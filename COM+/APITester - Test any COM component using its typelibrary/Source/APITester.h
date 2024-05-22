// APITester.h : main header file for the APITESTER application
//

#if !defined(AFX_APITESTER_H__0784C1A5_A947_11D4_A50A_F77134B5463F__INCLUDED_)
#define AFX_APITESTER_H__0784C1A5_A947_11D4_A50A_F77134B5463F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CAPITesterApp:
// See APITester.cpp for the implementation of this class
//

class CAPITesterApp : public CWinApp
{
public:
	CAPITesterApp();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAPITesterApp)
	public:
	virtual BOOL InitInstance();
	virtual BOOL InitApplication();
	//}}AFX_VIRTUAL

// Implementation

public:
	//{{AFX_MSG(CAPITesterApp)
	afx_msg void OnAppAbout();
	afx_msg void OnFilePrintSetup();
	afx_msg void OnApitestprintDirect();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	void PrintFuncDef();
	void PrintLog();

public:
	CString m_profilefile, m_logfile, m_logbuf, m_historyfile;
	
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_APITESTER_H__0784C1A5_A947_11D4_A50A_F77134B5463F__INCLUDED_)
