// Machine generated IDispatch wrapper class(es) created with ClassWizard
/////////////////////////////////////////////////////////////////////////////
// ICSIMBaseContext wrapper class

class ICSIMBaseContext : public COleDispatchDriver
{
public:
	ICSIMBaseContext() {}		// Calls COleDispatchDriver default constructor
	ICSIMBaseContext(LPDISPATCH pDispatch) : COleDispatchDriver(pDispatch) {}
	ICSIMBaseContext(const ICSIMBaseContext& dispatchSrc) : COleDispatchDriver(dispatchSrc) {}

// Attributes
public:

// Operations
public:
	LPDISPATCH GetGCInterface1();
	void SetGCInterface1(LPDISPATCH newValue);
	CString GetAllContext(VARIANT* pvbstrContextNames, VARIANT* pvContextValues);
	CString GetOneContextVariable(LPCTSTR bstrContextName, BOOL* pvBoolExists, VARIANT* pvContextValue);
	CString SetOneContextVariable(LPCTSTR bstrContextName, const VARIANT& vContextValue, BOOL boolAddContext, BOOL boolOverrideContext);
	CString DeleteOneContextVariable(LPCTSTR bstrContextName);
	CString Serialize(LPCTSTR bstrFileName, BOOL boolStoring);
	CString GetBstrGC1();
	void SetBstrGC1(LPCTSTR lpszNewValue);
	CString GetAllContextForNestedGC(LPCTSTR bstrNestedGCName, VARIANT* pvbstrContextNames, VARIANT* pvContextValues);
	CString RenameOneContextVariable(LPCTSTR bstrContextOldName, LPCTSTR bstrContextNewName);
	CString MergeInContext(LPDISPATCH pifGC, BOOL boolOverrideContext);
	CString GetGCInterface(LPDISPATCH* ppifGC);
	CString SetGCInterface(LPDISPATCH pifGC);
	CString GetBstrGC(BSTR* pbstrGC);
	CString SetBstrGC(LPCTSTR BstrGC);
};
