// OtherTriggerPage.cpp : implementation file
//

#include "stdafx.h"
#include "RVVPM.h"
#include "OtherTriggerPage.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// COtherTriggerPage property page

IMPLEMENT_DYNCREATE(COtherTriggerPage, CPropertyPage)

COtherTriggerPage::COtherTriggerPage() : CPropertyPage(COtherTriggerPage::IDD)
{
	//{{AFX_DATA_INIT(COtherTriggerPage)
	m_highmvpts = _T("");
	m_highpc = _T("");
	m_highpts = _T("");
	m_lowpc = _T("");
	m_lowpts = _T("");
	m_highmvtds = _T("");
	m_lowmvpts = _T("");
	m_lowmvtds = _T("");
	m_newspd = _T("");
	m_clearnews = FALSE;
	//}}AFX_DATA_INIT
}

COtherTriggerPage::~COtherTriggerPage()
{
}

void COtherTriggerPage::DoDataExchange(CDataExchange* pDX)
{
	CPropertyPage::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(COtherTriggerPage)
	DDX_Text(pDX, IDC_EDIT_HIGHMVPTS, m_highmvpts);
	DDX_Text(pDX, IDC_EDIT_HIGHPC, m_highpc);
	DDX_Text(pDX, IDC_EDIT_HIGHPTS, m_highpts);
	DDX_Text(pDX, IDC_EDIT_LOWPC, m_lowpc);
	DDX_Text(pDX, IDC_EDIT_LOWPTS, m_lowpts);
	DDX_Text(pDX, IDC_EDIT_HIGHMV_TD, m_highmvtds);
	DDX_Text(pDX, IDC_EDIT_LOW_MV_PTS, m_lowmvpts);
	DDX_Text(pDX, IDC_EDIT_LOWMV_TD, m_lowmvtds);
	DDX_Text(pDX, IDC_EDIT_NEWPD2, m_newspd);
	DDX_Check(pDX, IDC_AUTO4, m_clearnews);
	DDX_Text(pDX, IDC_EDIT_HIGHMVPRC, m_highmvprc);
	DDX_Text(pDX, IDC_EDIT_HIGHMV_TDPRC, m_highmvtdsprc);
	DDX_Text(pDX, IDC_EDIT_LOW_MV_PRC, m_lowmvprc);
	DDX_Text(pDX, IDC_EDIT_LOWMV_TDPRC, m_lowmvtdsprc);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(COtherTriggerPage, CPropertyPage)
	//{{AFX_MSG_MAP(COtherTriggerPage)
	ON_BN_CLICKED(IDC_RESET, OnReset)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// COtherTriggerPage message handlers

BOOL COtherTriggerPage::OnInitDialog() 
{
	CPropertyPage::OnInitDialog();
	
	// TODO: Add extra initialization here
	m_pimmaster = &((CRVVPMApp *)AfxGetApp())->m_imMaster;
	LoadTriggers();
	::SendMessage(GetDlgItem(IDC_RESET)->m_hWnd, BM_SETIMAGE, IMAGE_ICON, (long)AfxGetApp()->LoadIcon(IDI_RESET));
	
	UpdateData(FALSE);
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void COtherTriggerPage::OnOK() 
{
	// TODO: Add your specialized code here and/or call the base class
	UpdateData(FALSE);

	SaveTriggers();
	
	CPropertyPage::OnOK();
}



void COtherTriggerPage::LoadTriggers()
{

	string buf;

	m_pimmaster->GetPrivateProfileString("Triggers", "HIGHPOINTS", "", buf);
	m_highpts = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "LOWPOINTS", "", buf);
	m_lowpts = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "HIGHPERCENT", "", buf);
	m_highpc = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "LOWPERCENT", "", buf);
	m_lowpc = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVEPOINTS", "", buf);
	m_highmvpts = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVETRADES", "", buf);
	m_highmvtds = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVEPOINTS", "", buf);
	m_lowmvpts = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVETRADES", "", buf);
	m_lowmvtds = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVEPRC", "", buf);
	m_highmvprc = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "HIGHMOVETRADESPRC", "", buf);
	m_highmvtdsprc = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVEPRC", "", buf);
	m_lowmvprc = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "LOWMOVETRADESPRC", "", buf);
	m_lowmvtdsprc = buf.c_str();

	m_pimmaster->GetPrivateProfileString("Triggers", "NEWSPERIOD", "", buf);
	m_newspd = buf.c_str();



	UpdateData(FALSE);

}

void COtherTriggerPage::SaveTriggers()
{

	char buf[30];


	UpdateData(TRUE);
	lstrcpy(buf,  m_highpts );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "HIGHPOINTS", buf);

	lstrcpy(buf,  m_lowpts );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "LOWPOINTS", buf);

	lstrcpy(buf,  m_highpc );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "HIGHPERCENT", buf);

	lstrcpy(buf,  m_lowpc );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "LOWPERCENT", buf);

	lstrcpy(buf,  m_highmvpts );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "HIGHMOVEPOINTS", buf);

	lstrcpy(buf,  m_highmvtds );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "HIGHMOVETRADES", buf);

	lstrcpy(buf,  m_lowmvpts );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "LOWMOVEPOINTS", buf);

	lstrcpy(buf, m_lowmvtds);
	m_pimmaster->WritePrivateProfileString(  "Triggers", "LOWMOVETRADES", buf);
 
	lstrcpy(buf, m_highmvprc);
	m_pimmaster->WritePrivateProfileString("Triggers", "HIGHMOVEPRC", buf);

	lstrcpy(buf, m_highmvtdsprc);
	m_pimmaster->WritePrivateProfileString("Triggers", "HIGHMOVETRADESPRC", buf);

	lstrcpy(buf, m_lowmvprc);
	m_pimmaster->WritePrivateProfileString("Triggers", "LOWMOVEPRC", buf);

	lstrcpy(buf, m_lowmvtdsprc);
	m_pimmaster->WritePrivateProfileString("Triggers", "LOWMOVETRADESPRC", buf);

	lstrcpy(buf,  m_newspd  );
	m_pimmaster->WritePrivateProfileString(  "Triggers", "NEWSPERIOD", buf);
	

}


void COtherTriggerPage::OnReset() 
{
	// TODO: Add your command handler code here
	m_highmvpts = _T("");
	m_highpc = _T("");
	m_highpts = _T("");
	m_lowmvpts = _T("");
	m_lowpc = _T("");
	m_lowpts = _T("");
	m_highmvtds = _T("");
	m_lowmvtds = _T("");
	m_newspd = _T("");
	m_highmvprc = _T("");
	m_highmvtdsprc = _T("");
	m_lowmvprc = _T("");
	m_lowmvtdsprc = _T("");

	UpdateData(FALSE);
	SaveTriggers();

	
}

