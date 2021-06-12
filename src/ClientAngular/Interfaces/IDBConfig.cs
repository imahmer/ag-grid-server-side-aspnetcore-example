namespace ClientAngular.Interfaces
{
    public interface IDBConfig
    {
        string ConnectionString { get; }
        string GetDBName();
    }
}
