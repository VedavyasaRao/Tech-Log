#include "stdafx.h"
#include <windows.h>
#include <stdlib.h>
#include <stdio.h>

//https://msdn.microsoft.com/en-us/library/ff552291(v=vs.85).aspx
//https://msdn.microsoft.com/en-in/library/cc704588.aspx

#define STATUS_SUCCESS 0
#define STATUS_BUFFER_ALL_ZEROS 0x00000117


#define CMP_FRM_SLOW     COMPRESSION_FORMAT_LZNT1|COMPRESSION_ENGINE_MAXIMUM
#define CMP_FRM_FAST     COMPRESSION_FORMAT_LZNT1|COMPRESSION_ENGINE_STANDARD

typedef DWORD (__stdcall *RtlCompressBuffer_Fn)(
  __in   USHORT CompressionFormatAndEngine,
  __in   PUCHAR UncompressedBuffer,
  __in   ULONG UncompressedBufferSize,
  __out  PUCHAR CompressedBuffer,
  __in   ULONG CompressedBufferSize,
  __in   ULONG UncompressedChunkSize,
  __out  PULONG FinalCompressedSize,
  __in   PVOID WorkSpace
);


typedef DWORD (__stdcall *RtlDecompressBuffer_Fn)(
  __in   USHORT CompressionFormat,
  __out  PUCHAR UncompressedBuffer,
  __in   ULONG UncompressedBufferSize,
  __in   PUCHAR CompressedBuffer,
  __in   ULONG CompressedBufferSize,
  __out  PULONG FinalUncompressedSize
);

typedef DWORD (__stdcall *RtlGetCompressionWorkSpaceSize_Fn)(
  __in   USHORT CompressionFormatAndEngine,
  __out  PULONG CompressBufferWorkSpaceSize,
  __out  PULONG CompressFragmentWorkSpaceSize
);


RtlCompressBuffer_Fn fcmp;
RtlDecompressBuffer_Fn fdcp;
RtlGetCompressionWorkSpaceSize_Fn fgcw;

struct InitInfo
{
	static short index;
	USHORT  cmpfromatandengine;
	void	*tmpMem;
	ULONG CompressBufferWorkSpaceSize;
	ULONG CompressFragmentWorkSpaceSize;
} initInfoList[10];


short InitInfo::index = 0;
bool init(bool bfast, short* infoindex)
{
	if (InitInfo::index == 10)
		return false;

	*infoindex = InitInfo::index++;
	InitInfo& info = initInfoList[*infoindex];
	bool bret = false;
	DWORD rc;
	HMODULE hDLL = LoadLibrary ("ntdll.dll");
	
	info.cmpfromatandengine = ((bfast)?(CMP_FRM_FAST):(CMP_FRM_SLOW)); 

	fcmp = 0;
	fdcp = 0;
	fgcw = 0;
	
	if ( hDLL != NULL )
	{
		fcmp = (RtlCompressBuffer_Fn) GetProcAddress(hDLL, "RtlCompressBuffer");
		fdcp = (RtlDecompressBuffer_Fn) GetProcAddress(hDLL, "RtlDecompressBuffer");
		fgcw = (RtlGetCompressionWorkSpaceSize_Fn) GetProcAddress(hDLL, "RtlGetCompressionWorkSpaceSize");
		
		if ( fcmp && fdcp && fgcw)
		{
			rc = (*fgcw)(info.cmpfromatandengine, &info.CompressBufferWorkSpaceSize,  &info.CompressFragmentWorkSpaceSize);
			if (rc != STATUS_SUCCESS)
				return false;
			info.tmpMem = LocalAlloc(LPTR, info.CompressBufferWorkSpaceSize);
			if (info.tmpMem  != nullptr)
				bret = true;
		}
	}
	
	return bret;
}

bool msftcompress(int infoidx, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len)
{
	if (infoidx < 0 || infoidx > 9)
		return false;
	DWORD rc = ((*fcmp)(initInfoList[infoidx].cmpfromatandengine, pSource, source_len, pDest, *pDest_len, initInfoList[infoidx].CompressFragmentWorkSpaceSize, pDest_len, initInfoList[infoidx].tmpMem));
	return ((rc == STATUS_SUCCESS) || (rc == STATUS_BUFFER_ALL_ZEROS));
}

bool msftexpand(int infoidx, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len)
{
	if (infoidx < 0 || infoidx > 9)
		return false;
	return  ((*fdcp)(initInfoList[infoidx].cmpfromatandengine, pDest, *pDest_len, pSource, source_len, pDest_len) == STATUS_SUCCESS);
}


