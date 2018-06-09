*******************************Urian Paul Danut**************************************************
*************************************************************************************************
*****A Cryptography API: Next Generation Key Storage Provider for Cryptography in the Cloud******
*************************************************************************************************
Certificate Authority
	This is a hierarchical Certificate Authority developed with OpenSSL library and has the following
architecture: rootCertificate-- this is considered the root certificate authority and has the role
			     to release Intermediate certificates.
	      intermediateCertificate-- this is the CA, signed by rootCA, and which has the role to release
			     user certificates.
	The functionalities of this Certificate Authority are called by web application, which sends user's
	information to this CA and the CA releases a private/public key pair and a certificate.