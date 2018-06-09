*******************************Urian Paul Danut**************************************************
*************************************************************************************************
*****A Cryptography API: Next Generation Key Storage Provider for Cryptography in the Cloud******
*************************************************************************************************
SignServiceTest
	This project contains two applications, 
both written in C# language.
	The first one is a C# wcf web service, 
which has the role of signing service. This is the 
web service called by the KSP, at the moment of signing.
This service gets the document's hash, an alias which 
will be used to find the key container, and a password 
which will be used to decrypt the key, which was encrypted
after it was generated, with AES256.
The web service signs the hash with the user's private key
and returns the hash to the KSP. For performing cryptographic
operations, this web services uses a library called
BouncyCastle.Crypto.dll.
	The other application, callWEbService, was used 
just to test the service's functionalities.