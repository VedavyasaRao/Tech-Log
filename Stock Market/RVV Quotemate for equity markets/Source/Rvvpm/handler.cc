#include "stdafx.h"
#include "handler.h"
#include "cef_util.h"
#include "resource.h"
#include "utils.h"

ClientHandler::ClientHandler() :
	m_Browser(NULL), m_BrowserId(0), m_MainHwnd(NULL), m_bIsClosing(false)
{
}

ClientHandler::~ClientHandler()
{
}

void ClientHandler::OnAddressChange(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame> frame, const CefString& url)
{
}

void ClientHandler::OnTitleChange(CefRefPtr<CefBrowser> browser, const CefString& title)
{
}

void ClientHandler::OnBeforeDownload(CefRefPtr<CefBrowser> browser,	CefRefPtr<CefDownloadItem> download_item,
									const CefString& suggested_name, CefRefPtr<CefBeforeDownloadCallback> callback)
{
	REQUIRE_UI_THREAD()
	callback->Continue(suggested_name, true);
}

void ClientHandler::OnDownloadUpdated(CefRefPtr<CefBrowser> browser, CefRefPtr<CefDownloadItem> download_item,
										CefRefPtr<CefDownloadItemCallback> callback)
{
	REQUIRE_UI_THREAD()
	//if (download_item->IsComplete())
	//	PostMessage(m_MainHwnd, WM_COMMAND, ID_DOWNLOAD_COMPLETE, 0);
}

void ClientHandler::OnAfterCreated(CefRefPtr<CefBrowser> browser)
{
	REQUIRE_UI_THREAD()
	AutoLock lock_scope(this);

	if (!m_Browser.get()) {
		// Keep a reference to the main browser.
		m_Browser = browser;
		m_BrowserId = browser->GetIdentifier();
	} else if (browser->IsPopup()) {
		// Add to the list of popup browsers
		m_PopupList.push_back(browser);
	}
}

// Called when a browser has recieved a request to close.
bool ClientHandler::DoClose(CefRefPtr<CefBrowser> browser)
{
	REQUIRE_UI_THREAD()

	// Protect data members from access on multiple threads.
	AutoLock lock_scope(this);

	// Closing the main window requires special handling.
	if (m_BrowserId == browser->GetIdentifier()) {
		// Set a flag to indicate that the window close should be allowed.
		m_bIsClosing = true;
	}

	// Allow the close. For windowed browsers this will result in the OS close event being sent.
	return false;
}

void ClientHandler::OnBeforeClose(CefRefPtr<CefBrowser> browser)
{
	REQUIRE_UI_THREAD()
	AutoLock lock_scope(this);

	if (m_BrowserId == browser->GetIdentifier()) {
		CloseAllPopups(false);
		m_Browser = NULL;
	} else if (browser->IsPopup()) {
		std::list<CefRefPtr<CefBrowser> >::iterator it;
		for (it = m_PopupList.begin(); it != m_PopupList.end(); ++it) {
			if ((*it)->IsSame(browser)) {
				m_PopupList.erase(it);
				break;
			}
		}
	}
}

void ClientHandler::SetMainHwnd(CefWindowHandle hwnd)
{
	AutoLock lock_scope(this);
	m_MainHwnd = hwnd;
}

void ClientHandler::CloseAllPopups(bool force_close)
{
	std::list<CefRefPtr<CefBrowser> >::iterator it;
	for (it = m_PopupList.begin(); it != m_PopupList.end();) {
		(*it)->GetHost()->CloseBrowser(force_close);
		it = m_PopupList.erase(it);
	}
}
