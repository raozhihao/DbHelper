using System.Data.Common;
using System.Data.SqlClient;
using System.Security;
using System.Security.Permissions;

namespace DbHelper.Factory
{

    internal class SqlServerFactory : DbProviderFactory
    {
        public SqlServerFactory()
        {

        }
        public override DbConnection CreateConnection()
        {
            DbConnection connection = new SqlConnection();
            return connection;
        }

        public override DbCommand CreateCommand()
        {
            DbCommand command = new SqlCommand();
            return command;
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            DbCommandBuilder commandBuilder = new SqlCommandBuilder();
            return commandBuilder;
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new DbConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new SqlDataAdapter();
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return base.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return new SqlParameter();
        }

        public override CodeAccessPermission CreatePermission(PermissionState state)
        {

            return base.CreatePermission(state);
        }

    }
}
