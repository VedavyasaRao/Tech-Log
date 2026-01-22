// ColoredStatusBarCtrl.cpp : implementation file
//

#include "stdafx.h"
//#include "dlgviewtest.h"
#include "ColoredStatusBarCtrl.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CColoredStatusBarCtrl

CColoredStatusBarCtrl::CColoredStatusBarCtrl()
{
	m_szDow = "";
	m_szNasdaq = "";
	m_szBusy = "     ";
}

CColoredStatusBarCtrl::~CColoredStatusBarCtrl()
{
}


BEGIN_MESSAGE_MAP(CColoredStatusBarCtrl, CStatusBar)
	//{{AFX_MSG_MAP(CColoredStatusBarCtrl)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CColoredStatusBarCtrl message handlers

void CColoredStatusBarCtrl::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct)
{
	// Attach to a CDC object
	CDC dc;
	dc.Attach(lpDrawItemStruct->hDC);

	dc.SetBkMode(TRANSPARENT);
	// Get the pane rectangle and calculate text coordinates
	CRect rect(&lpDrawItemStruct->rcItem);
	CString temps;

	switch (lpDrawItemStruct->itemID) {
	case 1:
		dc.SetTextColor(RGB(255, 50, 255));
		dc.TextOut(rect.left, rect.top, m_szBusy);
		break;

	case 2:
		temps = m_szDow;
		if (temps.Find('-') != -1)
		{
			dc.SetTextColor(RGB(255, 0, 0));
			temps.Replace("-", " ");
		}
		else
			dc.SetTextColor(RGB(0, 128, 64));
		dc.TextOut(rect.left, rect.top, temps);
		break;

	case 3:
		temps = m_szNasdaq;
		if (temps.Find('-') != -1)
		{
			dc.SetTextColor(RGB(255, 0, 0));
			temps.Replace("-", " ");
		}
		else
			dc.SetTextColor(RGB(0, 128, 64));
		dc.TextOut(rect.left, rect.top, temps);
		break;
	}

	// Detach from the CDC object, otherwise the hDC will be
	// destroyed when the CDC object goes out of scope
	dc.Detach();
}
