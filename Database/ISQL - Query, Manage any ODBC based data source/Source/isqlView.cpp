// isqlView.cpp : implementation of the CIsqlView class
//

#include "stdafx.h"
#include "isql.h"

#include "isqlDoc.h"
#include "isqlView.h"
#include "io.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CIsqlView

IMPLEMENT_DYNCREATE(CIsqlView, CEditView)

BEGIN_MESSAGE_MAP(CIsqlView, CEditView)
	//{{AFX_MSG_MAP(CIsqlView)
	ON_CONTROL_REFLECT(EN_UPDATE, OnUpdate)
	ON_COMMAND(ID_EDIT_FIND, OnEditFind)
	ON_UPDATE_COMMAND_UI(ID_EDIT_FIND, OnUpdateEditFind)
	ON_COMMAND(ID_MOREROWS, OnMorerows)
	ON_UPDATE_COMMAND_UI(ID_MOREROWS, OnUpdateMorerows)
	ON_WM_LBUTTONDBLCLK()
	ON_COMMAND(ID_EDIT_CLEAR_ALL, OnEditClearAll)
	ON_WM_DESTROY()
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CEditView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CEditView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CEditView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CIsqlView construction/destruction

CIsqlView::CIsqlView()
{
	// TODO: add construction code here

}

CIsqlView::~CIsqlView()
{
}

BOOL CIsqlView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	BOOL bPreCreated = CEditView::PreCreateWindow(cs);
	//cs.style &= ~(ES_AUTOHSCROLL|WS_HSCROLL);	// Enable word-wrapping

	return bPreCreated;
}

/////////////////////////////////////////////////////////////////////////////
// CIsqlView drawing

void CIsqlView::OnDraw(CDC* pDC)
{
	CIsqlDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);

	// TODO: add draw code for native data here
}

/////////////////////////////////////////////////////////////////////////////
// CIsqlView printing

BOOL CIsqlView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default CEditView preparation
	return CEditView::OnPreparePrinting(pInfo);
}

void CIsqlView::OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo)
{
	// Default CEditView begin printing.
	CEditView::OnBeginPrinting(pDC, pInfo);
}

void CIsqlView::OnEndPrinting(CDC* pDC, CPrintInfo* pInfo)
{
	// Default CEditView end printing
	CEditView::OnEndPrinting(pDC, pInfo);
}

/////////////////////////////////////////////////////////////////////////////
// CIsqlView diagnostics

#ifdef _DEBUG
void CIsqlView::AssertValid() const
{
	CEditView::AssertValid();
}

void CIsqlView::Dump(CDumpContext& dc) const
{
	CEditView::Dump(dc);
}

CIsqlDoc* CIsqlView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CIsqlDoc)));
	return (CIsqlDoc*)m_pDocument;
}
#endif //_DEBUG


void CIsqlView::OnUpdate() 
{
	// TODO: If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CEditView::OnInitDialog()
	// function to send the EM_SETEVENTMASK message to the control
	// with the ENM_UPDATE flag ORed into the lParam mask.
	
	// TODO: Add your control notification handler code here
	
}


void CIsqlView::OnEditFind() 
{
	// TODO: Add your command handler code here
	CEditView::OnEditReplace();	
}

void CIsqlView::OnUpdateEditFind(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here

	
}

void CIsqlView::OnMorerows() 
{
	// TODO: Add your command handler code here
	SetWindowText("");
	((CIsqlApp*)AfxGetApp())->GetMoreRows(this);
	
}

void CIsqlView::OnUpdateMorerows(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable(!(((CIsqlDoc*)GetDocument())->m_sqlstmt == ""));
}

void CIsqlView::OnLButtonDblClk(UINT nFlags, CPoint point) 
{
	// TODO: Add your message handler code here and/or call default
	
	CString buf;

	GetWindowText(buf);
	int i, ps, p, pe, len;
	p = LOWORD(GetEditCtrl().CharFromPos(point));
	len = buf.GetLength();
	if (p == len)
		return;
	for (i=p; i>0; --i)
		if (buf.GetAt(i) == '\r')
			break;
	ps = i;
	for (i=p; i<len; ++i)
		if (buf.GetAt(i) == '\r')
			break;
	pe = i;
	GetEditCtrl().SetSel(ps, pe, FALSE);



	//CEditView::OnLButtonDblClk(nFlags, point);
}



void CIsqlView::OnEditClearAll() 
{
	// TODO: Add your command handler code here
	SetWindowText("");
	
}

void CIsqlView::OnInitialUpdate() 
{
	CEditView::OnInitialUpdate();
	
	// TODO: Add your specialized code here and/or call the base class
	if( (_access( "c:\\gg.txt", 0 )) == -1 )
		return;
   

	GetDocument()->OnOpenDocument("c:\\gg.txt");
}


void CIsqlView::OnDestroy() 
{
	CEditView::OnDestroy();
	
	// TODO: Add your message handler code here
	GetDocument( )->DoSave("c:\\gg.txt");

}
