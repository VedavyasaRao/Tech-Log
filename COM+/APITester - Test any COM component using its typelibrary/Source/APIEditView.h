#if !defined(AFX_APIEDITVIEW_H__0784C1B3_A947_11D4_A50A_F77134B5463F__INCLUDED_)
#define AFX_APIEDITVIEW_H__0784C1B3_A947_11D4_A50A_F77134B5463F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// APIEditView.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CAPIEditView view

class CAPIEditView : public CEditView
{
protected:
	CAPIEditView();           // protected constructor used by dynamic creation
	DECLARE_DYNCREATE(CAPIEditView)

// Attributes
public:

// Operations
public:
	void Addtolog(CAPIFunctionInfo&  funcinfo);

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAPIEditView)
	public:
	virtual void OnInitialUpdate();
	afx_msg void OnFilePrint();
	protected:
	virtual void OnDraw(CDC* pDC);      // overridden to draw this view
	//}}AFX_VIRTUAL

// Implementation
protected:
	virtual ~CAPIEditView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

	// Generated message map functions
protected:
	//{{AFX_MSG(CAPIEditView)
	afx_msg void OnEditChange();
	afx_msg void OnEditReplace();
	afx_msg void OnCmdclear();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

	CString			m_dispdata[5000][50];
	int				m_rowcount;
	int				m_colcount;

public:
	void AddtoDispData(CAPIParameterInfo& paraminfo);
	void FormatArData(CString&  results); 
	void GetGCInfo(CString&  results, IDispatch* gocptr);//CAPIParameterInfo& paraminfo); 
	void IterateGC(CString tempini, CString secstr, IDispatch* gocptr);
	void SetGCInfo(COleVariant& var, CString&  gctextfile);
	void CreateGCfromfile(CString tempini, CString secstr, IDispatch** gcptr);

};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_APIEDITVIEW_H__0784C1B3_A947_11D4_A50A_F77134B5463F__INCLUDED_)
