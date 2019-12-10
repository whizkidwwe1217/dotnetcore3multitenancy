using System.Collections.Generic;
using HordeFlow.Core;

namespace HordeFlow.Infrastructure.ViewModels
{
    public class EntityIdListViewModel<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, new()
    {
        public List<TKey> EntityIds { get; set; }
    }
}