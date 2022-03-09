using Npgsql;
using Microsoft.Extensions.Configuration;
using TourPlanner.Common;

namespace TourPlanner.DAL
{
    public sealed class DbContext
    {
        private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
        private static DbContext? _instance;
        
        public NpgsqlConnection Connection { get; }
        
        public NpgsqlTransaction Transaction { get; private set; }
        
        private DbContext()
        {
            string connectionString = Config["PostgresConnection"];
            Connection = new NpgsqlConnection(connectionString);
        }
        
        public static DbContext GetInstance()
        {
            return _instance ??= new DbContext();
        }

        public void Init()
        {
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }
        
        public void Commit()
        {
            Transaction.Commit();
            Transaction = Connection.BeginTransaction();
        }

        public void Rollback()
        {
            Transaction.Rollback();
            Transaction = Connection.BeginTransaction();
        }

        public void Dispose()
        {
            Transaction.Dispose();
            Connection.Dispose();
        }
    }
}