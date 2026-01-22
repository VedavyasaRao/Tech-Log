// PMListView.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"

#include "PMListView.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CPMListView

IMPLEMENT_DYNCREATE(CPMListView, CListViewEx)

CPMListView::CPMListView()
{
}

CPMListView::~CPMListView()
{
}


BEGIN_MESSAGE_MAP(CPMListView, CListViewEx)
	//{{AFX_MSG_MAP(CPMListView)
	ON_NOTIFY_REFLECT(NM_CLICK, OnClick)
	ON_NOTIFY_REFLECT(LVN_COLUMNCLICK, OnColumnclick)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CPMListView drawing


/////////////////////////////////////////////////////////////////////////////
// CPMListView diagnostics

#ifdef _DEBUG
void CPMListView::AssertValid() const
{
	CListViewEx::AssertValid();
}

void CPMListView::Dump(CDumpContext& dc) const
{
	CListViewEx::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CPMListView message handlers


void CPMListView::FillControl()
{
	CRVVPMApp* pApp = (CRVVPMApp*)AfxGetApp();
	CListCtrl *pList = &GetListCtrl();

	int		i, j;
	int		nColumnCount = pList->GetHeaderCtrl()->GetItemCount();
	CString str;

	for (i=0; i<pApp->m_currentlist.GetSize(); ++i)
	{
		
		CPMScrip*  pscrip = pApp->m_currentlist[i];
		for (j=0; j<nColumnCount; ++j)
		{
			str = pscrip->GetColumnValue(pApp->m_header[j].m_id );
			pList->SetItemText(i, j, str);
		}
	}


}

int CPMListView::GetColumnWidth(int ncol)
{
	CListCtrl *pList = &GetListCtrl();
	return pList->GetColumnWidth(ncol);
}

int CPMListView::GetColWidth(int colid)
{
	CString str;
	str.LoadString (colid);
	str = str.Mid(str.Find("\t")+1);
	return atol(str);

}


void CPMListView::ResetControl()
{

	CRVVPMApp* pApp = (CRVVPMApp*)AfxGetApp();
	CListCtrl *pList = &GetListCtrl();

	int		i, j;

	pList->DeleteAllItems();
	int nColumnCount = pList->GetHeaderCtrl()->GetItemCount();

	for (i=0; i<nColumnCount; ++i) 
	{
		pList->DeleteColumn(0);
	}

	CString		str;

	for (i=0, j=0; i<pApp->m_header.GetSize(); ++i) 
	{
	
		if ( pApp->m_header[i].m_show )
		{
			str = pApp->m_header[i].m_value;
			pList->InsertColumn(j++, str, LVCFMT_RIGHT, GetColWidth(pApp->m_header[i].m_id) );
		}

	}

	for (i=0; i<pApp->m_currentlist.GetSize(); ++i)
	{
		
		pList->InsertItem(i, "");
	}
	
}


BOOL CPMListView::PreCreateWindow(CREATESTRUCT& cs) 
{
	// TODO: Add your specialized code here and/or call the base class
	cs.style |= LVS_SHOWSELALWAYS | LVS_REPORT;

	CListCtrl& theCtrl = GetListCtrl();
	
	
	return CListViewEx::PreCreateWindow(cs);
}

CString CPMListView::GetExchange()
{
	CListCtrl *pList = &GetListCtrl();

	CString str;
	POSITION pos = pList->GetFirstSelectedItemPosition();
	if (pos != NULL)
	{
		int nItem = pList->GetNextSelectedItem(pos);
		str = pList->GetItemText(nItem, 0);		
	}
	
	return str;
}

CString CPMListView::GetSymbol()
{
	CListCtrl *pList = &GetListCtrl();

	CString str;
	POSITION pos = pList->GetFirstSelectedItemPosition();
	if (pos != NULL)
	{
		int nItem = pList->GetNextSelectedItem(pos);
		str = pList->GetItemText(nItem, 1);
	}

	return str;
}


void CPMListView::OnClick(NMHDR* pNMHDR, LRESULT* pResult) 
{
	// TODO: Add your control notification handler code here

	((CMainFrame*)AfxGetMainWnd())->LoadHtmlPane();
	
	*pResult = 0;
}


void CPMListView::OnInitialUpdate() 
{
	CListViewEx::OnInitialUpdate();
	
	// TODO: Add your specialized code here and/or call the base class
	GetListCtrl().SendMessage(WM_SETFONT, (DWORD)::CreateFont( 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New"), MAKELPARAM(1,0));
	GetListCtrl().GetHeaderCtrl()->SendMessage(WM_SETFONT, (DWORD)::CreateFont( 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "	Lucinda"), MAKELPARAM(1,0));
}


void CPMListView::OnColumnclick(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_LISTVIEW* pNMListView = (NM_LISTVIEW*)pNMHDR;
	// TODO: Add your control notification handler code here
	CRVVPMApp* pApp = (CRVVPMApp*)AfxGetApp();


	int i;
	for ( i=0; i<pApp->m_header.GetSize(); ++i)
	{
		if (pApp->m_header[i].m_sort != 0)
			break;
	}

	if (i < pApp->m_header.GetSize() && pNMListView->iSubItem != i )
		pApp->m_header[i].m_sort = 0;

	pApp->m_header[pNMListView->iSubItem].m_sort++;
	if (pApp->m_header[pNMListView->iSubItem].m_sort == 3)
		pApp->m_header[pNMListView->iSubItem].m_sort = 1;
	
	pApp->AddJob(CQMJob::Sort);
	
	*pResult = 0;
}


