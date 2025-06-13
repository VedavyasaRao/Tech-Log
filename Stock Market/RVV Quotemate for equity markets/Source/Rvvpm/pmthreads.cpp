#include <stdafx.h>
#include "rvvpm.h"
#include "pmscrip.h"
#include "DatasourceImp.h"


UINT PMClockIndicesThread( LPVOID pParam )
{
	CRVVPMApp*  pApp =  (CRVVPMApp*)AfxGetApp();

	CYahooSource	indexsrc;
	CString nasdaq, dow, curtime;
	IniFile         *pimDataSrcs = &pApp->m_imDataSrcs;
	CString datetimestr;
	int quotetimeout=0, newstimeout = 0;
	indexsrc.QueryDateTime(datetimestr);
	int year = 1970, mon = 12, day = 1, hour = 0, min = 0, sec;
	char ampm[10];
	sscanf(datetimestr.GetBuffer(), "%d-%d-%d %d:%d:%d", &year, &mon, &day,  &hour, &min, &sec);
	CTime nyctime (year, mon, day, hour, min, sec);
	auto difftime = CTime::GetCurrentTime() - nyctime;

	while (1)
	{
		if (pApp->m_quit)
			break;

		indexsrc.QueryIndices(dow, nasdaq);
		curtime = (CTime::GetCurrentTime() - difftime).Format("%I:%M %p");

		pApp->SetPaneInfo( nasdaq, dow, curtime);
		
		int ret = pimDataSrcs->GetPrivateProfileInt("Settings", "AutoRefresh", 1);
		if (ret)
		{
			int ret = pimDataSrcs->GetPrivateProfileInt("Settings", "QuoteRefresh", 5);
			if (++quotetimeout >= ret)
			{
				quotetimeout = 0;
				pApp->AddJob(CQMJob::QureyQuotes);
			}

			ret = pimDataSrcs->GetPrivateProfileInt("Settings", "NewsRefresh", 5);
			if (++newstimeout >= ret)
			{
				newstimeout = 0;
				pApp->AddJob(CQMJob::QueryNews);
			}
		}

		Sleep(60000);
	}
	SetEvent(pApp->m_quit_evts[0]);
	pApp->bstopped[0] = true;
	return 0;
}

UINT PMQuotesNewsThread( LPVOID pParam )
{

	CRVVPMApp*  pApp =  (CRVVPMApp*)AfxGetApp();

	while(1)
	{
		WaitForSingleObject(pApp->m_queryevent, INFINITE);
		if (pApp->m_quit)
			break;
		pApp->ProcessJob();
	}

	pApp->bstopped[1] = true;
	SetEvent(pApp->m_quit_evts[1]);
	return 0;
}



UINT PMAlaramThread( LPVOID pParam )
{
	CRVVPMApp* pApp = (CRVVPMApp*)AfxGetApp();

	CEvent&		alertevent = pApp->m_alertevent;
	HWND hwnd = AfxGetMainWnd()->m_hWnd;
	NOTIFYICONDATA* pIconData = &pApp->m_IconData;
	NOTIFYICONDATA IconData;

	memcpy(&IconData, pIconData, sizeof IconData);
	IconData.hIcon = AfxGetApp()->LoadIcon(IDI_DUMMY);

	char buf1[300], buf2[50] = "Alert  Alert  Alert";
		
	GetWindowText(hwnd, buf1, sizeof buf1);
	while (1)
	{
		WaitForSingleObject(alertevent, INFINITE);
		if (pApp->m_quit)
			break;
		if (IsWindowVisible(hwnd))
		{
			for (int i=0; i<20; ++i)
			{
				SetWindowText(hwnd, buf2);
				FlashWindow(hwnd, FALSE);
				Sleep(300);
				SetWindowText(hwnd, buf1);
				FlashWindow(hwnd, TRUE);
				Sleep(1000);
			}
		}
		else
		{
			while(!IsWindowVisible(hwnd))
			{
				Shell_NotifyIcon(NIM_MODIFY, &IconData);
				Sleep(300);
				Shell_NotifyIcon(NIM_MODIFY, pIconData);
				Sleep(1000);
			}
		}
	} 
	pApp->bstopped[2] = true;
	SetEvent(pApp->m_quit_evts[2]);
	return 0;
}

