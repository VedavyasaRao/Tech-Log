
#include "StdAfx.h"
#include "IniFile.h"


void IniFile::Serialize(const string& inifilename)
{
	m_changed = false;
	filename = inifilename;
	char	lpszReturnBuffer[5000], *pprtptr = lpszReturnBuffer;
	memset(lpszReturnBuffer, 0, sizeof lpszReturnBuffer);
	::GetPrivateProfileSectionNames(lpszReturnBuffer, sizeof lpszReturnBuffer, inifilename.c_str());
	while (strlen(pprtptr))
	{
		char szBuffer[25000], *pprtptr2 = szBuffer;
		memset(szBuffer, 0, sizeof szBuffer);
		::GetPrivateProfileSection(pprtptr, szBuffer, sizeof szBuffer, inifilename.c_str());
		char *cptr, szbuf[15000], *cptr2 = "Khri$ha";
		
		while (strlen(pprtptr2) )
		{
			lstrcpy(szbuf, pprtptr2);
			cptr = strchr(szbuf, '=');
			if (cptr)
			{
				*cptr = 0;
				cptr2 = cptr + 1;
			}
			WritePrivateProfileString(pprtptr, szbuf, cptr2);
			pprtptr2 += strlen(pprtptr2) + 1;
		}
		pprtptr += strlen(pprtptr) + 1;
	}
}

void IniFile::GetPrivateProfileString(const string&  lpAppName,  const string& lpKeyName,  const string& lpDefault,  string& lpReturnedString)
{
	auto titr = ini_file.find(lpAppName);
	if (titr != ini_file.end())
	{
		auto  titr2 =  titr->second.section.find(lpKeyName);
		lpReturnedString =  (titr2 != titr->second.section.end())?titr2->second:lpDefault;
	}
	else
		lpReturnedString =  lpDefault;
}

int IniFile::GetPrivateProfileInt(const string&  lpAppName, const string& lpKeyName, int lpDefault)
{
	string lpReturnedString;
	GetPrivateProfileString(lpAppName, lpKeyName, "", lpReturnedString);
	return lpReturnedString[0] ? atol(lpReturnedString.c_str()) : lpDefault;
}


void IniFile::DeleteKey(const string&  lpAppName, const string& lpKeyName)
{
	m_changed = true;
	ini_file[lpAppName].section.erase(lpKeyName);
	ini_file[lpAppName].keys.remove(lpAppName);
}

void IniFile::DeleteSection(const string&  lpAppName)
{
	m_changed = true;
	ini_file.erase(lpAppName);
	sections.remove(lpAppName);
}

void IniFile::DeleteFile()
{
	m_changed = true;
	ini_file= unordered_map <string, IniSection>();
	sections = list<string>();
}

void IniFile::WritePrivateProfileString(const string&  lpAppName,  const string& lpKeyName,  const string& lpString)
{
	m_changed = true;
	if (ini_file.find(lpAppName) == ini_file.end())
		sections.push_back(lpAppName);
	if (ini_file[lpAppName].section.find(lpKeyName) == ini_file[lpAppName].section.end())
		ini_file[lpAppName].keys.push_back(lpKeyName);
	ini_file[lpAppName].section.insert_or_assign(lpKeyName, lpString);
}

list<string>  IniFile::GetKeys(const string&  lpAppName)
{
	return ini_file[lpAppName].keys;

}

list<string> IniFile::GetSectionNames()
{
	return sections;

}

void IniFile::Backup()
{
	ini_file_backup = ini_file;
	sections_backup = sections;
}


void IniFile::Restore()
{
	ini_file = ini_file_backup;
	sections = sections_backup;
}

void IniFile::Persist()
{
	if (m_changed == false)
		return;
	char secbuf[25000];
	memset(secbuf, 0, sizeof secbuf);
	for (auto titr = sections.begin(); titr != sections.end(); ++titr)
	{
		char *cptr = secbuf;
		memset(secbuf, 0, sizeof secbuf);
		::WritePrivateProfileSection(titr->c_str(), nullptr, filename.c_str());
		for (auto titr2 = ini_file[*titr].keys.begin(); titr2 != ini_file[*titr].keys.end(); ++titr2)
		{
			auto temp = ini_file[*titr].section[*titr2];
			if (temp.compare("Khri$ha") == 0)
				sprintf(cptr, "%s", titr2->c_str());
			else
				sprintf(cptr, "%s=%s", titr2->c_str(),temp.c_str());
			cptr += (strlen(cptr) + 1);
		}
		::WritePrivateProfileSection(titr->c_str(), secbuf, filename.c_str());
	}
}


