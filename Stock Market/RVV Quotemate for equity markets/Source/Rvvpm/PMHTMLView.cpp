// PMHTMLView.cpp : implementation file
//

#include "stdafx.h"
#include "include/cef_app.h"
#include "RVVPM.h"
#include "PMHTMLView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CPMHTMLView

IMPLEMENT_DYNCREATE(CPMHTMLView, CView)
CPMHTMLView::CPMHTMLView():m_cefIsInitialized(false)
{
	//{{AFX_DATA_INIT(CPMHTMLView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}

CPMHTMLView::~CPMHTMLView()
{
	if (m_cefIsInitialized) {
		CefShutdown();
		m_cefIsInitialized = false;
	}
}

void CPMHTMLView::OnSize(UINT nType, int w, int h)
{
	CView::OnSize(nType, w, h);
	if (h == 0)
		return;
	CefWindowHandle wnd = m_browserHandler->GetBrowser()->GetHost()->GetWindowHandle();
	::MoveWindow(wnd, 0, 0, w,h, TRUE);
}

bool CPMHTMLView::init()
{
	m_browserHandler = new ClientHandler;
	CefMainArgs mainArgs(::GetModuleHandle(NULL));
	int rc = CefExecuteProcess(mainArgs, NULL, NULL);
	if (rc >= 0)
		return false;

	CefSettings settings;
	settings.multi_threaded_message_loop = true;
	settings.no_sandbox = true;
//	settings.single_process = false;

	if (!CefInitialize(mainArgs, settings, NULL, NULL)) {
		AfxMessageBox(_T("Failed to initialize CEF."), MB_ICONERROR);
		return false;
	}

	m_cefIsInitialized = true;

	CefRefPtr<CefCommandLine> commandLine = CefCommandLine::CreateCommandLine();
	commandLine->InitFromString(::GetCommandLineW());

	CWnd *parent = dynamic_cast<CWnd*>(this);
	HWND parentWnd = parent->GetSafeHwnd();
	RECT rect;
	parent->GetClientRect(&rect);
	CefWindowInfo windowInfo;
	CefBrowserSettings bsettings;
	windowInfo.SetAsChild(parentWnd, rect);
	m_browserHandler->SetMainHwnd(parentWnd);
	CefBrowserHost::CreateBrowser(windowInfo, m_browserHandler.get(), "", bsettings, NULL,NULL);
	return true;
}

void CPMHTMLView::OnDraw(CDC *dc)
{
	CView::OnDraw(dc);
}

void CPMHTMLView::OnInitialUpdate()
{
	CView::OnInitialUpdate();
	if (m_cefIsInitialized)
		return;
	init();
}

UINT Scrollfunc(LPVOID pParam)
{	std::tuple<CefBrowser *, int> *scrolldata = (std::tuple<CefBrowser *, int>*)pParam;
	CefBrowser *browser = std::get<0>(*scrolldata);
	int scroll = std::get<1>(*scrolldata);
	while (!browser->IsLoading())
		::Sleep(200);
	::Sleep(1000);

	INPUT input;
	POINT pos;
	GetCursorPos(&pos);

	input.type = INPUT_MOUSE;
	input.mi.dwFlags = MOUSEEVENTF_WHEEL;
	input.mi.time = NULL; //Windows will do the timestamp
	input.mi.mouseData = 120*-scroll; //A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.
	input.mi.dx = pos.x;
	input.mi.dy = pos.y;
	input.mi.dwExtraInfo = GetMessageExtraInfo();
	SendInput(1, &input, sizeof(input));

	//CefMouseEvent mouse_event;
	//mouse_event.Reset();
	//browser->GetHost()->SendMouseWheelEvent(mouse_event, 0, -scroll);

	return 0;
}

void CPMHTMLView::Navigate(CString url, int scroll)
{
	CefRefPtr<CefBrowser> browser = m_browserHandler->GetBrowser();
	browser->GetMainFrame()->LoadURL(CefString(url));
	m_scrolldata = std::tuple<CefBrowser *, int>((CefBrowser *)browser, scroll);

	// Convert screen coordinates to client coordinates. 
	POINT pt;                  // cursor location  
	RECT rc;
	GetClientRect(&rc);
	pt.x = rc.left + 50;
	pt.y = rc.top + 50;
	ClientToScreen(&pt);
	SetCursorPos(pt.x, pt.y);
	AfxBeginThread(Scrollfunc, &m_scrolldata);
}

void CPMHTMLView::NavigateBack()
{
	CefRefPtr<CefBrowser> browser = m_browserHandler->GetBrowser();
	browser->GoBack();
}

void CPMHTMLView::NavigateForward()
{
	CefRefPtr<CefBrowser> browser = m_browserHandler->GetBrowser();
	browser->GoForward();
}

BEGIN_MESSAGE_MAP(CPMHTMLView, CView)
	//{{AFX_MSG_MAP(CPMHTMLView)
	ON_WM_SIZE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CPMHTMLView diagnostics

#ifdef _DEBUG
void CPMHTMLView::AssertValid() const
{
	CHtmlView::AssertValid();
}

void CPMHTMLView::Dump(CDumpContext& dc) const
{
	CHtmlView::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CPMHTMLView message handlers
