#if !defined(AFX_APITESTERPRINTDLG_H__867FA6BA_C378_4055_9D1A_8D6E50C5F1B8__INCLUDED_)
#define AFX_APITESTERPRINTDLG_H__867FA6BA_C378_4055_9D1A_8D6E50C5F1B8__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// APITesterPrintDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAPITesterPrintDlg dialog

class CAPITesterPrintDlg : public CDialog
{
// Construction
public:
	CAPITesterPrintDlg(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CAPITesterPrintDlg)
	enum { IDD = IDD_APIPRINTDIALOG };
	BOOL	m_funcdef;
	BOOL	m_logfile;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAPITesterPrintDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CAPITesterPrintDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_APITESTERPRINTDLG_H__867FA6BA_C378_4055_9D1A_8D6E50C5F1B8__INCLUDED_)
