#if !defined(AFX_SYMBOLTRIGGERPAGE_H__72127241_B8D6_11D3_8A5B_E7A7BDCE604D__INCLUDED_)
#define AFX_SYMBOLTRIGGERPAGE_H__72127241_B8D6_11D3_8A5B_E7A7BDCE604D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// SymbolTriggerPage.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CSymbolTriggerPage dialog

class CSymbolTriggerPage : public CPropertyPage
{
	DECLARE_DYNCREATE(CSymbolTriggerPage)

// Construction
public:
	CSymbolTriggerPage();
	~CSymbolTriggerPage();
	CString m_fullsymbol;

// Dialog Data
	//{{AFX_DATA(CSymbolTriggerPage)
	enum { IDD = IDD_TRIGGER};
	CString	m_bought;
	CString	m_highpr;
	CString	m_lowpr;
	CString	m_paid;
	//}}AFX_DATA


// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CSymbolTriggerPage)
	public:
	virtual void OnOK();
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	// Generated message map functions
	//{{AFX_MSG(CSymbolTriggerPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnResetNew();
	afx_msg void OnSelchangeCombo1();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	IniFile         *m_pimSymbdata;
	IniFile         *m_pimPortfolios;
	CString m_portfolio;
	void LoadPortfolio();
	void LoadSymbol();
	void SaveSymbol();

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SYMBOLTRIGGERPAGE_H__72127241_B8D6_11D3_8A5B_E7A7BDCE604D__INCLUDED_)
