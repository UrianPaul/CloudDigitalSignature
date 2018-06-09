// This is the main DLL file.

#include "stdafx.h"
#include <atlstr.h>
#include "certInstDLL.h"
#include <wchar.h>

//use InteropServices namespace for Marshal class
using namespace System::Runtime::InteropServices;

//manually typedef these to avoid including windows.h
typedef wchar_t WCHAR;
typedef WCHAR* LPWSTR;

//specify that this function is implemented in another cpp file
extern bool instalare_Certificat(LPWSTR certSubject, LPWSTR privKey, LPWSTR keyStoreProv);

bool certInstDLL::CertificateUtils::InstallCertificate(String^ subject, String^ privateKey, String^ keyStoreProvider)
{
	//copy data from managed to unmanaged types
	IntPtr pSubject = Marshal::StringToHGlobalUni(subject);
	IntPtr pPrivateKey = Marshal::StringToHGlobalUni(privateKey);
	IntPtr pKeyStoreProvider = Marshal::StringToHGlobalUni(keyStoreProvider);

	//call unmanaged function
	if (instalare_Certificat((LPWSTR)pSubject.ToPointer(), (LPWSTR)pPrivateKey.ToPointer(), (LPWSTR)pKeyStoreProvider.ToPointer()))
	{
		Marshal::FreeHGlobal(pSubject);
		Marshal::FreeHGlobal(pPrivateKey);
		Marshal::FreeHGlobal(pKeyStoreProvider);
		return true;
	}

	//free unmanaged data
	Marshal::FreeHGlobal(pSubject);
	Marshal::FreeHGlobal(pPrivateKey);
	Marshal::FreeHGlobal(pKeyStoreProvider);
	return false;
}
