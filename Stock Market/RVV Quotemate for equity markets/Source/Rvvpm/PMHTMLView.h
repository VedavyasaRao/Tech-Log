#if !defined(AFX_PMHTMLVIEW_H__4CE23C53_B257_11D3_854D_006097B6FED6__INCLUDED_)
#define AFX_PMHTMLVIEW_H__4CE23C53_B257_11D3_854D_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// PMHTMLView.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CPMHTMLView html view

#ifndef __AFXEXT_H__
#include <afxext.h>
#endif
#include <afxwin.h>
#include "handler.h"

class CPMHTMLView : public CView
{
protected:
	CPMHTMLView();           // protected constructor used by dynamic creation
	DECLARE_DYNCREATE(CPMHTMLView)

// html Data
public:
	//{{AFX_DATA(CPMHTMLView)
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA
	void Navigate(CString url,int scroll);
	void NavigateBack();
	void NavigateForward();
// Attributes
public:
	CefRefPtr<ClientHandler> m_browserHandler;
	bool m_cefIsInitialized;
	LONG_PTR m_originalStyle;
	std::tuple<CefBrowser *, int> m_scrolldata;
	// Operations
public:
// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPMHTMLView)
	protected:

	//}}AFX_VIRTUAL

// Implementation
protected:
	bool init();
	virtual void OnDraw(CDC *);
	virtual void OnInitialUpdate();
	virtual void OnSize(UINT nType, int w, int h);
	virtual ~CPMHTMLView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

	// Generated message map functions
	//{{AFX_MSG(CPMHTMLView)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PMHTMLVIEW_H__4CE23C53_B257_11D3_854D_006097B6FED6__INCLUDED_)
