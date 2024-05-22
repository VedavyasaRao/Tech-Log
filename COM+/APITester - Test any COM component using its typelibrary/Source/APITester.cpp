// APITester.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "APITester.h"

#include "MainFrm.h"
#include "APITesterPrintDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAPITesterApp

BEGIN_MESSAGE_MAP(CAPITesterApp, CWinApp)
	//{{AFX_MSG_MAP(CAPITesterApp)
	ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
	ON_COMMAND(ID_APITESTPRINT_DIRECT, OnApitestprintDirect)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

CLSID IID__TLIApplication = {0x8B21775DL,0x717D,0x11CE, {0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00}};
CLSID CLSID_TLIApplication = {0x8B21775EL,0x717D,0x11CE,{0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00}};

/////////////////////////////////////////////////////////////////////////////
// CAPITesterApp construction

CAPITesterApp::CAPITesterApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
	m_profilefile = "APITESTER.INI";
	m_logfile = "APITESTER.LOG";
	m_historyfile = "HISTORY.DAT";
}




/////////////////////////////////////////////////////////////////////////////
// The one and only CAPITesterApp object

CAPITesterApp theApp;
Serializer srz;

void Serializer::writestring(std::ostream& os, const CString& s)
{
	os << s << '\n';

}

void Serializer::writeint(std::ostream& os, int i)
{
	os << i << '\n';

}

void Serializer::readline(std::istream&  is)
{
	memset(readbuf, 0, sizeof readbuf);
	is.getline(readbuf, sizeof readbuf);
}

CString Serializer::readstring(std::istream&  is)
{
	readline(is);
	return readbuf;
}

int Serializer::readint(std::istream&  is)
{
	auto s = readstring(is);
	if (s == "")
		return 0;
	return atoi(s.GetBuffer());
}


/////////////////////////////////////////////////////////////////////////////
// CAPITesterApp initialization

BOOL CAPITesterApp::InitInstance()
{
	AfxEnableControlContainer();

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif

	// Change the registry key under which our settings are stored.
	// TODO: You should modify this string to be something appropriate
	// such as the name of your company or organization.
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));


	// To create the main window, this code creates a new frame window
	// object and then sets it as the application's main window object.

	CMainFrame* pFrame = new CMainFrame;
	m_pMainWnd = pFrame;

	// create and load the frame with its resources

	pFrame->LoadFrame(IDR_MAINFRAME,
		WS_OVERLAPPEDWINDOW | FWS_ADDTOTITLE, NULL,
		NULL);

	// The one and only window has been initialized, so show and update it.
	pFrame->ShowWindow(SW_SHOW);
	pFrame->UpdateWindow();

	RECT	rect;
	pFrame->GetWindowRect(&rect);
	rect.right = rect.right - rect.left + 50;
	rect.bottom = rect.bottom - rect.top; 

	pFrame->SetWindowPos(NULL, rect.left,rect.top,rect.right, rect.bottom, SWP_SHOWWINDOW);

	pFrame->SetMessageText("Loading the LOG file, please wait as this might take a few minutes if the file is big");

	CString logfilename;
	logfilename = ((CAPITesterApp*)AfxGetApp())->m_logfile;

	try
	{

		CStdioFile logfile( logfilename, CFile::modeRead );
		UINT loglen = logfile.GetLength();
		CString buf, buf2;
		
		while (logfile.ReadString(buf))
		{
			buf2 = buf2 + "\r\n" + buf;
		}
	
		logfile.Close(); 

		m_logbuf = buf2;

		pFrame->m_papieditview->SetWindowText(m_logbuf);

	}
	catch(...)
	{
	}

	pFrame->SetMessageText("Loading the HISTORY file, please wait as this might take a few minutes if the file is big");

	CString histfilename;
	histfilename = ((CAPITesterApp*)AfxGetApp())->m_historyfile;

	try
	{
		std::ifstream ifs(histfilename);
		std::vector<CAPIFunctionInfo>& execlist =((CMainFrame*)AfxGetMainWnd())->m_executedapilist;
		int count= srz.readint(ifs);
		for (int i = 0; i < count; ++i)
		{
			CAPIFunctionInfo apifunc;
			ifs >> apifunc;
			execlist.push_back(apifunc);
		}
		ifs.close();
	}
	catch (...)
	{
	}

	pFrame->SetMessageText("Ready");

	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// CAPITesterApp message handlers





/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
		// No message handlers
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

// App command to run the dialog
void CAPITesterApp::OnAppAbout()
{
	CAboutDlg aboutDlg;
	aboutDlg.DoModal();
}

/////////////////////////////////////////////////////////////////////////////
// CAPITesterApp message handlers


CAPIParameterInfo::CAPIParameterInfo()
{
	m_name = "";
	foptional = 0;
	ldirection = 0;
	ldatatype = 0;
	lelemtype = 0;
	m_strvalue = "";
}

CAPIFunctionInfo::CAPIFunctionInfo()
{
	m_progid = "";
	m_funcname = "";
	m_result = "";
	m_coclassguidstr = "";
}

void CAPIFunctionInfo::Clear()
{
	m_progid = "";
	m_funcname = "";
	m_result = "";
	m_coclassguidstr = "";
	m_parameters.RemoveAll();
}

CAPIFunctionInfo::CAPIFunctionInfo(const CAPIFunctionInfo& in)
{
	this->operator=(in);
}

void CAPIFunctionInfo::operator=(const CAPIFunctionInfo& in)
{
	m_progid = in.m_progid;
	m_funcname = in.m_funcname;
	m_result = in.m_result;
	m_coclassguidstr = in.m_coclassguidstr;
	m_parameters.RemoveAll();
	for (int i = 0; i < in.m_parameters.GetSize(); ++i)
		m_parameters.Add(((CAPIFunctionInfo&)in).m_parameters[i]);
}

std::ostream& operator<<(std::ostream& os, const CAPIFunctionInfo& apifunc)
{
	srz.writestring(os, apifunc.m_progid);
	srz.writestring(os, apifunc.m_funcname);
	srz.writestring(os, apifunc.m_result);
	srz.writestring(os, apifunc.m_coclassguidstr);

	int count = apifunc.m_parameters.GetSize();
	srz.writeint(os, count);
	for (int i = 0; i < count; ++i)
		os << apifunc.m_parameters[i];
	return os;
}

std::istream& operator >> (std::istream& is, CAPIFunctionInfo& apifunc)
{
	apifunc.m_progid = srz.readstring(is);
	apifunc.m_funcname = srz.readstring(is);
	apifunc.m_result = srz.readstring(is);
	apifunc.m_coclassguidstr = srz.readstring(is);


	int count = srz.readint(is);
	apifunc.m_parameters.RemoveAll();
	for (int i = 0; i < count; ++i)
	{
		CAPIParameterInfo paraminfo;
		is >> paraminfo;
		apifunc.m_parameters.Add(paraminfo);
	}
	return is;
}



CAPIParameterInfo&  CAPIParameterInfo::operator=(CAPIParameterInfo& in)
{

	m_name = in.m_name;
	foptional = in.foptional;
	ldirection = in.ldirection;
	ldatatype = in.ldatatype;
	lelemtype = in.lelemtype;
	m_strvalue = in.m_strvalue;
	m_arvalue.Copy(in.m_arvalue);
	
	for (int i = 0; i<100; ++i)
		m_pdisp[i] = in.m_pdisp[i];

	return *this;
}

std::istream& operator >> (std::istream& is, CAPIParameterInfo& paraminfo)
{

	paraminfo.m_name = srz.readstring(is);
	paraminfo.foptional = srz.readint(is);
	paraminfo.ldirection = srz.readint(is);
	paraminfo.ldatatype = srz.readint(is);
	paraminfo.lelemtype = srz.readint(is);
	paraminfo.m_strvalue = srz.readstring(is);

	int count = srz.readint(is);
	paraminfo.m_arvalue.RemoveAll();
	for (int i = 0; i < count; ++i)
	{
		paraminfo.m_arvalue.Add(srz.readstring(is));
	}
	return is;
}

std::ostream& operator<<(std::ostream& os, const CAPIParameterInfo& paraminfo)
{
	srz.writestring(os, paraminfo.m_name);
	srz.writeint(os, paraminfo.foptional);
	srz.writeint(os, paraminfo.ldirection);
	srz.writeint(os, paraminfo.ldatatype);
	srz.writeint(os, paraminfo.lelemtype);
	srz.writestring(os, paraminfo.m_strvalue);

	int count = paraminfo.m_arvalue.GetCount();
	srz.writeint(os, count);
	for (int i = 0; i < count; ++i)
		os << paraminfo.m_arvalue[i];
	return os;
}

BOOL CAPITesterApp::InitApplication() 
{
	// TODO: Add your specialized code here and/or call the base class
	
	return CWinApp::InitApplication();
}

void CAPITesterApp::OnFilePrintSetup() 
{

	CWinApp::OnFilePrintSetup();
}

void CAPITesterApp::OnApitestprintDirect() 
{
	// TODO: Add your command handler code here
	CAPITesterPrintDlg	apiprintdlg;
	if (apiprintdlg.DoModal() == IDOK)
	{
		if (apiprintdlg.m_funcdef)
			PrintFuncDef();

		if (apiprintdlg.m_logfile)
			PrintLog();

	}

	
}

void CAPITesterApp::PrintFuncDef()
{

	int fontmulti = 1;

	CDC printerdc;

	CPrintDialog dlg(TRUE, PD_RETURNDEFAULT, this->m_pMainWnd );
	if (!dlg.GetDefaults())	
		return;
	
	LPDEVMODE   pDevInfo = dlg.GetDevMode( );

	pDevInfo->dmOrientation = 2;
	HDC hdc = dlg.CreatePrinterDC();
	printerdc.Attach(hdc);

	int nHorz = printerdc.GetDeviceCaps(HORZRES);
	int nVert = printerdc.GetDeviceCaps(VERTRES)-50;


	fontmulti = nHorz / 640;

    // Initialize the members of a DOCINFO structure. 
 
    DOCINFO di;
	di.cbSize = sizeof(DOCINFO); 
    di.lpszDocName = "APITester"; 
    di.lpszOutput = (LPTSTR) NULL; 
    di.lpszDatatype = (LPTSTR) NULL; 
    di.fwType = 0; 
 

    printerdc.StartDoc (&di); 
	
	CFont   courierfnt;
	courierfnt.CreateFont( 14 * fontmulti, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");

	CFont   courierfnt1;
	courierfnt1.CreateFont( 10 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");

	CFont   courierfnt2;
	courierfnt2.CreateFont( 10 * fontmulti, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");


	CFont   arialfnt;
	arialfnt.CreateFont( 18 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Arial");

	CFont   arialfnt1;
	arialfnt1.CreateFont( 14 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Arial");
	

	int x, y;
	CString st3, timestr;
	CSize sz;

	char tmpbuf[1000];

	_strdate( tmpbuf );
	timestr = tmpbuf;
	timestr += " ";
	_strtime( tmpbuf );
	timestr += tmpbuf;


	CStringArray	PrintCols;
	CStringArray	PrintData;

	PrintCols.Add("Parameter Name");
	PrintCols.Add("Direction");
	PrintCols.Add("Data Type");
	PrintCols.Add("Element Type");
	PrintCols.Add("Value");

	int		colwidth[] = {250, 100, 100, 150, 200};
	int pagecounter = 0, line=0;

	CAPIFunctionInfo&  funcinfo = ((CMainFrame*)AfxGetMainWnd())->m_wndToolBar.m_apifuncinfo;
	int lparamcount = funcinfo.m_parameters.GetSize();

	CString st;

	int i=1	;
	while(1)
	{
		st.Format("Page %4d", ++pagecounter);

		printerdc.StartPage(); 

		x = 10*fontmulti;
		y = 0;
		printerdc.SelectObject(&courierfnt2);
		printerdc.TextOut(x, y, timestr);
		
		CString st4 = "APITESTER";
		printerdc.SelectObject(&arialfnt);
		sz = printerdc.GetTextExtent( st4, st4.GetLength());
		x = (nHorz - sz.cx) / 2;
		y = 0;
		printerdc.TextOut(x, y, st4);

		st.Format("Page %4d", pagecounter);
		printerdc.SelectObject(&courierfnt2);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x = (nHorz - sz.cx - 50) ;
		y = 0;
		printerdc.TextOut(x, y, st);


		x = 0;
		y = sz.cy + 12*fontmulti;
		st = funcinfo.m_progid + "::" +	funcinfo.m_funcname;
		printerdc.SelectObject(&arialfnt1);
		sz = printerdc.GetTextExtent( st, st.GetLength());
		x += sz.cx + 10*fontmulti;

		x = (nHorz - x)/2;
		printerdc.SelectObject(&arialfnt1);
		printerdc.TextOut(x, y, st);


		int		j, k, wid;
		CString str;

		for(k=0; k<PrintCols.GetSize(); ++k)
		{
			wid = colwidth[k]/8;
			j = (wid - PrintCols[k].GetLength())/2;
			if (j < 0)
				j = 0;
			memset(tmpbuf, 32, sizeof tmpbuf);
			lstrcpy(tmpbuf+j, PrintCols[k]);
			tmpbuf[lstrlen(tmpbuf)] =  32;
			tmpbuf[wid] = 0;
			str += tmpbuf;
			str += " ";
		}


		
		x = 0;
		y += sz.cy + 12*fontmulti;
		printerdc.SelectObject(&courierfnt1);
		printerdc.TextOut(x, y, str);


		CString sdata;
		for (; i<lparamcount; ++i)
		{

			PrintData.RemoveAll();
			CAPIParameterInfo&	 paraminfo = funcinfo.m_parameters[i-1];

			sdata = paraminfo.m_name;
			if (paraminfo.foptional)
				sdata = CString("[") + paraminfo.m_name + CString("]");
			PrintData.Add(sdata);


			sdata = "";
			if (paraminfo.ldirection  & 1)
				sdata = "in ";
			if (paraminfo.ldirection  & 2)
				sdata  = sdata + "out";
			PrintData.Add(sdata);


			sdata = ((CMainFrame*)AfxGetMainWnd())->m_papiformview->getVarCode((short)paraminfo.ldatatype);
			PrintData.Add(sdata);

			sdata = "";
			if (paraminfo.lelemtype)
				sdata = ((CMainFrame*)AfxGetMainWnd())->m_papiformview->getElemCode(paraminfo.lelemtype);
			PrintData.Add(sdata);

			sdata = paraminfo.m_strvalue; 
			PrintData.Add(sdata);
		
			str = "";
			for (j=0; j<PrintCols.GetSize(); ++j)
			{
				
				wid = colwidth[j]/8;
				int jj = (wid - PrintData[j].GetLength())/2;
				if (jj < 0)
					jj = 0;
				
				memset(tmpbuf, 32, sizeof tmpbuf);
				lstrcpy(tmpbuf+jj, PrintData[j] );
				tmpbuf[lstrlen(tmpbuf)] =  32;
				tmpbuf[wid] = 0;
				str += tmpbuf;
				str += " ";
			}

			x = 0;
			y += sz.cy + 3*fontmulti;
			printerdc.SelectObject(&courierfnt2);
			printerdc.TextOut(x, y, str);
			if (y +sz.cy >= nVert)
				break;
		}
		printerdc.EndPage(); 

		if (i == lparamcount)
			break;
	}

	printerdc.EndDoc(); 
}



void CAPITesterApp::PrintLog()
{


	int fontmulti = 1;

	CDC printerdc;

	CPrintDialog dlg(TRUE, PD_RETURNDEFAULT, this->m_pMainWnd );
	if (!dlg.GetDefaults())	
		return;
	
	HDC hdc = dlg.CreatePrinterDC();
	printerdc.Attach(hdc);

	int nHorz = printerdc.GetDeviceCaps(HORZRES);
	int nVert = printerdc.GetDeviceCaps(VERTRES)-50;


	fontmulti = nHorz / 640;

    // Initialize the members of a DOCINFO structure. 
 
    DOCINFO di;
	di.cbSize = sizeof(DOCINFO); 
    di.lpszDocName = "APITester"; 
    di.lpszOutput = (LPTSTR) NULL; 
    di.lpszDatatype = (LPTSTR) NULL; 
    di.fwType = 0; 
 

    printerdc.StartDoc (&di); 
	
	CFont   courierfnt;
	courierfnt.CreateFont( 14 * fontmulti, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");

	CFont   courierfnt1;
	courierfnt1.CreateFont( 10 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");

	CFont   courierfnt2;
	courierfnt2.CreateFont( 10 * fontmulti, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");


	CFont   arialfnt;
	arialfnt.CreateFont( 18 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Arial");

	CFont   arialfnt1;
	arialfnt1.CreateFont( 14 * fontmulti, 0, 0, 0, 900, 0, 0, 0, 0, 0, 0, 0, 31, "Arial");
	

	int x, y;
	CString st3, timestr;
	CSize sz;

	char tmpbuf[1000];

	_strdate( tmpbuf );
	timestr = tmpbuf;
	timestr += " ";
	_strtime( tmpbuf );
	timestr += tmpbuf;


	int pagecounter = 0, line=0;


	CString st;


	try
	{
		CStdioFile logfile( m_logfile, CFile::modeRead );
		UINT loglen = logfile.GetLength();
		CString buf, buf2;

		int i=1	;
		int res = 0;
		while(1)
		{
			st.Format("Page %4d", ++pagecounter);

			printerdc.StartPage(); 

			x = 10*fontmulti;
			y = 0;
			printerdc.SelectObject(&courierfnt2);
			printerdc.TextOut(x, y, timestr);
			
			CString st4 = "APITESTER";
			printerdc.SelectObject(&arialfnt);
			sz = printerdc.GetTextExtent( st4, st4.GetLength());
			x = (nHorz - sz.cx) / 2;
			y = 0;
			printerdc.TextOut(x, y, st4);

			st.Format("Page %4d", pagecounter);
			printerdc.SelectObject(&courierfnt2);
			sz = printerdc.GetTextExtent( st, st.GetLength());
			x = (nHorz - sz.cx - 50) ;
			y = 0;
			printerdc.TextOut(x, y, st);


			x = 0;
			y = sz.cy + 12*fontmulti;

			while ((res = logfile.ReadString(buf)))
			{

				x = 0;
				//y += sz.cy + 10*fontmulti;
				y += sz.cy + 2*fontmulti;
				printerdc.SelectObject(&courierfnt2);
				printerdc.TextOut(x, y, buf);
				if (y +sz.cy >= nVert)
					break;
			}

			printerdc.EndPage(); 
			if (!res)
				break;
		}
	
		logfile.Close();
		
		printerdc.EndDoc(); 

	}

	catch(...)
	{

	}

}

