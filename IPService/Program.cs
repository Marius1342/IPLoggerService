using LoggerSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPService
{
    internal class Program
    {
        private static Dictionary<string, string> Keys = new Dictionary<string, string>();
        static void Main(string[] args)
        {
            if (File.Exists("./key.txt") == false)
            {
                File.Create("./key.txt");
            }
            else
            {
                foreach (string line in File.ReadAllLines("./key.txt"))
                {
                    Keys.Add(line.Split(':')[1], line.Split(':')[0]);
                }
            }

            if (args.Length == 1 && args[0] == "-h")
            {
                Console.WriteLine("-h          For help");
                Console.WriteLine("-newKey     For new key, second param Name");
            }

            if (args.Length == 2)
            {
                if (args[0] == "-newKey")
                {
                    Guid g = Guid.NewGuid();
                    string key = Convert.ToBase64String(g.ToByteArray());
                    Console.WriteLine($"New key: {key}");
                    File.AppendAllLines("./key.txt", new string[] { args[1] + ":" + key });
                    return;
                }
            }

            Logger.Init();
            Config.ReadConfig();
            Logger.Log("Start up server");
            WebServer();
            Logger.Log("Exit from user");
            Logger.Close();
        }

        private static void WebServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://*:{Config.Port}/");
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();

                if (context == null)
                {
                    Thread.Sleep(200);
                    continue;
                }

                if (context.Request.RawUrl.Contains(Config.Token))
                {
                    Uri uri;
                    try
                    {
                        uri = new Uri(context.Request.Url.AbsoluteUri);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex.Message);
                        continue;
                    }
                    

                    var pairs = Keys.FirstOrDefault(x => x.Key == uri.Segments[uri.Segments.Length - 1]);
                    string Host = "Unknown Host";
                    if (pairs.Value != null)
                    {
                        Host = pairs.Value;
                    }


                    if (context.Request.Headers["X-Real-IP"] != null)
                    {
                        Logger.Log($"New ip fron {Host} (proxy):{context.Request.Headers["X-Real-IP"]}");
                    }
                    Logger.Log($"New ip from {Host}:{context.Request.RemoteEndPoint}");

                    byte[] success = Encoding.UTF8.GetBytes("OK");
                    try
                    {
                        context.Response.OutputStream.Write(success, 0, success.Length);
                        context.Response.Close();
                    }
                    catch { }
                    Thread.Sleep(200);
                    continue;
                }

                try
                {
                    context.Response.Close();
                }
                catch { }
            }

            Thread.Sleep(200);
        }

    }
}
