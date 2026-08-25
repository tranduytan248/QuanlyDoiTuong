using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Inv.Models.Invs
{
    [XmlRoot(ElementName = "Customers")]
    public class InvCustomers
    {
        [XmlElement(ElementName = "Customer")] public List<InvCustomerModel> ListCustomers { get; set; }
    }

    [XmlRoot(ElementName = "Customer")]
    public class InvCustomerModel
    {
        [XmlElement(ElementName = "Name")] public string Name { get; set; }

        [XmlElement(ElementName = "Code")] public string Code { get; set; }

        [XmlElement(ElementName = "TaxCode")] public string TaxCode { get; set; }

        [XmlElement(ElementName = "Address")] public string Address { get; set; }

        [XmlElement(ElementName = "BankAccountName")]
        public string BankAccountName { get; set; }

        [XmlElement(ElementName = "BankName")] public string BankName { get; set; }

        [XmlElement(ElementName = "BankNumber")]
        public string BankNumber { get; set; }

        [XmlElement(ElementName = "Email")] public string Email { get; set; }

        [XmlElement(ElementName = "Fax")] public string Fax { get; set; }

        [XmlElement(ElementName = "Phone")] public string Phone { get; set; }

        [XmlElement(ElementName = "ContactPerson")]
        public string ContactPerson { get; set; }

        [XmlElement(ElementName = "RepresentPerson")]
        public string RepresentPerson { get; set; }

        [XmlElement(ElementName = "CusType")] public string CusType { get; set; }
    }
}