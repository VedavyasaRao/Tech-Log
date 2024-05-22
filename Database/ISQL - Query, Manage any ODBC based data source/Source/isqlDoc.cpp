// isqlDoc.cpp : implementation of the CIsqlDoc class
//

#include "stdafx.h"
#include "isql.h"
#include "isqlDoc.h"
#include "Childfrm.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CIsqlDoc

IMPLEMENT_DYNCREATE(CIsqlDoc, CDocument)

BEGIN_MESSAGE_MAP(CIsqlDoc, CDocument)
	//{{AFX_MSG_MAP(CIsqlDoc)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CIsqlDoc construction/destruction

CIsqlDoc::CIsqlDoc()
{
	// TODO: add one-time construction code here
	m_bCanClose = 0;
	m_startrow = 1;
	m_sqlstmt = "";
}

CIsqlDoc::~CIsqlDoc()
{
}

BOOL CIsqlDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CIsqlDoc serialization

void CIsqlDoc::Serialize(CArchive& ar)
{
	// CEditView contains an edit control which handles all serialization
	((CEditView*)m_viewList.GetHead())->SerializeRaw(ar);
}

/////////////////////////////////////////////////////////////////////////////
// CIsqlDoc diagnostics

#ifdef _DEBUG
void CIsqlDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CIsqlDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CIsqlDoc commands


void CIsqlDoc::OnCloseDocument() 
{
	// TODO: Add your specialized code here and/or call the base class
	if (m_bCanClose)
		return;
	
	CDocument::OnCloseDocument();
}


BOOL CIsqlDoc::SaveModified() 
{
	// TODO: Add your specialized code here and/or call the base class
	return 1;
	//return CDocument::SaveModified();
}
