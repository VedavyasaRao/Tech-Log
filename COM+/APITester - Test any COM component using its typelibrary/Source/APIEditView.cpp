// APIEditView.cpp : implementation file
//

#include "stdafx.h"
#include "APITester.h"
#include "APIEditView.h"
#include "mainfrm.h"
#include "apiformview.h"

#include "dfsbasecontext.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



/////////////////////////////////////////////////////////////////////////////
// CAPIEditView

IMPLEMENT_DYNCREATE(CAPIEditView, CEditView)

CAPIEditView::CAPIEditView()
{
}

CAPIEditView::~CAPIEditView()
{
}


BEGIN_MESSAGE_MAP(CAPIEditView, CEditView)
	//{{AFX_MSG_MAP(CAPIEditView)
	ON_CONTROL_REFLECT(EN_CHANGE, OnEditChange)
	ON_COMMAND(ID_EDIT_REPLACE, OnEditReplace)
	ON_COMMAND(ID_CMDCLEAR, OnCmdclear)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAPIEditView drawing

void CAPIEditView::OnDraw(CDC* pDC)
{
	CDocument* pDoc = GetDocument();
	// TODO: add draw code here
}

/////////////////////////////////////////////////////////////////////////////
// CAPIEditView diagnostics

#ifdef _DEBUG
void CAPIEditView::AssertValid() const
{
	CEditView::AssertValid();
}

void CAPIEditView::Dump(CDumpContext& dc) const
{
	CEditView::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CAPIEditView message handlers

void CAPIEditView::OnEditChange() 
{
	// TODO: If this is a RICHEDIT control, the control will not
	// send this notification unless you override the CEditView::OnInitDialog()
	// function and call CRichEditCtrl().SetEventMask()
	// with the ENM_CHANGE flag ORed into the mask.
	
	// TODO: Add your control notification handler code here
	
}

void CAPIEditView::OnInitialUpdate() 
{
	CEditView::OnInitialUpdate();
	
	// TODO: Add your specialized code here and/or call the base class
	GetEditCtrl().SendMessage(WM_SETFONT, (DWORD)::CreateFont( 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New"), MAKELPARAM(1,0));
}


void CAPIEditView::Addtolog(CAPIFunctionInfo&  funcinfo) 
{
	GetEditCtrl().SendMessage(WM_SETFONT, (DWORD)::CreateFont( 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New"), MAKELPARAM(1,0));

	CString disptxt;
	GetWindowText(disptxt);


	CString results;

	struct tm *newtime;
	char buf[151];

	memset (buf, '*', sizeof buf);
	buf[150] = '\0';
	results = CString(buf) + CString("\r\n");

	time_t aclock;
	time( &aclock );     
	newtime = localtime( &aclock );  
	CString ascdatestr(asctime( newtime ));

	results = results + funcinfo.m_progid + CString("::") + funcinfo.m_funcname +  CString("         ") + ascdatestr + "\r\n";

	int arstart = 0;
	for (int i = 0; i < funcinfo.m_parameters.GetSize(); ++i)
	{
		CAPIParameterInfo&  paraminfo = funcinfo.m_parameters[i];

		if ((paraminfo.m_arvalue.GetSize()) && (paraminfo.ldirection & 2))
		{
			
			if (arstart)
			{
				if (m_rowcount != paraminfo.m_arvalue.GetSize()+1)
				{
					FormatArData(results);
					arstart = 0;
				}
			}

		}
		else
		{
			if (arstart)
			{
				FormatArData(results);
				arstart = 0;
			}
		}
		


		memset(buf, ' ', sizeof buf);
		strcpy(buf, paraminfo.m_name);
		buf[strlen(buf)] = 32;
		buf[50] = '\0';
		results = results + CString(" ") + buf + CString(" = ") + paraminfo.m_strvalue + "\r\n";  

 
		if ((paraminfo.lelemtype & VT_DISPATCH) == VT_DISPATCH)
		{
			GetGCInfo(results, paraminfo.m_pdisp[0]);
		}


		if (paraminfo.m_arvalue.GetSize()  && (paraminfo.ldirection & 2))
		{

			if (!arstart)
			{
				m_rowcount = 1;
				m_colcount = 0;
				arstart = 1;
			}

			if ((paraminfo.lelemtype & VT_VARIANT) == VT_VARIANT)
			{
				for (int u=0; u<paraminfo.m_arvalue.GetSize(); ++u)
					GetGCInfo(results, paraminfo.m_pdisp[u]);
			}

			AddtoDispData(paraminfo);
		}

	}

	if (arstart)
		FormatArData(results);

	results = results + " Return Value = "+ funcinfo.m_result + "\r\n";
	results = results  + "\r\n\r\n";
	disptxt = results + disptxt;
	SetWindowText(disptxt);

	CString logfilename;
	logfilename = ((CAPITesterApp*)AfxGetApp())->m_logfile;
	try
	{
		
		CFile f;
		f.Open( logfilename, CFile::modeCreate | CFile::modeWrite );

		CArchive ar(&f, CArchive::store );
		ar.m_pDocument = NULL;
		SerializeRaw(ar);
		
	}
	catch(...)
	{
	}
}

void CAPIEditView::AddtoDispData(CAPIParameterInfo&		paraminfo) 
{

	m_dispdata[0][m_colcount] = paraminfo.m_name;
	int i;
	for ( i=0; i<paraminfo.m_arvalue.GetSize(); ++i)
	{
		m_dispdata[i+1][m_colcount] = paraminfo.m_arvalue[i];
	}

	if (m_rowcount < i+1)
		m_rowcount = i+1;

	m_colcount++;

}

void CAPIEditView::FormatArData(CString&  results) 
{

	char buf[30000];
	CStringArray  coldisp;
	int i, j;
	for ( i=0; i<m_rowcount; ++i)
		coldisp.Add(CString(""));

	for ( j=0; j<m_colcount; ++j)
	{
		int k;
		int collen = 0;
		for (k=0; k<m_rowcount; ++k)
		{
			CString tstr = m_dispdata[k][j];
			if (tstr.GetLength() >	collen )
				collen = tstr.GetLength(); 
		}

		for (k=0; k<m_rowcount; ++k)
		{
			CString tstr = m_dispdata[k][j];
			
			memset(buf, 32, sizeof buf);
			strcpy(buf + (collen - tstr.GetLength())/2, tstr);
			buf[strlen(buf)] = 32;
			buf[collen + 2] = 0;
			
			coldisp[k] = coldisp[k] + buf;
		}
	}

	results = results + "\r\n";
	for (i=0; i<m_rowcount; ++i)
	{
		results = results + coldisp[i] +  "\r\n";
	}
	results = results + "\r\n";

}

void CAPIEditView::OnEditReplace() 
{
	// TODO: Add your command handler code here

	CEditView::OnEditReplace();
	
}

void CAPIEditView::OnCmdclear() 
{
	// TODO: Add your command handler code here

	CString logfilename;
	logfilename = ((CAPITesterApp*)AfxGetApp())->m_logfile;
	CFile::Remove(logfilename );
	SetWindowText(CString(""));
}


void CAPIEditView::GetGCInfo(CString&  results, IDispatch* gocptr) //CAPIParameterInfo& paraminfo)
{
	CString tempini = "c:\\tempgcout.ini";
	CString secstr = "maingc";


	::remove(tempini);
	IterateGC(tempini, secstr, gocptr);//paraminfo.m_pdisp);

	try
	{
		CStdioFile logfile( tempini, CFile::modeRead );
		UINT loglen = logfile.GetLength();
		CString buf, buf2;

		while (logfile.ReadString(buf))
		{
			buf2 = buf2 + "\r\n" + buf;
		}
		logfile.Close();
		
		results = results + buf2 + "\r\n";

	}

	catch(...)
	{

	}
}




void CAPIEditView::IterateGC(CString tempini, CString secstr, IDispatch* gcptr)
{

	CMainFrame *pmainwnd = (CMainFrame *)AfxGetMainWnd();


	HRESULT		hr;
	ICSIMBaseContext  basectx;

	if ((ULONG)gcptr == 0xcccccccc)
		return;

	basectx.CreateDispatch("CSIMBaseContext.CSIMBaseContext.1");
	basectx.SetGCInterface(gcptr);


	VARIANT		vbstrContextNames;
	VARIANT		vContextValues;

	basectx.GetAllContext(&vbstrContextNames, &vContextValues);

	if (vbstrContextNames.vt == VT_EMPTY)
		return;

	COleSafeArray  namesa(vbstrContextNames), valuessa(vContextValues);

	long  lbound;
	namesa.GetUBound(1, &lbound);
	CString		sentry,sdata;

	sentry = "count";
	sdata.Format("%d", lbound + 1);
	WritePrivateProfileString(secstr, sentry, sdata, tempini);



	VARIANT  var;
	int	vartype;

	for (long i=0; i<=lbound; ++i)
	{
		CString	selemname;

		namesa.GetElement(&i, &var.bstrVal);
		selemname = (char*)_bstr_t(var.bstrVal);

		sentry.Format("ElementName%d", i);
		WritePrivateProfileString(secstr, sentry, selemname, tempini);


		valuessa.GetElement(&i, &var);
		CString sdatatype;
		CString	selemvalue;
		CString secstrnew; 
		switch(var.vt)
		{

		case VT_DISPATCH:
			sentry.Format("ElementDataType%d", i);
			WritePrivateProfileString(secstr, sentry, "VT_VARIANT", tempini);
			sentry.Format("ElementElementType%d", i);
			WritePrivateProfileString(secstr, sentry, "VT_DISPATCH", tempini);
			secstrnew = secstr + CString("::") + selemname;
			if (var.vt == VT_EMPTY)
				WritePrivateProfileString(secstrnew, "count", "0", tempini);
			else
				IterateGC(tempini, secstrnew, var.pdispVal);
			break;

		default:
			vartype = var.vt;
			hr = VariantChangeType(&var,  &var, 0, VT_BSTR);
			if (FAILED(hr))
			{
				CAPIParameterInfo param;
				VARIANTARG  var2;
				var2.vt = VT_BYREF | VT_VARIANT;
				var2.pvarVal = new VARIANT;
				VariantInit(var2.pvarVal);
				VariantCopy(var2.pvarVal, &var);
				((CMainFrame*)AfxGetMainWnd())->GetArrayArgument(COleVariant(var2), param);
				selemvalue = param.m_strvalue;

				sdatatype = pmainwnd->m_papiformview->getElemCode((var.vt)%VT_ARRAY);
				sentry.Format("ElementDataType%d", i);
				WritePrivateProfileString(secstr, sentry, sdatatype, tempini);

				sdatatype = pmainwnd->m_papiformview->getElemCode(param.lelemtype);
				sentry.Format("ElementElementType%d", i);
				WritePrivateProfileString(secstr, sentry, sdatatype, tempini);
			}
			else
			{
				selemvalue = CString((char *)_bstr_t(var));
				sdatatype = pmainwnd->m_papiformview->getElemCode(vartype);
				sentry.Format("ElementDataType%d", i);
				WritePrivateProfileString(secstr, sentry, sdatatype, tempini);
			}

			sentry.Format("ElementValue%d", i);
			WritePrivateProfileString(secstr, sentry, selemvalue, tempini);
		}
	}


}


void CAPIEditView::SetGCInfo(COleVariant& var, CString&  gctextfile)
{

	CString			secstr = "maingc";

	var.vt = VT_DISPATCH;
	CreateGCfromfile(gctextfile, secstr, &var.pdispVal);
}

int getdatatype(CString buf)
{
	
	if (buf == "VT_EMPTY")
		return VT_EMPTY;

	if (buf ==  "VT_UI1")
		return VT_UI1 ;

	if (buf ==  "VT_I2")
		return VT_I2 ;

	if (buf ==  "VT_I4")
		return VT_I4 ;

	if (buf ==  "VT_R4")
		return VT_R4 ;

	if (buf ==  "VT_R8")
		return VT_R8 ;

	if (buf ==  "VT_BOOL")
		return VT_BOOL ;

	if (buf ==  "VT_CY")
		return VT_CY ;

	if (buf ==  "VT_DATE")
		return VT_DATE ;

	if (buf ==  "VT_BSTR")
		return VT_BSTR     ;

	if (buf ==  "VT_VARIANT")
		return VT_VARIANT               ;

	if (buf ==  "VT_DISPATCH")
		return VT_DISPATCH  ;

	if (buf ==  "VT_EMPTY")
		return VT_EMPTY  ;
	
	
	return 0;


}
void CAPIEditView::CreateGCfromfile(CString tempini, CString ssection, IDispatch** gcptr)
{

	ICSIMBaseContext  basectx;
	CMainFrame *pmainwnd = (CMainFrame *)AfxGetMainWnd();

	basectx.CreateDispatch("CSIMBaseContext.CSIMBaseContext.1");


	int lcount = GetPrivateProfileInt( ssection, "Count", 0, tempini);


	CString		sentry, selemname,elemvalue;
	long		datatype, elemtype;


	char		buf[30000];
	for (int i=0; i < lcount; ++i)
	{
		sentry.Format("ElementName%d", i);
		GetPrivateProfileString(ssection, sentry, "", buf, sizeof buf, tempini);
		selemname = buf;

		sentry.Format("ElementDataType%d", i);
		GetPrivateProfileString(ssection, sentry, "", buf, sizeof buf, tempini);
		datatype = getdatatype(buf);

		sentry.Format("ElementElementType%d", i);
		GetPrivateProfileString(ssection, sentry, "", buf, sizeof buf, tempini);
		elemtype = getdatatype(buf);

		sentry.Format("ElementValue%d", i);
		GetPrivateProfileString(ssection, sentry, "", buf, sizeof buf, tempini);
		elemvalue = buf;

		COleVariant		var;
		VariantInit(&var);

		if (datatype == VT_VARIANT)
		{
			if (elemtype == VT_DISPATCH)
			{
				var.vt = VT_DISPATCH;
				CString		secstrnew = ssection + CString("::") + selemname;
				CreateGCfromfile(tempini, secstrnew, &var.pdispVal );
				
			}

			if (elemtype == VT_VARIANT)
			{
				elemtype = VT_VARIANT;
				pmainwnd->SetArrayArgument(var, elemtype , elemvalue);
			}
		}
		else 
		{
			pmainwnd->SetArgument(var, datatype, 0, elemvalue);
		}

		basectx.SetOneContextVariable(selemname, var, VARIANT_TRUE, VARIANT_FALSE);

	}


	*gcptr = basectx.GetGCInterface1();
}



void CAPIEditView::OnFilePrint()
{
	// TODO: Add your specialized code here and/or call the base class
	
	CEditView::OnFilePrint();
}
