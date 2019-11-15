using System.ComponentModel.DataAnnotations;

namespace i21Apis.Models
{
    public class tblARCustomer
    {
        [Key]
        public int intEntityId { get; set; }
        public string strCustomerNumber { get; set; }
        public string strType { get; set; }
        public decimal? dblCreditLimit { get; set; }
        public decimal? dblARBalance { get; set; }
        public string strAccountNumber { get; set; }
        public string strTaxNumber { get; set; }
        public string strCurrency { get; set; }
    }
}