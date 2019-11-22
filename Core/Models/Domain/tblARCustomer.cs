using System;
using System.ComponentModel.DataAnnotations;

namespace HordeFlow.Models
{
    public class tblARCustomer : IBaseEntity<int>
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
        public int Id { get; set; }

        [Timestamp]
        public byte[] ConcurrencyStamp { get; set; }
        public DateTime? ConcurrencyTimeStamp { get; set; }
    }
}