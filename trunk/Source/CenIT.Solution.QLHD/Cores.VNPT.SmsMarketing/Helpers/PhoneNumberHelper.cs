using System;

namespace Cores.VNPT.SmsMarketing.Helpers
{
    public static class PhoneNumberHelper
    {
        public static string ConvertPhone(string phoneNum)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNum)) return "";
                phoneNum = phoneNum.Replace(".", "");
                var firstNumberInPhone = phoneNum.Substring(0, 1);
                if (firstNumberInPhone == "0") return "84" + phoneNum.Substring(1, phoneNum.Length - 1);

                return phoneNum;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}