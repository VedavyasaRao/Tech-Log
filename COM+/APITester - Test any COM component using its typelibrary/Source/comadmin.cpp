// Machine generated IDispatch wrapper class(es) created with ClassWizard

#include "stdafx.h"
#include "comadmin.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



/////////////////////////////////////////////////////////////////////////////
// ICOMAdminCatalog properties

/////////////////////////////////////////////////////////////////////////////
// ICOMAdminCatalog operations

LPDISPATCH ICOMAdminCatalog::GetCollection(LPCTSTR bstrCollName)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		bstrCollName);
	return result;
}

LPDISPATCH ICOMAdminCatalog::Connect(LPCTSTR bstrConnectString)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x2, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		bstrConnectString);
	return result;
}

long ICOMAdminCatalog::GetMajorVersion()
{
	long result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

long ICOMAdminCatalog::GetMinorVersion()
{
	long result;
	InvokeHelper(0x4, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

void ICOMAdminCatalog::ImportComponent(LPCTSTR bstrApplIdOrName, LPCTSTR bstrCLSIDOrProgId)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR;
	InvokeHelper(0x6, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrApplIdOrName, bstrCLSIDOrProgId);
}

void ICOMAdminCatalog::InstallComponent(LPCTSTR bstrApplIdOrName, LPCTSTR bstrDLL, LPCTSTR bstrTLB, LPCTSTR bstrPSDLL)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR;
	InvokeHelper(0x7, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrApplIdOrName, bstrDLL, bstrTLB, bstrPSDLL);
}

void ICOMAdminCatalog::ShutdownApplication(LPCTSTR bstrApplIdOrName)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x8, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrApplIdOrName);
}

void ICOMAdminCatalog::ExportApplication(LPCTSTR bstrApplIdOrName, LPCTSTR bstrApplicationFile, long lOptions)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_I4;
	InvokeHelper(0x9, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrApplIdOrName, bstrApplicationFile, lOptions);
}

void ICOMAdminCatalog::InstallApplication(LPCTSTR bstrApplicationFile, LPCTSTR bstrDestinationDirectory, long lOptions, LPCTSTR bstrUserId, LPCTSTR bstrPassword, LPCTSTR bstrRSN)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_I4 VTS_BSTR VTS_BSTR VTS_BSTR;
	InvokeHelper(0xa, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrApplicationFile, bstrDestinationDirectory, lOptions, bstrUserId, bstrPassword, bstrRSN);
}

void ICOMAdminCatalog::StopRouter()
{
	InvokeHelper(0xb, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void ICOMAdminCatalog::RefreshRouter()
{
	InvokeHelper(0xc, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void ICOMAdminCatalog::StartRouter()
{
	InvokeHelper(0xd, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void ICOMAdminCatalog::Reserved1()
{
	InvokeHelper(0xe, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void ICOMAdminCatalog::Reserved2()
{
	InvokeHelper(0xf, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void ICOMAdminCatalog::RefreshComponents()
{
	InvokeHelper(0x12, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

void ICOMAdminCatalog::BackupREGDB(LPCTSTR bstrBackupFilePath)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x13, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrBackupFilePath);
}

void ICOMAdminCatalog::RestoreREGDB(LPCTSTR bstrBackupFilePath)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x14, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrBackupFilePath);
}

void ICOMAdminCatalog::StartApplication(LPCTSTR bstrApplIdOrName)
{
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x16, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrApplIdOrName);
}

long ICOMAdminCatalog::ServiceCheck(long lService)
{
	long result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x17, DISPATCH_METHOD, VT_I4, (void*)&result, parms,
		lService);
	return result;
}

void ICOMAdminCatalog::InstallEventClass(LPCTSTR bstrApplIdOrName, LPCTSTR bstrDLL, LPCTSTR bstrTLB, LPCTSTR bstrPSDLL)
{
	static BYTE parms[] =
		VTS_BSTR VTS_BSTR VTS_BSTR VTS_BSTR;
	InvokeHelper(0x19, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrApplIdOrName, bstrDLL, bstrTLB, bstrPSDLL);
}


/////////////////////////////////////////////////////////////////////////////
// ICatalogObject properties

/////////////////////////////////////////////////////////////////////////////
// ICatalogObject operations

VARIANT ICatalogObject::GetValue(LPCTSTR bstrPropName)
{
	VARIANT result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x1, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, parms,
		bstrPropName);
	return result;
}

void ICatalogObject::SetValue(LPCTSTR bstrPropName, const VARIANT& newValue)
{
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x1, DISPATCH_PROPERTYPUT, VT_EMPTY, NULL, parms,
		 bstrPropName, &newValue);
}

VARIANT ICatalogObject::GetKey()
{
	VARIANT result;
	InvokeHelper(0x2, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

VARIANT ICatalogObject::GetName()
{
	VARIANT result;
	InvokeHelper(0x3, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

BOOL ICatalogObject::IsPropertyReadOnly(LPCTSTR bstrPropName)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		bstrPropName);
	return result;
}

BOOL ICatalogObject::GetValid()
{
	BOOL result;
	InvokeHelper(0x5, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

BOOL ICatalogObject::IsPropertyWriteOnly(LPCTSTR bstrPropName)
{
	BOOL result;
	static BYTE parms[] =
		VTS_BSTR;
	InvokeHelper(0x6, DISPATCH_METHOD, VT_BOOL, (void*)&result, parms,
		bstrPropName);
	return result;
}


/////////////////////////////////////////////////////////////////////////////
// ICatalogCollection properties

/////////////////////////////////////////////////////////////////////////////
// ICatalogCollection operations

LPDISPATCH ICatalogCollection::GetItem(long lIndex)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x1, DISPATCH_PROPERTYGET, VT_DISPATCH, (void*)&result, parms,
		lIndex);
	return result;
}

long ICatalogCollection::GetCount()
{
	long result;
	InvokeHelper(0x60020002, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

void ICatalogCollection::Remove(long lIndex)
{
	static BYTE parms[] =
		VTS_I4;
	InvokeHelper(0x60020003, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 lIndex);
}

LPDISPATCH ICatalogCollection::Add()
{
	LPDISPATCH result;
	InvokeHelper(0x60020004, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

void ICatalogCollection::Populate()
{
	InvokeHelper(0x2, DISPATCH_METHOD, VT_EMPTY, NULL, NULL);
}

long ICatalogCollection::SaveChanges()
{
	long result;
	InvokeHelper(0x3, DISPATCH_METHOD, VT_I4, (void*)&result, NULL);
	return result;
}

LPDISPATCH ICatalogCollection::GetCollection(LPCTSTR bstrCollName, const VARIANT& varObjectKey)
{
	LPDISPATCH result;
	static BYTE parms[] =
		VTS_BSTR VTS_VARIANT;
	InvokeHelper(0x4, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, parms,
		bstrCollName, &varObjectKey);
	return result;
}

VARIANT ICatalogCollection::GetName()
{
	VARIANT result;
	InvokeHelper(0x6, DISPATCH_PROPERTYGET, VT_VARIANT, (void*)&result, NULL);
	return result;
}

BOOL ICatalogCollection::GetAddEnabled()
{
	BOOL result;
	InvokeHelper(0x7, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

BOOL ICatalogCollection::GetRemoveEnabled()
{
	BOOL result;
	InvokeHelper(0x8, DISPATCH_PROPERTYGET, VT_BOOL, (void*)&result, NULL);
	return result;
}

LPDISPATCH ICatalogCollection::GetUtilInterface()
{
	LPDISPATCH result;
	InvokeHelper(0x9, DISPATCH_METHOD, VT_DISPATCH, (void*)&result, NULL);
	return result;
}

long ICatalogCollection::GetDataStoreMajorVersion()
{
	long result;
	InvokeHelper(0xa, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

long ICatalogCollection::GetDataStoreMinorVersion()
{
	long result;
	InvokeHelper(0xb, DISPATCH_PROPERTYGET, VT_I4, (void*)&result, NULL);
	return result;
}

void ICatalogCollection::PopulateByQuery(LPCTSTR bstrQueryString, long lQueryType)
{
	static BYTE parms[] =
		VTS_BSTR VTS_I4;
	InvokeHelper(0xd, DISPATCH_METHOD, VT_EMPTY, NULL, parms,
		 bstrQueryString, lQueryType);
}
