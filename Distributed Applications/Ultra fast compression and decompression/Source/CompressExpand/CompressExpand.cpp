// CompressExpand.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "CompressExpand.h"



bool init(bool bfast, short* infoindex);
bool msftcompress(int infoidx, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len);
bool msftexpand(int infoidx, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len);

extern "C" COMPRESSEXPAND_API bool  __stdcall  Initialize(bool bfast, short *ptoken)
{
	return init(bfast, ptoken);
}

extern "C" COMPRESSEXPAND_API bool  __stdcall  Compress(short token, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len)
{
	
	return msftcompress(token, pDest, pDest_len, pSource, source_len);
}

extern "C" COMPRESSEXPAND_API bool  __stdcall  Expand(short token, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len)
{
	
	return msftexpand(token, pDest, pDest_len, pSource, source_len);
}
