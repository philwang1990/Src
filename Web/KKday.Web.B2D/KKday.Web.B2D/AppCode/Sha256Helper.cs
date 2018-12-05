using System;
using System.Security.Cryptography;
using System.Text;

namespace KKday.Web.B2D.EC.AppCode
{
    public class Sha256Helper
    {
        //SHA256編碼
        public static String Gethash(String token)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();//建立一個SHA256
            byte[] source = Encoding.Default.GetBytes(token);//將字串轉為Byte[]
            byte[] crypto = sha256.ComputeHash(source);//進行SHA256加密
            var chiperPasswod = Convert.ToBase64String(crypto);//把加密後的字串從Byte[]轉為字串

            return chiperPasswod;
        }
    }
}
