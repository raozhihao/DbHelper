using Oracle.DataAccess.Client;
using System.Data.Common;
using System.Security;
using System.Security.Permissions;

namespace DbHelper.Factory
{
    internal class OracleFactory : DbProviderFactory
    {
        public OracleFactory()
        {

        }
        public override DbConnection CreateConnection()
        {
            DbConnection connection = new OracleConnection();
            return connection;
        }

        public override DbCommand CreateCommand()
        {
            DbCommand command = new OracleCommand();
            return command;
        }

        public override DbCommandBuilder CreateCommandBuilder()
        {
            DbCommandBuilder commandBuilder = new OracleCommandBuilder();
            return commandBuilder;
        }

        public override DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return new DbConnectionStringBuilder();
        }

        public override DbDataAdapter CreateDataAdapter()
        {
            return new OracleDataAdapter();
        }

        public override DbDataSourceEnumerator CreateDataSourceEnumerator()
        {
            return base.CreateDataSourceEnumerator();
        }

        public override DbParameter CreateParameter()
        {
            return new OracleParameter();
        }

        public override CodeAccessPermission CreatePermission(PermissionState state)
        {

            return base.CreatePermission(state);
        }

    }
}
