// ComponentDlg.cpp : implementation file
//

#include "stdafx.h"
#include <vector>
#include "APITester.h"
#include "ComponentDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

OLECHAR * g_rgszTKind[] = {
	OLESTR("Enum"),		/* TKIND_ENUM */
	OLESTR("Struct"),		/* TKIND_RECORD */
	OLESTR("Module"),		/* TKIND_MODULE */
	OLESTR("Interface"),	/* TKIND_INTERFACE */
	OLESTR("Dispinterface"),	/* TKIND_DISPATCH */
	OLESTR("Coclass"),		/* TKIND_COCLASS */
	OLESTR("Typedef"),		/* TKIND_ALIAS */
	OLESTR("Union"),		/* TKIND_UNION */
};

/////////////////////////////////////////////////////////////////////////////
// CComponentDlg dialog


CComponentDlg::CComponentDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CComponentDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CComponentDlg)
	m_progid = _T("");
	m_selcomp = -1;
	m_selitf = -1;

	//}}AFX_DATA_INIT
}


void CComponentDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CComponentDlg)
	DDX_Control(pDX, IDC_APPLIST, m_applist);
	DDX_Control(pDX, IDC_COMPLIST, m_complist);
	DDX_Control(pDX, IDC_ITFLIST, m_itflist);
	DDX_Text(pDX, IDC_EDIT1, m_progid);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CComponentDlg, CDialog)
	//{{AFX_MSG_MAP(CComponentDlg)
	ON_LBN_SELCHANGE(IDC_APPLIST, OnSelchangeApplist)
	ON_LBN_SELCHANGE(IDC_COMPLIST, OnSelchangeComplist)
	ON_LBN_DBLCLK(IDC_COMPLIST, OnDblclkComplist)
	ON_BN_CLICKED(IDLOADCOMPS, OnLoadTypelib)
	ON_BN_CLICKED(IDCLEARLSTS, OnClearLists)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CComponentDlg message handlers

BOOL CComponentDlg::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	
	ICOMAdminCatalog	cat;

	((CFrameWnd*)AfxGetMainWnd())->SetMessageText("Reading catalog information...");

	cat.CreateDispatch("COMAdmin.COMAdminCatalog.1");
	
	ICatalogCollection  apps(cat.GetCollection("Applications"));
	apps.Populate();

	for (int lIndex=0; lIndex<apps.GetCount(); ++lIndex)
	{
		ICatalogObject  app(apps.GetItem(lIndex));
		m_applist.AddString(_bstr_t(app.GetName()));
	}


	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CComponentDlg::OnSelchangeApplist() 
{
	// TODO: Add your control notification handler code here

	docleanlists();

	CString   appsel;
	m_applist.GetText(m_applist.GetCurSel(), appsel);

	ICOMAdminCatalog	cat;
	cat.CreateDispatch("COMAdmin.COMAdminCatalog.1");
	
	ICatalogCollection  apps(cat.GetCollection("Applications"));
	apps.Populate();

	for (int lIndex=0; lIndex<apps.GetCount(); ++lIndex)
	{
		ICatalogObject  app(apps.GetItem(lIndex));

		CString  appname((LPCSTR)_bstr_t(app.GetName()));
		if (appname != appsel)
			continue;

		ICatalogCollection  comps(apps.GetCollection("Components", app.GetKey()));
		g_coclasses.clear();
		comps.Populate();
		for (int lIndex=0; lIndex<comps.GetCount(); ++lIndex)
		{
			LPDISPATCH pDisp = comps.GetItem(lIndex);
			ICatalogObject  comp(pDisp);
			CString progid = CString(comp.GetName());
			ProgidCoClassname triple = GetCoClassname(progid);
	    	g_coclasses.push_back(triple);
			m_complist.AddString(triple.coclassguidstr);
		}
	}
}

ProgidCoClassname  CComponentDlg::GetCoClassname(CString progidstr)
{
	_bstr_t bst(progidstr);
	HRESULT			hr;
	CLSID			clsid;
	IDispatch		*pdisp;
	ProgidCoClassname triple;

	hr = CLSIDFromProgID(bst, &clsid);
	if (FAILED(hr))
		return triple;

	hr = CoCreateInstance(clsid, NULL, CLSCTX_ALL, IID_IDispatch, (void**)&pdisp);
	if (FAILED(hr))
		return triple;

	TypeInfo *m_ptypeinfo = NULL;
	hr = m_tliapp->ClassInfoFromObject(pdisp, &m_ptypeinfo);
	if (FAILED(hr))
		return triple;
	
	BSTR compguidstr;
	m_ptypeinfo->get_GUID(&compguidstr);
	if (FAILED(hr))
		return triple;

	Interfaces *itfs;
	hr = m_ptypeinfo->get_Interfaces(&itfs);
	if (FAILED(hr))
		return triple;

	std::vector<InterfaceInfo*> itfvec = GetItfsWithMembers(itfs);
	if (itfvec.size() == 0)
		return triple;

	return ProgidCoClassname{ CString(progidstr), compguidstr, itfvec };
}

void CComponentDlg::LoadComponents(CString progidstr)
{
	_bstr_t bst(progidstr);
	HRESULT			hr;
	CLSID			clsid;
	IDispatch		*pdisp;

	((CFrameWnd*)AfxGetMainWnd())->SetMessageText("Retriving component names...");

	hr = CLSIDFromProgID(bst, &clsid);
	if (FAILED(hr))
		return;

	hr = CoCreateInstance(clsid, NULL, CLSCTX_ALL, IID_IDispatch, (void**)&pdisp);
	if (FAILED(hr))
		return;

	CTLIApplication tliapp;
	TypeInfo *ptypeinfo;
	hr = tliapp->ClassInfoFromObject(pdisp, &ptypeinfo);
	if (FAILED(hr))
		return;
	BSTR compname;
	ptypeinfo->get_Name(&compname);

}

void CComponentDlg::OnLoadTypelib()
{
	OPENFILENAME ofn;       // common dialog box structure
	char szFile[260];       // buffer for file name
	HWND hwnd;              // owner window

							// Initialize OPENFILENAME
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = this->GetSafeHwnd();
	ofn.lpstrFile = szFile;
	// Set lpstrFile[0] to '\0' so that GetOpenFileName does not 
	// use the contents of szFile to initialize itself.
	ofn.lpstrFile[0] = '\0';
	ofn.nMaxFile = sizeof(szFile);
	ofn.lpstrFilter = "All\0*.*\0DLL\0*.DLL\0TLB\0*.TLB\0";
	ofn.nFilterIndex = 1;
	ofn.lpstrFileTitle = NULL;
	ofn.nMaxFileTitle = 0;
	ofn.lpstrInitialDir = NULL;
	ofn.Flags = OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;

	// Display the Open dialog box. 

	if (GetOpenFileName(&ofn) == TRUE)
	{
		OpenTypeLib(CString(ofn.lpstrFile).AllocSysString());
	}
	UpdateData(FALSE);
}

void CComponentDlg::Cleanup()
{
	if (m_ptlib != NULL) 
	{
		m_ptlib->Release();
		m_ptlib = NULL;
	}
}

int CComponentDlg::GetMemberInfo(InterfaceInfo *itfinfo)
{

	Members		*pmembers;
	HRESULT hr = itfinfo->get_Members(&pmembers);
	if (FAILED(hr))
		return 0;

	SearchResults  *psrchresults;
	hr = pmembers->get_GetFilteredMembers(VARIANT_FALSE, &psrchresults);
	if (FAILED(hr))
		return 0;

	long lcount;
	hr = psrchresults->get_Count(&lcount);
	if (FAILED(hr))
		return 0;

	return lcount;
}

std::vector<InterfaceInfo*> CComponentDlg::GetItfsWithMembers(Interfaces *itfs)
{
	std::vector<InterfaceInfo*> itfvec;
	InterfaceInfo *itfinfo;

	short count;
	HRESULT hr = itfs->get_Count(&count);
	if (FAILED(hr))
		return itfvec;

	for (int y = 0; y < count; ++y)
	{
		hr = itfs->get_Item(y + 1, &itfinfo);
		if (FAILED(hr))
			return itfvec;

		if (GetMemberInfo(itfinfo) == 0)
			continue;
		itfvec.push_back(itfinfo);
	}
	return itfvec;
}

void CComponentDlg::OpenTypeLib(OLECHAR  *sztlib)
{
	OnClearLists();

	g_coclasses.clear();
	/* clear out globals */
	Cleanup();

	HRESULT hr = LoadTypeLib(_bstr_t(sztlib), &m_ptlib);
	if (FAILED(hr))
		return;

	hr = m_tliapp->TypeLibInfoFromITypeLib(m_ptlib, &m_ptyplibinfo);
	if (FAILED(hr))
		return;

	CoClasses *pcoclasses;
	hr = m_ptyplibinfo->get_CoClasses(&pcoclasses);
	if (FAILED(hr))
		return;

	short coclasscount = 0;
	pcoclasses->get_Count(&coclasscount);
	if (FAILED(hr))
		return;

	for (int x = 0; x < coclasscount; ++x)
	{
		CoClassInfo *pcoclassinfo;
		hr = pcoclasses->get_Item(x+1, &pcoclassinfo);
		if (FAILED(hr))
			return;

		BSTR coclassname;
		hr=pcoclassinfo->get_Name(&coclassname);
		if (FAILED(hr))
			return;

		BSTR coclassguidstr;
		hr = pcoclassinfo->get_GUID(&coclassguidstr);
		if (FAILED(hr))
			return;

		GUID coclassguid;
		hr = CLSIDFromString(CStringW(coclassguidstr), &coclassguid);
		if (FAILED(hr))
			return;

		BSTR progid;
		hr = ProgIDFromCLSID(coclassguid, &progid);
		if (FAILED(hr))
		{
			progid = L"";
		}

		Interfaces *itfs;
		HRESULT hr = pcoclassinfo->get_Interfaces(&itfs);
		if (FAILED(hr))
			return;

		std::vector<InterfaceInfo*> itfvec = GetItfsWithMembers(itfs);
		if (itfvec.size() == 0)
			continue;

		g_coclasses.push_back(ProgidCoClassname{ CString(progid), CString(coclassguidstr), itfvec });
		m_complist.AddString(CString(coclassguidstr));
	}
}


void CComponentDlg::OnOK() 
{
	// TODO: Add extra validation here
	if (m_itflist.GetCurSel() == -1)
	{
		AfxMessageBox("select a interface");
		return;
	}
	m_selcomp = m_complist.GetCurSel();
	m_selitf = m_itflist.GetCurSel();
	UpdateData(TRUE);
	CDialog::OnOK();
}


void CComponentDlg::docleanlists()
{
	m_itflist.ResetContent();
	m_complist.ResetContent();
	m_progid = "";
	UpdateData(FALSE);
}

void CComponentDlg::OnClearLists()
{
	docleanlists();
	m_applist.SetCurSel(-1);
	UpdateData(FALSE);

}

void CComponentDlg::LoadInterfaces(std::vector<InterfaceInfo*> itfvec)
{
	m_itflist.ResetContent();
	for (int i = 0; i < itfvec.size(); ++i)
	{
		BSTR  name;
		itfvec[i]->get_Name(&name);
		m_itflist.AddString(CString(name));
		SysFreeString(name);
	}
}

void CComponentDlg::OnSelchangeComplist() 
{
	// TODO: Add your control notification handler code here
	m_progid = g_coclasses[m_complist.GetCurSel()].progid;
	LoadInterfaces(g_coclasses[m_complist.GetCurSel()].pclassinfovec);
	UpdateData(FALSE);

}

void CComponentDlg::OnDblclkComplist() 
{
	// TODO: Add your control notification handler code here
	OnSelchangeComplist();
	OnOK();
}
