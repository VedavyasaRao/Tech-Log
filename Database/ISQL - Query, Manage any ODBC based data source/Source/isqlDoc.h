// isqlDoc.h : interface of the CIsqlDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_ISQLDOC_H__D8B94E94_C07A_11D2_846D_006097B6FED6__INCLUDED_)
#define AFX_ISQLDOC_H__D8B94E94_C07A_11D2_846D_006097B6FED6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000


class CIsqlDoc : public CDocument
{
protected: // create from serialization only
	CIsqlDoc();
	DECLARE_DYNCREATE(CIsqlDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CIsqlDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	virtual void OnCloseDocument();
	protected:
	virtual BOOL SaveModified();
	//}}AFX_VIRTUAL

// Implementation
public:
	int m_startrow;
	CString m_sqlstmt;
	int m_bCanClose;
	virtual ~CIsqlDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CIsqlDoc)
		// NOTE - the ClassWizard will add and remove member functions here.
		//    DO NOT EDIT what you see in these blocks of generated code !
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_ISQLDOC_H__D8B94E94_C07A_11D2_846D_006097B6FED6__INCLUDED_)
