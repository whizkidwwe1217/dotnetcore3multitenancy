using System.ComponentModel.DataAnnotations;

namespace HordeFlow.Models
{
    public class tblSMCompanyLocation
    {
        [Key]
        public int intCompanyLocationId { get; set; }
        public string strLocationName { get; set; }
        public string strLocationNumber { get; set; }
        public string strLocationType { get; set; }
        public string strAddress { get; set; }
        public string strZipPostalCode { get; set; }
        public string strCity { get; set; }
        public string strStateProvince { get; set; }
        public string strCountry { get; set; }
        public string strPhone { get; set; }
        public string strFax { get; set; }
        public string strEmail { get; set; }
        public string strWebsite { get; set; }
    }
}