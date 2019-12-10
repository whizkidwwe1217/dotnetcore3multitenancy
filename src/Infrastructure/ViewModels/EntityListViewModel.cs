using System.Collections.Generic;
using HordeFlow.Core;

namespace HordeFlow.Infrastructure.ViewModels
{
    public class EntityListViewModel<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, new()
    {
        public List<TEntity> Entities { get; set; }
    }
}