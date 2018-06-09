*******************************Urian Paul Danut**************************************************
*************************************************************************************************
*****A Cryptography API: Next Generation Key Storage Provider for Cryptography in the Cloud******
*************************************************************************************************
CertInstDLL
	This is a dynamic library project which allows the user,
through the website application, to install the generated digital 
certificate in his local windows certificate store.
	First at all, the certificate is installed in windows 
certificate store. After that, there's a method called 
InstallCertificate which calls another method, instalare_Certificat, 
which searches for the specified certificate in local windows certificate 
store and modifies some certificates properties, like
	pvData->pwszContainerName = privKey; //key_name;
	pvData->pwszProvName = keyStoreProv;//ksp name;