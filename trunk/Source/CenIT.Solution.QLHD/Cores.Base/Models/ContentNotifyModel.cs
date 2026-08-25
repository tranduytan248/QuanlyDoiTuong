using Cores.Base.Enums;
using TSFramework.Core.Enums;

namespace Cores.Base.Models
{
    public class ContentNotifyModel
    {
        //public User Sender { get; set; } = new User();

        //public User Receiver { get; set; } = new User();

        public InsiteNotificationModel InsiteNotification = new InsiteNotificationModel();

        //loại thông báo
        public EnumTypeEmail TypeEmail { get; set; }

        //Thông tin đơn vị
        public Union UnionInfo { get; set; } = new Union();

        //Thông tin hợp đồng
        public Contract ContractInfo { get; set; } = new Contract();

        //Thông tin khách hàng
        public Customer CusInfo { get; set; } = new Customer();

        public User UserInfo { get; set; } = new User();
    }

    public class InsiteNotificationModel
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public EnumMsgIcon Icon { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
        public string Placement { get; set; }
    }
}