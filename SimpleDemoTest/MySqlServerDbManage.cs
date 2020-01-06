using DbHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SimpleDemoTest
{
    internal class MySqlServerDbManage : DbManager
    {
        public MySqlServerDbManage(string connectionStr) : base(connectionStr, SqlClientFactory.Instance)
        {

        }

        public override void AddParameter(string name, object value)
        {
            if(value is byte[])
            {
                SqlParameter parameter = new SqlParameter(name, value);
                parameter.SqlDbType = System.Data.SqlDbType.Binary;
                base.parameters.Add(parameter);
            }
        }
    }
}
