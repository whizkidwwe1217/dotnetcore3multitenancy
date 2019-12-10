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
    public abstract class BaseService<TEntity, TKey, TRoleRepository> : BaseService<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>, new()
        where TRoleRepository : IRepository<TEntity, TKey>
    {
        public new TRoleRepository Repository { get; set; }
        public BaseService(TRoleRepository repository) : base(repository)
        {
            Repository = repository;
        }
    }

    public abstract class BaseService<TEntity, TKey> : IService<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>, new()
    {
        public BaseService(IRepository<TEntity, TKey> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IRepository<TEntity, TKey> Repository { get; private set; }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task AddAsync(TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await Repository.AddAsync(entity, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.GetAsync(id, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.GetAsync(predicate, cancellationToken);
        }

        public virtual async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.ListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.ListAsync(predicate, cancellationToken);
        }

        public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Repository.RemoveAsync(entity, cancellationToken);
        }

        public virtual async Task RemoveBatchAsync(List<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Repository.RemoveBatchAsync(entities, cancellationToken);
        }

        public virtual async Task RemoveBatchAsync(List<TKey> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Repository.RemoveBatchAsync(ids, cancellationToken);
        }

        public virtual async Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await Repository.SaveAsync(cancellationToken);
        }

        public async Task<SearchResponseData<TEntity>> SearchAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.SearchAsync(currentPage, pageSize, filter, sort, fields, cancellationToken);
        }

        public async Task<SearchResponseData<TEntity>> SearchAsync(SearchParams parameter,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.SearchAsync(parameter, cancellationToken);
        }

        public async Task<SearchResponseData> SearchDynamicAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.SearchDynamicAsync(currentPage, pageSize, filter, sort, fields, cancellationToken);
        }

        public async Task<SearchResponseData> SearchDynamicAsync(SearchParams parameter,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.SearchDynamicAsync(parameter, cancellationToken);
        }

        public virtual async Task<TEntity> SeekAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Repository.SeekAsync(id, cancellationToken);
        }

        public virtual async Task UpdateAsync(TKey id, TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await Repository.UpdateAsync(id, entity, cancellationToken);
        }

        public virtual async Task Update(TKey id, TEntity entity)
        {
            await Repository.Update(id, entity);
        }

        public virtual async Task Update(TEntity originalEntity, TEntity entity)
        {
            await Repository.Update(originalEntity, entity);
        }
    }
}