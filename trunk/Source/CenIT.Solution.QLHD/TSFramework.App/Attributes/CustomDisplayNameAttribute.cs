using System.ComponentModel;
using TSFramework.App.Processors;

namespace TSFramework.App.Attributes
{
    public class CustomDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _resourceName;

        public CustomDisplayNameAttribute(string resourceName)
        {
            _resourceName = resourceName;
        }

        /// <summary>
        /// Tra ve nhan hien thi lay tu bang thong diep (Sys_Messages).
        /// Neu khoa chua duoc khai bao, tra ve chinh ten khoa thay vi null:
        /// ASP.NET MVC se nem ArgumentNullException khi validate model neu
        /// DisplayName la null, lam toan bo request POST loi 500.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                var message = AppProcessor.Messagor.GetMessage(_resourceName);
                if (!string.IsNullOrEmpty(message)) return message;
                if (!string.IsNullOrEmpty(DisplayNameValue)) return DisplayNameValue;
                return _resourceName ?? string.Empty;
            }
        }
    }
}