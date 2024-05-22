// Machine generated IDispatch wrapper class(es) created with ClassWizard

#include "stdafx.h"
#include "dfsbasecontext.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



/////////////////////////////////////////////////////////////////////////////
// ICSIMBaseContext properties

/////////////////////////////////////////////////////////////////////////////
// ICSIMBaseContext operations

LPDISPATCH ICSIMBaseContext::GetGCInterface1()
{
	LPDISPATCH result;
	InvokeHelper(0x1, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void ICSIMBaseContext::SetGCInterface1(LPDISPATCH newValue)
{
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0x1, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 newValue);
}


CString ICSIMBaseContext::GetAllContext(VARIANT* pvbstrContextNames, VARIANT* pvContextValues)
{
	CString result;
	static BYTE parms[] =
		VTS_PVARIANT VTS_PVARIANT;
	InvokeHelper(0x2, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		pvbstrContextNames, pvContextValues);
	return result;
}

CString ICSIMBaseContext::GetOneContextVariable(LPCTSTR bstrContextName, BOOL* pvBoolExists, VARIANT* pvContextValue)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR VTS_PBOOL VTS_PVARIANT;
	InvokeHelper(0x3, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrContextName, pvBoolExists, pvContextValue);
	return result;
}

CString ICSIMBaseContext::SetOneContextVariable(LPCTSTR bstrContextName, const VARIANT& vContextValue, BOOL boolAddContext, BOOL boolOverrideContext)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT VTS_BOOL VTS_BOOL;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrContextName, &vContextValue, boolAddContext, boolOverrideContext);
	return result;
}

CString ICSIMBaseContext::DeleteOneContextVariable(LPCTSTR bstrContextName)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x5, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrContextName);
	return result;
}

CString ICSIMBaseContext::Serialize(LPCTSTR bstrFileName, BOOL boolStoring)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR VTS_BOOL;
	InvokeHelper(0x6, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrFileName, boolStoring);
	return result;
}

CString ICSIMBaseContext::GetBstrGC1()
{
	CString result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_BSTR, (void*)&result, NULL);
	return result;
}


void ICSIMBaseContext::SetBstrGC1(LPCTSTR lpszNewValue)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x7, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 lpszNewValue);
}

CString ICSIMBaseContext::GetAllContextForNestedGC(LPCTSTR bstrNestedGCName, VARIANT* pvbstrContextNames, VARIANT* pvContextValues)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR VTS_PVARIANT VTS_PVARIANT;
	InvokeHelper(0x8, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrNestedGCName, pvbstrContextNames, pvContextValues);
	return result;
}

CString ICSIMBaseContext::RenameOneContextVariable(LPCTSTR bstrContextOldName, LPCTSTR bstrContextNewName)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x9, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		bstrContextOldName, bstrContextNewName);
	return result;
}

CString ICSIMBaseContext::MergeInContext(LPDISPATCH pifGC, BOOL boolOverrideContext)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH VTS_BOOL;
	InvokeHelper(0xa, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		pifGC, boolOverrideContext);
	return result;
}

CString ICSIMBaseContext::GetGCInterface(LPDISPATCH* ppifGC)
{
	CString result;
	static BYTE parms[] =
		VTS_PDISPATCH;
	InvokeHelper(0xb, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		ppifGC);
	return result;
}

CString ICSIMBaseContext::SetGCInterface(LPDISPATCH pifGC)
{
	CString result;
	static BYTE parms[] =
		VTS_DISPATCH;
	InvokeHelper(0xc, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		pifGC);
	return result;
}

CString ICSIMBaseContext::GetBstrGC(BSTR* pbstrGC)
{
	CString result;
	static BYTE parms[] =
		VTS_PBSTR;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		pbstrGC);
	return result;
}

CString ICSIMBaseContext::SetBstrGC(LPCTSTR BstrGC)
{
	CString result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0xe, DISPATCH_METHOD, VT_BSTR, (void*)&result, parms,
		BstrGC);
	return result;
}
