// certInstDLL.h

#pragma once

using namespace System;

namespace certInstDLL {

	public ref class CertificateUtils
	{
	public:
		static bool InstallCertificate(String^ subject, String^ privateKey, String^ keyStoreProvider);

	};
}
