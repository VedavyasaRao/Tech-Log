#include "StdAfx.h"
#include "QMJob.h"
#include "RVVPM.h"


CQMJob::CQMJob(Job_type	job_type) :m_type(job_type)
{
};


void CQMJobManager::AddJob(CQMJob::Job_type	job_type)
{
	CRVVPMApp*  pApp = (CRVVPMApp*)AfxGetApp();

	m_cs.Lock();
	if (job_type == CQMJob::LoadPortfolio)
	{
		pApp->m_abort = true;
		m_joblist.RemoveAll();
	}
	m_joblist.Add(CQMJob(job_type));
	m_cs.Unlock();
	pApp->m_queryevent.PulseEvent();
}

void CQMJobManager::ProcessJob()
{
	CRVVPMApp*  pApp = (CRVVPMApp*)AfxGetApp();

	m_cs.Lock();
	if (m_joblist.GetSize() == 0)
	{
		m_cs.Unlock();
		return;
	}
	CQMJob job = m_joblist.ElementAt(0);
	m_joblist.RemoveAt(0);
	m_cs.Unlock();
	pApp->m_abort = false;

	if (job.m_type == CQMJob::QureyQuotes)
	{
		pApp->FillData();
	}
	else if (job.m_type == CQMJob::QueryNews)
	{
		pApp->LoadNews();
	}
	else if (job.m_type == CQMJob::Print)
	{
		pApp->PrintData();
	}
	else if (job.m_type == CQMJob::LoadPortfolio)
	{
		pApp->LoadPortfolio();
	}
	else if (job.m_type == CQMJob::Sort)
	{
		pApp->SortData();
	}
	ProcessJob();
}

void CQMJobManager::Clean()
{
	m_cs.Lock();
	m_joblist.RemoveAll();
	m_cs.Unlock();
}

