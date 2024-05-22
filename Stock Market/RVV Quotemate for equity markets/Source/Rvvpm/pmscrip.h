#ifndef PMSCRIP
#define PMSCRIP

class CPMScrip
{
public:
	char			m_exchange[20];
	char			m_symbol[20];
	char			m_alaram[20];
	double			m_open;
	double			m_high;
	double			m_low;
	double			m_volume;
	double			m_ask;
	double			m_bid;
	double			m_52wkhigh;
	double			m_52wklow;
	double			m_lasttrade;
	double			m_prvclose;
	double			m_close[20];
	CTime			m_closeday[20];
	double			m_change[20];
	double			m_changepercent[20];
	CTime			m_changetime[20];
	char			m_tempchangetime[20];
	double			m_tempchange;
	double			m_tempchangepercent;
	char			m_lastnews[500];
	int				m_alramflag;

private:
	IniFile*  m_pimmaster;
	IniFile*  m_pimsymbdata;
	IniFile*  m_pimDataSrcs;

private:
	void init();
	CString	tostr(double);

public:
	CPMScrip();
	CPMScrip(char *);
	void Calucalate();
	CString GetColumnValue(int,int=0);
	CPMScrip& operator=(CPMScrip& tmp);
	CString FullSymbol();
};

#endif