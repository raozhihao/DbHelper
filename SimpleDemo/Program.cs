using DbHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SimpleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            ///==== 实例化DbContext ====
            DbContext dbContext = new DbContext("Server=192.168.18.136;Port=5866;Database=tymap;User Id=tymap;Password=123456;", DataBaseType.PostgreSql);
            string query;
            bool re = false;
            object obj;


            /////======   创建表  ==========
            //string createTable = "CREATE TABLE TESTDEMO(TID SERIAL PRIMARY KEY,TNAME VARCHAR,TINT INT)";
            //re = dbContext.ExecuteNonQuery(createTable);
            //if (re)
            //    Console.WriteLine("创建表成功");
            //else
            //    Console.WriteLine("创建表失败:" + dbContext.ErroMsg);


            /////======= 向表中插入数据 ======
            //query = "INSERT INTO TESTDEMO(TNAME,TINT) VALUES(@TNAME,@INT)";
            //dbContext.AddParameter(new ParameterHelper("TNAME", "demo1"));
            //dbContext.AddParameter(new ParameterHelper("INT", 111));
            //re = dbContext.ExecuteNonQuery(query);
            //if (re)
            //    Console.WriteLine("插入成功");
            //else
            //    Console.WriteLine("插入失败:" + dbContext.ErroMsg);

            /////======= 向表中获取数据 ======
            //query = "SELECT COUNT(1) FROM TESTDEMO WHERE TINT=@INT";
            //dbContext.AddParameter(new ParameterHelper("INT", 111));
            //obj = dbContext.ExcuteSacler(query);
            //Console.WriteLine("查询成功,返回:" + obj.ToString());

            //////====== 获取表 =======
            //query = "SELECT * FROM TESTDEMO";
            //var table = dbContext.GetDataTable(query, "TestDemo");
            //PrintTable(table);

            /////===== 修改表 =======
            //table.Rows[0][1] = "hehe";
            //re = dbContext.UpdateTable(query, table);
            //if (re)
            //{
            //    Console.WriteLine("修改表成功");
            //    PrintTable(table);
            //}
            //else
            //{
            //    Console.WriteLine("修改表失败");
            //}

            /////===== 获取强类型集合 =======
            //query = "SELECT * FROM TESTDEMO";
            //var list = dbContext.GetList<Test>(query);
            //PrintList(list);

            

            Console.ReadKey();
        }

        private static void PrintList(List<Test> list)
        {
            foreach (Test t in list)
            {
                Console.WriteLine($"{t.Tid}\t{t.Tname}\t{t.Tint}");
            }
        }

        private static void PrintTable(DataTable table)
        {
            int i = 1;
            foreach (DataRow row in table.Rows)
            {
                string str = "行" + i+"\t";
                foreach (DataColumn col in table.Columns)
                {
                    str += row[col].ToString() + "\t";
                }
                str += Environment.NewLine;
                i++;
                Console.WriteLine(str);
            }
        }
    }

    public class Test
    {
        [DataProperty("TID")]
        public int Tid { get; set; }
        [DataProperty("TNAME")]
        public string Tname { get; set; }
        [DataProperty("TINT")]
        public int Tint { get; set; }
    }
}
