using System.Net;
using static System.Net.WebRequestMethods;

namespace IPServiceClient
{
    internal class Program
    {

        private static int interval = 300;
        static async Task Main(string[] args)
        {
            
            if (args.Length == 1 || args.Length < 3) {
                Console.WriteLine("First argument must be token!");
                Console.WriteLine("Second argument must be key!");
                Console.WriteLine("Third argument must be ip/domain!");
                Console.WriteLine("Forth argument must be the interval time in minutes, standard: 300 min");
            }

            if(args.Length < 3)
            {
                Console.WriteLine("First argument must be token!");
                Console.WriteLine("Second argument must be key!");
                Console.WriteLine("Third argument must be ip/domain!");
            }
            if(args.Length == 4)
            {
                if(int.TryParse(args[3], out int res) == false)
                {
                    Console.WriteLine("Cannot parse int!");
                }
                else
                {
                    Console.WriteLine($"Call interval set to {res} minutes");
                    interval = res;
                }
                
            }

            string url = $"{args[2]}/{args[0]}/{args[1]}";

            Console.WriteLine($"Url: {url}");

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };


            HttpClient client = new HttpClient(handler);
            while (true)
            {
                HttpResponseMessage res;
                try
                {
                     res = await client.GetAsync(url);
                }catch(Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    await Task.Delay(3600000);
                    continue;
                }
                
                if( await res.Content.ReadAsStringAsync() != "OK")
                {
                    Console.WriteLine($"Error with token or key or url: {url}");
                }
                else
                {
                    Console.WriteLine($"Successfully send ip");
                }

                await Task.Delay(interval * 60000);
            }


        }
    }
}
