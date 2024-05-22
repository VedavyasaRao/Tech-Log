// RVVPMView.h : interface of the CRVVPMView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_RVVPMVIEW_H__929F0DCD_9EBB_11D3_853E_006097B6FED6__INCLUDED_)
#define AFX_RVVPMVIEW_H__929F0DCD_9EBB_11D3_853E_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CRVVPMView : public CFormView
{
protected: // create from serialization only
	CRVVPMView();
	DECLARE_DYNCREATE(CRVVPMView)

public:
	//{{AFX_DATA(CRVVPMView)
	enum{ IDD = IDD_RVVPM_FORM };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

// Attributes
public:
	CRVVPMDoc* GetDocument();
	void FillControl();
// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRVVPMView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnPrint(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnDraw(CDC* pDC);
	//}}AFX_VIRTUAL

// Implementation
public:
	void ResetControl();
	virtual ~CRVVPMView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CRVVPMView)
	afx_msg void OnPmOneday();
	afx_msg void OnUpdatePmOneday(CCmdUI* pCmdUI);
	afx_msg void OnPmNews();
	afx_msg void OnUpdatePmNews(CCmdUI* pCmdUI);
	afx_msg void OnPmFiveday();
	afx_msg void OnUpdatePmFiveday(CCmdUI* pCmdUI);
	afx_msg void OnPmMo();
	afx_msg void OnUpdatePmMo(CCmdUI* pCmdUI);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	CString GetSymbol();
};

#ifndef _DEBUG  // debug version in RVVPMView.cpp
inline CRVVPMDoc* CRVVPMView::GetDocument()
   { return (CRVVPMDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RVVPMVIEW_H__929F0DCD_9EBB_11D3_853E_006097B6FED6__INCLUDED_)
