#if !defined(AFX_COMPONENTDLG_H__D28C6403_19E3_438F_845D_600269DBBB4F__INCLUDED_)
#define AFX_COMPONENTDLG_H__D28C6403_19E3_438F_845D_600269DBBB4F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ComponentDlg.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CComponentDlg dialog


class CComponentDlg : public CDialog
{

// Construction
public:
	CComponentDlg(CWnd* pParent = NULL);   // standard constructor
// Dialog Data
	//{{AFX_DATA(CComponentDlg)
	enum { IDD = IDD_COMPDIALOG };
	CListBox	m_complist;
	CListBox	m_applist;
	CListBox	m_itflist;
	CString		m_progid;
	int			m_selcomp;
	int			m_selitf;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CComponentDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CComponentDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSelchangeApplist();
	virtual void OnOK();
	afx_msg void OnSelchangeComplist();
	afx_msg void OnDblclkComplist();
	afx_msg void OnLoadTypelib();
	afx_msg void OnClearLists();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()


private:
	void OpenTypeLib(OLECHAR  *sztlib);
	void Cleanup();
	ProgidCoClassname GetCoClassname(CString progidstr);
	void docleanlists();
	void LoadComponents(CString progidstr);
	int GetMemberInfo(InterfaceInfo *itfinfo);
	std::vector<InterfaceInfo*> GetItfsWithMembers(Interfaces *itfs);
	void LoadInterfaces(std::vector<InterfaceInfo*> itfvec);
private:
	
	ITypeLib	*m_ptlib = NULL;
	_TypeLibInfo *m_ptyplibinfo = NULL;
	CTLIApplication m_tliapp;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_COMPONENTDLG_H__D28C6403_19E3_438F_845D_600269DBBB4F__INCLUDED_)
