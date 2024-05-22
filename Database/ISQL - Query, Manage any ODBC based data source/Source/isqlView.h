// isqlView.h : interface of the CIsqlView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_ISQLVIEW_H__D8B94E96_C07A_11D2_846D_006097B6FED6__INCLUDED_)
#define AFX_ISQLVIEW_H__D8B94E96_C07A_11D2_846D_006097B6FED6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

class CIsqlView : public CEditView
{
protected: // create from serialization only
	CIsqlView();
	DECLARE_DYNCREATE(CIsqlView)

// Attributes
public:
	CIsqlDoc* GetDocument();

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CIsqlView)
	public:
	virtual void OnDraw(CDC* pDC);  // overridden to draw this view
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	virtual void OnInitialUpdate();
	protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CIsqlView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CIsqlView)
	afx_msg void OnUpdate();
	afx_msg void OnEditFind();
	afx_msg void OnUpdateEditFind(CCmdUI* pCmdUI);
	afx_msg void OnMorerows();
	afx_msg void OnUpdateMorerows(CCmdUI* pCmdUI);
	afx_msg void OnLButtonDblClk(UINT nFlags, CPoint point);
	afx_msg void OnEditClearAll();
	afx_msg void OnDestroy();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in isqlView.cpp
inline CIsqlDoc* CIsqlView::GetDocument()
   { return (CIsqlDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ISQLVIEW_H__D8B94E96_C07A_11D2_846D_006097B6FED6__INCLUDED_)
