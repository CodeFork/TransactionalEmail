using System.Configuration;
using Conditions.Guards;
using TransactionalEmail.Core.Interfaces;

namespace TransactionalEmail.Infrastructure
{
    public class DbSettings : IDbSettings
    {
        private readonly string _connectionName;
        public DbSettings(string connectionName)
        {
            Check.If(connectionName).IsNotNullOrEmpty();

            _connectionName = connectionName;
        }
        public string ConnectionString => ConfigurationManager.ConnectionStrings[_connectionName].ConnectionString;
    }
}
