#ifndef _TLBINF_H_
#define _TLBINF_H_
interface CoClasses;
interface CoClassInfo;
interface ConstantInfo;
interface Constants;
interface CustomData;
interface CustomDataCollection;
interface CustomFilter;
interface CustomSort;
interface DeclarationInfo;
interface Declarations;
interface InterfaceInfo;
interface Interfaces;
interface IntrinsicAliases;
interface IntrinsicAliasInfo;
interface ListBoxNotification;
interface MemberInfo;
interface Members;
interface ParameterInfo;
interface Parameters;
interface RecordInfo;
interface Records;
interface _SearchHelper;
interface SearchItem;
interface SearchResults;
interface _TLIApplication;
interface TypeInfo;
interface TypeInfos;
interface _TypeLibInfo;
interface UnionInfo;
interface Unions;
interface VarTypeInfo;
#define TypeLibInfo _TypeLibInfo
#define TLIApplication _TLIApplication
#define SearchHelper _SearchHelper

enum TliErrors {
  tliErrNoCurrentTypelib=-2147220991,
  tliErrCantLoadLibrary=-2147220990,
  tliErrTypeLibNotRegistered=-2147220989,
  tliErrSearchResultsChanged=-2147220988,
  tliErrNotApplicable=-2147220987,
  tliErrIncompatibleData=-2147220986,
  tliErrIncompatibleSearchType=-2147220985,
  tliErrIncompatibleTypeKind=-2147220984,
  tliErrInaccessibleImportLib=-2147220983,
  tliErrNoDefaultValue=-2147220982,
  tliErrDataNotAvailable=-2147220981,
  tliErrNotAnEntryPoint=-2147220980,
  tliErrStopFiltering=-2147220979,
  tliErrArrayBoundsNotAvailable=-2147220978,
  tliErrTypeNotArray=-2147220977
};

typedef TYPEFLAGS TypeFlags;
#define TYPEFLAG_NONE 0

//typedef IMPLTYPEFLAGS ImplTypeFlags;

typedef TYPEKIND TypeKinds;

typedef FUNCFLAGS FuncFlags;
#define FUNCFLAG_NONE 0

typedef VARFLAGS VarFlags;
#define VARCFLAG_NONE 0

typedef SYSKIND SysKinds;

typedef LIBFLAGS LibFlags;

typedef INVOKEKIND InvokeKinds;
#define INVOKE_UNKNOWN 0
#define INVOKE_EVENTFUNC 0x10
#define INVOKE_CONST 0x20

typedef ULONG ParamFlags;

typedef DESCKIND DescKinds;

typedef VARTYPE TliVarType;

enum _TliSearchTypes {
  tliStDefault=4096,
  tliStClasses=1,
  tliStEvents=2,
  tliStConstants=4,
  tliStDeclarations=8,
  tliStAppObject=16,
  tliStRecords=32,
  tliStIntrinsicAliases=64,
  tliStUnions=128,
  tliStAll=239
};
typedef DWORD TliSearchTypes;

enum TliWindowTypes {
  tliWtListBox=0,
  tliWtComboBox=1
};

enum TliItemDataTypes {
  tliIdtMemberID=0,
  tliIdtInvokeKinds=1
};

enum TliCustomFilterAction {
  tliCfaLeave=0,
  tliCfaDuplicate=1,
  tliCfaExtract=2,
  tliCfaDelete=3
};

#define SEARCHDATA_SEARCHTYPE(sd) ((sd) >> 0x18 & 0xff)
#define SEARCHDATA_TYPEINFO(sd) ((sd) & 0x1fff)
#define SEARCHDATA_LIBNUM(sd) ((((sd) >> 0xd & 0x7) << 0x8) | ((sd) >> 0x10 & 0xff))
#define MAKE_SEARCHDATA(tinfo, libnum, searchtype) (((WORD)(tinfo) & 0x1fff) | \
  ((DWORD)(searchtype) << 0x18) | \
  (((DWORD)(libnum) & 0xff) << 0x10) | \
  (((DWORD)(libnum) & 0x700) << 0x5))

DEFINE_GUID(IID_CoClasses,0x8B217744L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(CoClasses, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, CoClassInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, CoClassInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, CoClassInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID_CoClassInfo,0x8B217743L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(CoClassInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ CoClassInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(_placeholder_VTableInterface)(THIS) PURE;
    STDMETHOD(_placeholder_GetMember)(THIS) PURE;
    STDMETHOD(_placeholder_Members)(THIS) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Interfaces)(THIS_ Interfaces FAR* FAR* retVal) PURE;
    STDMETHOD(get_DefaultInterface)(THIS_ InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_DefaultEventInterface)(THIS_ InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_ConstantInfo,0x8B21774DL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(ConstantInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(_placeholder_VTableInterface)(THIS) PURE;
    STDMETHOD(get_GetMember)(THIS_ VARIANT Index, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Members)(THIS_ Members FAR* FAR* retVal) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_ImpliedInterfaces)(THIS) PURE;
    STDMETHOD(_DefaultInterface)(THIS) PURE;
    STDMETHOD(_DefaultEventInterface)(THIS) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_Constants,0x8B21774CL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(Constants, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, ConstantInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, ConstantInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, ConstantInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID_CustomData,0x8B217763L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(CustomData, IDispatch)
{
    STDMETHOD(Me)(THIS_ CustomData FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_Value)(THIS_ VARIANT FAR* retVal) PURE;
};

DEFINE_GUID(IID_CustomDataCollection,0x8B217764L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(CustomDataCollection, IDispatch)
{
    STDMETHOD(Me)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, CustomData FAR* FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
};

DEFINE_GUID(IID_CustomFilter,0x8B217760L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(CustomFilter, IDispatch)
{
    STDMETHOD(Visit)(THIS_ SearchItem FAR* Item, TliCustomFilterAction FAR* Action) PURE;
};

DEFINE_GUID(IID_CustomSort,0x8B21775FL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(CustomSort, IDispatch)
{
    STDMETHOD(Compare)(THIS_ SearchItem FAR* Item1, SearchItem FAR* Item2, long FAR* Compare) PURE;
};

DEFINE_GUID(IID_DeclarationInfo,0x8B21774FL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(DeclarationInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ DeclarationInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(_placeholder_VTableInterface)(THIS) PURE;
    STDMETHOD(get_GetMember)(THIS_ VARIANT Index, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Members)(THIS_ Members FAR* FAR* retVal) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_ImpliedInterfaces)(THIS) PURE;
    STDMETHOD(_DefaultInterface)(THIS) PURE;
    STDMETHOD(_DefaultEventInterface)(THIS) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_Declarations,0x8B21774EL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(Declarations, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, DeclarationInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, DeclarationInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, DeclarationInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID_InterfaceInfo,0x8B217741L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(InterfaceInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_VTableInterface)(THIS_ InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_GetMember)(THIS_ VARIANT Index, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Members)(THIS_ Members FAR* FAR* retVal) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_ImpliedInterfaces)(THIS_ Interfaces FAR* FAR* retVal) PURE;
    STDMETHOD(_DefaultInterface)(THIS) PURE;
    STDMETHOD(_DefaultEventInterface)(THIS) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_Interfaces,0x8B217742L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(Interfaces, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, InterfaceInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID_IntrinsicAliases,0x8B217762L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(IntrinsicAliases, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, IntrinsicAliasInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, IntrinsicAliasInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, IntrinsicAliasInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID_IntrinsicAliasInfo,0x8B217761L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(IntrinsicAliasInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ IntrinsicAliasInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(_placeholder_VTableInterface)(THIS) PURE;
    STDMETHOD(_placeholder_GetMember)(THIS) PURE;
    STDMETHOD(_placeholder_Members)(THIS) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_ImpliedInterfaces)(THIS) PURE;
    STDMETHOD(_DefaultInterface)(THIS) PURE;
    STDMETHOD(_DefaultEventInterface)(THIS) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_ListBoxNotification,0x8B217758L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(ListBoxNotification, IDispatch)
{
    STDMETHOD(OnAddString)(THIS_ long lpstr, VARIANT_BOOL fUnicode) PURE;
};

DEFINE_GUID(IID_MemberInfo,0x8B217747L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(MemberInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_Parameters)(THIS_ Parameters FAR* FAR* retVal) PURE;
    STDMETHOD(get_ReturnType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldDescKind)(THIS) PURE;
    STDMETHOD(get_Value)(THIS_ VARIANT FAR* retVal) PURE;
    STDMETHOD(get_MemberId)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_VTableOffset)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_InvokeKind)(THIS_ InvokeKinds FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_DescKind)(THIS_ DescKinds FAR* retVal) PURE;
    STDMETHOD(GetDllEntry)(THIS_ BSTR FAR* DllName, BSTR FAR* EntryName, short FAR* Ordinal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_CallConv)(THIS_  CALLCONV FAR* retVal) PURE;
};

DEFINE_GUID(IID_Members,0x8B217748L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(Members, IDispatch)
{
    STDMETHOD(Me)(THIS_ Members FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(put_FuncFilter)(THIS_ FuncFlags retVal) PURE;
    STDMETHOD(get_FuncFilter)(THIS_ FuncFlags FAR* retVal) PURE;
    STDMETHOD(put_VarFilter)(THIS_ VarFlags retVal) PURE;
    STDMETHOD(get_VarFilter)(THIS_ VarFlags FAR* retVal) PURE;
    STDMETHOD(_OldFillList)(THIS) PURE;
    STDMETHOD(get_GetFilteredMembers)(THIS_ VARIANT_BOOL ShowUnderscore, SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(GetFilteredMembersDirect)(THIS_ int hWnd, TliWindowTypes WindowType, TliItemDataTypes ItemDataType, VARIANT_BOOL ShowUnderscore, short FAR* retVal) PURE;
};

DEFINE_GUID(IID_ParameterInfo,0x8B217749L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(ParameterInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ ParameterInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_Optional)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(get__OldFlags)(THIS) PURE;
    STDMETHOD(get_VarTypeInfo)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Default)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(get_DefaultValue)(THIS_ VARIANT FAR* retVal) PURE;
    STDMETHOD(get_HasCustomData)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_Flags)(THIS_ ParamFlags FAR* retVal) PURE;
};

DEFINE_GUID(IID_Parameters,0x8B21774AL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(Parameters, IDispatch)
{
    STDMETHOD(Me)(THIS_ Parameters FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, ParameterInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_OptionalCount)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_DefaultCount)(THIS_ short FAR* retVal) PURE;
};

DEFINE_GUID(IID_RecordInfo,0x8B21775BL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(RecordInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(_placeholder_VTableInterface)(THIS) PURE;
    STDMETHOD(get_GetMember)(THIS_ VARIANT Index, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Members)(THIS_ Members FAR* FAR* retVal) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_ImpliedInterfaces)(THIS) PURE;
    STDMETHOD(_DefaultInterface)(THIS) PURE;
    STDMETHOD(_DefaultEventInterface)(THIS) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_Records,0x8B21775CL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(Records, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, RecordInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, RecordInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, RecordInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID__SearchHelper,0x8B217751L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(_SearchHelper, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_OldInit)(THIS) PURE;
    STDMETHOD(CheckHaveMatch)(THIS_ BSTR szName, VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(Init)(THIS_ SysKinds SysKind, long LCID, short GrowSize) PURE;
};

DEFINE_GUID(CLSID_SearchHelper,0x8B217752L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
class CSearchHelper
{
public:
    CSearchHelper()
    {
      m_pInternal = NULL;
      IUnknown FAR* pUnk;
      if SUCCEEDED(m_hrLaunch = CoCreateInstance(CLSID_SearchHelper, NULL, CLSCTX_SERVER, 
                                               IID_IUnknown, (void FAR* FAR*) &pUnk)) {
        m_hrLaunch = pUnk->QueryInterface(IID__SearchHelper, (void FAR* FAR*) &m_pInternal);  
        pUnk->Release();
      }
    }
    virtual ~CSearchHelper(){if (m_pInternal) m_pInternal->Release();}
    HRESULT LaunchError(){return m_hrLaunch;}
    inline _SearchHelper* operator->(){return m_pInternal;}
private:
    _SearchHelper FAR* m_pInternal;
    HRESULT m_hrLaunch;
};

DEFINE_GUID(IID_SearchItem,0x8B217756L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(SearchItem, IDispatch)
{
    STDMETHOD(Me)(THIS_ SearchItem FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_SearchData)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get__OldLibNum)(THIS_ unsigned char FAR* retVal) PURE;
    STDMETHOD(get_SearchType)(THIS_ TliSearchTypes FAR* retVal) PURE;
    STDMETHOD(get_MemberId)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_InvokeKinds)(THIS_ InvokeKinds FAR* retVal) PURE;
    STDMETHOD(get_NamePtrW)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_LibNum)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Constant)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(get_Hidden)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(get_InvokeGroup)(THIS_ short FAR* retVal) PURE;
};

DEFINE_GUID(IID_SearchResults,0x8B217757L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(SearchResults, IDispatch)
{
    STDMETHOD(Me)(THIS_ SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get__OldItem)(THIS_ short Index, SearchItem FAR* FAR* retVal) PURE;
    STDMETHOD(get__OldCount)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Sorted)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(Sort)(THIS_ CustomSort FAR* CustomSort) PURE;
    STDMETHOD(Filter)(THIS_ CustomFilter FAR* CustomFilter, SearchResults FAR* FAR* AppendExtractedTo, SearchItem FAR* StartAfter, SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ long Index, SearchItem FAR* FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(LocateSorted)(THIS_ CustomSort FAR* CustomSort, long FAR* retVal) PURE;
    STDMETHOD(Locate)(THIS_ BSTR SearchString, CustomSort FAR* CustomSort, long StartAfter, long FAR* retVal) PURE;
};

//DEFINE_GUID(IID__TLIApplication,0x8B21775DL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
extern CLSID IID__TLIApplication;// = {0x8B21775DL,0x717D,0x11CE, {0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00}};
DECLARE_INTERFACE_(_TLIApplication, IDispatch)
{
    STDMETHOD(Me)(THIS_ TLIApplication FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(TypeLibInfoFromFile)(THIS_ BSTR FileName, TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(InterfaceInfoFromObject)(THIS_ LPDISPATCH Object, InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_ListBoxNotification)(THIS_ ListBoxNotification FAR* FAR* retVal) PURE;
    STDMETHOD(putref_ListBoxNotification)(THIS_ ListBoxNotification FAR* retVal) PURE;
    STDMETHOD(get_ResolveAliases)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(put_ResolveAliases)(THIS_ VARIANT_BOOL retVal) PURE;
    STDMETHOD(InvokeHook)(THIS_ LPDISPATCH Object, VARIANT ID, InvokeKinds InvokeKind, LPSAFEARRAY FAR* ReverseArgList, VARIANT FAR* retVal) PURE;
    STDMETHOD(InvokeHookArray)(THIS_ LPDISPATCH Object, VARIANT ID, InvokeKinds InvokeKind, LPSAFEARRAY FAR* ReverseArgList, VARIANT FAR* retVal) PURE;
    STDMETHOD(InvokeHookSub)(THIS_ LPDISPATCH Object, VARIANT ID, InvokeKinds InvokeKind, LPSAFEARRAY FAR* ReverseArgList) PURE;
    STDMETHOD(InvokeHookArraySub)(THIS_ LPDISPATCH Object, VARIANT ID, InvokeKinds InvokeKind, LPSAFEARRAY FAR* ReverseArgList) PURE;
    STDMETHOD(ClassInfoFromObject)(THIS_ LPUNKNOWN Object, TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(InvokeID)(THIS_ LPDISPATCH Object, BSTR Name, long FAR* retVal) PURE;
    STDMETHOD(get_InvokeLCID)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(put_InvokeLCID)(THIS_ long retVal) PURE;
    STDMETHOD(TypeInfoFromITypeInfo)(THIS_ LPUNKNOWN ptinfo, TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(TypeLibInfoFromITypeLib)(THIS_ IUnknown * pITypeLib, TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(TypeLibInfoFromRegistry)(THIS_ BSTR TypeLibGuid, short MajorVersion, short MinorVersion, long LCID, TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(TypeInfoFromRecordVariant)(THIS_ VARIANT FAR* RecordVariant, TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_RecordField)(THIS_ VARIANT FAR* RecordVariant, BSTR FAR* FieldName, VARIANT FAR* retVal) PURE;
    STDMETHOD(put_RecordField)(THIS_ VARIANT FAR* RecordVariant, BSTR FAR* FieldName, VARIANT FAR* NewValue) PURE;
    STDMETHOD(putref_RecordField)(THIS_ VARIANT FAR* RecordVariant, BSTR FAR* FieldName, VARIANT FAR* NewValue) PURE;
};

//DEFINE_GUID(CLSID_TLIApplication,0x8B21775EL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
extern CLSID CLSID_TLIApplication;// = {0x8B21775EL,0x717D,0x11CE,{0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00}};
class CTLIApplication
{
public:
    CTLIApplication()
    {
      m_pInternal = NULL;
      IUnknown FAR* pUnk;
      if SUCCEEDED(m_hrLaunch = CoCreateInstance(CLSID_TLIApplication, NULL, CLSCTX_SERVER, 
                                               IID_IUnknown, (void FAR* FAR*) &pUnk)) {
        m_hrLaunch = pUnk->QueryInterface(IID__TLIApplication, (void FAR* FAR*) &m_pInternal);  
        pUnk->Release();
      }
    }
    virtual ~CTLIApplication(){if (m_pInternal) m_pInternal->Release();}
    HRESULT LaunchError(){return m_hrLaunch;}
    inline _TLIApplication* operator->(){return m_pInternal;}
private:
    _TLIApplication FAR* m_pInternal;
    HRESULT m_hrLaunch;
};

DEFINE_GUID(IID_TypeInfo,0x8B217759L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(TypeInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_VTableInterface)(THIS_ InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_GetMember)(THIS_ VARIANT Index, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Members)(THIS_ Members FAR* FAR* retVal) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Interfaces)(THIS_ Interfaces FAR* FAR* retVal) PURE;
    STDMETHOD(get_DefaultInterface)(THIS_ InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_DefaultEventInterface)(THIS_ InterfaceInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_TypeInfos,0x8B21775AL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(TypeInfos, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, TypeInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID__TypeLibInfo,0x8B217745L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(_TypeLibInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_ContainingFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(put_ContainingFile)(THIS_ BSTR retVal) PURE;
    STDMETHOD(LoadRegTypeLib)(THIS_ BSTR TypeLibGuid, short MajorVersion, short MinorVersion, long LCID) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_LCID)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get__OldSysKind)(THIS) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get_CoClasses)(THIS_ CoClasses FAR* FAR* retVal) PURE;
    STDMETHOD(get_Interfaces)(THIS_ Interfaces FAR* FAR* retVal) PURE;
    STDMETHOD(get_Constants)(THIS_ Constants FAR* FAR* retVal) PURE;
    STDMETHOD(get_Declarations)(THIS_ Declarations FAR* FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoCount)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get__OldGetTypeKind)(THIS) PURE;
    STDMETHOD(get_GetTypeInfo)(THIS_ VARIANT FAR* Index, TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_GetTypeInfoNumber)(THIS_ BSTR Name, short FAR* retVal) PURE;
    STDMETHOD(IsSameLibrary)(THIS_ TypeLibInfo FAR* CheckLib, VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(_OldResetSearchCriteria)(THIS) PURE;
    STDMETHOD(_OldGetTypesWithMember)(THIS) PURE;
    STDMETHOD(_OldGetMembersWithSubString)(THIS) PURE;
    STDMETHOD(_OldGetTypesWithSubString)(THIS) PURE;
    STDMETHOD(_OldCaseTypeName)(THIS) PURE;
    STDMETHOD(_OldCaseMemberName)(THIS) PURE;
    STDMETHOD(_OldFillTypesList)(THIS) PURE;
    STDMETHOD(_OldFillTypesCombo)(THIS) PURE;
    STDMETHOD(_OldFillMemberList)(THIS) PURE;
    STDMETHOD(_OldAddClassTypeToList)(THIS) PURE;
    STDMETHOD(put_AppObjString)(THIS_ BSTR retVal) PURE;
    STDMETHOD(put_LibNum)(THIS_ short retVal) PURE;
    STDMETHOD(get_ShowLibName)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(put_ShowLibName)(THIS_ VARIANT_BOOL retVal) PURE;
    STDMETHOD(putref__OldListBoxNotification)(THIS_ ListBoxNotification FAR* Param1) PURE;
    STDMETHOD(get_GetTypeKind)(THIS_ short TypeInfoNumber, TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_SysKind)(THIS_ SysKinds FAR* retVal) PURE;
    STDMETHOD(get_SearchDefault)(THIS_ TliSearchTypes FAR* retVal) PURE;
    STDMETHOD(put_SearchDefault)(THIS_ TliSearchTypes retVal) PURE;
    STDMETHOD(CaseTypeName)(THIS_ BSTR FAR* bstrName, TliSearchTypes SearchType, TliSearchTypes FAR* retVal) PURE;
    STDMETHOD(CaseMemberName)(THIS_ BSTR FAR* bstrName, TliSearchTypes SearchType, VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(ResetSearchCriteria)(THIS_ TypeFlags TypeFilter, VARIANT_BOOL IncludeEmptyTypes, VARIANT_BOOL ShowUnderscore) PURE;
    STDMETHOD(GetTypesWithMember)(THIS_ BSTR MemberName, SearchResults FAR* FAR* StartResults, TliSearchTypes SearchType, VARIANT_BOOL Sort, VARIANT_BOOL ShowUnderscore, 
        SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(GetTypesWithMemberDirect)(THIS_ BSTR MemberName, int hWnd, TliWindowTypes WindowType, TliSearchTypes SearchType, VARIANT_BOOL ShowUnderscore, short FAR* 
        retVal) PURE;
    STDMETHOD(GetMembersWithSubString)(THIS_ BSTR SubString, SearchResults FAR* FAR* StartResults, TliSearchTypes SearchType, VARIANT_BOOL SearchMiddle, SearchHelper 
        FAR* FAR* Helper, VARIANT_BOOL Sort, VARIANT_BOOL ShowUnderscore, SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(GetMembersWithSubStringDirect)(THIS_ BSTR SubString, int hWnd, TliWindowTypes WindowType, TliSearchTypes SearchType, VARIANT_BOOL SearchMiddle, SearchHelper 
        FAR* FAR* Helper, VARIANT_BOOL ShowUnderscore, short FAR* retVal) PURE;
    STDMETHOD(GetTypesWithSubString)(THIS_ BSTR SubString, SearchResults FAR* FAR* StartResults, TliSearchTypes SearchType, VARIANT_BOOL SearchMiddle, VARIANT_BOOL Sort, 
        SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(GetTypesWithSubStringDirect)(THIS_ BSTR SubString, int hWnd, TliWindowTypes WindowType, TliSearchTypes SearchType, VARIANT_BOOL SearchMiddle, short FAR* 
        retVal) PURE;
    STDMETHOD(GetTypes)(THIS_ SearchResults FAR* FAR* StartResults, TliSearchTypes SearchType, VARIANT_BOOL Sort, SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(GetTypesDirect)(THIS_ int hWnd, TliWindowTypes WindowType, TliSearchTypes SearchType, short FAR* retVal) PURE;
    STDMETHOD(GetMembers)(THIS_ long SearchData, VARIANT_BOOL ShowUnderscore, SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(GetMembersDirect)(THIS_ long SearchData, int hWnd, TliWindowTypes WindowType, TliItemDataTypes ItemDataType, VARIANT_BOOL ShowUnderscore, short FAR* retVal) PURE;
    STDMETHOD(SetMemberFilters)(THIS_ FuncFlags FuncFilter, VarFlags VarFilter) PURE;
    STDMETHOD(MakeSearchData)(THIS_ BSTR TypeInfoName, TliSearchTypes SearchType, long FAR* retVal) PURE;
    STDMETHOD(get_TypeInfos)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(get_Records)(THIS_ Records FAR* FAR* retVal) PURE;
    STDMETHOD(get_IntrinsicAliases)(THIS_ IntrinsicAliases FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(GetMemberInfo)(THIS_ long SearchData, InvokeKinds InvokeKinds, long MemberId, BSTR MemberName, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Unions)(THIS_ Unions FAR* FAR* retVal) PURE;
    STDMETHOD(AddTypes)(THIS_ LPSAFEARRAY FAR* TypeInfoNumbers, SearchResults FAR* FAR* StartResults, TliSearchTypes SearchType, VARIANT_BOOL Sort, SearchResults FAR* 
        FAR* retVal) PURE;
    STDMETHOD(AddTypesDirect)(THIS_ LPSAFEARRAY FAR* TypeInfoNumbers, int hWnd, TliWindowTypes WindowType, TliSearchTypes SearchType, short FAR* retVal) PURE;
    STDMETHOD(FreeSearchCriteria)(THIS) PURE;
    STDMETHOD(Register)(THIS_ BSTR HelpDir) PURE;
    STDMETHOD(UnRegister)(THIS) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_AppObjString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_LibNum)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(GetMembersWithSubStringEx)(THIS_ BSTR SubString, LPSAFEARRAY FAR* InvokeGroupings, SearchResults FAR* FAR* StartResults, TliSearchTypes SearchType, VARIANT_BOOL 
        SearchMiddle, VARIANT_BOOL Sort, VARIANT_BOOL ShowUnderscore, SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(GetTypesWithMemberEx)(THIS_ BSTR MemberName, InvokeKinds InvokeKind, SearchResults FAR* FAR* StartResults, TliSearchTypes SearchType, VARIANT_BOOL Sort, 
        VARIANT_BOOL ShowUnderscore, SearchResults FAR* FAR* retVal) PURE;
    STDMETHOD(get_ITypeLib)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(putref_ITypeLib)(THIS_ LPUNKNOWN retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_BestEquivalentType)(THIS_ BSTR TypeInfoName, BSTR FAR* retVal) PURE;
};

DEFINE_GUID(CLSID_TypeLibInfo,0x8B217746L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
class CTypeLibInfo
{
public:
    CTypeLibInfo()
    {
      m_pInternal = NULL;
      IUnknown FAR* pUnk;
      if SUCCEEDED(m_hrLaunch = CoCreateInstance(CLSID_TypeLibInfo, NULL, CLSCTX_SERVER, 
                                               IID_IUnknown, (void FAR* FAR*) &pUnk)) {
        m_hrLaunch = pUnk->QueryInterface(IID__TypeLibInfo, (void FAR* FAR*) &m_pInternal);  
        pUnk->Release();
      }
    }
    virtual ~CTypeLibInfo(){if (m_pInternal) m_pInternal->Release();}
    HRESULT LaunchError(){return m_hrLaunch;}
    inline _TypeLibInfo* operator->(){return m_pInternal;}
private:
    _TypeLibInfo FAR* m_pInternal;
    HRESULT m_hrLaunch;
};

DEFINE_GUID(IID_UnionInfo,0x8B217765L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(UnionInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get_Name)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_GUID)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get__OldHelpString)(THIS) PURE;
    STDMETHOD(get_HelpContext)(THIS_ long FAR* retVal) PURE;
    STDMETHOD(get_HelpFile)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_AttributeMask)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_AttributeStrings)(THIS_ LPSAFEARRAY FAR* AttributeArray, short FAR* retVal) PURE;
    STDMETHOD(get__OldTypeKind)(THIS) PURE;
    STDMETHOD(get_TypeKindString)(THIS_ BSTR FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(_placeholder_VTableInterface)(THIS) PURE;
    STDMETHOD(get_GetMember)(THIS_ VARIANT Index, MemberInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_Members)(THIS_ Members FAR* FAR* retVal) PURE;
    STDMETHOD(get_Parent)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_ImpliedInterfaces)(THIS) PURE;
    STDMETHOD(_DefaultInterface)(THIS) PURE;
    STDMETHOD(_DefaultEventInterface)(THIS) PURE;
    STDMETHOD(get_TypeKind)(THIS_ TypeKinds FAR* retVal) PURE;
    STDMETHOD(get_ResolvedType)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_CustomDataCollection)(THIS_ CustomDataCollection FAR* FAR* retVal) PURE;
    STDMETHOD(get_HelpString)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_ITypeInfo)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_MajorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_MinorVersion)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_HelpStringDll)(THIS_ long LCID, BSTR FAR* retVal) PURE;
    STDMETHOD(get_HelpStringContext)(THIS_ long FAR* retVal) PURE;
};

DEFINE_GUID(IID_Unions,0x8B217766L,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(Unions, IDispatch)
{
    STDMETHOD(Me)(THIS_ TypeInfos FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(_NewEnum)(THIS_ LPUNKNOWN FAR* retVal) PURE;
    STDMETHOD(get_Count)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_Item)(THIS_ short Index, UnionInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_IndexedItem)(THIS_ short TypeInfoNumber, UnionInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_NamedItem)(THIS_ BSTR FAR* TypeInfoName, UnionInfo FAR* FAR* retVal) PURE;
};

DEFINE_GUID(IID_VarTypeInfo,0x8B21774BL,0x717D,0x11CE,0xAB,0x5B,0xD4,0x12,0x03,0xC1,0x00,0x00);
DECLARE_INTERFACE_(VarTypeInfo, IDispatch)
{
    STDMETHOD(Me)(THIS_ VarTypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(_placeholder_destructor)(THIS) PURE;
    STDMETHOD(get__OldVarType)(THIS) PURE;
    STDMETHOD(get_TypeInfo)(THIS_ TypeInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_TypeInfoNumber)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_TypedVariant)(THIS_ VARIANT FAR* retVal) PURE;
    STDMETHOD(get_IsExternalType)(THIS_ VARIANT_BOOL FAR* retVal) PURE;
    STDMETHOD(get_TypeLibInfoExternal)(THIS_ TypeLibInfo FAR* FAR* retVal) PURE;
    STDMETHOD(get_PointerLevel)(THIS_ short FAR* retVal) PURE;
    STDMETHOD(get_VarType)(THIS_ TliVarType FAR* retVal) PURE;
    STDMETHOD(ArrayBounds)(THIS_ LPSAFEARRAY FAR* Bounds, short FAR* retVal) PURE;
    STDMETHOD(get_ElementPointerLevel)(THIS_ short FAR* retVal) PURE;
};
#endif //_TLBINF_H_


