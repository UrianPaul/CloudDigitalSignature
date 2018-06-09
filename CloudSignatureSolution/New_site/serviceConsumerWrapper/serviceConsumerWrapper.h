#pragma once
#include "WebService.h"


using namespace System;
using namespace System::Runtime::InteropServices;

namespace serviceConsumerWrapper {

	public ref class WSConsumer
	{
		public:

			/*The ITEM returned is freed using freeItem(ITEM) function*/
			static ITEM callWS(char* myAlias, ITEM hash, char* myPassword);

			/*This function deletes an item which was returned from callWS(...) function*/
			static void freeITEM(ITEM item);
		
	};
}
