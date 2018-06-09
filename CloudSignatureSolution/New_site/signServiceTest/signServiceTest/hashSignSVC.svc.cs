using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Org.BouncyCastle;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using signServiceTest;

namespace signServiceTest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "hashSignSVC" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select hashSignSVC.svc or hashSignSVC.svc.cs at the Solution Explorer and start debugging.
    public class hashSignSVC : IhashSignSVC
    {
        public byte[] SignandReturn(string myAlias, byte[] myHash, string myPassword)
       {
            byte[] signedData;
            signedData = signHash(myAlias, myHash, myPassword);
            if(signedData!=null)
            {
                return signedData;
            }
            return null;
        }

        public byte[] signHash(string myAlias, byte[] myHash, string mypassword)
        {
            bool existAlias = false;
            bool existKey = false;
            string path = @"E:\Licenta_2016_Urian\New_site\New_site\Content\Alias_list";
            string keyDirectory = string.Empty;

            string keyPath = string.Empty;
            string certPath = string.Empty;

            //search for key directory path to get the private key.
            keyDirectory = searchAlias(myAlias, path);
            if(keyDirectory!=null)
            {
                existAlias = true;
            }
            else
            {
                Console.WriteLine("You don't have an alias created yet!");
                return null;
            }

            //search for the cert+key in the directory we have already found.
            List<string> certKeyDetails;
            string certName = string.Empty;

            certKeyDetails = existKeyCert(keyDirectory);
            if(certKeyDetails != null)
            {
                existKey = true;
            }
            else
            {
                Console.WriteLine("You don't have a generated certificate yet!");
                return null;
            }

            //load certificate
            certPath = certKeyDetails[0];
            X509Certificate2 myCert = new X509Certificate2(certPath);
            
            //load key.
            keyPath = certKeyDetails[1];
            RSAParameters newParams= ImportRSAKey(keyPath, mypassword);

            //sign data.
            //AsymmetricCipherKeyPair myKey;
            //IPasswordFinder passFinder = new PasswordStore(mypassword.ToCharArray());
            //using (var reader = File.OpenText(keyPath.ToString()))
            //    myKey = (AsymmetricCipherKeyPair)new PemReader(reader, passFinder).ReadObject();
            //byte[] signedData;
            //AsymmetricKeyParameter privateKey = myKey.Private;
            //ISigner mySignature = SignerUtilities.GetSigner("SHA256withRSA");
            //mySignature.Init(true, privateKey);
            //mySignature.BlockUpdate(myHash, 0, myHash.Length);
            //signedData = mySignature.GenerateSignature();
            //if(signedData!=null)
            //{
            //    return signedData;
            //}

            byte[] signedData;
            signedData = HashAndSignBytes(myHash, newParams);

            if (VerifySignedHash(myHash, signedData, newParams,myCert))
            {
                Console.WriteLine("The signature has been verified!");
                return signedData;
            }

            return null;
        }
        public static RSAParameters ImportRSAKey(string fileName, string password)
        {
            StreamReader sr = new StreamReader(fileName);

            IPasswordFinder passwordFinder = new PasswordStore(password.ToCharArray());

            PemReader pr = new PemReader(sr, passwordFinder);

            AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
            RSAParameters rsaParameter = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);

            return rsaParameter;

        }

        public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
               //SHA256 hashAlg = new SHA256Managed();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                //sha256managed modified
                return RSAalg.SignHash(DataToSign, CryptoConfig.MapNameToOID("SHA256"));
                //return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key,X509Certificate2 myCert)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.
                //RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();
                RSACryptoServiceProvider RSAalg = (RSACryptoServiceProvider)myCert.PublicKey.Key;

                //RSAalg.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                //sha256 managed
                return RSAalg.VerifyHash(DataToVerify, CryptoConfig.MapNameToOID("SHA256"), SignedData);
                //return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return false;
            }
        }

        public string searchAlias(string myAlias,string path)
        {
            bool found = false;
            string keyDirectory = string.Empty;
            try
            {
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(path));

                foreach (var dir in dirs)
                {
                    String search = dir.Substring(dir.LastIndexOf("\\") + 1);
                    if (myAlias.Equals(search))
                    {
                        keyDirectory = dir;
                        found = true;
                    }
                }
                
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }
            
            if(found)
            {
                return keyDirectory;
            }
            return null;
        }

        public List<string> existKeyCert(string keyPath)
        {   
            try
            {
                List<string> files = new List<string>(Directory.EnumerateFiles(keyPath));

                if(files.Count!=2)
                {
                    return null;
                }
                else
                {
                    return files;
                }

            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }

            return null;
        }
    }

    class PasswordStore : IPasswordFinder
    {
        private char[] password;

        public PasswordStore(
                    char[] password)
        {
            this.password = password;
        }

        public char[] GetPassword()
        {
            return (char[])password.Clone();
        }

    }
}
