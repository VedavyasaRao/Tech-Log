// RVVPMView.cpp : implementation of the CRVVPMView class
//

#include "stdafx.h"
#include "RVVPM.h"
#include "pmcolumn.h"
#include "pmscrip.h"
#include "datasrc.h"
#include "RVVPMDoc.h"
#include "RVVPMView.h"
#include "RVVPMHTMLView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CRVVPMView

IMPLEMENT_DYNCREATE(CRVVPMView, CFormView)

BEGIN_MESSAGE_MAP(CRVVPMView, CFormView)
	//{{AFX_MSG_MAP(CRVVPMView)
	ON_COMMAND(ID_PM_ONEDAY, OnPmOneday)
	ON_UPDATE_COMMAND_UI(ID_PM_ONEDAY, OnUpdatePmOneday)
	ON_COMMAND(ID_PM_NEWS, OnPmNews)
	ON_UPDATE_COMMAND_UI(ID_PM_NEWS, OnUpdatePmNews)
	ON_COMMAND(ID_PM_FIVEDAY, OnPmFiveday)
	ON_UPDATE_COMMAND_UI(ID_PM_FIVEDAY, OnUpdatePmFiveday)
	ON_COMMAND(ID_PM_MO, OnPmMo)
	ON_UPDATE_COMMAND_UI(ID_PM_MO, OnUpdatePmMo)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRVVPMView construction/destruction

CRVVPMView::CRVVPMView()
	: CFormView(CRVVPMView::IDD)
{
	//{{AFX_DATA_INIT(CRVVPMView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CRVVPMView::~CRVVPMView()
{
}

void CRVVPMView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRVVPMView)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}

BOOL CRVVPMView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CRVVPMView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	GetParentFrame()->RecalcLayout();
	ResizeParentToFit();
}

/////////////////////////////////////////////////////////////////////////////
// CRVVPMView printing

BOOL CRVVPMView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CRVVPMView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CRVVPMView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CRVVPMView::OnPrint(CDC* pDC, CPrintInfo* /*pInfo*/)
{
	// TODO: add customized printing code here
}

/////////////////////////////////////////////////////////////////////////////
// CRVVPMView diagnostics

#ifdef _DEBUG
void CRVVPMView::AssertValid() const
{
	CFormView::AssertValid();
}

void CRVVPMView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CRVVPMDoc* CRVVPMView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CRVVPMDoc)));
	return (CRVVPMDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CRVVPMView message handlers



void CRVVPMView::OnDraw(CDC* pDC) 
{
	// TODO: Add your specialized code here and/or call the base class
	CListCtrl *pList = (CListCtrl *)GetDlgItem(IDC_LIST1);
	CRect   rect;

	pList->GetWindowRect(&rect);
	GetClientRect(&rect);
	pList->SetWindowPos(NULL, 0, 0, rect.Width(), rect.Height(), 0);



}


void CRVVPMView::FillControl()
{
	CRVVPMDoc* pDoc = GetDocument();
	CListCtrl *pList = (CListCtrl *)GetDlgItem(IDC_LIST1);

	int		i, j;
	int		nColumnCount = pList->GetHeaderCtrl()->GetItemCount();
	CString str;

	for (i=0; i<pDoc->m_currentlist.GetSize(); ++i)
	{
		
		CPMScrip*  pscrip = pDoc->m_currentlist[i];
		for (j=0; j<nColumnCount; ++j)
		{
			str = pscrip->GetColumnValue(pDoc->m_header[j].m_id );
			pList->SetItemText(i, j, str);
		}
	}


}

int GetColWidth(int colid)
{
	CString str;
	str.LoadString (colid);
	str = str.Mid(str.Find("\t")+1);
	return atol(str);

}


void CRVVPMView::ResetControl()
{

	CRVVPMDoc*	pDoc = GetDocument();
	CListCtrl*	pList = (CListCtrl *)GetDlgItem(IDC_LIST1);

	int		i, j;

	pList->DeleteAllItems();
	int nColumnCount = pList->GetHeaderCtrl()->GetItemCount();

	for (i=0; i<nColumnCount; ++i) 
	{
		pList->DeleteColumn(0);
	}

	CString		str;

	for (i=0, j=0; i<pDoc->m_header.GetSize(); ++i) 
	{
	
		if ( pDoc->m_header[i].m_show )
		{
			str = pDoc->m_header[i].m_value;
			pList->InsertColumn(j++, str, LVCFMT_RIGHT, GetColWidth(pDoc->m_header[i].m_id) ); //pList->GetStringWidth(str) +15 );
		}

	}

	for (i=0; i<pDoc->m_currentlist.GetSize(); ++i)
	{
		
		pList->InsertItem(i, "");
	}
	
}


void CRVVPMView::OnPmOneday() 
{
	// TODO: Add your command handler code here
	CRVVPMDoc*	pDoc = GetDocument();
	CString str = GetSymbol();
	if (str != "")
	{
		pDoc->m_pHTMLView->Navigate2(_T("http://i1chart.yahoo.com/b?s=" + str),NULL,NULL);

	}


	
		
}

void CRVVPMView::OnUpdatePmOneday(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable();
//http://biz.yahoo.com/c/u.html	
}

void CRVVPMView::OnPmNews() 
{
	// TODO: Add your command handler code here
	CRVVPMDoc*	pDoc = GetDocument();
	CString str = GetSymbol();
	if (str != "")
	{
		str = CString("http://biz.yahoo.com/n/") + CString(str.Mid(0,1)) + CString("/") + str + CString(".html");
		pDoc->m_pHTMLView->Navigate2(str, NULL, NULL);

	}

}

void CRVVPMView::OnUpdatePmNews(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable();
	
}

void CRVVPMView::OnPmFiveday() 
{
	// TODO: Add your command handler code here
	CRVVPMDoc*	pDoc = GetDocument();
	CString str = GetSymbol();
	if (str != "")
	{
		pDoc->m_pHTMLView->Navigate2(_T("http://i1chart.yahoo.com/w?s=" + str),NULL,NULL);
	}
}

void CRVVPMView::OnUpdatePmFiveday(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable();
}

CString CRVVPMView::GetSymbol()
{
	CListCtrl*	pList = (CListCtrl *)GetDlgItem(IDC_LIST1);

	CString str;
	POSITION pos = pList->GetFirstSelectedItemPosition();
	if (pos != NULL)
	{
		int nItem = pList->GetNextSelectedItem(pos);
		str = pList->GetItemText(nItem, 0);		
	}
	
	return str;


}

void CRVVPMView::OnPmMo() 
{
	// TODO: Add your command handler code here
	CRVVPMDoc*	pDoc = GetDocument();
	CString str = GetSymbol();
	if (str != "")
	{
		str = CString("http://chart.yahoo.com/c/3m/") + CString(str.Mid(0,1)) + CString("/") + str + CString(".gif");
		str.MakeLower();
		pDoc->m_pHTMLView->Navigate2(str, NULL, NULL);


	}
	
}

void CRVVPMView::OnUpdatePmMo(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	
}
