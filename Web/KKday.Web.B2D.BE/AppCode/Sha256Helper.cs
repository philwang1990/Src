using System;
using System.Security.Cryptography;
using System.Text;

namespace KKday.Web.B2D.BE.AppCode
{
    public class Sha256Helper
    {
        //SHA256編碼
        public static String Gethash(String token)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(token));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
