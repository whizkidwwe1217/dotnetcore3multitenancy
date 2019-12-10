using System;

namespace HordeFlow.Core.Models.Common
{
    public abstract class GuidCompanyGuidBaseEntity : GuidTenantGuidBaseEntity, ICompanyEntity<Guid?, Guid, Guid>, IHasCompany
    {
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}