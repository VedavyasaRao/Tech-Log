// RVVPMHTMLView.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "RVVPMHTMLView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CRVVPMHTMLView

IMPLEMENT_DYNCREATE(CRVVPMHTMLView, CHtmlView)

CRVVPMHTMLView::CRVVPMHTMLView()
{
	//{{AFX_DATA_INIT(CRVVPMHTMLView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}

CRVVPMHTMLView::~CRVVPMHTMLView()
{
}

void CRVVPMHTMLView::DoDataExchange(CDataExchange* pDX)
{
	CHtmlView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CRVVPMHTMLView)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CRVVPMHTMLView, CHtmlView)
	//{{AFX_MSG_MAP(CRVVPMHTMLView)
		// NOTE - the ClassWizard will add and remove mapping macros here.
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRVVPMHTMLView diagnostics

#ifdef _DEBUG
void CRVVPMHTMLView::AssertValid() const
{
	CHtmlView::AssertValid();
}

void CRVVPMHTMLView::Dump(CDumpContext& dc) const
{
	CHtmlView::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CRVVPMHTMLView message handlers

void CRVVPMHTMLView::OnInitialUpdate() 
{
	//TODO: This code navigates to a popular spot on the web.
	//Change the code to go where you'd like.
	//Navigate2(_T("http://i1chart.yahoo.com/b?s=arba"),NULL,NULL);
}
