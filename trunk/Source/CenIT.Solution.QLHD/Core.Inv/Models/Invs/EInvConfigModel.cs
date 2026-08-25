namespace Core.Inv.Models.Invs
{
    public class EInvConfigModel
    {
        public string EInvoice_ProdUnit { get; set; }

        public string EInvoice_Service_Account { get; set; }

        public string EInvoice_Service_ACPassword { get; set; }

        public string EInvoice_ProdName { get; set; } =
            "Dịch vụ thu gom, vận chuyển và xử lý rác thải sinh hoạt tháng {0}";

        public string EInvoice_VATRate { get; set; } = "10";

        public string EInvoice_Grouping_CusCode { get; set; }

        public string EInvoice_Grouping_CusBuyer { get; set; }

        public string EInvoice_Grouping_CusName { get; set; }

        public string EInvoice_Grouping_CusPhone { get; set; }

        public string EInvoice_Grouping_CusTaxCode { get; set; }

        public string EInvoice_Grouping_CusAddress { get; set; }

        public string EInvoice_Grouping_CusEmail { get; set; }

        public string EInvoice_Grouping_CusType { get; set; }

        public string EInvoice_Grouping_BusinessNotInv_CusCode { get; set; }

        public string EInvoice_Grouping_BusinessNotInv_Buyer { get; set; }
        public string EInvoice_Grouping_BusinessNotInv_CusName { get; set; }

        public string EInvoice_Grouping_BusinessNotInv_CusPhone { get; set; }

        public string EInvoice_Grouping_BusinessNotInv_CusTaxCode { get; set; }

        public string EInvoice_Grouping_BusinessNotInv_CusAddress { get; set; }

        public string EInvoice_Grouping_BusinessNotInv_CusEmail { get; set; }

        public string EInvoice_Grouping_BusinessNotInv_CusType { get; set; }

        public string EInvoice_Grouping_ProName { get; set; }

        public string EInvoice_Grouping_ProUnit { get; set; }

        public string EInvoice_Buyer_NotTaxCode { get; set; }

        public string Email_Subject { get; set; }
    }
}