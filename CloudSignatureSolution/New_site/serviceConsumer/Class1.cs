using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using serviceConsumer.myService;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ServiceModel;

namespace ManagedDLL
{

    [ComVisible(true)]
    public interface IWSProxy
    {
        IntPtr callWS(string myAlias, IntPtr hash, int len, string myPassword);
    }

    [ComVisible(true), ComDefaultInterface(typeof(IWSProxy))]
    [ClassInterface(ClassInterfaceType.None)]
    public class WSProxy:IWSProxy
    {
        public WSProxy()
        {

        }

        IntPtr IWSProxy.callWS(string myAlias, IntPtr hash,int len, string myPassword)
        {
            BasicHttpBinding basicHttpBinding= new BasicHttpBinding(BasicHttpSecurityMode.None);
            basicHttpBinding.Name= "BasicHttpBinding_Paul";
            basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            EndpointAddress endPointAdress = new EndpointAddress("http://localhost:22987/hashSignSVC.svc");
            IhashSignSVCClient newClient = new IhashSignSVCClient(basicHttpBinding,endPointAdress);
            
            //MessageBox.Show("We're in svc!");
            //prepare alias.
          

            char[] myChar = new char[myAlias.Length - 8];
            for(int i=0;i<myAlias.Length-8;i++)
            {
                myChar[i] = myAlias[i];
            }
            string newAlias = new string(myChar);
            //prepare alias.
            //convert intptr-->byte[]

            byte[] myHash = new byte[len];
            Marshal.Copy(hash, myHash, 0, len);

            byte[] signedData = newClient.SignandReturn(newAlias, myHash, myPassword);
            //test//
            //byte[] testData = new byte[256];
            //for(int i=0;i<256;i++)
            //{
            //    testData[i] = 0x1F;
            //}
            //test//
            newClient.Close();
       
            if (signedData != null)
            {

                int signedLen = signedData.Length;
                IntPtr signature = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)) * signedData.Count());
                Marshal.Copy(signedData, 0, signature, signedData.Count());

                byte[] verificareSigned = new byte[signedLen];
                Marshal.Copy(signature, verificareSigned, 0, signedLen);
               

                return signature;
            }
            return IntPtr.Zero;
        }

    }; //
}
