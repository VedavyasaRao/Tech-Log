#if !defined(AFX_MAINTOOLBAR_H__8689FB61_AAD6_11D4_A50A_B5F002C9D03A__INCLUDED_)
#define AFX_MAINTOOLBAR_H__8689FB61_AAD6_11D4_A50A_B5F002C9D03A__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// MainToolbar.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CMainToolbar window

class CMainToolbar : public CToolBar
{
// Construction
public:
	CMainToolbar();
	void LoadFunctions();
// Attributes
public:
	CAPIFunctionInfo	m_apifuncinfo;
	// Operations

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainToolbar)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMainToolbar();

private:
	std::vector<SearchItem*> GetMemberInfo(InterfaceInfo *itfinfo);

	// Generated message map functions
protected:
	//{{AFX_MSG(CMainToolbar)
	afx_msg void OnDropdownCombo1();
	afx_msg void OnSelChangeCombo1();
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINTOOLBAR_H__8689FB61_AAD6_11D4_A50A_B5F002C9D03A__INCLUDED_)
