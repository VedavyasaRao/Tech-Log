// MainToolbar.cpp : implementation file
//

#include "stdafx.h"
#include "combaseapi.h"
#include "combaseapi.h"
#include "APITester.h"
#include "MainFrm.h"
#include "MainToolbar.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMainToolbar

CMainToolbar::CMainToolbar()
{
}

CMainToolbar::~CMainToolbar()
{
}


BEGIN_MESSAGE_MAP(CMainToolbar, CToolBar)
	//{{AFX_MSG_MAP(CMainToolbar)
	ON_CBN_DROPDOWN(IDC_COMBO1, OnDropdownCombo1)
	ON_CBN_SELENDOK(IDC_COMBO1, OnSelChangeCombo1)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CMainToolbar message handlers

std::vector<SearchItem*> CMainToolbar::GetMemberInfo(InterfaceInfo *itfinfo)
{
	std::vector<SearchItem*> memvec;

	Members		*pmembers;
	HRESULT hr = itfinfo->get_Members(&pmembers);
	if (FAILED(hr))
		return memvec;

	SearchResults  *psrchresults;
	hr = pmembers->get_GetFilteredMembers(VARIANT_FALSE, &psrchresults);
	if (FAILED(hr))
		return memvec;

	long lcount;
	hr = psrchresults->get_Count(&lcount);
	if (FAILED(hr))
		return memvec;

	for (int i = 0; i < lcount; ++i)
	{
		SearchItem *psrchitm;

		hr = psrchresults->get_Item(i+1, &psrchitm);
		if (FAILED(hr))
			return memvec;
		memvec.push_back(psrchitm);
	}
	return memvec;
}

void CMainToolbar::OnDropdownCombo1() 
{
	// TODO: Add your control notification handler code here
	CComboBox  *pcmbfunc = (CComboBox  *)GetDlgItem(IDC_COMBO1);
	int compidx = ((CMainFrame*)AfxGetMainWnd())->m_compidx;
	int itfidx = ((CMainFrame*)AfxGetMainWnd())->m_itfidx;

	pcmbfunc->ResetContent();
	LoadFunctions();
	if (compidx == -1)
		return;

	HRESULT hr;
	std::vector<SearchItem*> memvec = GetMemberInfo(g_coclasses[compidx].pclassinfovec[itfidx]);

	int i, j = pcmbfunc->GetCount(), lcount = memvec.size();

	for (i=0; i<lcount; ++i, j++) 
	{
		BSTR bstrname;
		hr  = memvec[i]->get_Name(&bstrname);
		if (FAILED(hr))
			return ;

		pcmbfunc->AddString(CString(bstrname));
		long memberid = 0;
		hr  = memvec[i]->get_MemberId(&memberid);
		if (FAILED(hr))
			return ;
		pcmbfunc->SetItemData( j, memberid);
	}

}

void CMainToolbar::OnSelChangeCombo1() 
{
	// TODO: Add your control notification handler code here
	CComboBox  *pcmbfunc = (CComboBox  *)GetDlgItem(IDC_COMBO1);

	((CFrameWnd*)AfxGetMainWnd())->SetMessageText("Retriving function parameter information...");
	long itmdata = pcmbfunc->GetItemData(pcmbfunc->GetCurSel());
	if (itmdata == -1)
	{ 
		std::vector<CAPIFunctionInfo>& m_executedapilist = ((CMainFrame*)AfxGetMainWnd())->m_executedapilist;
		m_apifuncinfo = m_executedapilist[pcmbfunc->GetCurSel()];
	}
	else
	{
		HRESULT   hr;
		int compidx = ((CMainFrame*)AfxGetMainWnd())->m_compidx;
		int itfidx = ((CMainFrame*)AfxGetMainWnd())->m_itfidx;

		MemberInfo		*pmeminfo;
		Parameters		*pparams;
		ParameterInfo	*pparaminfo;

		auto progidstr = g_coclasses[compidx].progid;
		auto coclassguidstr = g_coclasses[compidx].coclassguidstr;
		auto pitfinfo = g_coclasses[compidx].pclassinfovec[itfidx];

		hr = pitfinfo->get_GetMember(_variant_t(itmdata).Detach(), &pmeminfo);
		if (FAILED(hr))
			return;

		hr = pmeminfo->get_Parameters(&pparams);
		if (FAILED(hr))
			return;

		m_apifuncinfo.Clear();
		m_apifuncinfo.m_progid = progidstr.MakeUpper();
		pcmbfunc->GetWindowText(m_apifuncinfo.m_funcname);
		m_apifuncinfo.m_coclassguidstr = coclassguidstr;


		short lparamcount;
		hr = pparams->get_Count(&lparamcount);
		if (FAILED(hr))
			return;

		for (int i = 1; i <= lparamcount; ++i)
		{

			CAPIParameterInfo	apiparaminfo;

			hr = pparams->get_Item(i, &pparaminfo);
			if (FAILED(hr))
				return;

			BSTR bstrname;
			hr = pparaminfo->get_Name(&bstrname);
			if (FAILED(hr))
				return;
			_bstr_t bstrt(bstrname, false);

			apiparaminfo.m_name = CString((char *)bstrt);

			VARIANT_BOOL defflag;
			hr = pparaminfo->get_Default(&defflag);
			if (FAILED(hr))
				return;

			VARIANT_BOOL optfalg;
			hr = pparaminfo->get_Optional(&optfalg);
			if (FAILED(hr))
				return;

			if (defflag == VARIANT_TRUE || optfalg == VARIANT_TRUE)
			{
				apiparaminfo.foptional = 1;
			}


			ParamFlags piflag;
			hr = pparaminfo->get_Flags(&piflag);
			if (FAILED(hr))
				return;

			if (piflag & 1)
				apiparaminfo.ldirection = 1;

			if (piflag & 2)
				apiparaminfo.ldirection |= 2;

			VarTypeInfo  *pvartypeinfo;
			hr = pparaminfo->get_VarTypeInfo(&pvartypeinfo);
			if (FAILED(hr))
				return;

			variant_t vtyp;
			VariantInit(&vtyp);
			hr = pvartypeinfo->get_TypedVariant(&vtyp);
			if (FAILED(hr))
				return;
			VARTYPE vartype = vtyp.vt;

			short ptrlvl = 0;
			hr = pvartypeinfo->get_PointerLevel(&ptrlvl);
			if (FAILED(hr))
				return;

			vartype |= ((ptrlvl) ? VT_BYREF : 0);
			apiparaminfo.ldatatype = vartype;

			m_apifuncinfo.m_parameters.Add(apiparaminfo);

		}
	}
	((CMainFrame*)GetParent())->m_papiformview->LoadGrid(m_apifuncinfo);
}

void CMainToolbar::LoadFunctions()
{

	CComboBox  *pcmbfunc = (CComboBox  *)GetDlgItem(IDC_COMBO1);
	int i = 0;
	std::vector<CAPIFunctionInfo>& m_executedapilist = ((CMainFrame*)AfxGetMainWnd())->m_executedapilist;
	for (auto itr = m_executedapilist.begin(); itr != m_executedapilist.end(); ++itr,++i)
	{
		CString  progid(itr->m_progid);
		if (progid == "")
			progid = itr->m_coclassguidstr;
		progid = progid + "::" + itr->m_funcname;
		pcmbfunc->AddString(progid);
		pcmbfunc->SetItemData(i, -1);
	}

}



//void CMainToolbar::LoadFunctions() 
//{
//
//	CComboBox  *pcmbfunc = (CComboBox  *)GetDlgItem(IDC_COMBO1);
//
//	CString profilename;
//	profilename = ((CAPITesterApp*)AfxGetApp())->m_profilefile;
//
//
//	char	lpszReturnBuffer[5000], *pprtptr = lpszReturnBuffer;
//
//	memset(lpszReturnBuffer, 0, sizeof lpszReturnBuffer);
//	GetPrivateProfileSectionNames( lpszReturnBuffer,  sizeof lpszReturnBuffer, profilename  );
//	
//	int i = 0;
//	while (strlen(pprtptr) )
//	{
//		CString  progid(pprtptr);
//		pcmbfunc->AddString(progid);
//		pcmbfunc->SetItemData( i, -1);
//		i++;
//
//		pprtptr += strlen(pprtptr) + 1;
//	}
//}


