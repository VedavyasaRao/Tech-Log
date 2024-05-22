#if !defined(AFX_SOURCEPAGE_H__6C0B8FD4_9958_11D3_8539_006097B6FED6__INCLUDED_)
#define AFX_SOURCEPAGE_H__6C0B8FD4_9958_11D3_8539_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// SourcePage.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CSourcePage dialog

class CSourcePage : public CPropertyPage
{
	DECLARE_DYNCREATE(CSourcePage)

// Construction
public:
	CSourcePage();
	~CSourcePage();
	CString m_szMasterFile;

// Dialog Data
	//{{AFX_DATA(CSourcePage)
	enum { IDD = IDD_PROPPAGE_SOURCE };
	BOOL	m_autorefresh;
	int		m_datasrc;
	CString	m_news;
	CString	m_quote;
	CString	m_passwd;
	CString	m_userid;
	//}}AFX_DATA


// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CSourcePage)
	public:
	virtual void OnOK();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	// Generated message map functions
	//{{AFX_MSG(CSourcePage)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SOURCEPAGE_H__6C0B8FD4_9958_11D3_8539_006097B6FED6__INCLUDED_)
