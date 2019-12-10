using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HordeFlow.Core.Models
{
    public interface IBaseEntity<TKey> : IConcurrencyEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        TKey Id { get; set; }
    }    
}