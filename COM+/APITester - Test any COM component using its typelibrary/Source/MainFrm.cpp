// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include <algorithm>
#include <vector>
#include "APITester.h"
#include "resource.h"

#include "comdef.h"
#include "tlbinfo.h"

#include "MainFrm.h"
#include "componentdlg.h"
#include "apisaveas.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


#include "atlbase.h"
CComModule _Module;

#include "atlcom.h"

/////////////////////////////////////////////////////////////////////////////
// CMainFrame

IMPLEMENT_DYNAMIC(CMainFrame, CFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
	//{{AFX_MSG_MAP(CMainFrame)
	ON_WM_CREATE()
	ON_COMMAND(ID_CMDEXECUTE, OnCmdexecute)
	ON_COMMAND(ID_CMDLOG, OnCmdlog)
	ON_UPDATE_COMMAND_UI(ID_CMDLOG, OnUpdateCmdlog)
	ON_COMMAND(ID_CMDPROGIDS, OnCmdprogids)
	ON_COMMAND(ID_FILE_SAVE_AS, OnFileSaveAs)
	ON_WM_CLOSE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

static UINT indicators[] =
{
	ID_SEPARATOR,           // status line indicator
	ID_INDICATOR_CAPS,
};


/////////////////////////////////////////////////////////////////////////////
// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
	// TODO: add member initialization code here
	m_logview = 0;
	m_compidx = -1;
	m_itfidx = -1;
	m_pdisp = NULL;
}

CMainFrame::~CMainFrame()
{
}

void CMainFrame::OnClose()
{
	CString histfilename;
	histfilename = ((CAPITesterApp*)AfxGetApp())->m_historyfile;

	try
	{
		std::ofstream ofs(histfilename);
		std::vector<CAPIFunctionInfo>& execlist = ((CMainFrame*)AfxGetMainWnd())->m_executedapilist;
		int count = execlist.size();
		ofs << count << '\n';
		for (int i = 0; i < count; ++i)
		{
			ofs << execlist[i];
		}
		ofs.close();
	}
	catch (...)
	{
	}
	CFrameWnd::OnClose();
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	
	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC  ) ||
		!m_wndToolBar.LoadToolBar(IDR_MAINFRAME))
	{
		TRACE0("Failed to create toolbar\n");
		return -1;      // fail to create
	}
	
	RECT rc;
	m_wndToolBar.SetButtonInfo(1, 1001, TBBS_SEPARATOR, 325);
	m_wndToolBar.GetItemRect(1, &rc);
	m_cmbFuncHistory.Create(
      WS_CHILD|WS_VISIBLE|CBS_DROPDOWNLIST|WS_HSCROLL|WS_VSCROLL|CBS_AUTOHSCROLL,
      CRect(rc.left+3,rc.top, 370, 400), 
	  &m_wndToolBar, IDC_COMBO1);
	m_cmbFuncHistory.SendMessage(WM_SETFONT, (DWORD)::CreateFont( 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "MS Sans Serif"), MAKELPARAM(1,0));
	m_cmbFuncHistory.SetDroppedWidth(400);


	if (!m_wndStatusBar.Create(this) ||
		!m_wndStatusBar.SetIndicators(indicators,
		  sizeof(indicators)/sizeof(UINT)))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}

	UINT	nID;
	UINT	nStyle;
	int		cxWidth;


	m_wndStatusBar.GetPaneInfo( 1, nID, nStyle, cxWidth );
	m_wndStatusBar.SetPaneInfo( 1, nID, nStyle, 60);

	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs
	cs.hMenu = NULL;
	cs.lpszClass = AfxRegisterWndClass(0, 0, 0, AfxGetApp()->LoadIcon(IDI_ICON1));
	cs.dwExStyle &= ~WS_EX_CLIENTEDGE;


	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CFrameWnd::Dump(dc);
}

#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMainFrame message handlers
BOOL CMainFrame::OnCreateClient(LPCREATESTRUCT lpcs, CCreateContext* pContext) 
{
	// TODO: Add your specialized code here and/or call the base class

	BOOL rtn;
	rtn = m_wndSplitter.CreateStatic(this, 2, 1);
	rtn |= m_wndSplitter.CreateView(0, 0, RUNTIME_CLASS(CAPIFormView), CSize(1000, 1000), pContext);
	rtn |= m_wndSplitter.CreateView(1, 0, RUNTIME_CLASS(CAPIEditView), CSize(0, 0), pContext);

	m_papiformview = (CAPIFormView  *)m_wndSplitter.GetPane(0,0);
	m_papieditview = (CAPIEditView  *)m_wndSplitter.GetPane(1,0);

	return rtn;
}


void CMainFrame::SaveFunction(const CAPIFunctionInfo&  funcinfo) 
{
	auto itr = m_executedapilist.begin();
	for (; itr != m_executedapilist.end(); ++itr)
	{
		if ((itr->m_coclassguidstr == funcinfo.m_coclassguidstr) && (itr->m_funcname == funcinfo.m_funcname))
		{
			*itr = funcinfo;
			break;
		}
	}

	if (itr == m_executedapilist.end())
		m_executedapilist.push_back(funcinfo);

	for (auto itr = m_executedapilist.begin(); itr != m_executedapilist.end(); ++itr)
	{
		for (auto itr2 = m_executedapilist.begin(); itr2 != m_executedapilist.end(); ++itr2)
		{
			if (CString(itr->m_coclassguidstr + itr->m_funcname).Compare(CString(itr2->m_coclassguidstr + itr2->m_funcname)))
			{
				CAPIFunctionInfo temp = *itr;
				*itr = *itr2;
				*itr2 = temp;
			}
		}
	}
}


void CMainFrame::SortIniFile() 
{
	CString profilename;
	profilename = ((CAPITesterApp*)AfxGetApp())->m_profilefile;

	CString tempprofilename = "c:\\temp.ini";
	::remove(tempprofilename);
	
	char  buf[100000];
	
	memset(buf, 0, sizeof buf);
	GetPrivateProfileSectionNames(buf, sizeof buf, profilename);


	CStringArray  progidlist, sectionlist;


	char *cptr = buf;

	while (*cptr)
	{
		sectionlist.Add(CString(cptr));
		cptr = cptr+ strlen(cptr) + 1;

	}

	int i, j;

	for (i=0; i<sectionlist.GetSize(); ++i)
	{

		CString outerprogid = sectionlist[i];

		int p = outerprogid.Find(":");
		if (p == 0)
			continue;

		outerprogid = outerprogid.Mid(1, p-1);
		
		for (j=0; j<progidlist.GetSize(); ++j)
		{
			if (progidlist[j] == outerprogid)
				break;

		}
	
		if (j < progidlist.GetSize())
			continue;
			
		progidlist.Add(outerprogid);
		
		for (j=0; j<sectionlist.GetSize(); ++j)
		{

			CString innerprogid = sectionlist[j];

			int p = innerprogid.Find(":");
			if (p == 0)
				continue;

			innerprogid = innerprogid.Mid(1, p-1);

			if (innerprogid != outerprogid)
				continue;

			memset(buf, 0, sizeof buf);
			GetPrivateProfileSection(sectionlist[j], buf, sizeof buf, profilename);
			WritePrivateProfileSection(sectionlist[j], buf,  tempprofilename);

		}

	}

	GetWindowsDirectory(buf, sizeof buf);
	CString winprofilename = CString(buf) + CString("\\") + profilename;
	CopyFile(tempprofilename, winprofilename, FALSE);

}

void CMainFrame::OnCmdexecute() 
{
	// TODO: Add your command handler code here

	CAPIFunctionInfo&  funcinfo = m_wndToolBar.m_apifuncinfo;
	if (funcinfo.m_coclassguidstr == "" || funcinfo.m_funcname == "")
		return;
	m_papiformview->GetGridData(funcinfo);
	Invoke(funcinfo);
	m_papiformview->LoadGrid(funcinfo);
	SaveFunction(funcinfo);
	m_papieditview->Addtolog(funcinfo);
	//SortIniFile();

}

IDispatch* CMainFrame::CreateComponent(CString clsidstr)
{
	IDispatch *pdisp = NULL;
	CLSID  clsid;
	HRESULT hr = CLSIDFromString(CStringW(clsidstr), &clsid);
	if (FAILED(hr))
		return pdisp;

	hr = CoCreateInstance(clsid, NULL, CLSCTX_ALL, IID_IDispatch, (void**)&pdisp);
	if (FAILED(hr))
		return pdisp;
	return pdisp;
}


void CMainFrame::Invoke(CAPIFunctionInfo&  funcinfo) 
{

	HRESULT			hr;
	short lparamcount = funcinfo.m_parameters.GetSize();

	COleVariant  varargs[300];
	std::vector<COleVariant> vecvarargs;

	long i, j;
	for (i=0, j=lparamcount-1; i<lparamcount; i++, j--)
	{
		
		CAPIParameterInfo&  paraminfo =  funcinfo.m_parameters[i];

		short ldirection = paraminfo.ldirection;
		short ldatatype = paraminfo.ldatatype;
		short lelemtype = paraminfo.lelemtype;
		short loptflag = paraminfo.foptional;
		if (loptflag)
			continue;
		if (ldirection == 2)
			lelemtype = 0;

		if  (((ldatatype ^ VT_BYREF) == VT_DISPATCH) && (ldatatype & VT_BYREF))
		{
			COleVariant var2;
			varargs[j].vt = VT_BYREF | VT_DISPATCH;
			varargs[j].pvarVal = new VARIANTARG;
			VariantInit(varargs[j].pvarVal);
			if (!(lelemtype & VT_ARRAY))
			{
				SetArgument(var2, lelemtype, loptflag, paraminfo.m_strvalue);
				*varargs[j].pvarVal = var2;
			}

		}
		else if ((ldatatype & VT_VARIANT) != VT_VARIANT)
		{
			SetArgument(varargs[j], ldatatype, loptflag, paraminfo.m_strvalue);
		}
		else
		{
			COleVariant var2;
			varargs[j].vt = VT_BYREF | VT_VARIANT;
			varargs[j].pvarVal = new VARIANTARG;
			VariantInit(varargs[j].pvarVal);
			if (loptflag)
			{
				if (ldatatype == VT_EMPTY)
				{
					varargs[j].pvarVal->vt = VT_ERROR;
					varargs[j].pvarVal->scode = DISP_E_PARAMNOTFOUND;
				}
			}
			else if (!(lelemtype & VT_ARRAY))
			{
				SetArgument(var2, lelemtype, loptflag, paraminfo.m_strvalue);
				VariantCopy(varargs[j].pvarVal,  var2);
			}
			else
			{
				SetArrayArgument(var2, lelemtype % VT_ARRAY, paraminfo.m_strvalue);
				varargs[j].pvarVal->vt = var2.vt;
				
				hr = SafeArrayCopy(var2.parray, &varargs[j].pvarVal->parray);
				if (FAILED(hr))
					return;
			}
		}
		vecvarargs.insert(vecvarargs.begin(),varargs[j]);
	}


	IDispatch		*pdisp = m_pdisp;
	
	if (funcinfo.m_coclassguidstr != m_coclassguidstr)
	{
		pdisp = CreateComponent(funcinfo.m_coclassguidstr);
		m_pdisp = pdisp;
		m_coclassguidstr = funcinfo.m_coclassguidstr;
	}

	DISPID dispid ;	
	BSTR  bstrfucnname = _bstr_t(funcinfo.m_funcname);
	hr = pdisp->GetIDsOfNames(IID_NULL, &bstrfucnname, 1, GetUserDefaultLCID(), &dispid);
	if (FAILED(hr))
	{
		pdisp = CreateComponent(funcinfo.m_coclassguidstr);
		m_pdisp = pdisp;

		BSTR  bstrfucnname = _bstr_t(funcinfo.m_funcname);
		hr = pdisp->GetIDsOfNames(IID_NULL, &bstrfucnname, 1, GetUserDefaultLCID(), &dispid);
		if (FAILED(hr))
			return;
	}

	// Fill in the DISPPARAMS structure.
	DISPPARAMS param ;
	param.cArgs = vecvarargs.size();// lparamcount;                 // Number of arguments
	param.rgvarg = vecvarargs.data();// varargs;            // Arguments
	param.cNamedArgs = 0 ;            // Number of named args
	param.rgdispidNamedArgs = NULL ;  // Named arguments

	VARIANT varResult ;
	::VariantInit(&varResult) ;
	
	unsigned int uArgErr=0;

	EXCEPINFO excepinfo ;
	SetMessageText("Executing...");
	hr = pdisp->Invoke(dispid, IID_NULL, GetUserDefaultLCID(), DISPATCH_METHOD,
	                        &param, &varResult, &excepinfo, &uArgErr);

	if (FAILED(hr))
	{
		if (hr == DISP_E_EXCEPTION)
		{
			funcinfo.m_result = (char*)_bstr_t(excepinfo.bstrDescription);
		}
		else
			funcinfo.m_result.Format("Unknown error : %ld", hr);

		SetMessageText(funcinfo.m_result);
		return;
	}

	funcinfo.m_result = (char*)_bstr_t(varResult);
	SetMessageText(funcinfo.m_result);

	for (i=0, j=lparamcount-1; i < lparamcount; ++i, --j)
	{
		CAPIParameterInfo&  paraminfo =  funcinfo.m_parameters[i];

		short ldirection = paraminfo.ldirection;
		if (ldirection == 1)
			continue;
		
		COleVariant& var = varargs[j];

		if ((var.vt & VT_BYREF) == VT_BYREF) 
		{
			
			if (var.vt == (VT_BYREF | VT_VARIANT))
			{
				paraminfo.lelemtype = var.pvarVal->vt;
				if (var.pvarVal->vt == VT_DISPATCH)
					paraminfo.m_pdisp[0] = var.pvarVal->pdispVal;
			}

			if (var.vt == (VT_BYREF | VT_DISPATCH))
			{
				paraminfo.lelemtype = VT_DISPATCH;
				paraminfo.m_pdisp[0] = *var.ppdispVal;

			}
		}

		if (paraminfo.lelemtype != VT_DISPATCH)
		{
			hr = VariantChangeType(&var,  &var, 0, VT_BSTR);
			if (FAILED(hr))
			{
				GetArrayArgument(var, paraminfo);
			}
			else
				paraminfo.m_strvalue = CString((char *)_bstr_t(var));
		}

	}

}

void CMainFrame::SetArgument(COleVariant& var, int ldatatype, int loptflag, CString strdata)
{

	if (strdata == "")
	{
		if (loptflag)
		{
			var.Clear();
			var.vt = VT_ERROR;
			var.scode = DISP_E_PARAMNOTFOUND;
			return;
		}
		else
		{
			switch (ldatatype % VT_BYREF)
			{
				case VT_UI1:
				case VT_I2:
				case VT_I4:
				case VT_R4:
				case VT_R8:
				case VT_BOOL:
				case VT_CY:
					strdata = "0";
					break;
			}

		}

	}




	BSTR bstrdata = _bstr_t(strdata);
	var.Clear();
	var.vt = ldatatype;


	switch (ldatatype)
	{
		case VT_UI1:
		case VT_I2:
		case VT_I4:
		case VT_R4:
		case VT_R8:
		case VT_BOOL:
		case VT_DATE:
		case VT_CY:
		case VT_BSTR:
			var = _variant_t(bstrdata).Detach();
			var.ChangeType(ldatatype);
			break;

		case VT_DISPATCH:
			m_papieditview->SetGCInfo(var, strdata);
			break;


		case VT_UI1 | VT_BYREF:
			var.pbVal  =  new unsigned char;
			VarUI1FromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.pbVal );
			break;

		case VT_I2 | VT_BYREF:
			var.piVal  =  new short;
			VarI2FromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.piVal );
			break;

		case VT_I4 | VT_BYREF:
			var.plVal  =  new long;
			VarI4FromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.plVal );
			break;

		case VT_R4 | VT_BYREF:
			var.pfltVal  =  new float;
			VarR4FromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.pfltVal );
			break;


		case VT_R8 | VT_BYREF:
			var.pdblVal  =  new double;
			VarR8FromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.pdblVal );
			break;


		case VT_BOOL | VT_BYREF:
			var.pboolVal  =  new VARIANT_BOOL;
			VarBoolFromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.pboolVal );
			break;


		case VT_CY | VT_BYREF:
			var.pcyVal  =  new CY;
			VarCyFromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.pcyVal );
			break;


		case VT_DATE | VT_BYREF:
			var.pdate  =  new DATE;
			VarDateFromStr(bstrdata, GetSystemDefaultLCID(), 
				LOCALE_NOUSEROVERRIDE, var.pdate );
			break;

		case VT_BSTR | VT_BYREF:
			var.pbstrVal  =  new BSTR;
			*var.pbstrVal  = ::SysAllocString(bstrdata);
			break;

	}


	return;

}


void CMainFrame::SetArrayArgument(COleVariant& varar, int ldatatype, CString strdata)
{

	int lcount = 0, nstart = 0, nend;
	CString  grdtxt = strdata;

	HRESULT hr;
	while ((nstart = grdtxt.Find(';' , nstart)) != -1)
	{
		lcount++;
		nstart++;
	}

	if (grdtxt != "")
		lcount++;


	
	SAFEARRAY *psa;
	SAFEARRAYBOUND rgsabound[1];
	rgsabound[0].lLbound = 0;
	rgsabound[0].cElements = lcount;
	psa = SafeArrayCreate(ldatatype, 1, rgsabound);
	if(psa == NULL)
		return;

	nstart = 0;
	for (long i = 0; i<lcount; ++i)
	{

		COleVariant  var;

		CString temps;
		BSTR bstrdata;

		nend = grdtxt.Find(';' , nstart);
		
		if (nend == -1)
			temps = grdtxt.Mid(nstart);
		else
			temps = grdtxt.Mid(nstart, nend - nstart);

		bstrdata = _bstr_t(temps);
		nstart = nend + 1;

		var.Clear();
		var.vt = ldatatype;
		var = _variant_t(bstrdata).Detach();
		if ((ldatatype != VT_VARIANT) && (ldatatype != VT_DISPATCH))
			var.ChangeType(ldatatype);

		void	*pvoid;
		switch (ldatatype)
		{
			case VT_UI1:
				pvoid = &var.bVal;
				break;

			case VT_I2:
				pvoid = &var.iVal;
				break;

			case VT_I4:
				pvoid = &var.lVal;
				break;

			case VT_R4:
				pvoid = &var.fltVal;
				break;

			case VT_R8:
				pvoid = &var.dblVal;
				break;

			case VT_BOOL:
				pvoid = &var.boolVal;
				break;

			case VT_CY:
				pvoid = &var.cyVal;
				break;

			case VT_DATE:
				pvoid = &var.date;
				break;

			case VT_BSTR:
				pvoid = bstrdata;
				break;

			case VT_VARIANT:
				pvoid = &var;
				break;

			case VT_DISPATCH:
				VariantClear(&var);
				m_papieditview->SetGCInfo(var, temps);
				pvoid = &var;
				break;


		}

		hr = SafeArrayPutElement(psa,  &i, pvoid);
		if (FAILED(hr))
			return; 

	}

	varar.vt = ldatatype | VT_ARRAY;
	varar.parray = psa;


	return;

}


void CMainFrame::GetArrayArgument(COleVariant& var, CAPIParameterInfo& paraminfo)
{

	HRESULT  hr;

	CString disptext;

	if (var.vt != (VT_BYREF|VT_VARIANT))
		return;

	if (!(var.pvarVal->vt & VT_ARRAY))
		return;

	long lUbound;
	hr = SafeArrayGetUBound( var.pvarVal->parray, 1, &lUbound);
	if (FAILED(hr))
		return;

	VARTYPE vt;
	hr = SafeArrayGetVartype( var.pvarVal->parray,  &vt);
	if (FAILED(hr))
		return;
	
	int splvt = 0;

	paraminfo.m_arvalue.RemoveAll();
	char buf[100];
	for (long k=0; k <= lUbound; ++k)
	{
		hr = SafeArrayGetElement( var.pvarVal->parray, &k, &buf);
		if (FAILED(hr))
			return;

		BSTR bstrdata = _bstr_t("");
		switch (vt)
		{
			case VT_UI1:
				VarBstrFromUI1(*(unsigned char*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_I2:
				VarBstrFromI2(*(short*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_I4:
				VarBstrFromI4(*(long*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_R4:
				VarBstrFromR4(*(float*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_R8:
				VarBstrFromR8(*(double*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_BOOL:
				VarBstrFromBool(*(VARIANT_BOOL*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_DATE:
				VarBstrFromDate(*(DATE*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_CY:
				VarBstrFromCy(*(CY*)buf, GetSystemDefaultLCID(), LOCALE_NOUSEROVERRIDE, &bstrdata );
				break;

			case VT_BSTR:
				bstrdata = *(BSTR*)buf;
				break;

			case VT_VARIANT:
				splvt = ((VARIANT*)buf)->vt;
				hr = VariantChangeType((VARIANT*)buf,  (VARIANT*)buf, 0, VT_BSTR);
				if (FAILED(hr))
				{
					bstrdata = _bstr_t("");
					paraminfo.m_pdisp[k] = ((VARIANT*)buf)->pdispVal;
					splvt = 0;

				}
				else
				{
					bstrdata = ((VARIANT*)buf)->bstrVal;
				}
				break;


		}

		paraminfo.m_arvalue.Add(CString((char*)_bstr_t(bstrdata)));
		disptext = disptext + CString((char*)_bstr_t(bstrdata));
			
		if (k < lUbound)
			disptext = disptext+ CString(";");
	}

	paraminfo.m_strvalue = disptext;
	paraminfo.lelemtype = splvt?splvt:vt;
	paraminfo.lelemtype |= VT_ARRAY;
}


void CMainFrame::OnCmdlog() 
{
	// TODO: Add your command handler code here

	if (m_logview)
	{
		m_logview = 0;
		m_wndSplitter.SetRowInfo(0, 1000, 1000);
		m_wndSplitter.RecalcLayout();
	}
	else
	{
		m_logview = 1;
		m_wndSplitter.SetRowInfo(0, 100, 100);
		m_wndSplitter.RecalcLayout();
	}
	
}

void CMainFrame::OnUpdateCmdlog(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->SetCheck(m_logview);
	
}



void CMainFrame::OnCmdprogids() 
{
	// TODO: Add your command handler code here
	CComponentDlg	dlgcomp;
	m_compidx = -1;
	m_itfidx = -1;
	if (dlgcomp.DoModal() == IDOK)
	{

		m_compidx = dlgcomp.m_selcomp;
		m_itfidx = dlgcomp.m_selitf;

		CString str;
		str.LoadString(IDR_MAINFRAME);
		str = str + "-" + g_coclasses[dlgcomp.m_selcomp].progid.MakeUpper();
		SetWindowText(str);

	}
}

void CMainFrame::OnFileSaveAs() 
{
	// TODO: Add your command handler code here

	CAPISaveAs	apisaveasdlg;

	apisaveasdlg.DoModal();
	
}

