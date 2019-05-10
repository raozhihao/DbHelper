using Npgsql;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace DbHelper.Factory
{
    internal class NpgsqlFactory: DbProviderFactory
    {
        public NpgsqlFactory()
        {
           
        }
        public override DbConnection CreateConnection()
        {
            DbConnection connection = new NpgsqlConnection();
            return connection;
        }

        public override DbCommand CreateCommand()
        {
            DbCommand command = new NpgsqlCommand();
            return command;
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            DbCommandBuilder commandBuilder = new NpgsqlCommandBuilder();
            return commandBuilder;
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new DbConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new NpgsqlDataAdapter();
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return base.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return new Npgsql.NpgsqlParameter();
        }

        public override CodeAccessPermission CreatePermission(PermissionState state)
        {
            
            return base.CreatePermission(state);
        }

    }
}
