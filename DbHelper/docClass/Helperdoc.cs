using System;

namespace DbHelper.docClass
{
    /// <summary>
    /// 帮助文档
    /// </summary>
    public class Helperdoc
    {
        /// <summary>
        /// 开始
        /// </summary>
        /// <returns>返回文档描述字符串</returns>
        public static string GetStart()
        {
            return "开始之前,要知道此项目只是一个简单的DbHelper,有三种可以实例化的方式," + Environment.NewLine
                + "1.使用连接字符串+DataBaseType的方式,DataBaseType内置了三种,SqlServer,Oracle,Postgresql" + Environment.NewLine
                + "   三种内置驱动的程序集相关信息为:" + Environment.NewLine
                + "   Oracle:程序集 Oracle.DataAccess, Version=4.112.1.2, Culture=neutral, PublicKeyToken=89b483f429c47342" + Environment.NewLine
                + "   PostgreSql: 程序集 Npgsql, Version=4.0.5.0, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7" + Environment.NewLine
                + "    SqlServer以本机GAC为准" + Environment.NewLine
                + "2.使用配置文件来存储,配置文件的例子如下:" + Environment.NewLine
                + "<? xml version = \"1.0\" encoding = \"utf-8\" ?>" + Environment.NewLine
                + "<configuration>" + Environment.NewLine
                + "    <startup>" + Environment.NewLine
                + "        <supportedRuntime version=\"v4.0\" sku=\".NETFramework,Version = v4.5\" />" + Environment.NewLine
                + "    </startup>" + Environment.NewLine
                + "    <connectionStrings>" + Environment.NewLine
                + "        <!--" + Environment.NewLine
                + "        在此项内存储驱动的自定义名称及连接字符串,其中name为自定义,也是稍候需要传入构造函数中的SelectName," + Environment.NewLine
                + "        而providerName与下面的DbProviderFactories节点中的invariant属性保持一致" + Environment.NewLine
                + "        -->" + Environment.NewLine
                + "        <add name=\"NpgsqlProvide\" providerName=\"Npgsql\" connectionString=\"Server = 192.168.18.136; Port = 5866; Database = tymap; User Id = tymap; Password = 123456; \"/>" + Environment.NewLine
                + "        <add name=\"SqlClientProvide\" providerName=\"System.Data.SqlClient\" connectionString=\"Data Source =.; Initial Catalog = GISTwo; Integrated Security = True\"/>" + Environment.NewLine
                + "        <add name=\"DataAccessProvide\" providerName=\"Oracle.DataAccess.Client\" connectionString=\"Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.18.68)(PORT = 1521)))(CONNECT_DATA = (sid = orcl))); User Id = TESTPG; Password = TESTPG\"/>" + Environment.NewLine
                + "    </connectionStrings>" + Environment.NewLine
                + "    <system.data>" + Environment.NewLine
                + "        <DbProviderFactories>" + Environment.NewLine
                + "            <remove invariant=\"Oracle.DataAccess.Client\"/>" + Environment.NewLine
                + "            <clear/>" + Environment.NewLine
                + "            <!--" + Environment.NewLine
                + "            有关如何配置DbProviderFactories,请自行百度网络" + Environment.NewLine
                + "            在此示例中,name一般比较固定,invariant为程序集名称" + Environment.NewLine
                + "            description为描述信息" + Environment.NewLine
                + "            type为 程序集名称.对应的Factory,程序集名称,版本信息等(此项可以在程序集内找到)" + Environment.NewLine
                + "            -->" + Environment.NewLine
                + "            <add name=\"Oracle Data Provider for .NET\" invariant=\"Oracle.DataAccess.Client\" description=\"Oracle Data Provider for .NET\" type=\"Oracle.DataAccess.Client.OracleClientFactory, Oracle.DataAccess, Version = 4.112.1.2, Culture = neutral, PublicKeyToken = 89b483f429c47342\"/>" + Environment.NewLine
                + "            <add name=\"SqlClient Data Provider\" invariant=\"System.Data.SqlClient\" description=\".Net Framework Data Provider for SqlServer\" type=\"System.Data.SqlClient.SqlClientFactory, System.Data, Version = 2.0.0.0, Culture = neutral, PublicKeyToken = b77a5c561934e089\"/>" + Environment.NewLine
                + "            <add name=\"Npgsql Data Provider\" invariant=\"Npgsql\" description=\".Net Data Provider for PostgreSQL\" type=\"Npgsql.NpgsqlFactory, Npgsql, Version = 4.0.5.0, Culture = neutral, PublicKeyToken = 5d8b90d52f46fda7\"/>" + Environment.NewLine
                + "        </DbProviderFactories>" + Environment.NewLine
                + "    </system.data>" + Environment.NewLine
                + "</configuration>" + Environment.NewLine
                + Environment.NewLine;
              
        }
    }
}
