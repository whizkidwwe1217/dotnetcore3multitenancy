using System;
using System.ComponentModel.DataAnnotations;
using i21Apis.Data;
using i21Apis.Multitenancy;

namespace i21Apis.Models
{
    public class Tenant : ITenant
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string HostName { get; set; }
        public string DatabaseProvider { get; set; } = "SqlServer";
    }
}