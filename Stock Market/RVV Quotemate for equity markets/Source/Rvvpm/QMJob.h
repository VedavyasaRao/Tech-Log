#pragma once

class CQMJob
{
public:
	enum Job_type
	{
		None, QureyIndicies, QureyQuotes, QueryNews, Print, LoadPortfolio, Sort
	} ;
public:
	Job_type	m_type;
	CQMJob(Job_type	job_type=None);
};

class CQMJobManager
{
public:
	typedef	CArray<CQMJob, CQMJob&>	JOBLIST;

public:
	JOBLIST				m_joblist;
	CCriticalSection	m_cs;

public:
	void AddJob(CQMJob::Job_type job_type);
	void ProcessJob();
	void Clean();
};

