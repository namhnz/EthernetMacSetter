using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestCodeProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(GetRegistryValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Class\{4d36e972-e325-11ce-bfc1-08002be10318}\0002\NetworkAddress", 0)); 

            Console.ReadLine();
        }

        public static void TestConvertMac()
        {
            string str = "08E-DB+.9B*B-0F-9F";
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            str = rgx.Replace(str, "");
            Console.WriteLine(str);
        }

        public static string ConvertMac(string anyTypeOfMac, bool toSeparate)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            string onlyWordMac = rgx.Replace(anyTypeOfMac, "");
            if (onlyWordMac.Length != 12)
                return null;

            string macAfterConvert = null;

            if (toSeparate)
            {
                macAfterConvert = "";
                for (int i = 0; i < onlyWordMac.Length; i++)
                {
                    macAfterConvert += onlyWordMac[i];
                    if ((i % 2 == 1) && (i != onlyWordMac.Length - 1))
                    {
                        macAfterConvert += "-";
                    }
                }
            }
            else
            {
                macAfterConvert = onlyWordMac;
            }
            return macAfterConvert;
        }

        public static void ReadFile(string path, int lastPosition)
        {
            if (File.Exists(path))
            {
                StreamReader reader = new StreamReader(path);


                string line = null;
                int i = 0;
                do
                {
                    line = reader.ReadLine();
                    i++;
                    if (line == null)
                    {
                        reader.DiscardBufferedData();
                        reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
                        i = 0;

                        line = reader.ReadLine();
                        i++;
                        break;
                    }
                }
                while (i <= lastPosition);

                Console.WriteLine(line);
            }
        }

        static object GetRegistryValue(string fullPath, object defaultValue)
        {
            string keyName = Path.GetDirectoryName(fullPath);
            Console.WriteLine(keyName);
            string valueName = Path.GetFileName(fullPath);
            Console.WriteLine(valueName);
            return Registry.GetValue(keyName, valueName, defaultValue);
        }
    }
}
