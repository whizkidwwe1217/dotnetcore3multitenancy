using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Core;
using HordeFlow.Core.Common;
using HordeFlow.Infrastructure.Repositories;

namespace HordeFlow.Infrastructure.Services
{
    public interface IService<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, new()
    {
        IRepository<TEntity, TKey> Repository { get; }
        Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> SeekAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task RemoveBatchAsync(List<TKey> ids, CancellationToken cancellationToken = default(CancellationToken));
        Task RemoveBatchAsync(List<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken));
        Task UpdateAsync(TKey id, TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task Update(TEntity originalEntity, TEntity entity);
        Task Update(TKey id, TEntity entity);
        Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData<TEntity>> SearchAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "", CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData<TEntity>> SearchAsync(SearchParams parameter, CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData> SearchDynamicAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "", CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData> SearchDynamicAsync(SearchParams parameter, CancellationToken cancellationToken = default(CancellationToken));
    }
}