using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HordeFlow.Core;
using HordeFlow.Core.Common;
using HordeFlow.Infrastructure.Services;
using HordeFlow.Infrastructure.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HordeFlow.Infrastructure.Controllers
{
    public interface IServiceController<TEntity, TKey> where TEntity : class, IBaseEntity<TKey>, new()
    {
        IService<TEntity, TKey> Service { get; }
        Task<ActionResult<List<TEntity>>> List(CancellationToken cancellationToken = default(CancellationToken));
        Task<ActionResult<TEntity>> Get(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IActionResult> Create([FromBody] TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<IActionResult> Update(TKey id, [FromBody] TEntity entity, CancellationToken cancellationToken = default(CancellationToken));
        Task<IActionResult> Delete(TKey id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IActionResult> DeleteBatch([FromBody] EntityListViewModel<TEntity, TKey> entitiesParam, CancellationToken cancellationToken = default(CancellationToken));
        Task<IActionResult> DeleteBatchById([FromBody] EntityIdListViewModel<TEntity, TKey> entityIdsParam, CancellationToken cancellationToken = default(CancellationToken));
        Task<ActionResult<SearchResponseData<TEntity>>> SearchQuery(
            int? currentPage = 1, int? pageSize = 100, string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken));
        Task<ActionResult<SearchResponseData<TEntity>>> Search(SearchParams parameter, CancellationToken cancellationToken = default(CancellationToken));
        Task<ActionResult<SearchResponseData>> SearchDynamicQuery(
            int? currentPage = 1, int? pageSize = 100, string filter = "", string sort = "", string fields = "",
            CancellationToken cancellationToken = default(CancellationToken));
        Task<ActionResult<SearchResponseData>> SearchDynamic(SearchParams parameter, CancellationToken cancellationToken = default(CancellationToken));
    }
}