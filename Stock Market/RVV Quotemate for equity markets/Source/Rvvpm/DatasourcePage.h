#if !defined(AFX_DATASOURCEPAGE_H__BEE4B6A6_B2B8_11D3_854F_006097B6FED6__INCLUDED_)
#define AFX_DATASOURCEPAGE_H__BEE4B6A6_B2B8_11D3_854F_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DatasourcePage.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CDatasourcePage dialog

class CDatasourcePage : public CPropertyPage
{
	DECLARE_DYNCREATE(CDatasourcePage)

// Construction
public:
	CDatasourcePage();
	~CDatasourcePage();

// Dialog Data
	//{{AFX_DATA(CDatasourcePage)
	enum { IDD = IDD_PROPPAGE_SOURCE };
	BOOL	m_autorefresh;
	int		m_datasrc;
	CString	m_passwd;
	CString	m_quote;
	CString	m_news;
	BOOL	m_enablerigger;
	//}}AFX_DATA


// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CDatasourcePage)
	public:
	virtual void OnOK();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	// Generated message map functions
	//{{AFX_MSG(CDatasourcePage)
	virtual BOOL OnInitDialog();
	afx_msg void OnSelchangeCombo1();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	CString GetPasscode(CString szBuf);
	void SetPasscode(CString szBuf, CString val);

private:
	IniFile         *m_pimDataSrcs;

};



//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DATASOURCEPAGE_H__BEE4B6A6_B2B8_11D3_854F_006097B6FED6__INCLUDED_)
