
#pragma once
#include <string>
#include <unordered_map>
using namespace std;

struct IniSection
{
	list<string> keys;
	unordered_map<string,string>  section;
};

 class  IniFile
{
public:
	void Serialize(const string& inifilename);
	void Persist();
	void GetPrivateProfileString(const string&  lpAppName,  const string& lpKeyName,  const string& lpDefault,  string& lpReturnedString);
	int GetPrivateProfileInt(const string&  lpAppName,  const string& lpKeyName,  int lpDefault);
	void WritePrivateProfileString(const string&  lpAppName,  const string& lpKeyName,  const string& lpString);
	void DeleteKey(const string&  lpAppName, const string& lpKeyName);
	void DeleteSection(const string&  lpAppName);
	void DeleteFile();
	list<string> GetKeys(const string&  lpAppName);
	list<string> GetSectionNames();
	void Backup();
	void Restore();

private:
	list<string> sections;
	list<string> sections_backup;
	unordered_map <string, IniSection> ini_file;
	unordered_map <string, IniSection> ini_file_backup;
	string filename;
	bool m_changed;

};

