namespace WsFirst.Models.Database
{
    public class ProxyDatabaseSettings : IProxyDatabaseSettings
    {
        public string ServiceCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public interface IProxyDatabaseSettings
    {
        string ServiceCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
    }
}


