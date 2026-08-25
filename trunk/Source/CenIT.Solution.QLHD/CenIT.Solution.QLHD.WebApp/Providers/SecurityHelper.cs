using System;
using System.Web;

namespace CenIT.Solution.QLHD.WebApp.Providers
{
    public class SecurityHelper
    {
        public static string EncryptId(Guid id)
        {
            byte[] bytes = id.ToByteArray();
            return HttpServerUtility.UrlTokenEncode(bytes);
        }

        public static Guid DecryptId(string encryptedId)
        {
            byte[] bytes = HttpServerUtility.UrlTokenDecode(encryptedId);
            return new Guid(bytes);
        }
    }
}