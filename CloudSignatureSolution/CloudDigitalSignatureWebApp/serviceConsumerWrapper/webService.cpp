#include "Stdafx.h"
#include "webService.h"
#include "serviceConsumerWrapper.h"


ITEM callWS(char* myAlias, ITEM hash, char* myPassword)
{
	return serviceConsumerWrapper::WSConsumer::callWS(myAlias, hash, myPassword);
}

void freeITEM(ITEM item)
{
	serviceConsumerWrapper::WSConsumer::freeITEM(item);
}