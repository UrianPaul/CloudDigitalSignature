using System;
using System.Linq;

namespace CspEnum
{
    using CERTENROLLLib;

    class Program
    {
        static void Main(string[] args)
        {
            var providers = new CCspInformations();
            providers.AddAvailableCsps();
            int i = 0;
            foreach (var providerInfo in providers.OfType<CCspInformation>())
            {
                Console.WriteLine("{0} {1}: {2}",
                  i++
                  , providerInfo.LegacyCsp ? "LEGACY" : "CNG"
                  , providerInfo.Name);
            }
            Console.WriteLine("\n\nHit any key to continue: ");
            Console.ReadKey();
        }
    }
}