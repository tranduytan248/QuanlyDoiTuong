using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Inv.Models.Invs
{
    [XmlRoot(ElementName = "Product")]
    public class InvProduct
    {
        [XmlElement(ElementName = "ProdName")] public string ProdName { get; set; }

        [XmlElement(ElementName = "ProdUnit")] public string ProdUnit { get; set; }

        [XmlElement(ElementName = "ProdQuantity")]
        public string ProdQuantity { get; set; }

        [XmlElement(ElementName = "ProdPrice")]
        public string ProdPrice { get; set; }

        /// <summary>
        ///     Tổng tiền sau thuế
        /// </summary>
        [XmlElement(ElementName = "Amount")]
        public string Amount { get; set; }

        /// <summary>
        ///     Tổng tiền trước thuế
        /// </summary>
        [XmlElement(ElementName = "Total")]
        public string Total { get; set; }

        /// <summary>
        ///     VAT
        /// </summary>
        [XmlElement(ElementName = "VATRate")]
        public string TaxRate { get; set; }

        /// <summary>
        ///     Tiền thuế VAT
        /// </summary>
        [XmlElement(ElementName = "VATAmount")]
        public string TaxAmount { get; set; }

        /// <summary>
        ///     Tính chất:
        ///     - 0: Hàng hóa, dịch vụ;
        ///     - 1: Khuyến mại;
        ///     - 2: Chiết khấu thương mại;
        ///     - 4: Ghi chú/diễn giải
        /// </summary>
        [XmlElement(ElementName = "IsSum")]
        public string IsSum { get; set; } = "0";

        [XmlIgnore] public Guid? ProdId { get; set; }

        [XmlIgnore] public double Price { get; set; }
    }

    [XmlRoot(ElementName = "Products")]
    public class InvProducts
    {
        [XmlElement(ElementName = "Product")]
        public List<InvProduct> ListProducts { get; set; } = new List<InvProduct>();
    }

    [XmlRoot(ElementName = "Invoice")]
    public class InvInvoice
    {
        //[XmlElement(ElementName = "Pattern")]
        //public string Pattern { get; set; }

        //[XmlElement(ElementName = "Serial")]
        //public string Serial { get; set; }

        [XmlElement(ElementName = "CusCode")] public string CusCode { get; set; }

        [XmlElement(ElementName = "CusBankNo")]
        public string CusBankNo { get; set; }

        public string CusBankName { get; set; }

        [XmlElement(ElementName = "Buyer")] public string Buyer { get; set; }

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

        [XmlElement(ElementName = "DiscountAmount")]
        public string DiscountAmount { get; set; }

        [XmlElement(ElementName = "VATRate")] public string TaxRate { get; set; }

        [XmlElement(ElementName = "VATAmount")]
        public string TaxAmount { get; set; }

        [XmlElement(ElementName = "Amount")] public string Amount { get; set; }

        [XmlElement(ElementName = "AmountInWords")]
        public string AmountInWords { get; set; }

        [XmlIgnore] public string CusEmail { get; set; }

        [XmlIgnore] public string CusType { get; set; } // 1 - Doanh nghiệp || O - Hộ dân

        [XmlIgnore] public bool IsPayed { get; set; } = true;

        [XmlElement(ElementName = "PaymentStatus")]
        public string PaymentStatus { get; set; }

        [XmlElement(ElementName = "EmailDeliver")]
        public string EmailDeliver { get; set; }

        [XmlElement(ElementName = "Extra")] public string Extra { get; set; } // Lưu danh sách phiếu vào ghi chú

        //[XmlElement(ElementName = "Fkey")] public string FKey { get; set; }
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

        [XmlElement(ElementName = "Note")] public string Note { get; set; }

        [XmlElement(ElementName = "Extra9")] public string Extra9 { get; set; }
        [XmlElement(ElementName = "Extra10")] public string Extra10 { get; set; }
        [XmlElement(ElementName = "CCCDan")] public string CCCDan { get; set; }

        [XmlIgnore] public string SellerName { get; set; }

        [XmlIgnore] public string SellerTaxCode { get; set; }

        [XmlIgnore] public string SellerAddress { get; set; }
    }

    [XmlRoot(ElementName = "Inv")]
    public class InvInv
    {
        [XmlIgnore] public Guid? ContractId { get; set; }

        [XmlElement(ElementName = "key")] public string FKey { get; set; }

        [XmlElement(ElementName = "Invoice")] public InvInvoice Invoice { get; set; } = new InvInvoice();
    }

    [XmlRoot(ElementName = "Invoices")]
    public class InvInvoices
    {
        [XmlIgnore] public string Pattern { get; set; }
        [XmlIgnore] public string Serial { get; set; }
        [XmlElement(ElementName = "Inv")] public List<InvInv> ListInvs { get; set; } = new List<InvInv>();
    }

    public class InvProductModel
    {
        public string Ticket { get; set; }

        public string ProdName { get; set; } =
            "Dịch vụ thu gom, vận chuyển và xử lý rác thải sinh hoạt Tháng {0}"; //Dịch vụ thu gom, vận chuyển và xử lý rác thải sinh hoạt Tháng {0}

        public string ProdUnit { get; set; }
        public string ProdQuantity { get; set; }
        public string ProdPrice { get; set; }
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "Item")]
    public class InvoiceItem
    {
        [XmlElement(ElementName = "index")] public string Index { get; set; }

        [XmlElement(ElementName = "cusCode")] public string CusCode { get; set; }

        [XmlElement(ElementName = "month")] public string Month { get; set; }

        [XmlElement(ElementName = "name")] public string Name { get; set; }

        [XmlElement(ElementName = "publishDate")]
        public string PublishDate { get; set; }

        [XmlElement(ElementName = "signStatus")]
        public string SignStatus { get; set; }

        [XmlElement(ElementName = "pattern")] public string Pattern { get; set; }

        [XmlElement(ElementName = "serial")] public string Serial { get; set; }

        [XmlElement(ElementName = "invNum")] public string InvNum { get; set; }

        [XmlElement(ElementName = "amount")] public string Amount { get; set; }

        [XmlElement(ElementName = "status")] public string Status { get; set; }

        [XmlElement(ElementName = "cusname")] public string Cusname { get; set; }

        [XmlElement(ElementName = "buyer")] public string Buyer { get; set; }

        [XmlElement(ElementName = "publishBy")]
        public string PublishBy { get; set; }

        [XmlElement(ElementName = "payment")] public string Payment { get; set; }

        [XmlElement(ElementName = "converted")]
        public string Converted { get; set; }
    }

    [XmlRoot(ElementName = "Data")]
    public class DataInvoices
    {
        [XmlElement(ElementName = "Item")] public List<InvoiceItem> InvoiceItems { get; set; }
    }

    public class InvInvoiceModel
    {
        public InvCustomers CusInfos { get; set; }
        public InvInvoices InvInfos { get; set; }
    }
}