#include <stdio.h>
#include <afx.h>
#include <mapi.h>
#include <conio.h>
#include <io.h>
#include <stdlib.h>
#include <direct.h>
#include <string>

std::string sourcefile;
std::string destinationfile;
int chunksize = 0;

std::string removequotes(std::string tempin)
{
	std::string temp = tempin;
	int k = temp.length();
	if (temp[0] == '"')
		temp = temp.substr(1);
	else
		return temp;
	if (temp[k - 2] == '"')
		temp = temp.substr(0, k - 2);
	return temp;
}

void Disassembler()
{

	char dstfilename[1000];

	CFile	srcfile(sourcefile.c_str(), CFile::typeBinary | CFile::modeRead );
	DWORD	srcfilesize = srcfile.GetLength(), byteswritten=0;
	char    *filebuf =  new char [chunksize];
	int		count=0, buflength=chunksize;

	sprintf (dstfilename, "Del %s.??", destinationfile.c_str());
	system (dstfilename);

	while (srcfilesize > byteswritten)
	{
		sprintf (dstfilename, "%s.%02d", destinationfile.c_str(), count);
		
		CFile  tgtfile(dstfilename, CFile::typeBinary | CFile::modeCreate | CFile::modeWrite );
		
		if ((byteswritten + buflength) > srcfilesize)
			buflength = srcfilesize - byteswritten;

		srcfile.Read(filebuf, buflength);
		tgtfile.Write(filebuf, buflength);
		tgtfile.Flush();
		tgtfile.Close();
		printf ("Writing file: %s \n", dstfilename);
		count++;
		byteswritten += buflength;
	}
	srcfile.Close();
	delete[] filebuf;

	printf("%d files were created.\n", count);
}


void Assembler()
{
	char	srcfilename[1000];
	char	*filebuf = NULL;
	long	buflength;
	int		i = 0;

	CFile  tgtfile(destinationfile.c_str(), CFile::typeBinary | CFile::modeWrite | CFile::modeCreate );

	while(1)
	{
		sprintf(srcfilename, "%s.%02d", sourcefile.c_str(), i);
		if (_access(srcfilename, 0) != 0)
			break;

		printf("Reading file %s \n", srcfilename);
		CFile  srcfile(srcfilename, CFile::typeBinary | CFile::modeRead );
		buflength = srcfile.GetLength();
		filebuf = new char[buflength];
		srcfile.Read(filebuf, buflength);
		tgtfile.Write(filebuf, buflength);
		tgtfile.Flush();
		delete [] filebuf ;
		srcfile.Close();

		i++;
	}
	printf("Writing done \n");
	tgtfile.Close();

}



void main(int argc, char **argv)
{

	if (argc < 2)
	{
		printf("Syntax: FileSplitterMerger {D|A}\nD-->Disassemble    A-->Assemble\n\n");
		printf("FileSplitterMerger D	SourceFile TruncationSize [TargetFile]\n");
		printf("FileSplitterMerger A	SourceFile [TargetFile]\n\n");
		printf("Example: FileSplitterMerger D Test.Dat 10485760\n");
		printf("Splits  Test.Dat into 10 MB files Test00.dat Test001.Dat etc\n\n");
		printf("Example: FileSplitterMerger D Test.Dat 10485760 Pieces.dat\n");
		printf("Splits  Test.Dat into 10 MB files Pieces00.dat Pieces01.Dat etc\n\n");
		printf("Example: FileSplitterMerger A Test.Dat \n");
		printf("Merges Test00.dat Test001.Dat etc into Test.Dat \n\n");
		printf("Example: FileSplitterMerger A Test.Dat  Pieces.dat\n");
		printf("Merges Test00.dat Test001.Dat etc into Pieces.Dat \n\n");
		getch();
		return;
	}


	if (*argv[1] == 'D' || *argv[1] == 'd')
	{
		sourcefile = removequotes(argv[2]);
		chunksize = atol(argv[3]);
		if (argc == 5)
			destinationfile = removequotes(argv[4]);
		else
			destinationfile = sourcefile;

		if (_access(sourcefile.c_str(), 0) != 0)
		{
			printf("%s does not exist\n", sourcefile.c_str());
			return;
		}
		Disassembler();
	}

	if (*argv[1] == 'A' || *argv[1] == 'a')
	{
		sourcefile = removequotes(argv[2]);
		if (argc == 4)
			destinationfile = removequotes(argv[3]);
		else
			destinationfile = sourcefile;
		Assembler();
	}



}
