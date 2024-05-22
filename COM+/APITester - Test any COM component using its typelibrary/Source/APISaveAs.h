#if !defined(AFX_APISAVEAS_H__64BE0CB9_831E_4F3B_A7BD_E4AA4E340F3B__INCLUDED_)
#define AFX_APISAVEAS_H__64BE0CB9_831E_4F3B_A7BD_E4AA4E340F3B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// APISaveAs.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAPISaveAs dialog

class CAPISaveAs : public CDialog
{
// Construction
public:
	CAPISaveAs(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CAPISaveAs)
	enum { IDD = IDD_SAVEDLG };
	CString	m_logfile;
	CString	m_prototype;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAPISaveAs)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CAPISaveAs)
	afx_msg void OnFuncfilebtn();
	afx_msg void OnLogfilebtn();
	virtual void OnOK();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_APISAVEAS_H__64BE0CB9_831E_4F3B_A7BD_E4AA4E340F3B__INCLUDED_)
