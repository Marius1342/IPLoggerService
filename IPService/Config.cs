using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPService
{
    internal class Config
    {
        public static int Port { get; set; }
        public static string Token { get; set; }

        public static void ReadConfig()
        {
            if(File.Exists("./config.txt") == false)
            {
                CreateConfig();
            }
            string[] content = File.ReadAllLines("./config.txt");
            foreach (string line in content)
            {
                string key = line.Split('=')[0];
                string value = line.Split('=')[1];
                if(key == "Port")
                {
                    Port = int.Parse(value);
                }else if(key == "Token")
                {
                    Token = value;
                }
            }
        }

        public static void CreateConfig()
        {
            File.WriteAllLines("./config.txt", new string[] { "Token=123", "Port=8084" });
        }

    }
}
