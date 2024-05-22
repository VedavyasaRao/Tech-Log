#if !defined(AFX_BOOKMARKDLG_H__50C4F273_BA3D_11D3_8561_006097B6FED6__INCLUDED_)
#define AFX_BOOKMARKDLG_H__50C4F273_BA3D_11D3_8561_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// BookmarkDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CBookmarkDlg dialog

class CBookmarkDlg : public CDialog
{
// Construction
public:
	CBookmarkDlg(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CBookmarkDlg)
	enum { IDD = IDD_URLS };
	CString	m_bookmark;
	CString	m_address;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CBookmarkDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	// Generated message map functions
	//{{AFX_MSG(CBookmarkDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSelchangeCombo1();
	afx_msg BOOL OnToolTipText(UINT, NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnQmNewbm();
	afx_msg void OnQmSavebm();
	afx_msg void OnQmDeletebm();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

private:
	CToolBar    m_wndToolBar;
	void LoadURLs();
	void SetCurrent();
	void saveURL();


	IniFile*  m_pimmaster;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BOOKMARKDLG_H__50C4F273_BA3D_11D3_8561_006097B6FED6__INCLUDED_)
