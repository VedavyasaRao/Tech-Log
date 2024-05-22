// MainFrm.h : interface of the CMainFrame class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MAINFRM_H__0784C1A9_A947_11D4_A50A_F77134B5463F__INCLUDED_)
#define AFX_MAINFRM_H__0784C1A9_A947_11D4_A50A_F77134B5463F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "apiformView.h"
#include "apieditView.h"
#include "maintoolbar.h"


class CMainFrame : public CFrameWnd
{
	
public:
	CMainFrame();
protected: 
	DECLARE_DYNAMIC(CMainFrame)

// Attributes
public:
	CAPIFormView	*m_papiformview;
	CAPIEditView	*m_papieditview;
	IDispatch		*m_pdisp;
	CString			m_coclassguidstr;
	int				m_compidx;
	int				m_itfidx;
	std::vector<CAPIFunctionInfo> m_executedapilist;

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CMainFrame)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual BOOL OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CMainFrame();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

public:  // control bar embedded members
	CStatusBar		m_wndStatusBar;
	CMainToolbar	m_wndToolBar;
	CSplitterWnd	m_wndSplitter;
	CComboBox		m_cmbFuncHistory;
	CComboBox		m_cmbProgid1;
	CComboBox		m_cmbFuncs;
	InterfaceInfo	*pm_IntInfo;
	int				m_logview;

// Generated message map functions
protected:
	//{{AFX_MSG(CMainFrame)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnCmdexecute();
	afx_msg void OnCmdlog();
	afx_msg void OnUpdateCmdlog(CCmdUI* pCmdUI);
	afx_msg void OnCmdprogids();
	afx_msg void OnFileSaveAs();
	afx_msg void OnClose();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

//private:
public:
	void SaveFunction(const CAPIFunctionInfo&  funcinfo);
	void Invoke(CAPIFunctionInfo&  funcinfo);
	void SetArgument(COleVariant& var, int ldatatype, int loptflag, CString strdata);
	void SetArrayArgument(COleVariant& varar, int ldatatype, CString strdata);
	void GetArrayArgument(COleVariant& var, CAPIParameterInfo& paraminfo);
	void SortIniFile();
	IDispatch *CreateComponent(CString clsidstr);
	bool compareapi(CAPIFunctionInfo  func, CAPIFunctionInfo  func2);
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINFRM_H__0784C1A9_A947_11D4_A50A_F77134B5463F__INCLUDED_)
