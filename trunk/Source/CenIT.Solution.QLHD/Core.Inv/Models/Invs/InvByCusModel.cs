using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Core.Inv.Models.Invs
{
    [XmlRoot(ElementName = "Data")]
    public class DataInvByCusModel
    {
        [XmlElement(ElementName = "Item")] public List<InvByCusModel> Items { get; set; }
    }

    [XmlRoot(ElementName = "Item")]
    public class InvByCusModel
    {
        [XmlElement(ElementName = "Amount")]
        [DisplayName("Tổng tiền của hóa đơn")]
        public string Amount { get; set; }

        [XmlElement(ElementName = "index")]
        [DisplayName("Tháng xuất hóa đơn")]
        public string Index { get; set; }

        [XmlElement(ElementName = "invNum")]
        [DisplayName("Số hóa đơn")]
        public string InvNum { get; set; }

        [XmlElement(ElementName = "invToken")]
        [DisplayName("Chuỗi token để xác định hóa đơn")]
        public string InvToken { get; set; }

        [XmlElement(ElementName = "name")]
        [DisplayName("Tên hóa đơn")]
        public string Name { get; set; }

        [XmlElement(ElementName = "pattern")]
        [DisplayName("Mẫu hóa đơn")]
        public string Pattern { get; set; }

        [XmlElement(ElementName = "payment")]
        [DisplayName("Trạng thái thanh toán(0,1)")]
        public string Payment { get; set; }

        [XmlElement(ElementName = "publishDate")]
        [DisplayName("Ngày phát hành hóa đơn")]
        public string PublishDate { get; set; }

        [XmlElement(ElementName = "serial")]
        [DisplayName("Serial hóa đơn")]
        public string Serial { get; set; }

        [XmlElement(ElementName = "signStatus")]
        [DisplayName("Trạng thái kí khách hàng")]
        public string SignStatus { get; set; }

        [XmlElement(ElementName = "status")]
        [DisplayName("Trạng thái hóa đơn (1,3,4)")]
        public string Status { get; set; }

        [XmlElement(ElementName = "fkey")]
        [DisplayName("FKey nhận dạng hoá đơn")]
        public string Fkey { get; set; }

        [XmlElement(ElementName = "total")]
        [DisplayName("Tổng tiền (Trước thuế)")]
        public string Total { get; set; }

        [XmlElement(ElementName = "cusname")]
        [DisplayName("Tên khách hàng")]
        public string Cusname { get; set; }
    }
}