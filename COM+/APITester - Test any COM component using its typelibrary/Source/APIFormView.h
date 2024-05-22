//{{AFX_INCLUDES()
#include "msflexgrid.h"
//}}AFX_INCLUDES
#if !defined(AFX_APIFORMVIEW_H__0784C1B6_A947_11D4_A50A_F77134B5463F__INCLUDED_)
#define AFX_APIFORMVIEW_H__0784C1B6_A947_11D4_A50A_F77134B5463F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// APIFormView.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAPIFormView form view

#ifndef __AFXEXT_H__
#include <afxext.h>
#endif

class CAPIFormView : public CFormView
{
protected:
	CAPIFormView();           // protected constructor used by dynamic creation
	DECLARE_DYNCREATE(CAPIFormView)

// Form Data
public:
	//{{AFX_DATA(CAPIFormView)
	enum { IDD = IDD_FORMVIEW_FORM };
	CMSFlexGrid	m_funcgrid;
	//}}AFX_DATA

// Attributes
public:
	CComboBox		m_combo;
	CSuperEdit		m_edit;

// Operations
public:
	void LoadGrid(CAPIFunctionInfo&   funcinfo);
	void GetGridData(CAPIFunctionInfo&   funcinfo);
	CString getVarCode(VARTYPE vtype);
	CString getElemCode(long vtype); 


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAPIFormView)
	public:
	virtual void OnInitialUpdate();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	virtual ~CAPIFormView();
	int m_lBorderWidth;
	int m_lBorderHeigth;
	int m_nLogX;
	int m_nLogY;

#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

public:
	// Generated message map functions
	//{{AFX_MSG(CAPIFormView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnLeaveCellMsflexgrid1();
	afx_msg void OnMouseDownMsflexgrid1(short Button, short Shift, long x, long y);
	afx_msg void OnFileOpen();
	afx_msg void OnFilePrint();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_APIFORMVIEW_H__0784C1B6_A947_11D4_A50A_F77134B5463F__INCLUDED_)
