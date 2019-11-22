using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HordeFlow.Data;

namespace HordeFlow.Models
{
    public class Tenant : ITenant, IBaseEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string HostName { get; set; }
        public DatabaseProvider DatabaseProvider { get; set; } = DatabaseProvider.SqlServer;
        public byte[] ConcurrencyStamp { get; set; }
        public DateTime? ConcurrencyTimeStamp { get; set; }
        public bool IsDedicated { get; set; }
    }
}