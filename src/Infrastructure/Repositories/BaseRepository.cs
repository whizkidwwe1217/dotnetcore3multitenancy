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
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IBaseEntity<TKey>, new()
    {
        private const string Action = "Update";
        private TEntity modifiedEntity;
        private IRepositoryManager<TKey> repositoryManager;
        public IRepositoryManager<TKey> RepositoryManager { get => repositoryManager; set => repositoryManager = value; }
        public DbContext DbContext => RepositoryManager.DbContext;
        public Tenant Tenant => RepositoryManager.Tenant;
        protected virtual object GetCompanyId() { return null; }
        public virtual bool UseTransaction { get; set; } = true;

        public BaseRepository(IRepositoryManager<TKey> repositoryManager)
        {
            this.repositoryManager = repositoryManager ?? throw new ArgumentNullException(nameof(repositoryManager));
            var type = this.GetType();
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Any(predicate).AnyAsync(cancellationToken);
        }

        protected virtual void AddEntityDefaultTenantValues(TEntity entity)
        {
            if (typeof(ITenantEntity<Guid, TKey>).IsAssignableFrom(typeof(TEntity)))
            {
                ((ITenantEntity<Guid, TKey>)entity).TenantId = Tenant.Id;
            }
        }
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            AddEntityDefaultTenantValues(entity);
            await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
            modifiedEntity = entity;
        }

        private bool Compare<T>(T x, T y)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }

        private bool IsItNew(TEntity entity) => !DbContext.Entry<TEntity>(entity).IsKeySet;

        public virtual async Task UpdateAsync(TKey id, TEntity entity,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Update(id, entity);
            // AddEntityDefaultTenantValues(entity);
            // entity = await ModificationPipeline.Execute(entity);
            // AddEntityTimestampValues(entity, AuditActions.Update);
            // DbContext.Update<TEntity>(entity);
            // await AuditChanges(AuditActions.Update, id, originalEntity, entity);
        }

        public virtual async Task Update(TKey id, TEntity entity)
        {
            AddEntityDefaultTenantValues(entity);
            var originalEntity = await Set<TEntity, TKey>()
                .AsNoTracking()
                .Where(x => Compare<TKey>(x.Id, id))
                .FirstOrDefaultAsync();
            DbContext.Update<TEntity>(entity);
        }

        public virtual async Task Update(TEntity originalEntity, TEntity entity)
        {
            AddEntityDefaultTenantValues(entity);
            DbContext.Update<TEntity>(entity);
            await Task.FromResult(0);
        }

        public virtual async Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (UseTransaction)
                await SaveInTransaction(cancellationToken);
            else
                await Save(cancellationToken);
        }

        protected virtual async Task Save(CancellationToken cancellationToken = default(CancellationToken))
        {
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual void HandleConcurrency(DbUpdateConcurrencyException exception)
        {

        }

        public virtual bool OverrideConcurrencyHandling => false;

        protected virtual async Task SaveInTransaction(CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await Save(cancellationToken);
                    transaction.Commit();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (OverrideConcurrencyHandling)
                        HandleConcurrency(ex);
                    else
                        throw new Exception("This record has been modified by another user", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error saving record", ex);
                }
            }
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Get().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual async Task<TEntity> GetAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await GetAsync(e => e.Id.Equals(id), cancellationToken);
        }

        protected virtual IQueryable<TEntity> GetEntity(bool clearFilter = false)
        {
            var entity = (GetTenantEntity() ?? DbContext.Set<TEntity>());
            var filter = GetDefaultFilter();
            if (filter != null && !clearFilter)
                return entity.Where(filter);
            return entity;
        }

        public virtual async Task<TEntity> SeekAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await GetEntity().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        public virtual async Task<List<TEntity>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await List().ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await List(predicate).ToListAsync(cancellationToken);
        }

        public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            var id = entity.Id;
            DbContext.Set<TEntity>().Remove(entity);
            await Task.FromResult(0);
        }

        public virtual async Task RemoveBatchAsync(List<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
            await Task.FromResult(0);
        }

        public virtual async Task RemoveBatchAsync(List<TKey> ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            var entities = await List(e => ids.Contains(e.Id)).ToListAsync(cancellationToken);
            await RemoveBatchAsync(entities);
        }

        public async virtual Task<SearchResponseData<TEntity>> SearchAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Search(currentPage, pageSize, filter, sort, fields)
                .ToListAsync<TEntity>(currentPage, pageSize, fields, cancellationToken);
        }

        public async virtual Task<SearchResponseData<TEntity>> SearchAsync(
            SearchParams parameter,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Search(parameter)
                .ToListAsync<TEntity>(parameter.CurrentPage, parameter.PageSize,
                    parameter.Fields, cancellationToken);
        }

        public async virtual Task<SearchResponseData> SearchDynamicAsync(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Search(currentPage, pageSize, filter, sort, fields)
                .ToListAsync<object>(currentPage, pageSize, fields, cancellationToken);
        }

        public async virtual Task<SearchResponseData> SearchDynamicAsync(SearchParams parameter,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Search(parameter)
                .ToListAsync<object>(parameter.CurrentPage, parameter.PageSize,
                    parameter.Fields, cancellationToken);
        }

        /* This is are the recommended overridable methods when customizing the repository. */

        public virtual IQueryable<TEntity> Get()
        {
            return GetEntity();
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return GetEntity().Where(predicate);
        }

        public virtual IQueryable<TEntity> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return (GetTenantEntity() ?? DbContext.Set<TEntity>().Where(predicate)).AsNoTracking();
        }

        public virtual Expression<Func<TEntity, bool>> GetDefaultFilter()
        {
            return null;
        }

        public virtual IQueryable<TEntity> List()
        {
            return GetEntity().AsNoTracking();
        }

        public virtual IQueryable<TEntity> List(Expression<Func<TEntity, bool>> predicate)
        {
            return List().Where(predicate);
        }

        public virtual IQueryable<TEntity> Search(int? currentPage = 1, int? pageSize = 100,
            string filter = "", string sort = "", string fields = "")
        {
            return GetEntity().Search<TEntity, TKey>(filter).AsNoTracking();
        }

        public virtual IQueryable<TEntity> Search(SearchParams parameter)
        {
            return GetEntity().Search<TEntity, TKey>(parameter).AsNoTracking();
        }

        public virtual IQueryable<TEntity> Set()
        {
            return GetEntity();
        }

        public virtual IQueryable<TDbEntity> Set<TDbEntity, TDbKey>() where TDbEntity : class, new()
        {
            return (GetTenantEntity<TDbEntity, TDbKey>() ?? DbContext.Set<TDbEntity>());
        }

        public virtual IQueryable<TEntity> GetTenantEntity()
        {
            // if (Tenant != null)
            // {
            //     if (typeof(ITenantEntity<Guid, TKey>).IsAssignableFrom(typeof(TEntity)))
            //     {
            //         return DbContext.Set<TEntity>()
            //             .Cast<ITenantEntity<Guid, TKey>>()
            //             .Where(e => e.TenantId == Tenant.Id && !e.Deleted && e.Active)
            //             .Cast<TEntity>();
            //     }
            // }

            // return null;
            return GetTenantEntity<TEntity, TKey>();
        }

        public virtual IQueryable<TDbEntity> GetTenantEntity<TDbEntity, TDbKey>() where TDbEntity : class, new()
        {
            if (Tenant != null)
            {
                if (typeof(ITenantEntity<Guid, TDbKey>).IsAssignableFrom(typeof(TDbEntity)))
                {
                    return DbContext.Set<TDbEntity>()
                        .Cast<ITenantEntity<Guid, TDbKey>>()
                        .Where(e => e.TenantId == Tenant.Id)
                        .Cast<TDbEntity>();
                }
            }

            return null;
        }
    }
}