using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace signServiceTest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IhashSignSVC" in both code and config file together.
    [ServiceContract]
    public interface IhashSignSVC
    {
        [OperationContract]
        byte[] SignandReturn(string myAlias, byte[] myHash, string myPassword);
    }
}
