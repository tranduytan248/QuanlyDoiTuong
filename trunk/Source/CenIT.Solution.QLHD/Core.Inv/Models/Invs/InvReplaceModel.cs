using System.Xml.Serialization;

namespace Core.Inv.Models.Invs
{
    [XmlRoot(ElementName = "ReplaceInv")]
    public class InvReplaceInv
    {
        [XmlElement(ElementName = "key")] public string FKey { get; set; }

        [XmlElement(ElementName = "CusCode")] public string CusCode { get; set; }

        [XmlIgnore] public string CusEmail { get; set; }
        [XmlElement(ElementName = "CusName")] public string CusName { get; set; }

        [XmlElement(ElementName = "CusAddress")]
        public string CusAddress { get; set; }

        [XmlElement(ElementName = "CusPhone")] public string CusPhone { get; set; }

        [XmlElement(ElementName = "CusTaxCode")]
        public string CusTaxCode { get; set; }

        [XmlElement(ElementName = "PaymentMethod")]
        public string PaymentMethod { get; set; }

        [XmlElement(ElementName = "CurrencyUnit")]
        public string CurrencyUnit { get; set; }

        [XmlElement(ElementName = "KindOfService")]
        public string KindOfService { get; set; }

        [XmlElement(ElementName = "Products")] public InvProducts Products { get; set; }

        [XmlElement(ElementName = "Total")] public string Total { get; set; }

        [XmlElement(ElementName = "VATRate")] public string TaxRate { get; set; }

        [XmlElement(ElementName = "VATAmount")]
        public string TaxAmount { get; set; }

        [XmlElement(ElementName = "Amount")] public string Amount { get; set; }

        [XmlElement(ElementName = "AmountInWords")]
        public string AmountInWords { get; set; }

        [XmlElement(ElementName = "Extra")] public string Extra { get; set; }

        [XmlElement(ElementName = "GrossValue")]
        public string GrossValue { get; set; } = "0";

        [XmlElement(ElementName = "GrossValue0")]
        public string GrossValue0 { get; set; } = "0";

        [XmlElement(ElementName = "VatAmount0")]
        public string VatAmount0 { get; set; } = "0";

        [XmlElement(ElementName = "GrossValue5")]
        public string GrossValue5 { get; set; } = "0";

        [XmlElement(ElementName = "VatAmount5")]
        public string VatAmount5 { get; set; } = "0";

        [XmlElement(ElementName = "GrossValue8")]
        public string GrossValue8 { get; set; } = "0";

        [XmlElement(ElementName = "VatAmount8")]
        public string VatAmount8 { get; set; } = "0";

        [XmlElement(ElementName = "GrossValue10")]
        public string GrossValue10 { get; set; } = "0";

        [XmlElement(ElementName = "VatAmount10")]
        public string VatAmount10 { get; set; } = "0";

        [XmlIgnore] public string SellerName { get; set; }

        [XmlIgnore] public string SellerTaxCode { get; set; }

        [XmlIgnore] public string SellerAddress { get; set; }
    }
}