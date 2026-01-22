#if !defined(AFX_OTHERTRIGGERPAGE_H__72127242_B8D6_11D3_8A5B_E7A7BDCE604D__INCLUDED_)
#define AFX_OTHERTRIGGERPAGE_H__72127242_B8D6_11D3_8A5B_E7A7BDCE604D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// OtherTriggerPage.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// COtherTriggerPage dialog

class COtherTriggerPage : public CPropertyPage
{
	DECLARE_DYNCREATE(COtherTriggerPage)

// Construction
public:
	COtherTriggerPage();
	~COtherTriggerPage();

// Dialog Data
	//{{AFX_DATA(COtherTriggerPage)
	enum { IDD = IDD_PROPAGE_OTRTRIGGER };
	CString	m_highmvpts;
	CString	m_highpc;
	CString	m_highpts;
	CString	m_lowpc;
	CString	m_lowpts;
	CString	m_highmvtds;
	CString	m_lowmvpts;
	CString	m_lowmvtds;
	CString	m_highmvprc;
	CString	m_highmvtdsprc;
	CString	m_lowmvprc;
	CString	m_lowmvtdsprc;
	CString	m_newspd;
	BOOL	m_clearnews;
	//}}AFX_DATA


// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(COtherTriggerPage)
	public:
	virtual void OnOK();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	// Generated message map functions
	//{{AFX_MSG(COtherTriggerPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnReset();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	IniFile*  m_pimmaster;
	void LoadTriggers();
	void SaveTriggers();

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_OTHERTRIGGERPAGE_H__72127242_B8D6_11D3_8A5B_E7A7BDCE604D__INCLUDED_)
