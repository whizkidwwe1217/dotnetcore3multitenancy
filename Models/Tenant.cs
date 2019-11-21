using System;
using System.ComponentModel.DataAnnotations;
using HordeFlow.Data;
using HordeFlow.Multitenancy;

namespace HordeFlow.Models
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