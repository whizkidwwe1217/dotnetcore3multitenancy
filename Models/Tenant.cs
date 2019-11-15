using System;
using System.ComponentModel.DataAnnotations;
using i21Apis.Multitenancy;

namespace i21Apis.Models
{
    public class Tenant
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string HostName { get; set; }
    }
}