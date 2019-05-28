﻿namespace DbHelper.ConnectionStrBuilder
{
    /// <summary>
    /// PostgreSq的数据库连接字符串对象
    /// </summary>
    public class PostgreSqlConStrBuilder: BaseConStrBuilder
    {
        /// <summary>
        /// 连接的数据库名称
        /// </summary>
        public new string DataBase { get; set; }
        /// <summary>
        /// 数据库端口号
        /// </summary>
        public new string Port { get; set; }

        /// <summary>
        /// 构建数据库连接字符串对象
        /// </summary>
        /// <param name="host">数据库IP地址</param>
        /// <param name="port">数据库端口号</param>
        /// <param name="userId">数据库用户名</param>
        /// <param name="password">数据库密码</param>
        /// <param name="dataBase">连接的数据库名称</param>
        public PostgreSqlConStrBuilder(string host,string port, string dataBase, string userId , string password)
        {
            base.Host = host;
            this.DataBase = dataBase;
            base.Uid = userId;
            base.Pwd = password;
            this.Port = port;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public PostgreSqlConStrBuilder() { }
        /// <summary>
        /// 返回对象的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Server={base.Host};Port={this.Port};Database={this.DataBase};User Id={base.Uid};Password={base.Pwd};";
        }
    }
}
