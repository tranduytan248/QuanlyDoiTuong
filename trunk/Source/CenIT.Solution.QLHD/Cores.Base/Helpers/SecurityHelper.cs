using System;
using System.Web;

namespace Cores.Base.Helpers
{
    public class SecurityHelper
    {
        public static string EncryptId(Guid id)
        {
            var bytes = id.ToByteArray();
            return HttpServerUtility.UrlTokenEncode(bytes);
        }

        public static Guid DecryptId(string encryptedId)
        {
            var bytes = HttpServerUtility.UrlTokenDecode(encryptedId);
            return new Guid(bytes);
        }
    }
}