// SuperEdit.cpp : implementation file
//

#include "stdafx.h"
#include "APITester.h"
#include "SuperEdit.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSuperEdit

CSuperEdit::CSuperEdit()
{
}

CSuperEdit::~CSuperEdit()
{
}


BEGIN_MESSAGE_MAP(CSuperEdit, CEdit)
	//{{AFX_MSG_MAP(CSuperEdit)
	ON_WM_KEYUP()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSuperEdit message handlers

int prevchar = 0;
void CSuperEdit::OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags) 
{
	// TODO: Add your message handler code here and/or call default
	
	if (nChar == VK_CONTROL)
	{
		if (prevchar == 67)
			SendMessage(WM_COPY, 0, 0);

		if (prevchar == 86)
			SendMessage(WM_PASTE, 0, 0);

		if (prevchar == 88)
			SendMessage(WM_CUT, 0, 0);

		if (prevchar == 90)
			SendMessage(WM_UNDO, 0, 0);

		
		prevchar = 0;
	}
	else
		prevchar = nChar;

	CEdit::OnKeyUp(nChar, nRepCnt, nFlags);
}
