#include <stdafx.h>
#include "pmscrip.h"
#include "DatasourceImp.h"
#include "rvvpm.h"
#include "login.h"
#include <strstream>

extern "C"
{
	_declspec(dllimport) char* __cdecl QueryNews(char* ticker);
	_declspec(dllimport) char*  __cdecl QueryIndicies();
	_declspec(dllimport) char*  __cdecl QueryCloses(char* ticker);
	_declspec(dllimport) char*  __cdecl QueryQuote(char* ticker);
	_declspec(dllimport) char*  __cdecl QueryDateTime();
}

using namespace std;

#define WRITEFILE(FILENAME) CFile frw((FILENAME), CFile::modeCreate | CFile::modeWrite ); frw.Write(readbuf, sizeof readbuf); frw.Close();

CPMDataSource::CPMDataSource()
{
	readbuf = new char[BUFSIZE];

	m_psess = InternetOpen(
		"Mozilla/5.0",
		INTERNET_OPEN_TYPE_PRECONFIG,
		NULL,
		NULL,
		0);

	m_pHttpConnect	= NULL;
	m_pFile			= NULL;;
	m_pimDataSrcs = &((CRVVPMApp*)AfxGetApp())->m_imDataSrcs;

}

void removecomma(CString& astr)
{
	int idx = -1;
	while ((idx=astr.Find(',')) != -1)
	{
		astr.Delete(idx);
	}
}

void CPMDataSource::QueryQuotes(CPMScrip* pscrip)
{
	((CRVVPMApp*)AfxGetApp())->SetStatusMsg( CString("Querying data for ") + pscrip->m_symbol );

}
void CPMDataSource::ConnectToServer(char *url, char *header)
{
	while ((m_pFile = InternetOpenUrl(
		m_psess,
		url,
		header,
		-1,
		INTERNET_FLAG_SECURE,
		NULL
	)) == NULL)
	{
		printf("HttpSendRequest error : (%lu)\n", GetLastError());

		InternetErrorDlg(
			GetDesktopWindow(),
			m_pFile,
			ERROR_INTERNET_CLIENT_AUTH_CERT_NEEDED,
			FLAGS_ERROR_UI_FILTER_FOR_ERRORS |
			FLAGS_ERROR_UI_FLAGS_GENERATE_DATA |
			FLAGS_ERROR_UI_FLAGS_CHANGE_OPTIONS,
			NULL);
	}

}


void CPMDataSource::ReadBuffer(char *readbuf, int readbuflen)
{
	DWORD dwBytesRead = 0;
	BOOL bRead = TRUE;
	DWORD idx = 0;

	try
	{
		memset(readbuf, 0, readbuflen);
		do
		{
			bRead = InternetReadFile(
				m_pFile,
				readbuf+idx,
				readbuflen - 2,
				&dwBytesRead);
			idx += dwBytesRead;
		} while (bRead && dwBytesRead > 0);
		readbuf[idx + 1] = 0;
	}
	catch(...)
	{
	}
	FILE *fp = fopen("d:\\result.html", "wt");
	fprintf(fp, "%s", readbuf);
	fclose(fp);

	InternetCloseHandle(m_pFile);
}

void getDatestr(char *buf, int hour, int min)
{
	CTime tm;
	tm = tm.GetCurrentTime();
	tm = tm - CTimeSpan(0, 9, 30, 0);
	tm = CTime(tm.GetYear(),tm.GetMonth(),tm.GetDay(), hour, min, 0);
	sprintf(buf, "%d %d %d %d:%d", tm.GetYear(),tm.GetMonth(),tm.GetDay(), hour, min);
}


CYahooSource::CYahooSource() :CPMDataSource()
{
}

void CYahooSource::ConnectToServer(char *lpszUrl)
{
	CPMDataSource::ConnectToServer(lpszUrl, "");
}

void CYahooSource::QueryQuotes(CPMScrip* pscrip)
{
	int p1, p2;
	CString temp;
	string temps;
	int hour=12, min=0;
	ZeroMemory(readbuf, BUFSIZE);
	strcpy(readbuf, ::QueryQuote(pscrip->m_symbol));
	if (reader.parse(readbuf, root, false))
	{
		temp = CString(((root["quote"])["lasttrade"]).asString().c_str());
		if (temp.IsEmpty())
			return;
		removecomma(temp);
		pscrip->m_lasttrade = atof(temp);

		temp = ((root["quote"])["lasttradetime"]).asString().c_str();
		auto p = temp.Find(" at ");
		if (p != -1)
		{
			temp = temp.Mid(p + 4);
			//sscanf(, "%*s %*s %d:%d", &hour, &min);
			sscanf(temp, "%d:%d", &hour, &min);
			getDatestr(pscrip->m_tempchangetime, hour, min);
		}

		temp = CString(((root["quote"])["change"]).asString().c_str());
		removecomma(temp);
		pscrip->m_tempchange = atof(temp);

		temps = temp.GetBuffer();
		p1 = temps.find('(');
		p2 = temps.find('%');
		if (p1 != string::npos && p2 != string::npos)
		{
			temps = temps.substr(p1 + 1, p2 - p1 - 1);
			pscrip->m_tempchangepercent = atof(temps.c_str());
		}

		temp = CString(((root["quote"])["open"]).asString().c_str());
		removecomma(temp);
		pscrip->m_open = atof(temp);

		temp = CString(((root["quote"])["volume"]).asString().c_str());
		removecomma(temp);
		pscrip->m_volume = atof(temp.GetBuffer())/100000;

		temp = CString(((root["quote"])["ask"]).asString().c_str());
		removecomma(temp);
		sscanf(temp, "%lf", &pscrip->m_ask);

		temp = CString(((root["quote"])["bid"]).asString().c_str());
		removecomma(temp);
		sscanf(temp, "%lf", &pscrip->m_bid);

		temp = CString(((root["quote"])["dayrange"]).asString().c_str());
		removecomma(temp);
		sscanf(temp, "%lf - %lf", &pscrip->m_low, &pscrip->m_high);

		temp = CString(((root["quote"])["week52range"]).asString().c_str());
		removecomma(temp);
		sscanf(temp, "%lf - %lf", &pscrip->m_52wklow, &pscrip->m_52wkhigh);
	}
	return;

}

void CYahooSource::QueryClose(CPMScrip* pscrip)
{
	if (pscrip->m_prvclose > 0.0)
		return;
	
	char szbuf[100];
	int year, mon, day;
	ZeroMemory(readbuf, BUFSIZE);
	strcpy(readbuf, ::QueryCloses(pscrip->m_symbol));

	if (reader.parse(readbuf, root, false))
	{
		for (int k = 0; k < 6; ++k)
		{
			sprintf(szbuf, "close%d", k);
			pscrip->m_close[k] = ((root["PrevCloses"])[szbuf]).asDouble();
			
			sprintf(szbuf, "day%d", k);
			string temps = ((root["PrevCloses"])[szbuf]).asString().c_str();
			if (!temps.empty() && strstr(temps.c_str(),"null") != 0)
			{
				sscanf(temps.c_str(), "%d %d %d", &year, &mon, &day);
				pscrip->m_closeday[k] = CTime(year, mon, day, 0, 0, 0);
			}
		}

		pscrip->m_prvclose = pscrip->m_close[0];
		for (int i = 0; i<6; ++i)
			pscrip->m_close[i] -= pscrip->m_close[i + 1];
	}
}


void CYahooSource::QueryNews(CPMScrip* pscrip)
{
	char szbuf[50];
	sprintf(szbuf, "%s:%s", pscrip->m_exchange, pscrip->m_symbol);
	ZeroMemory(readbuf, BUFSIZE);
	strcpy(readbuf, ::QueryNews(szbuf));
	if (reader.parse(readbuf, root, false))
	{
		CString temp = CString(((root["news"])["timestamp"]).asString().c_str());
		if (!temp.IsEmpty())
			strcpy(pscrip->m_lastnews, temp);
	}
}


void CYahooSource::QueryIndices(CString& dow, CString& nasdaq)
{

	char szbuf[100];
	ZeroMemory(readbuf, BUFSIZE);
	strcpy(readbuf, ::QueryIndicies());
	if (reader.parse(readbuf, root, false))
	{
		CString temp = CString(((root["Indices"])["Dow"]).asString().c_str());
		removecomma(temp);
		sprintf(szbuf, "%d", atoi(temp.GetBuffer()));
		dow = CString("Dow: ") + CString(szbuf);

		temp = CString(((root["Indices"])["Nasdaq"]).asString().c_str());
		removecomma(temp);
		sprintf(szbuf, "%d", atoi(temp.GetBuffer()));
		nasdaq = CString("Nasdaq: ") + CString(szbuf);
	}
	return;
}

void CYahooSource::QueryDateTime(CString& datetime)
{
	char szbuf[100];
	ZeroMemory(readbuf, BUFSIZE);
	strcpy(readbuf, ::QueryDateTime());
	if (reader.parse(readbuf, root, false))
	{
		datetime = CString(((root["DateTime"])).asString().c_str());
	}
	return;
}


CTDWSource::CTDWSource():CPMDataSource()
{
	CRVVPMApp *pApp = (CRVVPMApp*)AfxGetApp();
	string sbuf;
	m_pimDataSrcs->GetPrivateProfileString("TD_WATHERHOUSE", "value", "", sbuf);
	m_authorization = sbuf.c_str();

}

void CTDWSource::ConnectToServer(char *url)
{
	char szBuf[2000];
	sprintf(szBuf, "Authorization: Bearer %s", m_authorization);
	CPMDataSource::ConnectToServer(url, szBuf);

}

void CTDWSource::QueryIndices(CString& dow, CString& nasdaq)
{
	ConnectToServer("https://api.tdameritrade.com/v1/marketdata/$DJI/quotes");
	ReadBuffer(readbuf, sizeof readbuf);
	if (reader.parse(readbuf, root, false))
	{
		dow = CString("Dow:")+ CString(((root["$DJI"])["netChange"]).asString().c_str());
	}


	ConnectToServer("https://api.tdameritrade.com/v1/marketdata/$COMPX/quotes");
	ReadBuffer(readbuf, sizeof readbuf);
	if (reader.parse(readbuf, root, false))
	{
		nasdaq = CString("Nasdaq:") + CString(((root["$COMPX"])["netChange"]).asString().c_str());
	}
}

void CTDWSource::QueryQuotes(CPMScrip* pscrip)
{
	return;
}

CAlphavantage::CAlphavantage() :CPMDataSource()
{
	CRVVPMApp *pApp = (CRVVPMApp*)AfxGetApp();
	string sbuf;
	m_pimDataSrcs->GetPrivateProfileString("ALPHAVANTAGE", "value", "", sbuf);
	m_apikey = sbuf.c_str();
}

void CAlphavantage::QueryIndices(CString& dow, CString& nasdaq)
{
	char szbuf[1000];
	sprintf(szbuf, "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=%s&apikey=%s", "^DJI", m_apikey);
	ConnectToServer(szbuf);
	ReadBuffer(readbuf, sizeof readbuf);

}

void CAlphavantage::QueryQuotes(CPMScrip* pscrip)
{
	return;
}

void CFINNHUB::ConnectToServer(char *lpszUrl) 
{
	CPMDataSource::ConnectToServer(lpszUrl, "");
}

CFINNHUB::CFINNHUB() :CPMDataSource()
{
	CRVVPMApp *pApp = (CRVVPMApp*)AfxGetApp();
	string sbuf;
	m_pimDataSrcs->GetPrivateProfileString("FINNHUB", "value", "", sbuf);
	m_token = sbuf.c_str();

}

void CFINNHUB::QueryIndices(CString& dow, CString& nasdaq)
{
	char szbuf[1000];
	sprintf(szbuf, "https://finnhub.io/api/v1/quote?symbol=%s&&token=%s", "^DJI", m_token);
	ConnectToServer(szbuf);
	ReadBuffer(readbuf, sizeof readbuf);
	if (reader.parse(readbuf, root, false))
	{
		sprintf(szbuf, "%d", root["c"].asInt() - root["pc"].asInt());
		dow = CString("Dow:") + CString(szbuf);
	}

	sprintf(szbuf, "https://finnhub.io/api/v1/quote?symbol=%s&&token=%s", "^IXIC", m_token);
	ConnectToServer(szbuf);
	ReadBuffer(readbuf, sizeof readbuf);
	if (reader.parse(readbuf, root, false))
	{
		sprintf(szbuf, "%d", root["c"].asInt() - root["pc"].asInt());
		nasdaq = CString("Nasdaq:") + CString(szbuf);
	}
}

void CFINNHUB::QueryQuotes(CPMScrip* pscrip)
{
	return;
}


