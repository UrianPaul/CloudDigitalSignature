#include "Stdafx.h"
#pragma comment (lib, "crypt32")

#include <iostream>
#include <stdio.h>
#include <windows.h>
#include <Wincrypt.h>
#include <string.h>
using namespace std;


#define MY_ENCODING_TYPE  (PKCS_7_ASN_ENCODING | X509_ASN_ENCODING)


bool instalare_Certificat(LPWSTR certSubject, LPWSTR privKey, LPWSTR keyStoreProv)
{
	HCERTSTORE hSysStore = NULL;
	PCCERT_CONTEXT  pDesiredCert = NULL;   // Set to NULL for the first 
	PCCERT_CONTEXT     pCertContext = NULL;

	PCRYPT_KEY_PROV_INFO pvData = (CRYPT_KEY_PROV_INFO*)malloc(sizeof(CRYPT_KEY_PROV_INFO));

	//open My store.

	if (hSysStore = CertOpenStore(
		CERT_STORE_PROV_SYSTEM,          // The store provider type
		0,                               // The encoding type is
										 // not needed
		NULL,                            // Use the default HCRYPTPROV
		CERT_SYSTEM_STORE_CURRENT_USER,  // Set the store location in a
										 // registry location
		L"MY"                            // The store name as a Unicode 
										 // string
	))
	{

	}
	else
	{
		return false;
	}

	//search a specified certificate by subject.

	if (pDesiredCert = CertFindCertificateInStore(
		hSysStore,
		MY_ENCODING_TYPE,             // Use X509_ASN_ENCODING
		0,                            // No dwFlags needed 
		CERT_FIND_SUBJECT_STR,        // Find a certificate with a
									  // subject that matches the 
									  // string in the next parameter
		certSubject, // The Unicode string to be found
					 // in a certificate's subject
		NULL))                        // NULL for the first call to the
									  // function 
									  // In all subsequent
									  // calls, it is the last pointer
									  // returned by the function
	{

	}
	else
	{
		return false;
	}


	//initializing the structure.
	pvData->dwProvType = 0; //0 means key from ksp;
	pvData->pwszContainerName = privKey; //key_name;
	pvData->pwszProvName = keyStoreProv;//ksp name;
	pvData->dwFlags = 2;
	pvData->cProvParam = 0;
	pvData->rgProvParam = NULL;
	pvData->dwKeySpec = AT_SIGNATURE;


	if (CertSetCertificateContextProperty(
		pDesiredCert,
		CERT_KEY_PROV_INFO_PROP_ID,
		NULL,
		pvData))
	{

	}
	else
	{
		return false;
	}

	free(pvData);

	return true;
}