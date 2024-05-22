#if !defined(AFX_CONNECTDSN_H__D8B94EB4_C07A_11D2_846D_006097B6FED6__INCLUDED_)
#define AFX_CONNECTDSN_H__D8B94EB4_C07A_11D2_846D_006097B6FED6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000
// ConnectDSN.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CConnectDSN dialog

class CConnectDSN : public CDialog
{
// Construction
public:
	HDBC m_hdbc;
	CConnectDSN(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CConnectDSN)
	enum { IDD = IDD_CONNECTTODSN };
	CString	m_dsn;
	CString	m_passwd;
	CString	m_userid;
	BOOL	m_chkpwd;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CConnectDSN)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CConnectDSN)
	virtual void OnOK();
	afx_msg void OnDsnbtn();
	virtual void OnCancel();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_CONNECTDSN_H__D8B94EB4_C07A_11D2_846D_006097B6FED6__INCLUDED_)
