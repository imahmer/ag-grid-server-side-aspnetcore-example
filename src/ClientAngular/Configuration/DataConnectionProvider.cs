using System.Data;
using System.Data.SqlClient;

namespace ClientAngular.Configuration
{
    public class DataConnectionProvider : IDataConnection
    {
        private SqlConnection Connection { get; set; }

        public SqlConnection Connect(string dbName, string connectionString)
        {
            if (Connection?.State == ConnectionState.Open) return Connection;
            var connString = Transform(dbName, connectionString);
            if (string.IsNullOrEmpty(connString)) return Connection;
            Connection = new SqlConnection(connString);
            Connection.Open();
            return Connection;
        }

        public void Disconnect()
        {
            if (Connection == null) return;
            if (Connection.State == ConnectionState.Open)
            {
                Connection?.Close();
            }
            Connection.Dispose();
        }

        private string Transform(string dbName, string connectionString)
        {
            return connectionString?.Replace("DB_Placeholder", dbName);
        }
    }
}
