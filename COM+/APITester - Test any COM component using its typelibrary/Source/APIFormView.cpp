// APIFormView.cpp : implementation file
//

#include "stdafx.h"
#include "APITester.h"
#include "APIFormView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#include "mainfrm.h"

/////////////////////////////////////////////////////////////////////////////
// CAPIFormView

IMPLEMENT_DYNCREATE(CAPIFormView, CFormView)

CAPIFormView::CAPIFormView()
	: CFormView(CAPIFormView::IDD)
{
	//{{AFX_DATA_INIT(CAPIFormView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}

CAPIFormView::~CAPIFormView()
{
}

void CAPIFormView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAPIFormView)
	DDX_Control(pDX, IDC_MSFLEXGRID1, m_funcgrid);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CAPIFormView, CFormView)
	//{{AFX_MSG_MAP(CAPIFormView)
	ON_WM_SIZE()
	ON_COMMAND(ID_FILE_OPEN, OnFileOpen)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CAPIFormView diagnostics

#ifdef _DEBUG
void CAPIFormView::AssertValid() const
{
	CFormView::AssertValid();
}

void CAPIFormView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CAPIFormView message handlers


void CAPIFormView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here

	if (!IsWindow(m_funcgrid.m_hWnd))
		return;

	m_funcgrid.MoveWindow(0, 0, cx-10, cy);

	CAPIFunctionInfo&  funcinfo = ((CMainFrame*)AfxGetMainWnd())->m_wndToolBar.m_apifuncinfo;
	if (funcinfo.m_progid == "" || funcinfo.m_funcname == "")
		return;
	LoadGrid(funcinfo);
}


CString CAPIFormView::getVarCode(VARTYPE vtype) 
{

	CString str;

	switch (vtype % VT_BYREF)
	{

	case VT_UI1 :
		str =  "Byte"; break;

	case VT_I2 :
		str =  "Short"; break;

	case VT_I4 :
		str =  "long"; break;

	case VT_R4 :
		str =  "float"; break;

	case VT_R8 :
		str =  "double"; break;

	case VT_BOOL :
		str =  "VARIANT_BOOL"; break;

	case VT_CY :
		str =  "CY"; break;

	case VT_DATE :
		str =  "DATE"; break;

	case VT_BSTR     :
		str =  "BSTR"; break;

	case VT_VARIANT               :
		str =  "VARIANT"; break;

	case VT_DISPATCH  :
		str =  "IDispatch*"; break;

	case VT_EMPTY  :
		str =  "EMPTY"; break;



	default:
		str = "????"; break;
	}

	if ((vtype & VT_BYREF))
		str = str + "*";
	
	return str;
}

CString CAPIFormView::getElemCode(long vtype) 
{

	switch (vtype)
	{
		case  VT_UI1 : return("VT_UI1");
		case  VT_UI1 | VT_ARRAY : return("VT_UI1 | VT_ARRAY");

		case  VT_I2 : return("VT_I2");
		case  VT_I2 | VT_ARRAY : return("VT_I2 | VT_ARRAY");

		case  VT_I4 : return("VT_I4");
		case  VT_I4 | VT_ARRAY : return("VT_I4 | VT_ARRAY");

		case  VT_R4 : return("VT_R4");
		case  VT_R4 | VT_ARRAY : return("VT_R4 | VT_ARRAY");

		case  VT_R8 : return("VT_R8");
		case  VT_R8 | VT_ARRAY : return("VT_R8 | VT_ARRAY");

		case  VT_DATE : return("VT_DATE");
		case  VT_DATE | VT_ARRAY : return("VT_DATE | VT_ARRAY");

		case  VT_BSTR : return("VT_BSTR");
		case  VT_BSTR | VT_ARRAY : return("VT_BSTR | VT_ARRAY");

		case  VT_DISPATCH : return("VT_DISPATCH");
		case  VT_DISPATCH | VT_ARRAY : return("VT_DISPATCH | VT_ARRAY");

		case  VT_BOOL : return("VT_BOOL");
		case  VT_BOOL | VT_ARRAY : return("VT_BOOL | VT_ARRAY");

		case  VT_VARIANT : return("VT_VARIANT");
		case  VT_VARIANT | VT_ARRAY : return("VT_VARIANT | VT_ARRAY");

		case  VT_UNKNOWN : return("VT_UNKNOWN");
		case  VT_UNKNOWN | VT_ARRAY : return("VT_UNKNOWN | VT_ARRAY");

		case  VT_EMPTY : return("VT_EMPTY");

	}

	return "????";

}

void CAPIFormView::LoadGrid(CAPIFunctionInfo&   funcinfo) 
{

	int lparamcount = funcinfo.m_parameters.GetSize();


	m_funcgrid.Clear();
	m_funcgrid.SetRows(lparamcount+1);


	m_funcgrid.SetColWidth(0, 3000);
	m_funcgrid.SetColWidth(1, 800);
	m_funcgrid.SetColWidth(2, 1200);
	m_funcgrid.SetColWidth(3, 2200);
	


	RECT  rect;
	GetClientRect(&rect);

	CClientDC cltdc(this);
	int WU_LOGPIXELSX = 88;
	int nTwipsPerInch = 1440;
	int lngPixelsPerInch = cltdc.GetDeviceCaps(WU_LOGPIXELSX);
	int  colwidth  = ((rect.right / lngPixelsPerInch) * nTwipsPerInch) - 7250;

	char buf[100];
	sprintf(buf, "%d\n", colwidth);
	OutputDebugString(buf);

	//colwidth = 4000;
	m_funcgrid.SetColWidth(4, colwidth);
	m_funcgrid.SetRow(0);
	m_funcgrid.SetCol(0);
	m_funcgrid.SetText("Parameter Name");

	m_funcgrid.SetCol(1);
	m_funcgrid.SetText("Direction");

	m_funcgrid.SetCol(2);
	m_funcgrid.SetText("Data Type");

	m_funcgrid.SetCol(3);
	m_funcgrid.SetText("Element Type");
	
	m_funcgrid.SetCol(4);
	m_funcgrid.SetText("Value");


	CString sdata;
	for (int i=1; i<=lparamcount; ++i)
	{

		CAPIParameterInfo&	 paraminfo = funcinfo.m_parameters[i-1];
		m_funcgrid.SetRow(i);

		m_funcgrid.SetCol(0);
		sdata = paraminfo.m_name;
		if (paraminfo.foptional)
			sdata = CString("[") + paraminfo.m_name + CString("]");
		m_funcgrid.SetText(sdata);

		sdata = "";
		m_funcgrid.SetCol(1);
        if (paraminfo.ldirection  & 1)
			sdata = "in ";
        if (paraminfo.ldirection  & 2)
			sdata  = sdata + "out";
		m_funcgrid.SetText(sdata);


		m_funcgrid.SetCol(2);
		sdata = getVarCode((short)paraminfo.ldatatype);
		m_funcgrid.SetText(sdata);

		sdata = "";
		m_funcgrid.SetCol(3);
		if (paraminfo.lelemtype)
			sdata = getElemCode(paraminfo.lelemtype);
		m_funcgrid.SetText(sdata);

		m_funcgrid.SetCol(4);
		sdata = paraminfo.m_strvalue; 
		m_funcgrid.SetText(sdata);
	
	}
}


void CAPIFormView::GetGridData(CAPIFunctionInfo&   funcinfo) 
{

	int lparamcount = funcinfo.m_parameters.GetSize();

	CString sdata;
	for (int i=1; i<=lparamcount; ++i)
	{

		CAPIParameterInfo&	 paraminfo = funcinfo.m_parameters[i-1];
		m_funcgrid.SetRow(i);

		m_funcgrid.SetCol(3);
		sdata = m_funcgrid.GetText();
		if (sdata != "")
		{
			paraminfo.lelemtype = -1;
			if (m_combo.SelectString(-1, sdata) != CB_ERR)
				paraminfo.lelemtype = (unsigned short)m_combo.GetItemData(m_combo.GetCurSel());
		}

		m_funcgrid.SetCol(4);
		sdata = m_funcgrid.GetText();
		paraminfo.m_strvalue = sdata;
	}
}


void CAPIFormView::OnInitialUpdate() 
{
	CFormView::OnInitialUpdate();
	
	// TODO: Add your specialized code here and/or call the base class

	if (IsWindow(m_combo.m_hWnd))
		return;

	m_funcgrid.SetRow(0);
	m_funcgrid.SetCol(0);
	m_lBorderWidth = m_funcgrid.GetCellLeft();
	m_lBorderHeigth = m_funcgrid.GetCellTop();

	CDC* pDC = GetDC();
	m_nLogX = pDC->GetDeviceCaps(LOGPIXELSX);
	m_nLogY = pDC->GetDeviceCaps(LOGPIXELSY);
	ReleaseDC(pDC);


	m_combo.Create(WS_CHILD|WS_VSCROLL|CBS_AUTOHSCROLL|CBS_DROPDOWNLIST,
      CRect(10,10,200,300), &m_funcgrid, 1000);
	m_combo.SetDroppedWidth(300);
	m_combo.SendMessage(WM_SETFONT, (DWORD)::CreateFont( 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "MS Sans Serif"), MAKELPARAM(1,0));

	m_combo.AddString("VT_EMPTY");

	m_combo.AddString("VT_UI1");
	m_combo.AddString("VT_UI1 | VT_ARRAY");

	m_combo.AddString("VT_I2");
	m_combo.AddString("VT_I2 | VT_ARRAY");

	m_combo.AddString("VT_I4");
	m_combo.AddString("VT_I4 | VT_ARRAY");

	m_combo.AddString("VT_R4");
	m_combo.AddString("VT_R4 | VT_ARRAY");

	m_combo.AddString("VT_R8");
	m_combo.AddString("VT_R8 | VT_ARRAY");

	m_combo.AddString("VT_DATE");
	m_combo.AddString("VT_DATE | VT_ARRAY");

	m_combo.AddString("VT_BSTR");
	m_combo.AddString("VT_BSTR | VT_ARRAY");

	m_combo.AddString("VT_BOOL");
	m_combo.AddString("VT_BOOL | VT_ARRAY");

	m_combo.AddString("VT_VARIANT");
	m_combo.AddString("VT_VARIANT | VT_ARRAY");

	m_combo.AddString("VT_DISPATCH");
	m_combo.AddString("VT_DISPATCH | VT_ARRAY");


	m_combo.SetItemData(0, VT_EMPTY);

	m_combo.SetItemData( 1, VT_UI1);
	m_combo.SetItemData( 2, VT_UI1 | VT_ARRAY);

	m_combo.SetItemData( 3, VT_I2);
	m_combo.SetItemData( 4, VT_I2 | VT_ARRAY);

	m_combo.SetItemData( 5, VT_I4);
	m_combo.SetItemData( 6, VT_I4 | VT_ARRAY);

	m_combo.SetItemData( 7, VT_R4);
	m_combo.SetItemData( 8, VT_R4 | VT_ARRAY);

	m_combo.SetItemData( 9, VT_R8);
	m_combo.SetItemData( 10, VT_R8 | VT_ARRAY);

	m_combo.SetItemData( 11, VT_DATE);
	m_combo.SetItemData( 12, VT_DATE | VT_ARRAY);

	m_combo.SetItemData( 13, VT_BSTR);
	m_combo.SetItemData( 14, VT_BSTR | VT_ARRAY);

	m_combo.SetItemData( 15, VT_BOOL);
	m_combo.SetItemData( 16, VT_BOOL | VT_ARRAY);

	m_combo.SetItemData( 17, VT_VARIANT);
	m_combo.SetItemData( 18, VT_VARIANT | VT_ARRAY);

	m_combo.SetItemData( 19, VT_DISPATCH);
	m_combo.SetItemData( 20, VT_DISPATCH | VT_ARRAY);


	m_edit.Create(WS_BORDER | ES_AUTOHSCROLL | WS_CHILD| ES_NOHIDESEL,
		CRect(15, 15, 100, 100), this, 1000);

	m_edit.SendMessage(WM_SETFONT, (DWORD)::CreateFont( 13, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 31, "MS Sans Serif"), MAKELPARAM(1,0));
	
}

BEGIN_EVENTSINK_MAP(CAPIFormView, CFormView)
    //{{AFX_EVENTSINK_MAP(CAPIFormView)
	ON_EVENT(CAPIFormView, IDC_MSFLEXGRID1, 72 /* LeaveCell */, OnLeaveCellMsflexgrid1, VTS_NONE)
	ON_EVENT(CAPIFormView, IDC_MSFLEXGRID1, -605 /* MouseDown */, OnMouseDownMsflexgrid1, VTS_I2 VTS_I2 VTS_I4 VTS_I4)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()


void CAPIFormView::OnMouseDownMsflexgrid1(short Button, short Shift, long x, long y) 
{
	// TODO: Add your control notification handler code here

	if (m_funcgrid.GetCol() < 3)
		return;


	int curcol = m_funcgrid.GetCol();
	m_funcgrid.SetCol(1);
	if (m_funcgrid.GetText().Find("in") == -1)
		return;


	m_funcgrid.SetCol(2);
	CString strtext = m_funcgrid.GetText();

	m_funcgrid.SetCol(curcol);
	if (m_funcgrid.GetCol() == 4)
	{
		m_edit.SetWindowText( m_funcgrid.GetText());
		m_edit.MoveWindow(((m_funcgrid.GetCellLeft() - m_lBorderWidth) * m_nLogX) / 1440,
			((m_funcgrid.GetCellTop() - m_lBorderHeigth) * m_nLogY) / 1440,
			(m_funcgrid.GetCellWidth()* m_nLogX) / 1440,
			(m_funcgrid.GetCellHeight()* m_nLogY) / 1440);
		m_edit.ShowWindow(SW_NORMAL);

		m_edit.SetFocus();

		RECT  rect;
		m_funcgrid.GetWindowRect(&rect);
		int msx,  msy;
		msx = x + rect.left; 
		msy = y + rect.top;


		CPoint  pt;
		pt = m_edit.PosFromChar(10);
		
		int newx, newy;
		newx = x - m_funcgrid.GetCellLeft() / 15;
		newy = 0;

		int pos = m_edit.CharFromPos(CPoint(newx,newy));
		m_edit.SetSel(pos, pos, TRUE);
		if (Button == 2)
		{
			m_edit.SendMessage(WM_CONTEXTMENU,  (WORD)m_edit.m_hWnd, MAKELPARAM(msx, msy));
			m_edit.SetSel(0, -1);
		}
		m_edit.SetSel(0, -1);

		return;
	}

	m_funcgrid.SetCol(2);
	if (m_funcgrid.GetText().Find("VARIANT") == -1)
		return;

	m_funcgrid.SetCol(3);

	if (m_combo.SelectString(-1, m_funcgrid.GetText()) == CB_ERR)
		m_combo.SetCurSel(0);
	m_combo.SetItemHeight(-1, 50);
	m_combo.MoveWindow(((m_funcgrid.GetCellLeft() - m_lBorderWidth) * m_nLogX) / 1440,
		((m_funcgrid.GetCellTop() - m_lBorderHeigth) * m_nLogY) / 1440,
		(m_funcgrid.GetCellWidth()* m_nLogX) / 1440,
		(m_funcgrid.GetCellHeight()* m_nLogY) / 1440);

	m_combo.ShowWindow(SW_NORMAL);
	

	
}


void CAPIFormView::OnLeaveCellMsflexgrid1() 
{
	// TODO: Add your control notification handler code here
	if (m_combo.m_hWnd == NULL)
		return;

	if (m_funcgrid.GetCol() == 4)
	{
		if (m_edit.IsWindowVisible())
		{
			CString str;
			m_edit.GetWindowText( str );
			m_funcgrid.SetText(str);
			m_edit.ShowWindow(SW_HIDE);
			int dws=0, dwe=0;
			m_edit.GetSel(dws, dwe);
		}

		return;

	}

	if (m_combo.IsWindowVisible())
	{
		CString str;
		m_combo.GetLBText( m_combo.GetCurSel(), str);
		m_funcgrid.SetText(str);
		m_combo.ShowWindow(SW_HIDE);
	}
	

}

void CAPIFormView::OnFilePrint() 
{
	//CFormView::OnFilePrint();
}


void CAPIFormView::OnFileOpen() 
{
	// TODO: Add your command handler code here
	char  buf[10000] = "";
	CFileDialog dlgFile(TRUE, "TXT", NULL, OFN_FILEMUSTEXIST|OFN_ALLOWMULTISELECT, "API Tester GC Files (*.txt)|*.txt|");
	dlgFile.m_ofn.lpstrFile = buf;
	dlgFile.m_ofn.nMaxFile = 10000;

	if (dlgFile.DoModal() == IDOK)
	{
		POSITION pos = dlgFile.GetStartPosition( );
		CString   filenamesstr;

		while (1)
		{
			filenamesstr = filenamesstr + dlgFile.GetNextPathName( pos );
			if (pos ==  NULL)
				break;
			
			filenamesstr = filenamesstr + ";";

		}

		m_edit.SetWindowText(filenamesstr );
	}
	
}
