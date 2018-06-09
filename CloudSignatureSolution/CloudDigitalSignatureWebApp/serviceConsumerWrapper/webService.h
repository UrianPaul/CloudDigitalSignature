#pragma once


// Use ITEM structure as byte array
typedef struct _ITEM {
	unsigned char* data;
	unsigned int len;

} ITEM;



#ifdef __cplusplus
extern "C" {
#endif
	/*The ITEM returned is freed using freeItem(ITEM) function*/
	_declspec(dllexport) ITEM callWS(char* myAlias, ITEM hash, char* myPassword);

	/*This function deletes an item which was returned from callWS(...) function*/
	_declspec(dllexport) void freeITEM(ITEM item);

#ifdef __cplusplus
}
#endif

