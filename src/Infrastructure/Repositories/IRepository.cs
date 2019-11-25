using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Core;
using HordeFlow.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace HordeFlow.Infrastructure.Repositories
{
    public interface IRepository<TKey, TEntity> where TEntity : class, IBaseEntity<TKey>, new()
    {
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Any(Expression<Func<TEntity, bool>> predicate);
        Expression<Func<TEntity, bool>> GetDefaultFilter();
        IQueryable<TEntity> List();
        IQueryable<TEntity> List(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> Search(int? currentPage = 1, int? pageSize = 100, string filter = "", string sort = "", string fields = "");
        IQueryable<TEntity> Search(SearchParams parameter);
        IQueryable<TEntity> Set();
        IQueryable<TEntity> GetTenantEntity();
        IQueryable<TDbEntity> Set<TDbEntity, TDbKey>() where TDbEntity : class, new();
        IQueryable<TDbEntity> GetTenantEntity<TDbEntity, TDbKey>() where TDbEntity : class, new();
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
        void HandleConcurrency(DbUpdateConcurrencyException exception);
        bool OverrideConcurrencyHandling { get; }
        Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData<TEntity>> SearchAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData<TEntity>> SearchAsync(SearchParams parameter,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData> SearchDynamicAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken));
        Task<SearchResponseData> SearchDynamicAsync(SearchParams parameter,
            CancellationToken cancellationToken = default(CancellationToken));
        DbContext DbContext { get; }
        bool UseTransaction { get; set; }
    }
}