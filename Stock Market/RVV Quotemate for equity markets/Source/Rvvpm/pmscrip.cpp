#include <stdafx.h>
#include "resource.h"
#include <strstream>
#include  <iomanip>
#include "rvvpm.h"
#include "pmscrip.h"
#include <time.h>


void CPMScrip::init()
{
	memset (this, 0, sizeof *this);
}

CPMScrip::CPMScrip()
{
	init();
}

CPMScrip::CPMScrip(char* symbol)
{
	init();
	char szbuf[200];
	strcpy(szbuf, symbol);
	char *cptr = strchr(szbuf, ',');
	*cptr = 0;
	strcpy(m_exchange, szbuf);
	strcpy(m_symbol, cptr+1);
	m_pimmaster = &((CRVVPMApp *)AfxGetApp())->m_imMaster;
	m_pimsymbdata = &((CRVVPMApp *)AfxGetApp())->m_imSymbdata;
	m_pimDataSrcs = &((CRVVPMApp *)AfxGetApp())->m_imDataSrcs;

}


CString CPMScrip::tostr(double dbl)
{
	char buf[30];

	_gcvt(dbl, 3, buf);

	return CString(buf);
}

CPMScrip& CPMScrip::operator=(CPMScrip& tmp)
{
	memcpy (this, &tmp, sizeof tmp);
	return *this;
}

char* FillChange(char *buf, double change, CTime changetime)
{
	char temp[20];
	std::strstream	tmps;

	sprintf(temp, "%.02f ", change);
	tmps << right << std::setfill(' ') << setw(8) << temp;
	sprintf(temp, "%s", changetime.Format("%I:%M"));
	tmps <<  temp << ends;
	strcpy(buf, tmps.str());
	return buf;
}

CString CPMScrip::FullSymbol()
{
	return CString(m_exchange) + "," + CString(m_symbol);
}

char* FillRange(char *buf, double high, double low)
{
	char temp[20];
	std::strstream	tmps;
	sprintf(temp, "%.02f ", low);
	tmps << right << std::setfill(' ') << setw(8) << temp;
	sprintf(temp, "%.02f ", high);
	tmps << setw(8) << temp << ends;
	strcpy(buf, tmps.str());
	return buf;
}

CString CPMScrip::GetColumnValue(int colid,int bprint)
{

	char buf[100];
	string sbuf;
	double	d1=0.0, d2=0.0;
	
	IniFile*  m_pimmaster = &((CRVVPMApp *)AfxGetApp())->m_imMaster;
	IniFile*  m_pimsymbdata = &((CRVVPMApp *)AfxGetApp())->m_imSymbdata;
	string fullsymbol = FullSymbol().GetBuffer();

	switch(colid)
	{
		case PM_EXCHANGE:
			return m_exchange;

		case PM_SYMBOL:
			return m_symbol;

		case PM_OPEN:
			sprintf(buf,"%0.2f", m_open);
			return buf;

		case PM_ASK:
			sprintf(buf,"%0.2f", m_ask);
			return buf;

		case PM_BID:
			sprintf(buf,"%0.2f", m_bid);
			return buf;

		case PM_DAYRANGE:
			FillRange(buf, m_high, m_low);
			return buf;

		case PM_52WKRANGE:
			FillRange(buf, m_52wkhigh, m_52wklow);
			return buf;

		case PM_BOUGHT:
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "Bought", "", sbuf);
			return sbuf.c_str();

		case PM_VOLUME:
			sprintf(buf,"%0.2f", m_volume);
			return buf;

		case PM_PAID:
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "Paid", "", sbuf);
			return sbuf.c_str();

		case PM_GAIN:
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "Bought", "0.0", sbuf);
			if (sbuf[0])
				sscanf(sbuf.c_str(), "%lf", &d1);
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "Paid", "0.0", sbuf);
			if (sbuf[0])
				sscanf(sbuf.c_str(), "%lf", &d2);
			buf[0] = 0;
			if (d1 != 0.0 && d2 != 0.0)
				sprintf(buf, "%0.2f", d1*(m_lasttrade-d2));
			return buf;

		case PM_ACTVALUE:
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "Bought", "0.0", sbuf);
			if (sbuf[0])
				sscanf(sbuf.c_str(), "%lf", &d1);
			buf[0] = 0;
			if (d1 != 0.0)
				sprintf(buf, "%0.2f", d1*(m_lasttrade));
			return buf;

		case PM_CLOSEDAY1:
			sprintf(buf,"%0.2f", m_close[0]);
			return buf;

		case PM_CLOSEDAY2:
			sprintf(buf,"%0.2f", m_close[1]);
			return buf;

		case PM_CLOSEDAY3:
			sprintf(buf,"%0.2f", m_close[2]);
			return buf;

		case PM_CLOSEDAY4:
			sprintf(buf,"%0.2f", m_close[3]);
			return buf;

		case PM_CLOSEDAY5:
			sprintf(buf,"%0.2f", m_close[4]);
			return buf;

		case PM_HGHALRM:
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "HIGHPRICE", "", sbuf);
			return sbuf.c_str();

		case PM_LOWALRM:
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "LOWPRICE", "", sbuf);
			return sbuf.c_str();

		case PM_LASTTRADE:
			if (bprint == 0)

				sprintf(buf,"%0.2f", m_lasttrade);
			else
				FillChange(buf, m_lasttrade, m_changetime[0]);
			return buf;

		case PM_CHANGE:
			return FillChange(buf, m_change[0], m_changetime[0]);

		case PM_CHANGE2:
			return FillChange(buf, m_change[1], m_changetime[1]);

		case PM_CHANGE3:
			return FillChange(buf, m_change[2], m_changetime[2]);

		case PM_CHANGE4:
			return FillChange(buf, m_change[3], m_changetime[3]);

		case PM_CHANGE5:
			return FillChange(buf, m_change[4], m_changetime[4]);

		case PM_ALARAM:
			return m_alaram;

	}

	return "";
}


void CPMScrip::Calucalate()
{
	for (int i=19; i>0; --i)
	{
		m_change[i] = m_change[i-1];
		m_changepercent[i] = m_changepercent[i - 1];
		m_changetime[i] = m_changetime[i-1];
	}

	CString m_fullsymbol = m_exchange + CString(",") + m_symbol;
	CString alram;
	m_change[0] = m_tempchange;
	m_changepercent[0] = m_tempchangepercent;
	int year=1970, mon=12, day=1, hour=0, min=0;
	sscanf(m_tempchangetime, "%d %d %d %d:%d", &year, &mon, &day, &hour, &min);
	m_changetime[0] = CTime(year, mon, day, hour, min, 0);

	string fullsymbol = m_fullsymbol.GetBuffer();
	string sbuf;
	if (((CRVVPMApp *)AfxGetApp())->m_bQuoteOndemand || (m_pimDataSrcs->GetPrivateProfileInt("Settings", "EnableTrigger", 1)))
	{
		m_pimsymbdata->GetPrivateProfileString(fullsymbol, "HIGHPRICE", "", sbuf);
		if (sbuf[0])
		{
			if (m_lasttrade >= atof(sbuf.c_str()))
			{
				alram += "$P+";
			}
		}

		m_pimsymbdata->GetPrivateProfileString(fullsymbol, "LOWPRICE", "", sbuf);
		if (sbuf[0])
		{
			if (m_lasttrade <= atof(sbuf.c_str()))
			{
				alram += "$P-";
			}
		}

		m_pimmaster->GetPrivateProfileString("Triggers", "HIGHPOINTS", "", sbuf);
		if (sbuf[0])
		{
			if (m_change[0] >= atof(sbuf.c_str()))
			{
				alram += "$C+";
			}
		}

		m_pimmaster->GetPrivateProfileString("Triggers", "LOWPOINTS", "", sbuf);
		if (sbuf[0])
		{
			if (m_change[0] <= atof(sbuf.c_str()) * -1.0)
			{
				alram += "$C-";
			}
		}

		m_pimmaster->GetPrivateProfileString("Triggers", "HIGHPERCENT", "", sbuf);
		if (sbuf[0])
		{
			if (m_changepercent[0] >= atof(sbuf.c_str()))
			{
				alram += "%C+";
			}
		}

		m_pimmaster->GetPrivateProfileString("Triggers", "LOWPERCENT", "", sbuf);
		if (sbuf[0])
		{
			if (m_changepercent[0] <= atof(sbuf.c_str()) * -1.0)
			{
				alram += "%C-";
			}
		}

		int timemin;
		double pts;
		m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVEPOINTS", "", sbuf);
		if (sbuf[0])
		{
			pts = atof(sbuf.c_str());
			m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVETRADES", "", sbuf);
			if (sbuf[0])
			{
				timemin = atol(sbuf.c_str());
				for (int i = 1; i < 20; ++i)
				{
					if (m_changetime[i].GetTime() != 0)
					{
						if (difftime(m_changetime[0].GetTime(), m_changetime[i].GetTime()) <= timemin)
						{
							if (m_change[0] - m_change[i] > pts)
							{
								alram += "$I+";
								break;

							}
						}
					}
				}
			}
		}

		m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVEPOINTS", "", sbuf);
		if (sbuf[0])
		{
			pts = atof(sbuf.c_str());
			m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVETRADES", "", sbuf);
			if (sbuf[0])
			{
				timemin = atol(sbuf.c_str());
				for (int i = 1; i < 20; ++i)
				{
					if (m_changetime[i].GetTime() != 0)
					{
						if (difftime(m_changetime[0].GetTime(), m_changetime[i].GetTime()) <= timemin)
						{
							if (m_change[0] - m_change[i] < pts * -1.0)
							{
								alram += "$I-";
								break;

							}
						}
					}
				}
			}


			m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVEPRC", "", sbuf);
			if (sbuf[0])
			{
				pts = atof(sbuf.c_str());
				m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVETRADESPRC", "", sbuf);
				if (sbuf[0])
				{
					timemin = atol(sbuf.c_str());
					for (int i = 1; i < 20; ++i)
					{
						if (m_changetime[i].GetTime() != 0)
						{
							if (difftime(m_changetime[0].GetTime(), m_changetime[i].GetTime()) <= timemin)
							{
								if (m_changepercent[0] - m_changepercent[i] > pts)
								{
									alram += "%I+";
									break;

								}
							}
						}
					}
				}
			}

			m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVEPRC", "", sbuf);
			if (sbuf[0])
			{
				pts = atof(sbuf.c_str());
				m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVETRADESPRC", "", sbuf);
				if (sbuf[0])
				{
					timemin = atol(sbuf.c_str());
					for (int i = 1; i < 20; ++i)
					{
						if (m_changetime[i].GetTime() != 0)
						{
							if (difftime(m_changetime[0].GetTime(), m_changetime[i].GetTime()) <= timemin)
							{
								if (m_changepercent[0] - m_changepercent[i] < pts * -1.0)
								{
									alram += "%I-";
									break;

								}
							}
						}
					}
				}
			}
		}
	}

	if (((CRVVPMApp *)AfxGetApp())->m_bNewsOndemand || (m_pimDataSrcs->GetPrivateProfileInt("Settings", "EnableTrigger", 1)))
	{

		if (m_lastnews[0])
		{
			char buf[20];
			memset(buf, 0, sizeof buf);
			m_pimmaster->GetPrivateProfileString("Triggers", "NEWSPERIOD", "1", sbuf);
			int pos = atol(sbuf.c_str());

			CTime t1, t2;
			year = 1970; mon = 12; day = 1; hour = 0; min = 0;
			m_pimsymbdata->GetPrivateProfileString(fullsymbol, "LASTNEWS", "1995 03 08 05:30", sbuf);
			sscanf(sbuf.c_str(), "%d %d %d %d:%d", &year, &mon, &day, &hour, &min);
			t1 = CTime(year, mon, day, hour, min, 0);
			year = 1970; mon = 12; day = 1; hour = 0; min = 0;
			sscanf(m_lastnews, "%d %d %d %d:%d", &year, &mon, &day, &hour, &min);
			t2 = CTime(year, mon, day, hour, min, 0);

			double dt = difftime(t2.GetTime(), t1.GetTime());
			dt /= (60.0);
			if (dt >= pos)
			{
				alram += "N";

			}

			m_pimsymbdata->WritePrivateProfileString(fullsymbol, "LASTNEWS", m_lastnews);
		}
	}
	m_alramflag = (alram.GetLength() > 0);
	lstrcpy(m_alaram, alram);

}

