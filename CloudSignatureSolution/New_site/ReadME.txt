*******************************Urian Paul Danut**************************************************
*************************************************************************************************
*****A Cryptography API: Next Generation Key Storage Provider for Cryptography in the Cloud******
*************************************************************************************************
New_site
	This project contains the following applications:
		*New_site --the web application used to register new users+get user certificates.
		*signServiceTest --the signing web service, which is the one which actually signs the
				document's hash
		*serviceConsumer --a c# class library which exposes a method(callWS) which calls the web service.
		*serviceConsumerWrapper --a c++ application which calls the method exposed by the class library
		The last 2 applications, were needed to be able to call the web service, which is a C# application
	     	by the KSP which is a C application
	Each application contains a ReadME.txt file which explains its usage.