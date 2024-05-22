#if !defined(AFX_SUPEREDIT_H__768F56D3_B6AA_11D4_AA19_000629F55DEF__INCLUDED_)
#define AFX_SUPEREDIT_H__768F56D3_B6AA_11D4_AA19_000629F55DEF__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// SuperEdit.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CSuperEdit window

class CSuperEdit : public CEdit
{
// Construction
public:
	CSuperEdit();

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CSuperEdit)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CSuperEdit();

	// Generated message map functions
protected:
	//{{AFX_MSG(CSuperEdit)
	afx_msg void OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SUPEREDIT_H__768F56D3_B6AA_11D4_AA19_000629F55DEF__INCLUDED_)
