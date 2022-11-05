using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("输入要加密的串：");
                var msg = Console.ReadLine();
                Console.WriteLine(Test(msg));
            }

            Console.ReadLine();
        }

        static string Test(string msg)
        {
            return SM4Util.Sm4EncryptECB(msg);
        }
    }
}
