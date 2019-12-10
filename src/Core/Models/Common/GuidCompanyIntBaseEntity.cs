using System;

namespace HordeFlow.Core.Models.Common
{
    public abstract class GuidCompanyIntBaseEntity : GuidTenantIntBaseEntity, ICompanyEntity<Guid?, Guid, int>, IHasCompany
    {
        public Guid? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}