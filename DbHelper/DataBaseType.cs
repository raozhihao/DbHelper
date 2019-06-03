namespace DbHelper
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DataBaseType
    {
        /// <summary>
        /// 不使用任何内置配置,直接使用配置文件项
        /// </summary>
        Config,
        
        /// <summary>
        /// 使用DbProviderFactory进行注册
        /// </summary>
        Factory,

        /// <summary>
        /// 使用内置的SqlServer
        /// </summary>
        SqlServer,

        /// <summary>
        /// 使用内置的Oracle
        /// 程序集 Oracle.DataAccess, Version=4.112.1.2, Culture=neutral, PublicKeyToken=89b483f429c47342
        /// </summary>
        Oracle,

        /// <summary>
        /// 使用内置的PostgreSql
        /// 程序集 Npgsql, Version=4.0.5.0, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7
        /// </summary>
        PostgreSql
    }
}
