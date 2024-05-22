// isql.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "isql.h"
#include "list"

#include "MainFrm.h"
#include "ChildFrm.h"
#include "isqlDoc.h"
#include "isqlView.h"
#include "connectdsn.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CIsqlApp

BEGIN_MESSAGE_MAP(CIsqlApp, CWinApp)
	//{{AFX_MSG_MAP(CIsqlApp)
	ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
	ON_COMMAND(ID_EXECSQL, OnExecsql)
	ON_UPDATE_COMMAND_UI(ID_EXECSQL, OnUpdateExecsql)
	ON_COMMAND(ID_CONNECT, OnConnect)
	ON_COMMAND(ID_TILEHORIZ, OnTilehoriz)
	ON_UPDATE_COMMAND_UI(ID_TILEHORIZ, OnUpdateTilehoriz)
	ON_COMMAND(ID_TILEVERT, OnTilevert)
	ON_UPDATE_COMMAND_UI(ID_TILEVERT, OnUpdateTilevert)
	ON_COMMAND(ID_SHOWSQLWINDOW, OnShowsqlwindow)
	ON_UPDATE_COMMAND_UI(ID_SHOWSQLWINDOW, OnUpdateShowsqlwindow)
	//}}AFX_MSG_MAP
	// Standard file based document commands
	ON_COMMAND(ID_FILE_NEW, CWinApp::OnFileNew)
	ON_COMMAND(ID_FILE_OPEN, CWinApp::OnFileOpen)
	// Standard print setup command
	ON_COMMAND(ID_FILE_PRINT_SETUP, CWinApp::OnFilePrintSetup)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CIsqlApp construction

CIsqlApp::CIsqlApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CIsqlApp object

CIsqlApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CIsqlApp initialization

BOOL CIsqlApp::InitInstance()
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
	// You should modify this string to be something appropriate
	// such as the name of your company or organization.
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));

	LoadStdProfileSettings();  // Load standard INI file options (including MRU)

	// Register the application's document templates.  Document templates
	//  serve as the connection between documents, frame windows and views.


	CMultiDocTemplate* pDocTemplate;
	pDocTemplate = new CMultiDocTemplate(
		IDR_ISQLTYPE,
		RUNTIME_CLASS(CIsqlDoc),
		RUNTIME_CLASS(CChildFrame), // custom MDI child frame
		RUNTIME_CLASS(CIsqlView));
	AddDocTemplate(pDocTemplate);

	// create main MDI Frame window
	CMainFrame* pMainFrame = new CMainFrame;
	if (!pMainFrame->LoadFrame(IDR_MAINFRAME))
		return FALSE;
	m_pMainWnd = pMainFrame;
	pMainFrame->ShowWindow(m_nCmdShow);
	pMainFrame->UpdateWindow();

	CString stsmsg = "Please select a DSN for connection ...";
	((CMainFrame*)m_pMainWnd)->SetMessageText( stsmsg );

	RETCODE retcode; 

	retcode = SQLAllocEnv(&m_henv); /* Environment handle */ 
	if (retcode != SQL_SUCCESS) 
		return FALSE;
	retcode = SQLAllocConnect(m_henv, &m_hdbc); /* Connection handle */ 
	if (retcode != SQL_SUCCESS) 
		return FALSE;

	SQLSetConnectOption(m_hdbc, SQL_LOGIN_TIMEOUT, 5); 
	if 	(retcode != SQL_SUCCESS  && retcode != SQL_SUCCESS_WITH_INFO)
	{
		ShowSQLError();
		return FALSE;
	}

	
	CConnectDSN conDlg;
	char buf[100];
	GetPrivateProfileString("DSN", "DSN", "", buf, 100, "VSQL.INI");
	conDlg.m_dsn = buf;
	GetPrivateProfileString("DSN", "UID", "", buf, 100, "VSQL.INI");
	conDlg.m_userid = buf;
	GetPrivateProfileString("DSN", "PWD", "", buf, 100, "VSQL.INI");
	conDlg.m_chkpwd = 1;
	conDlg.m_passwd = buf;
	conDlg.m_hdbc = m_hdbc;	

	if (conDlg.DoModal() == IDCANCEL)
		return FALSE;
	pMainFrame->UpdateWindow();

	WritePrivateProfileString("DSN", "DSN", conDlg.m_dsn,     "VSQL.INI");
	WritePrivateProfileString("DSN", "UID", conDlg.m_userid,  "VSQL.INI");
	if (conDlg.m_chkpwd == 1)
		WritePrivateProfileString("DSN", "PWD", conDlg.m_passwd,  "VSQL.INI");

	m_psqlvw = getOutputView(0);
	m_psqlvw->GetDocument()->SetTitle ("SQL Commands");


	((CIsqlDoc*)m_psqlvw->GetDocument())->m_bCanClose = 1;
	m_hfont = ::CreateFont( 14, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "Courier New");


/*
	// Parse command line for standard shell commands, DDE, file open
	CCommandLineInfo cmdInfo;
	ParseCommandLine(cmdInfo);

	// Dispatch commands specified on the command line
	if (!ProcessShellCommand(cmdInfo))
		return FALSE;
*/
	// The main window has been initialized, so show and update it.
	pMainFrame->ShowWindow(m_nCmdShow);
	pMainFrame->UpdateWindow();

	return TRUE;
}

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
void CIsqlApp::OnAppAbout()
{
	CString	instr;
	m_psqlvw->GetWindowText(instr);
	instr += "\r\n\r\n";
	instr += "***********************************************************************************************************\r\n\r\n";
	instr += "Welcome to Interactive SQL\r\n\r\n";

	instr += "To execute one or more SQL statements, please follow the steps mentioned below:\r\n\r\n";

	instr += "1. Type the sql statements in the 'SQL Commands' window. Make sure that each statement is\r\n";
	instr += "   separated by either a semi-colon(;) or a colon(:).\r\n\r\n";

	instr += "2. If a qury is separated by a colon(:), the output will appear in a new window.\r\n\r\n";

	instr += "3. If a qury is separated by a semi-colon(;), the output will appear in a common window.\r\n\r\n";

	instr += "4. After typing, Highlite the sql statements to be executed. Make sure \r\n";
	instr += "   that the selection ends at either a semi-colon(;) or a colon(:).\r\n\r\n";

	instr += "5. After executing, tile all output windows either horizontally or vertically.\r\n\r\n";

	instr += "6. To get SQL Commands window back, click on the button SQL.\r\n\r\n";
	instr += "Samples\r\n\r\n";
	instr += "delete from wip_lot where lot_id like 'LOTA%';\r\n";
	instr += "select * from wip_lot:\r\n";
	instr += "select * from user_tables:\r\n";
	instr += "delete from wip_matlgroup where mg_matl_ns like 'LOTA%';\r\n";
	instr += "select * from wip_multi_prod;\r\n\r\n";
	instr += "***********************************************************************************************************\r\n\r\n";
	m_psqlvw->SetWindowText(instr);
	//m_psqlvw->SendMessage(WM_COMMAND, ID_EDIT_SELECT_ALL, 0);


	
	CAboutDlg aboutDlg;
	aboutDlg.DoModal();
}

/////////////////////////////////////////////////////////////////////////////
// CIsqlApp commands

CIsqlApp::~CIsqlApp()
{
//	SQLDisconnect(m_hdbc); 
	SQLFreeConnect(m_hdbc); 
	SQLFreeEnv(m_henv); 
	DeleteObject(m_hfont);
}

int CIsqlApp::execSQL(CString & sqltxt)
{

	RETCODE		retcode;
	CString		txt = sqltxt;
	CString		sqlstmt;	
	int			flag=!IsWindow (m_hwnd);

	if (txt == "")
		return 1;

	int pos=0, pipe=0, ofst=0;

	BeginWaitCursor();

	

	while (pos != -1)
	{
		pos = 0;
		while (1)
		{
			ofst = txt.Mid(pos).FindOneOf(":;");
			if (ofst == -1)
				break;
			pos += ++ofst;
			if (pos == txt.GetLength())
				break;
			if (txt.GetAt(pos) == '\r')
				break;
			ofst += pos;
		}

		if (ofst == -1)
			break;
		pos--;
		retcode = SQLAllocStmt(m_hdbc, &m_hstmt); /* Statement handle */
		if (retcode != SQL_SUCCESS)
			return 2;
		sqlstmt = txt.Mid(0, pos);
		sqlstmt.TrimLeft();
		sqlstmt.TrimRight();
		pipe = (txt[pos] == ';');

		if (pipe && !flag)
		{
			flag = 1;
			CString temps;
			m_poutvw->GetWindowText(temps);
			temps = "\r\n" + CString('*', 1000) + "\r\n" + temps;
			m_poutvw->SetWindowText(temps);
		}
	
		execQuery(sqlstmt, pipe);
		txt = txt.Mid(pos+1);
		SQLFreeStmt(m_hstmt, SQL_DROP);
	}
	EndWaitCursor();
	return 0;
}


int CIsqlApp::execQuery(CString & sqlstmt, int pipe)
{
	RETCODE  ret;
	CView*	pView;
	CString temps;

	ret = SQLExecDirect(m_hstmt, (UCHAR FAR *)(LPCSTR)sqlstmt, sqlstmt.GetLength());
	if 	(ret != SQL_SUCCESS  && ret != SQL_SUCCESS_WITH_INFO)
	{
		ShowSQLError(sqlstmt);
		return 1;
	}


	temps = sqlstmt;
	temps.MakeUpper();

	if ( temps.Find("SELECT") == -1 )
	{
		SDWORD	rows;
		ret = SQLRowCount(m_hstmt, &rows);
		if 	(ret != SQL_SUCCESS  && ret != SQL_SUCCESS_WITH_INFO)
		{
			ShowSQLError();
			return 1;
		}
		CString	msg;  ;
		if (rows != -1)
			msg.Format("\r\n\r\n%s\r\nRows affected = %d", sqlstmt , rows);
		else
			msg.Format("\r\n\r\n%s\r\nCompleted Successfully", sqlstmt);
		pView = getOutputView(pipe);
		if (pView == NULL)
			return 2;
		((CEditView *)pView)->GetEditCtrl().SendMessage((DWORD)WM_SETFONT, (DWORD)m_hfont, MAKELPARAM(1,0));
		pView->GetWindowText(temps);
		temps = msg + temps   ;
		pView->SetWindowText(temps);
		((CMainFrame*)m_pMainWnd)->MDIActivate(pView->GetParentFrame());
		return 0;
			
	}

	UCHAR	rgbDesc[100];
 	SWORD   cbDesc;
	SDWORD  fDesc;
	ret = SQLColAttributes(m_hstmt, -1, SQL_COLUMN_COUNT, rgbDesc, sizeof rgbDesc, &cbDesc, &fDesc);
	if 	(ret != SQL_SUCCESS  && ret != SQL_SUCCESS_WITH_INFO)
	{
		ShowSQLError();
		return 2;
	}


	SDWORD	ccol = fDesc;
	SWORD	fCType;
	SDWORD	cbValue;

	CStringArray colNames;
	CArray<int, int> colOffset;
	CArray<long, long> colType;
	CArray<long, long> colWidth;

	SDWORD	colpos=0;
	UCHAR	colBuffer[10000];
	SWORD	ibScale, fNullable;
	UDWORD	collen;

	for (int i=1; i<=ccol; ++i)
	{

		ret = SQLDescribeCol(m_hstmt, i, rgbDesc, sizeof rgbDesc, &cbDesc, &fCType , &collen, &ibScale, &fNullable);
		if 	(ret != SQL_SUCCESS  && ret != SQL_SUCCESS_WITH_INFO)
		{
			ShowSQLError();
			return 2;
		}
		temps = rgbDesc;
		temps.TrimRight();
		colNames.Add(temps);
		colWidth.Add(temps.GetLength());

		ret = SQLColAttributes(m_hstmt, i, SQL_COLUMN_LENGTH, rgbDesc, sizeof rgbDesc, &cbDesc, (LONG *)&collen);
		if 	(ret != SQL_SUCCESS  && ret != SQL_SUCCESS_WITH_INFO)
		{
			ShowSQLError();
			return 2;
		}
		collen += 50;

		ret = SQLBindCol(m_hstmt, i, SQL_C_CHAR, colBuffer+colpos, collen, &cbValue);
		if 	(ret != SQL_SUCCESS  && ret != SQL_SUCCESS_WITH_INFO)
		{
			colType.Add(fCType);
			ret = SQLBindCol(m_hstmt, i, fCType, colBuffer+colpos, collen, &cbValue);
			if 	(ret != SQL_SUCCESS  && ret != SQL_SUCCESS_WITH_INFO)
			{
				ShowSQLError();
				return 4;
			}
		}
		else
			colType.Add(SQL_C_CHAR);
			
		colOffset.Add(colpos);
		colpos+=collen;
	}
	colOffset.Add(colpos);

	
	CString			disptxt;
	CStringArray	colValues[100];
	int  prev=0;
	
	pView = getOutputView(pipe);
	if (pView == NULL)
		return 2;
	
	temps = CString("\r\n\r\n") + sqlstmt + CString("\r\n");
	disptxt += temps;
	
	int start=1;
	if (pipe != 1)
		start = ((CIsqlDoc*)pView->GetDocument())->m_startrow;
	int reccount=0;
	if (pipe != 1)
		((CIsqlDoc*)pView->GetDocument())->m_sqlstmt = sqlstmt;
	else
		((CIsqlDoc*)pView->GetDocument())->m_sqlstmt = "";

	int   index=0;
	while ( reccount++ < (99+start) )
	{
		memset (colBuffer, 0, sizeof colBuffer);
		ret = SQLFetch(m_hstmt);
		if 	(ret == SQL_SUCCESS  || ret == SQL_SUCCESS_WITH_INFO)
		{
			if (reccount < start)
				continue;
			
			colValues[index].RemoveAll();
			for (auto i=0; i<ccol; ++i)
			{
				temps = (LPCSTR)colBuffer+colOffset[i];
				temps.TrimRight();
				if (temps.GetLength() > colWidth[i])
					colWidth[i] = temps.GetLength();
				colValues[index].Add(temps);
			}
			index++;
			continue;
		}
		if (ret == SQL_NO_DATA_FOUND)
			break;

		ShowSQLError();
		return 4;
	}
	if (pipe != 1)
		((CIsqlDoc*)pView->GetDocument())->m_startrow = reccount;


	char bufx[300], buft[300];
	int ofst;
	for (int i=0; i < colNames.GetSize(); ++i)
	{
		lstrcpy(buft, colNames[i]);
		ofst = (colWidth[i] - lstrlen(buft))/2;
		memset(bufx, 32, 300);
		lstrcpy(bufx+ofst, buft);
		bufx[lstrlen(bufx)] = 32;
		bufx[colWidth[i]] = 0;
		disptxt = disptxt + bufx;
		disptxt = disptxt  + "\t";
	}

	for (int j=0; j <index; ++j)
	{
		
		disptxt = disptxt  + "\r\n";
		for (int i=0; i < colNames.GetSize(); ++i)
		{
			lstrcpy(buft, colValues[j].GetAt(i));
			ofst = (colWidth[i] - lstrlen(buft))/2;
			memset(bufx, 32, 300);
			lstrcpy(bufx+ofst, buft);
			bufx[lstrlen(bufx)] = 32;
			bufx[colWidth[i]] = 0;
			disptxt = disptxt + bufx;
			disptxt = disptxt  + "\t";
		}

	}

	
	char buf[40];
	sprintf(buf, "\r\nRows: %d - %d", start, reccount-1);
	if (start == reccount)
		sprintf(buf, "\r\nNo Rows");
	disptxt += buf;
	((CEditView *)pView)->GetEditCtrl().SendMessage((DWORD)WM_SETFONT, (DWORD)m_hfont, MAKELPARAM(1,0));
	pView->GetWindowText(temps);
	temps = disptxt + temps;
	pView->SetWindowText(temps);
	((CMainFrame*)m_pMainWnd)->MDIActivate(pView->GetParentFrame());

	return 0;
}

void CIsqlApp::ShowSQLError(CString sqlst)
{

	RETCODE  ret;
		
	UCHAR	pszSqlState[100], szErrorMsg[100];
	SDWORD	pfNativeError;
	SWORD   cbErrorMsgMax=100, pcbErrorMsg;

	CString msg;
	msg = sqlst;
	ret = SQLError(m_henv, m_hdbc, m_hstmt, pszSqlState, &pfNativeError, szErrorMsg, cbErrorMsgMax, &pcbErrorMsg);
	if 	(ret == SQL_SUCCESS  || ret == SQL_SUCCESS_WITH_INFO)
		msg = msg + "\r\n"+ szErrorMsg;

	if (msg != "")
		::MessageBox(NULL, msg, "SQL Error", 0);
}

CView* CIsqlApp::getOutputView(int pipe)
{
	if (pipe == 0 )
		OnFileNew();

	if (!IsWindow (m_hwnd) && pipe == 1)
	{
		OnFileNew();
		m_poutvw = ((CMainFrame*)m_pMainWnd)->GetActiveFrame()->GetActiveView();
		m_hwnd = m_poutvw->m_hWnd;
	}

	if (pipe == 1)
		return m_poutvw;

	return ((CMainFrame*)m_pMainWnd)->GetActiveFrame()->GetActiveView();
		
}

void CIsqlApp::OnExecsql() 
{
	// TODO: Add your command handler code here
	CString		sqltxt;

	int st,ed;
	((CEditView*)m_psqlvw)->GetEditCtrl().GetSel( st, ed);
	((CEditView*)m_psqlvw)->GetEditCtrl().GetWindowText(sqltxt);
	sqltxt = sqltxt.Mid(st, ed-st);
	execSQL(sqltxt);
}

void CIsqlApp::OnUpdateExecsql(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->Enable();
	
}



void CIsqlApp::OnConnect() 
{
	// TODO: Add your command handler code here
	CloseAllDocuments(FALSE);
	OnShowsqlwindow();
	return;


}

void CIsqlApp::OnTilehoriz() 
{
	// TODO: Add your command handler code here

	m_psqlvw->GetParentFrame()->ShowWindow(SW_HIDE );
	m_pMainWnd->SendMessage(WM_COMMAND, ID_WINDOW_TILE_HORZ, 0);
}

void CIsqlApp::OnUpdateTilehoriz(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	if (m_psqlvw)
		pCmdUI->Enable();

	
}

void CIsqlApp::OnTilevert() 
{
	// TODO: Add your command handler code here
	m_psqlvw->GetParentFrame()->ShowWindow(SW_HIDE );
	m_pMainWnd->SendMessage(WM_COMMAND, ID_WINDOW_TILE_VERT, 0);
	
}

void CIsqlApp::OnUpdateTilevert(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	if (m_psqlvw)
		pCmdUI->Enable();
	
}

void CIsqlApp::OnShowsqlwindow() 
{
	// TODO: Add your command handler code here
	m_psqlvw->GetParentFrame()->ShowWindow(SW_SHOW );
	m_psqlvw->GetParentFrame()->BringWindowToTop( );
	
}

void CIsqlApp::OnUpdateShowsqlwindow(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	if (m_psqlvw)
		pCmdUI->Enable();
	
}

void CIsqlApp::GetMoreRows(CView* pView)
{
	BeginWaitCursor();
	SQLAllocStmt(m_hdbc, &m_hstmt); 
	execQuery(((CIsqlDoc*)pView->GetDocument())->m_sqlstmt, 2 );
	SQLFreeStmt(m_hstmt, SQL_DROP);
	EndWaitCursor();

}


CDocument* CIsqlApp::OpenDocumentFile(LPCTSTR lpszFileName) 
{
	// TODO: Add your specialized code here and/or call the base class
	
	CString		oldtxt, newtxt;
	m_psqlvw->GetWindowText(oldtxt);
	if ( m_psqlvw->GetDocument()->OnOpenDocument(lpszFileName) )
	{
		m_psqlvw->GetWindowText(newtxt);
		m_psqlvw->SetWindowText(newtxt + oldtxt ) ;
	
		AddToRecentFileList(lpszFileName);		
		return m_psqlvw->GetDocument();
	}
		
	return NULL;

//	return CWinApp::OpenDocumentFile(lpszFileName);
}
