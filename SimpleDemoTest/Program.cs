using RzhDbHelper;
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
            // TestLearn();

            var db = new DbManager(@"Data Source=D:\Mobot Files\tasks\task.db;Version=3;Legacy Format=True;", System.Data.SQLite.SQLiteFactory.Instance);
           bool re= db.GetDataTable("SELECT * from ScriptExecuteDetailResult", out var dt);
        }

        private static void TestList()
        {
            //连接Access mdb
            string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\\Database1.mdb;Persist Security Info=False";
            DbManager db = new DbManager(connStr, System.Data.OleDb.OleDbFactory.Instance);
            List<CustomMoel> customMoels1 = db.QueryList<CustomMoel>("SELECT * FROM CUSTOM");//获取全字段

            List<CustomMoel> customMoels2 = db.QueryList<CustomMoel>("SELECT [ID],[NAME] FROM CUSTOM");//获取前两个字段

            List<CustomMoel> customMoels3 = db.QueryList<CustomMoel>("SELECT [ID],[PHONE] FROM CUSTOM");//获取间隔字段

            List<CustomMoel> customMoels4 = db.QueryList<CustomMoel>("SELECT [ID],[PHONE],[NAME],[GENGER] FROM CUSTOM");//非顺序性全字段
        }

        private static void TestLearn()
        {
            //连接字符串对象
            var conn = new RzhDbHelper.PostgreSqlConStrBuilder("127.0.0.1", "5432", "postgres", "postgres", "123456");
            //推荐使用DbManager,DbContext已不再维护
            DbManager db = new DbManager(conn, Npgsql.NpgsqlFactory.Instance);

            bool re = db.GetDataTable("SELECT * FROM PG_TABLES", out var dt);
            if (!re)
            {
                Console.WriteLine(db.ErroMsg);
            }


            //执行 増,删,改
            string query = "INSERT INTO TEST(NAME,GENDER) VALUES(@NAME,@GEN)";
            db.AddParameter("NAME", "AAA");
            db.AddParameter("GEN", "bbb");
            re = db.ExcuteNonQuery(query);//如果需要执行存储过程或函数,请使用此方法的可选参数,或使用ExcuteProcedure
            if (!re)
            {
                Console.WriteLine(db.ErroMsg);
            }

            //执行单个查询
            object obj;
            re = db.ExcuteSacler("SELECT COUNT(1) FROM PG_TABLES", out obj);
            if (obj == null)
            {
                Console.WriteLine(db.ErroMsg);
            }

            //执行事务,
            //增加,删除,修改,使用 TransctionExcuteNonQuery,事务内单查询使用 TransctionExcuteSacler
            //事务内执行函数/存储过程,使用 TransctionExcuteNonQuery的可选参数或 TransctionExcuteProcedure
            //事务内执行更新表 TransactionUpdateDataTable
            //事务内查询表 TransactionGetDataTable
            query = "INSERT INTO TEST(NAME,GENDER) VALUES('ABC','11A')";
            re = db.TransctionExcuteNonQuery(query);
            if (!re)
            {
                Console.WriteLine(db.ErroMsg);//内部自动关闭事务连接
            }

            query = "INSERT INTO TEST(NAME,GENDER) VALUES('BBA','DF')";
            re = db.TransctionExcuteNonQuery(query);
            if (!re)
            {
                Console.WriteLine(db.ErroMsg);//内部自动关闭事务连接
            }

            if (re)
            {
                db.TransctionCommit();//提交事务 
            }
            Console.ReadKey();
        }
    }
}
