// RVVPMDoc.h : interface of the CRVVPMDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_RVVPMDOC_H__D1BC505B_9703_11D3_8534_006097B6FED6__INCLUDED_)
#define AFX_RVVPMDOC_H__D1BC505B_9703_11D3_8534_006097B6FED6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

typedef	CArray<CPMColumn,CPMColumn&>	COLLIST;
typedef	CArray<CPMScrip*,CPMScrip*>		SCRIPLIST;

class CRVVPMHTMLView;
class CRVVPMView;


class CRVVPMDoc : public CDocument
{
protected: // create from serialization only
	CRVVPMDoc();
	DECLARE_DYNCREATE(CRVVPMDoc)

// Attributes
public:
	CPMDataSource*	m_pdatasrc;
	COLLIST			m_header;
	SCRIPLIST		m_currentlist;
	SCRIPLIST		m_scriplist;


// Operations
public:
	void FillData();
	void LoadPortfolio();

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CRVVPMDoc)
	public:
	virtual void Serialize(CArchive& ar);
	virtual void SetTitle(LPCTSTR lpszTitle);
	//}}AFX_VIRTUAL

// Implementation
public:
	CRVVPMHTMLView* m_pHTMLView;
	CRVVPMView* m_pListView;
	virtual ~CRVVPMDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CRVVPMDoc)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
	int GetColumnId(CString col);
};



/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_RVVPMDOC_H__D1BC505B_9703_11D3_8534_006097B6FED6__INCLUDED_)
