using TSFramework.App.Attributes;

namespace Modules.Cate.Areas.Cate.Models
{
    public class SearchCustomerModel
    {
        [CustomDisplayName("Customer_Label_TuKhoa")]
        public string FullName { get; set; }

        [CustomDisplayName("Customer_Label_UserType")]
        public string UserType { get; set; }

    }
}