#if !defined(AFX_TRIGGERS_H__DF0DA441_B4E5_11D3_8A5B_444553540000__INCLUDED_)
#define AFX_TRIGGERS_H__DF0DA441_B4E5_11D3_8A5B_444553540000__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Triggers.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CTriggers dialog

class CTriggers : public CDialog
{
// Construction
public:
	CTriggers(CWnd* pParent = NULL);   // standard constructor
	CString m_szMasterFile;
	CString m_portfolio;
	CString m_symbol;


// Dialog Data
	//{{AFX_DATA(CTriggers)
	enum { IDD = IDD_TRIGGER };
	CString	m_highmvpts;
	CString	m_highpc;
	CString	m_highpr;
	CString	m_highpts;
	CString	m_lowmvpts;
	CString	m_lowpc;
	CString	m_lowpr;
	CString	m_lowpts;
	CString	m_bought;
	CString	m_paid;
	CString	m_highmvtds;
	CString	m_lowmvtds;
	CString	m_newspd;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTriggers)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CTriggers)
	virtual BOOL OnInitDialog();
	afx_msg void OnSelchangeCombo1();
	afx_msg void OnQmReset();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	void LoadPortfolio();
	void LoadSymbol();
	void SaveSymbol();

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TRIGGERS_H__DF0DA441_B4E5_11D3_8A5B_444553540000__INCLUDED_)
