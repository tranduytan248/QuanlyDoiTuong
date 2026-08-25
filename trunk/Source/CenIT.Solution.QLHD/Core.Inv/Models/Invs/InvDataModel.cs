using System.Collections.Generic;

namespace Core.Inv.Models.Invs
{
    public class InvDataModel
    {
        public List<InvCustomerModel> CusInfos { get; } = new List<InvCustomerModel>();
        public InvInvoices Invoices { get; set; } = new InvInvoices();
    }
}