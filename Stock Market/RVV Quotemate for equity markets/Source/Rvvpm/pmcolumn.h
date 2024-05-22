#ifndef PMCOLUMN
#define PMCOLUMN


class CPMColumn
{
public:

	int			m_id;
	CString		m_value;
	int			m_show;
	int         m_sort;


	CPMColumn() {};
	CPMColumn(int id, CString val, int show, int sort=0);

	CPMColumn& operator=(CPMColumn& tmp);
};

#endif
