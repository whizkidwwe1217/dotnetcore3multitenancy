using System;

namespace HordeFlow.Core.Models.Common
{
    public abstract class GuidCompanyBaseEntity<TKey> : GuidTenantBaseEntity<TKey>, ICompanyEntity<Guid?, Guid, TKey>, IHasCompany
    {
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}