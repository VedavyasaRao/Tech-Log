// RVVPMDoc.cpp : implementation of the CRVVPMDoc class
//

#include "stdafx.h"
#include "RVVPM.h"
#include "pmcolumn.h"
#include "pmscrip.h"
#include "datasrc.h"
#include "RVVPMDoc.h"
#include "RVVPMView.h"
#include "RVVPMHTMLView.h"
#include "mainfrm.h"



#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CRVVPMDoc

IMPLEMENT_DYNCREATE(CRVVPMDoc, CDocument)

BEGIN_MESSAGE_MAP(CRVVPMDoc, CDocument)
	//{{AFX_MSG_MAP(CRVVPMDoc)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CRVVPMDoc construction/destruction

CRVVPMDoc::CRVVPMDoc()
{
	// TODO: add one-time construction code here
	m_pdatasrc = NULL;
}

CRVVPMDoc::~CRVVPMDoc()
{

}

void CRVVPMDoc::LoadPortfolio()
{

	char szBuffer[5000], *pprtptr = szBuffer;
	
	m_header.RemoveAll();

	CString szMasterFile ;
	szMasterFile = ((CRVVPMApp *)AfxGetApp())->m_szMasterFile;

	memset(szBuffer, 0, sizeof szBuffer);
	GetPrivateProfileSection( "Columns", szBuffer,  sizeof szBuffer, szMasterFile );
	char *cptr, szbuf[50];
	while (strlen(pprtptr) )
	{
		
		lstrcpy(szbuf, pprtptr);
		cptr = strchr(szbuf, '=');
		*cptr = 0;
		m_header.Add(CPMColumn(GetColumnId(szbuf), szbuf, *(cptr + 1) - 48 )); 
		pprtptr += strlen(pprtptr) + 1;
	}



	char szCurrent[100];

	delete m_pdatasrc;
	GetPrivateProfileString("Settings", "DataSource", "Yahoo", szCurrent, sizeof szCurrent, szMasterFile );
	if (CString(szCurrent) == CString("Yahoo") )
	{
		m_pdatasrc = new CYahooSource();
	}

	if (CString(szCurrent) == CString("WaterHouse") )
	{
		m_pdatasrc = new CTDWSource();
	}

	if (CString(szCurrent) == CString("RagingQuotes") )
	{
		m_pdatasrc = new CRBSource();
	}

	m_currentlist.RemoveAll();

	GetPrivateProfileString("Settings", "Current", "", szCurrent, sizeof szCurrent, szMasterFile );
	memset(szBuffer, 0, sizeof szBuffer);
	GetPrivateProfileSection( szCurrent, szBuffer,  sizeof szBuffer, szMasterFile );
	pprtptr = szBuffer;
	while (strlen(pprtptr) )
	{
		
		CPMScrip*	pscrip;

		for (int i=0; i<m_scriplist.GetSize(); ++i)
			if (m_scriplist[i]->m_symbol == CString(pprtptr))
				break;

		if (i == m_scriplist.GetSize())
		{
			pscrip = new CPMScrip(pprtptr);
			m_scriplist.Add( pscrip );
			m_pdatasrc->QueryQuotes(pscrip);

		}
		else
		{
			pscrip = m_scriplist[i];
		}

		m_currentlist.Add( pscrip );

		pprtptr += strlen(pprtptr) + 1;
	}

	m_pListView->ResetControl();

}


void CRVVPMDoc::FillData()
{
	for (int i=0; i<m_currentlist.GetSize(); ++i)
	{
		m_pdatasrc->QueryQuotes(m_currentlist[i]);
	}

	m_pListView->FillControl();
}


/////////////////////////////////////////////////////////////////////////////
// CRVVPMDoc serialization

void CRVVPMDoc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
		// TODO: add storing code here
	}
	else
	{
		// TODO: add loading code here
	}
}

/////////////////////////////////////////////////////////////////////////////
// CRVVPMDoc diagnostics

#ifdef _DEBUG
void CRVVPMDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CRVVPMDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CRVVPMDoc commands


void CRVVPMDoc::SetTitle(LPCTSTR lpszTitle) 
{
	// TODO: Add your specialized code here and/or call the base class
	CString szMasterFile ;
	szMasterFile = ((CRVVPMApp *)AfxGetApp())->m_szMasterFile;
	char szCurrent[100];
	GetPrivateProfileString("Settings", "Current", "", szCurrent, sizeof szCurrent, szMasterFile );
	
	CDocument::SetTitle(szCurrent);
}

int CRVVPMDoc::GetColumnId(CString col)
{
	CString str;
	for (int i=PM_FIRSTCOLUMN+1; i < PM_LASTCOLUMN; ++i)
	{
		str.LoadString(i);
		str = str.Mid(0, str.Find("\t"));
		if (str == col)
			return i;

	}

	return -1;
}

