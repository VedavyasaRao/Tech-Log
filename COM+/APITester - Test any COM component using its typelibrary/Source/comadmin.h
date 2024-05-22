// Machine generated IDispatch wrapper class(es) created with ClassWizard
/////////////////////////////////////////////////////////////////////////////
// ICOMAdminCatalog wrapper class
#pragma once

class ICOMAdminCatalog : public COleDispatchDriver
{
public:
	ICOMAdminCatalog() {}		// Calls COleDispatchDriver default constructor
	ICOMAdminCatalog(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	ICOMAdminCatalog(const ICOMAdminCatalog& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetCollection(LPCTSTR bstrCollName);
	LPDISPATCH Connect(LPCTSTR bstrConnectString);
	long GetMajorVersion();
	long GetMinorVersion();
	// method 'GetCollectionByQuery' not emitted because of invalid return type or parameter type
	void ImportComponent(LPCTSTR bstrApplIdOrName, LPCTSTR bstrCLSIDOrProgId);
	void InstallComponent(LPCTSTR bstrApplIdOrName, LPCTSTR bstrDLL, LPCTSTR bstrTLB, LPCTSTR bstrPSDLL);
	void ShutdownApplication(LPCTSTR bstrApplIdOrName);
	void ExportApplication(LPCTSTR bstrApplIdOrName, LPCTSTR bstrApplicationFile, long lOptions);
	void InstallApplication(LPCTSTR bstrApplicationFile, LPCTSTR bstrDestinationDirectory, long lOptions, LPCTSTR bstrUserId, LPCTSTR bstrPassword, LPCTSTR bstrRSN);
	void StopRouter();
	void RefreshRouter();
	void StartRouter();
	void Reserved1();
	void Reserved2();
	// method 'InstallMultipleComponents' not emitted because of invalid return type or parameter type
	// method 'GetMultipleComponentsInfo' not emitted because of invalid return type or parameter type
	void RefreshComponents();
	void BackupREGDB(LPCTSTR bstrBackupFilePath);
	void RestoreREGDB(LPCTSTR bstrBackupFilePath);
	// method 'QueryApplicationFile' not emitted because of invalid return type or parameter type
	void StartApplication(LPCTSTR bstrApplIdOrName);
	long ServiceCheck(long lService);
	// method 'InstallMultipleEventClasses' not emitted because of invalid return type or parameter type
	void InstallEventClass(LPCTSTR bstrApplIdOrName, LPCTSTR bstrDLL, LPCTSTR bstrTLB, LPCTSTR bstrPSDLL);
	// method 'GetEventClassesForIID' not emitted because of invalid return type or parameter type
};
/////////////////////////////////////////////////////////////////////////////
// ICatalogObject wrapper class

class ICatalogObject : public COleDispatchDriver
{
public:
	ICatalogObject() {}		// Calls COleDispatchDriver default constructor
	ICatalogObject(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	ICatalogObject(const ICatalogObject& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	VARIANT GetValue(LPCTSTR bstrPropName);
	void SetValue(LPCTSTR bstrPropName, const VARIANT& newValue);
	VARIANT GetKey();
	VARIANT GetName();
	BOOL IsPropertyReadOnly(LPCTSTR bstrPropName);
	BOOL GetValid();
	BOOL IsPropertyWriteOnly(LPCTSTR bstrPropName);
};
/////////////////////////////////////////////////////////////////////////////
// ICatalogCollection wrapper class

class ICatalogCollection : public COleDispatchDriver
{
public:
	ICatalogCollection() {}		// Calls COleDispatchDriver default constructor
	ICatalogCollection(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	ICatalogCollection(const ICatalogCollection& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetItem(long lIndex);
	long GetCount();
	void Remove(long lIndex);
	LPDISPATCH Add();
	void Populate();
	long SaveChanges();
	LPDISPATCH GetCollection(LPCTSTR bstrCollName, const VARIANT& varObjectKey);
	VARIANT GetName();
	BOOL GetAddEnabled();
	BOOL GetRemoveEnabled();
	LPDISPATCH GetUtilInterface();
	long GetDataStoreMajorVersion();
	long GetDataStoreMinorVersion();
	// method 'PopulateByKey' not emitted because of invalid return type or parameter type
	void PopulateByQuery(LPCTSTR bstrQueryString, long lQueryType);
};
