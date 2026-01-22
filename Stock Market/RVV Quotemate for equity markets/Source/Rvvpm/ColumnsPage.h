#if !defined(AFX_COLUMNSPAGE_H__BEE4B6A5_B2B8_11D3_854F_006097B6FED6__INCLUDED_)
#define AFX_COLUMNSPAGE_H__BEE4B6A5_B2B8_11D3_854F_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ColumnsPage.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CColumnsPage dialog

class CColumnsPage : public CPropertyPage
{
	DECLARE_DYNCREATE(CColumnsPage)

// Construction
public:
	CColumnsPage();
	~CColumnsPage();

// Dialog Data
	//{{AFX_DATA(CColumnsPage)
	enum { IDD = IDD_PROPPAGE_COLUMNS };
		// NOTE - ClassWizard will add data members here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	BOOL	m_sizecols;
	//}}AFX_DATA


// Overrides
	// ClassWizard generate virtual function overrides
	//{{AFX_VIRTUAL(CColumnsPage)
	public:
	virtual void OnOK();
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	// Generated message map functions
	//{{AFX_MSG(CColumnsPage)
	virtual BOOL OnInitDialog();
	afx_msg void OnAdd();
	afx_msg void OnDown();
	afx_msg void OnRemove();
	afx_msg void OnUp();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	void LoadColumns();

private:
	IniFile*  m_pimmaster;

};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_COLUMNSPAGE_H__BEE4B6A5_B2B8_11D3_854F_006097B6FED6__INCLUDED_)
