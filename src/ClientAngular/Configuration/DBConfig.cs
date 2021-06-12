using ClientAngular.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ClientAngular.Configuration
{
    public class DBConfig : IDBConfig
    {
        private readonly IConfiguration _config;
        public DBConfig(IConfiguration config)
        {
            _config = config;
        }
        public string ConnectionString => GetConfig("ConnectionString")?.Value;

        public string GetDBName()
        {
            return "TestingDB";
        }
        public IConfigurationSection GetConfig(string key) => _config.GetSection(key);
    }
}
