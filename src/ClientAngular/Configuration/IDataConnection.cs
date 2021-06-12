using System.Data.SqlClient;

namespace ClientAngular.Configuration
{
    public interface IDataConnection
    {
        SqlConnection Connect(string dbName, string connectionString);
        void Disconnect();
    }
}
