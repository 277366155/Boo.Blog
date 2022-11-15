using Org.BouncyCastle.Utilities.Encoders;
using System.Text;

namespace Boo.Blog.ToolKits
{
    public class SM4Util
    {

        private static string _key = "c2l4dW5jbG91ZAo=";


        public static string Sm4EncryptECB(string data)
        {
            byte[] key = Encoding.UTF8.GetBytes(_key);
            byte[] plain = Encoding.UTF8.GetBytes(data);

            byte[] bs = GmUtil.Sm4EncryptECB(key, plain, GmUtil.SM4_ECB_PKCS5Padding);
            return Encoding.UTF8.GetString(Hex.Encode(bs));
        }


        public static string Sm4DecryptECB(string data)
        {
            byte[] key = Encoding.UTF8.GetBytes(_key);
            byte[] cipher = Hex.Decode(data);

            byte[] bs = GmUtil.Sm4DecryptECB(key, cipher, GmUtil.SM4_ECB_PKCS5Padding);
            return Encoding.UTF8.GetString(bs);
        }

    }
}
