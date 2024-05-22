#ifndef DATASRC
#define DATASRC

class CPMDataSource
{
protected:
	HINTERNET 	m_psess;
	HINTERNET 	m_pHttpConnect ;
	HINTERNET 	m_pFile;
	Json::Reader reader;
	Json::Value root;
	const int BUFSIZE = 1024 * 1024;
	char *readbuf;
	IniFile* m_pimDataSrcs;

protected:
	void ReadBuffer(char *readbuf, int readbuflen);

public:
	CPMDataSource();

	virtual void ConnectToServer(char *lpszUrl, char *header);
	virtual void QueryQuotes(CPMScrip* )=0;
	virtual void QueryIndices(CString& dow, CString& nasdaq) = 0;

};


class CYahooSource: public CPMDataSource
{
public:
	CYahooSource();
	void ConnectToServer(char *lpszUrl);
	void QueryQuotes(CPMScrip* );
	void QueryClose(CPMScrip* pscrip);
	void QueryIndices(CString& dow, CString& nasdaq);
	void QueryNews(CPMScrip* pscrip);
	void QueryDateTime(CString& datetime);
};

class CTDWSource: public CPMDataSource
{
private:
private:
	CString m_authorization;

public:
	CTDWSource();
	void ConnectToServer(char *lpszUrl);
	void QueryQuotes(CPMScrip* );
	void QueryIndices(CString& dow, CString& nasdaq);
};

class CAlphavantage : public CPMDataSource
{
private:
	CString m_apikey;

public:
	CAlphavantage();
	void ConnectToServer(char *lpszUrl) {};
	void QueryQuotes(CPMScrip*);
	void QueryIndices(CString& dow, CString& nasdaq);
};

class CFINNHUB : public CPMDataSource
{
private:
	CString m_token;

public:
	CFINNHUB();
	void ConnectToServer(char *lpszUrl);
	void QueryQuotes(CPMScrip*);
	void QueryIndices(CString& dow, CString& nasdaq);
};

#endif


