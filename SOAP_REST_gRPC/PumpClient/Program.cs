using PumpClient.PumpServiceReference;
using System;
using System.ServiceModel;

namespace PumpClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            PumpServiceClient client = new PumpServiceClient(instanceContext);

            client.UpdateAndCompileScript(@"D:\GB\SOAP_REST_gRPC\SOAP_REST_gRPC\PumpService\Scripts\Sample.script");
            client.RunScript();

            Console.WriteLine("Please, Enter to exit ...");
            Console.ReadKey(true);
            client.Close();
        }
    }
}
