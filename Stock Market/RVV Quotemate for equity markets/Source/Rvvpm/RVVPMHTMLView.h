#if !defined(AFX_RVVPMHTMLVIEW_H__F4310056_A961_11D3_8542_006097B6FED6__INCLUDED_)
#define AFX_RVVPMHTMLVIEW_H__F4310056_A961_11D3_8542_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// RVVPMHTMLView.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CRVVPMHTMLView html view

#ifndef __AFXEXT_H__
#include <afxext.h>
#endif
#include <afxhtml.h>

class CRVVPMHTMLView : public CHtmlView
{
protected:
	CRVVPMHTMLView();           // protected constructor used by dynamic creation
	DECLARE_DYNCREATE(CRVVPMHTMLView)

// html Data
public:
	//{{AFX_DATA(CRVVPMHTMLView)
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRVVPMHTMLView)
	public:
	virtual void OnInitialUpdate();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	virtual ~CRVVPMHTMLView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

	// Generated message map functions
	//{{AFX_MSG(CRVVPMHTMLView)
		// NOTE - the ClassWizard will add and remove member functions here.
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RVVPMHTMLVIEW_H__F4310056_A961_11D3_8542_006097B6FED6__INCLUDED_)
