#pragma once
//Declare ITEM struct again (the same definition from dll sources)
typedef struct {
	unsigned char* data;
	unsigned int len;
} ITEM;


//Declare function pointers
typedef ITEM(*_fptr_callWS)   (char*, ITEM, char*); //pointer to a function with the following signature:  ITEM function_name (char* ,ITEM ,char* )
typedef void(*_fptr_freeITEM) (ITEM); //pointer to a function with the following signature: void function_name (ITEM )

									  /*The ITEM returned is freed using freeItem(ITEM) function*/
ITEM callWS(char* myAlias, ITEM hash, char* myPassword);

/*This function deletes an item which was returned from callWS(...) function*/
void freeITEM(ITEM item);