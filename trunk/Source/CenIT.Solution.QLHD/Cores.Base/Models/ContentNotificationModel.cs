using Cores.Base.Enums;

namespace Cores.Base.Models
{
    public class ContentNotificationModel
    {
        //loại thông báo
        public EnumTypeEmail TypeEmail { get; set; }

        //link tra cứu hợp đồng
        public string SearchContractUrl { get; set; }

        //link tra cứu chi tiết hợp đồng
        public string SearchContractDetailUrl { get; set; }

        //Phòng ban
        public Union UnionInfo { get; set; } = new Union();

        //Nội dung hợp đồng
        public Contract ContractInfo { get; set; } = new Contract();

        //Nội dung khách hàng
        public Customer CusInfo { get; set; } = new Customer();

        public User UserInfo { get; set; } = new User();
    }

    public class Union
    {
        public string UnionName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class Contract
    {
        //Mã HĐ = ContractNo/ContractSignal
        public string ContractNo { get; set; }
        public string ContractSignal { get; set; }
        public string ContractNoInfo { get; set; }

        public string SearchContractUrl { get; set; }

        //link tra cứu chi tiết hợp đồng
        public string SearchContractDetailUrl { get; set; }
    }

    public class Customer //Lấy từ Major_Customers
    {
        public string CusName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TypeCus { get; set; }
    }

    public class Notification //Lấy từ Sys_Notification
    {
        public string NotificationCode { get; set; }
        public string ChannelType { get; set; }
        public string Situation { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}