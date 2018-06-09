using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using callWebService.myServiceReference;
using System.Security.Cryptography;

namespace callWebService
{
    class Program
    {
        static void Main(string[] args)
        {
            string alias = "bogdanmarc@gmail.com";
            string password = "paul1994";
            Program myProgram = new Program();
            byte[] signedData = myProgram.callWS(alias, myProgram.getSha256Hash(), password);
            if(signedData!=null)
            {
                Console.WriteLine("Work done!");
            }
            else
            {
                Console.WriteLine("Something failed!");
            }
        }

        public byte[] callWS(string alias, byte[] myHash,string myPassword)
        {
            IhashSignSVCClient client = new IhashSignSVCClient();
            byte[] signedData= client.SignandReturn(alias, getSha256Hash(), myPassword);
            if (signedData != null)
            {
                Console.WriteLine(signedData);
                return signedData;
            }
            return null;
        }
        
        public byte[] getSha256Hash()
        {
            string data = "urian paul danut";
            byte[] bPlainText;
            bPlainText = Encoding.UTF8.GetBytes(data);
            SHA256 sha256 = new SHA256Managed();
            bPlainText = sha256.ComputeHash(bPlainText);
            return bPlainText;
        }
    }
}
