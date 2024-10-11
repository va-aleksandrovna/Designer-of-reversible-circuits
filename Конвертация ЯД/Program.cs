using System;
using System.Diagnostics;
using System.IO;

namespace Конвертация_ЯД
{
    class Program
    {
        private static bool TryCreateAndRunBat()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("PREPRO21.bat", false))
                {
                    writer.WriteLine("copy n_7925_79-Au-197.dat prepro21.in");
                    writer.WriteLine("linear.exe");
                    writer.WriteLine("copy LINEAR.OUT PREPRO21.OUT");
                    writer.WriteLine("del LINEAR.OUT");
                    writer.WriteLine("groupie.exe");
                    writer.WriteLine("copy GROUPIE.OUT PREPRO22.OUT");
                }
                Process.Start("PREPRO21.bat");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        static void Main(string[] args)
        {
            TryCreateAndRunBat();
        }
    }
}