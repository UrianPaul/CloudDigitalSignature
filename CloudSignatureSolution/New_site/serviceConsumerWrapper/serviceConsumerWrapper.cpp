// This is the main DLL file.

#include "stdafx.h"
#include "serviceConsumerWrapper.h"

void serviceConsumerWrapper::WSConsumer::freeITEM(ITEM item)
{
	if (item.data != nullptr)
		delete[] item.data;
}

ITEM serviceConsumerWrapper::WSConsumer::callWS(char* myAlias, ITEM hash, char* myPassword)
{
	//Marshal unmanaged types to managed types
	String^ sMyAlias = Marshal::PtrToStringAnsi(IntPtr(myAlias));
	String^ sMyPassword = Marshal::PtrToStringAnsi(IntPtr(myPassword));
	array<unsigned char, 1>^ pHash = gcnew array<unsigned char, 1>(hash.len);
	Marshal::Copy(IntPtr(hash.data), pHash, 0, hash.len);

	array<unsigned char, 1>^ pSignature = nullptr;

	//call web service through proxy managed DLL
	 
	ManagedDLL::WSProxy newService;
	pSignature = newService.callWS(sMyAlias, pHash, sMyPassword);
	
	//pSignature = serviceConsumerWrapper::WSConsumer::callWS(sMyAlias, pHash, sMyPassword);

	ITEM signature = { nullptr,0 };

	//Marshal managed type to unmanaged type
	signature.len = pSignature->Length;
	signature.data = new unsigned char[signature.len];
	Marshal::Copy(pSignature, 0, IntPtr(signature.data), signature.len);

	return signature;
}


