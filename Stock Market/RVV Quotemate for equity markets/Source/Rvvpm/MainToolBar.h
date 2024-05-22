#if !defined(AFX_MAINTOOLBAR_H__E39F5946_BB20_11D3_8561_006097B6FED6__INCLUDED_)
#define AFX_MAINTOOLBAR_H__E39F5946_BB20_11D3_8561_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// MainToolBar.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CMainToolBar window

class CMainToolBar : public CToolBar
{
// Construction
public:
	CMainToolBar();

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainToolBar)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMainToolBar();

	// Generated message map functions
protected:
	//{{AFX_MSG(CMainToolBar)
		// NOTE - the ClassWizard will add and remove member functions here.
	afx_msg void OnSelchangeCombo1();
	afx_msg void OnSelchangeCombo2();
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINTOOLBAR_H__E39F5946_BB20_11D3_8561_006097B6FED6__INCLUDED_)
