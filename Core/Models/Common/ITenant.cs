using HordeFlow.Data;

namespace HordeFlow.Data
{
    public interface ITenant
    {
        string Name { get; set; }
        string HostName { get; set; }
        string ConnectionString { get; set; }
        DatabaseProvider DatabaseProvider { get; set; }
        bool IsDedicated { get; set; }
    }
}