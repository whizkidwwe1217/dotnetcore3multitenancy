namespace HordeFlow.Data
{
    public interface ITenant
    {
        string Name { get; set; }
        string HostName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseProvider { get; set; }
    }
}