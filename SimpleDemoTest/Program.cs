using DbHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDemoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string conStr = "";

            //全局注册使用的数据库类型
            DbHelper.DbProviderGlobal.Register(Npgsql.NpgsqlFactory.Instance);
            //第一种,直接实例化
            DbHelper.DbManager manager = new DbManager(conStr);

            ////第二种,获取,但要指定连接字符串
            //DbHelper.DbManager manager = DbManager.Instance;
            ////给定连接字符串
            //manager.ConnectionString = conStr;
            bool re = manager.GetDataTable("SELECT * FROM PARTS LIMIT 10", out var dt);
            if (!re)
            {
                Console.WriteLine(manager.ErroMsg);
            }

            Console.ReadKey();
        }
    }
}
