using System.Collections.Generic;
using System.Configuration;
using CenIT.Lib.SMSBrandName.Providers;
using TSFramework.Plugable.Interfaces;

namespace CenIT.Lib.SMSBrandName.Helpers
{
    public class APIHelper
    {

        // Ký tự đặc biệt của XML: Có 5 ký tự đặc biệt của XML, khi gửi theo kiểu XML, nếu
        // gặp 5 ký tự đặc biệt là “, ‘, <, >, & thì phải thay thế tương ứng như sau:
        //  " &quot;
        //  ' &apos;
        //  < &lt;
        //  > &gt;
        //  & &amp;

        private readonly string _baseUrl;

        public APIHelper()
        {
            _baseUrl = ConfigurationManager.AppSettings["BASE_URL_SMS_API"];
        }

        private string Urlapi(string name)
        {
            string urlConvert = ConfigurationManager.AppSettings[name];
            urlConvert = urlConvert.Replace("SSOAPIAND", "&");
            return urlConvert;
        }

        private Dictionary<string, string> HeaderParams(IBasePrincipal user = null)
        {
            //var user = User == null ? HttpContext.Current.User as BasePrincipal : User;
            return new Dictionary<string, string>
            {
            };
        }

        /// <summary>
        /// Gọi API của SMS BrandName
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public T CallAPI<T>(string data, IBasePrincipal user = null)
        {
            string url = _baseUrl + Urlapi("URL_SMS_BRANDNAME");
            return WebApiProviders.PostRaw<T>(url, data, HeaderParams(user));
        }


    }
}
