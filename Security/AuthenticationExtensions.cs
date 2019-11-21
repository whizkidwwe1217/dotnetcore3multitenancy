using Lamar;

namespace HordeFlow.Security
{
    public static class AuthenticationExtensions
    {
        public static ServiceRegistry AddPerTenantAuthentication(this ServiceRegistry services)
        {
            return services;
        }
    }
}