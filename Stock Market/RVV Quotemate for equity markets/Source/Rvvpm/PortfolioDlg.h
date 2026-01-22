#include "afxwin.h"
#if !defined(AFX_PORTFOLIODLG_H__BEE4B6A3_B2B8_11D3_854F_006097B6FED6__INCLUDED_)
#define AFX_PORTFOLIODLG_H__BEE4B6A3_B2B8_11D3_854F_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// PortfolioDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CPortfolioDlg dialog

class CPortfolioDlg : public CDialog
{
// Construction
public:
	CPortfolioDlg(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CPortfolioDlg)
	enum { IDD = IDD_PORTDLG };
	CString	m_Portfolio;
	CString	m_newTicker;
	BOOL m_dow;
	BOOL m_nasdaq;
	BOOL m_snp;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CPortfolioDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	afx_msg void OnAdd();
	afx_msg void OnDown();
	afx_msg void OnRemove();
	afx_msg void OnUp();
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CPortfolioDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSelchangeCombo1();
	afx_msg BOOL OnToolTipText(UINT, NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnSelendokCombo1();
	afx_msg void OnQmDeletepf();
	afx_msg void OnQmNewpf();
	afx_msg void OnQmSavepf();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	CToolBar    m_wndToolBar;
	IniFile         *m_pimPortfolios;
	CString	m_Symbols;

	void SetCurrent();
	void LoadPortfolios();
	void savePortfolio();

};


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PORTFOLIODLG_H__BEE4B6A3_B2B8_11D3_854F_006097B6FED6__INCLUDED_)
