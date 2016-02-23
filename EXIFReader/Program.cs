using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXIFReader
{
    class Program
    {
        static string pathName = "";
        static string contextName = "";
        static void Main(string[] args)
        {
            try
            {
                if (args[0] != null)
                {
                    
                    EXIFData ed = new EXIFData(args[0]);
                    ed.extractInfo(ed.PicturePath);
                    Environment.Exit(0);
                }

                else
                {
                    Environment.Exit(0);
                }
            }

            catch
            {
                checkReg();
                Console.WriteLine("Added to registry");
                Console.ReadKey();

            }
        }

        public static void checkReg()
        {
            string MenuName = "Show on Google Map";
            string Command = "Show on Google Map\\command";
            RegistryKey menu = null;
            RegistryKey value = null;
            pathName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            try
            {
                menu = Registry.ClassesRoot.OpenSubKey("*\\shell", true);
                menu.CreateSubKey(MenuName);


                value = Registry.ClassesRoot.OpenSubKey("*\\shell", true);
                value.CreateSubKey(Command);
                
                if (value != null)
                {
                    RegistryKey cValue = Registry.ClassesRoot.OpenSubKey("*\\shell\\Show on Google Map\\command", true);
                    cValue.SetValue("", pathName + "\\EXIFReader.exe" + " " + "%1");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured");
            }
        }


        public void checkReg1()
        {

        }
    }
}
