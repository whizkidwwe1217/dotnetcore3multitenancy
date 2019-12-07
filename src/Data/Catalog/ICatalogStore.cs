using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HordeFlow.Data
{
    public interface ICatalogStore<TTenant> where TTenant : class
    {
        Task<List<TTenant>> GetTenantsAsync(Expression<Func<TTenant, bool>> predicate = null,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}