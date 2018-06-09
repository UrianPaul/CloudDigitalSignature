*******************************Urian Paul Danut**************************************************
*************************************************************************************************
*****A Cryptography API: Next Generation Key Storage Provider for Cryptography in the Cloud******
*************************************************************************************************
KeyStorageProviderSample
	This project contains two applications, both written in C programming language.
**Provider
	This is the Key Storage Provider application, which gets the document hash from the signing application,
initializes the cryptographic provider's components and some key components which were read from the selected
certificate, and calls the signing web service, sending to it the document's hash, the key alias, and a password
provided by user, to decrypt the private key, before signing.
	It contains also a pop-up window which is activated after the user chooses a certificate to sign the
document, so a pop-up will appear and he will have to introduce his password/pin.
**Client
	This is an Client application, written also in C programming language which allows the user to register a
new provider to his working machine, to unregister a provider from his working machine and to enumerate installed
providers.
		Usage: SampleKSPconfig -enum | -register | -unregister