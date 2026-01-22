#if !defined(AFX_PMLISTVIEW_H__4CE23C52_B257_11D3_854D_006097B6FED6__INCLUDED_)
#define AFX_PMLISTVIEW_H__4CE23C52_B257_11D3_854D_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// PMListView.h : header file
//

#include "ListVwEx.h"

/////////////////////////////////////////////////////////////////////////////
// CPMListView view

class CPMListView : public CListViewEx
{
protected:
	CPMListView();           // protected constructor used by dynamic creation
	DECLARE_DYNCREATE(CPMListView)

// Attributes

// Operations
public:
void FillControl();
void ResetControl();
CString GetExchange();
CString GetSymbol();
int GetColumnWidth(int ncol);

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPMListView)
	public:
	virtual void OnInitialUpdate();
	protected:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	//}}AFX_VIRTUAL

// Implementation
protected:
	virtual ~CPMListView();

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

	// Generated message map functions
protected:
	//{{AFX_MSG(CPMListView)
	afx_msg void OnClick(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnColumnclick(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	int GetColWidth(int colid);

};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PMLISTVIEW_H__4CE23C52_B257_11D3_854D_006097B6FED6__INCLUDED_)
