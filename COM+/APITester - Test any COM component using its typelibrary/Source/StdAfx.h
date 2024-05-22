// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__0784C1A7_A947_11D4_A50A_F77134B5463F__INCLUDED_)
#define AFX_STDAFX_H__0784C1A7_A947_11D4_A50A_F77134B5463F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdisp.h>        // MFC Automation classes
#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
//#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#include <afxtempl.h>
#include <vector>
#include <istream>
#include <ostream>
#include <fstream>
#include "comdef.h"
#include "tlbinfo.h"
#include "comadmin.h"
#include "superedit.h"

class CAPIParameterInfo
{
public:
	CString			m_name;
	int				foptional;
	int				ldirection;
	VARTYPE			ldatatype;
	VARTYPE			lelemtype;

	CString			m_strvalue;
	CStringArray	m_arvalue;
	IDispatch*		m_pdisp[100];

	CAPIParameterInfo();
	CAPIParameterInfo&  operator=(CAPIParameterInfo& in);

	friend std::istream& operator >> (std::istream& is,  CAPIParameterInfo& paraminfo);
	friend std::ostream& operator<<(std::ostream& os, const CAPIParameterInfo& paraminfo);

};

class CAPIFunctionInfo
{
public:
	CString			m_progid;
	CString			m_funcname;
	CString			m_result;
	CString			m_coclassguidstr;

	CArray<CAPIParameterInfo, CAPIParameterInfo&>	m_parameters;
	void operator=(const CAPIFunctionInfo& in);
	CAPIFunctionInfo(const CAPIFunctionInfo& in);
	CAPIFunctionInfo();
	void Clear();
	friend std::istream& operator>>(std::istream& is, CAPIFunctionInfo& apifunc);
	friend std::ostream& operator<<(std::ostream& os, const CAPIFunctionInfo& apifunc);
};

typedef
struct
{
	CString		progid;
	CString		coclassguidstr;
	std::vector<InterfaceInfo*>	pclassinfovec;
} ProgidCoClassname;
extern std::vector<ProgidCoClassname> g_coclasses;
class Serializer
{
public:
	void readline(std::istream&  is);
	CString readstring(std::istream&  is);
	int readint(std::istream&  is);
	void writestring(std::ostream& os, const CString& s);
	void writeint(std::ostream& os, int i);
private:
	char readbuf[1000];

};

//#endif // _AFX_NO_AFXCMN_SUPPORT


//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__0784C1A7_A947_11D4_A50A_F77134B5463F__INCLUDED_)
