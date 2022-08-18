// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the COMPRESSEXPAND_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// COMPRESSEXPAND_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef COMPRESSEXPAND_EXPORTS
#define COMPRESSEXPAND_API __declspec(dllexport)
#else
#define COMPRESSEXPAND_API __declspec(dllimport)
#endif

extern "C" COMPRESSEXPAND_API bool  __stdcall  Initialize(bool bfast, short *ptoken);
extern "C" COMPRESSEXPAND_API bool  __stdcall  Compress(short token, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len);
extern "C" COMPRESSEXPAND_API bool  __stdcall  Expand(short token, unsigned char *pDest, unsigned long  *pDest_len, unsigned char *pSource, unsigned long  source_len);

