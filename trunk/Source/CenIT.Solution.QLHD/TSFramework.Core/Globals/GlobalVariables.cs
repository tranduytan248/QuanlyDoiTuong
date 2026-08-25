using System.Configuration;
using TSFramework.Core.Consts;

namespace TSFramework.Core.Globals
{
    public static class GlobalVariables
    {
        private static string _appLanguageCode = "en-US";
        private static int _cookieExpired = 7;
        private static bool _hasEncrypt;

        public static string LanguageCode
        {
            get
            {
                var sAppLanguageCode = ConfigurationManager.AppSettings[AppSettingConst.LANGUAGE_CODE_KEY];
                _appLanguageCode = string.IsNullOrEmpty(sAppLanguageCode) ? _appLanguageCode : sAppLanguageCode;

                return _appLanguageCode;
            }
            set => _appLanguageCode = value;
        }

        public static int CookieExpired
        {
            get
            {
                var sCookieExpired = ConfigurationManager.AppSettings[AppSettingConst.COOKIE_EXPIRED_KEY];
                _cookieExpired = string.IsNullOrEmpty(sCookieExpired) ? _cookieExpired : int.Parse(sCookieExpired);
                return _cookieExpired;
            }
        }
    }
}