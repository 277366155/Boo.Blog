using Boo.Blog.ToolKits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Test
{
    internal class SM4Test : ITest
    {

        public void TestStart()
        {
            while (true)
            {
                Console.WriteLine("请选择：\r\n\t1-加密\r\n\t2-解密");
                Console.WriteLine("----------------------------------------");
                var whileFlag = "1";
                var read = Console.ReadLine();

                if (read == "1")
                {
                    while (whileFlag.ToLower() != "q")
                    {
                        Console.Write("待加密串（输入q返回菜单）：");
                        whileFlag = Console.ReadLine();
                        if (whileFlag?.ToLower() == "q")
                            continue;
                        Console.Write("加密结果：");
                        Console.WriteLine(SM4Util.Sm4EncryptECB(whileFlag));
                        Console.Write("解密结果：");
                        Console.WriteLine(SM4Util.Sm4DecryptECB(SM4Util.Sm4EncryptECB(whileFlag)));
                        Console.WriteLine("----------------------------------------");
                    }
                }
                else if (read == "2")
                {
                    while (whileFlag?.ToLower() != "q")
                    {
                        Console.Write("待解密串（输入q返回菜单）：");
                        whileFlag = Console.ReadLine();
                        if (whileFlag?.ToLower() == "q")
                            continue;
                        Console.Write("解密结果：");
                        Console.WriteLine(SM4Util.Sm4DecryptECB(whileFlag));
                        Console.Write("加密结果：");
                        Console.WriteLine(SM4Util.Sm4EncryptECB(SM4Util.Sm4DecryptECB(whileFlag)));
                        Console.WriteLine("----------------------------------------");
                    }                    
                }
                Console.Clear();
            }
        }

        public Task TestStartAsync()
        {
            throw new NotImplementedException();
        }
    }
}
