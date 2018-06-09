#include"dllWrapper.h"
#include <Windows.h>
#include <stdio.h>

ITEM callWS(char* myAlias, ITEM hash, char* myPassword)
{
	ITEM signature;
	signature.data = NULL;
	signature.len = 0;

	//Load DLL
	HINSTANCE dllHandle = LoadLibraryA("serviceConsumerWrapper.dll");
	if (!dllHandle)
	{
		printf("Error loading serviceConsumerWrapper.dll\n");
		return signature;
	}

	//Get callWS function pointer from loaded DLL
	_fptr_callWS fptrCallWS = (_fptr_callWS)GetProcAddress(dllHandle, "callWS");
	if (!fptrCallWS)
	{
		printf("Error loading callWS function pointer\n");
		return signature;
	}

	//call function callWS using function pointer
	return fptrCallWS(myAlias, hash, myPassword);

}


void freeITEM(ITEM item)
{
	//Load DLL
	HINSTANCE dllHandle = LoadLibraryA("serviceConsumerWrapper.dll");
	if (!dllHandle)
	{
		printf("Error loading serviceConsumerWrapper.dll\n");
		return;
	}

	//Get callWS function pointer from loaded DLL
	_fptr_freeITEM fptrFreeItem = (_fptr_freeITEM)GetProcAddress(dllHandle, "freeITEM");
	if (!fptrFreeItem)
	{
		printf("Error loading freeITEM function pointer\n");
		return;
	}

	//call function freeITEM using function pointer
	fptrFreeItem(item);
}
