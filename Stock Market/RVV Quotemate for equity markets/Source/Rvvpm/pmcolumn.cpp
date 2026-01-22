#include <stdafx.h>
#include "pmcolumn.h"

CPMColumn::CPMColumn(int id, CString val, int show, int sort) 
{
	m_id	= id;
	m_value = val;
	m_show	= show;
	m_sort = sort;
}

CPMColumn& CPMColumn::operator=(CPMColumn& tmp)
{
	m_id	= tmp.m_id;
	m_value = tmp.m_value;
	m_show = tmp.m_show;
	m_sort = tmp.m_sort;

	return *this;
}
