using System.Data;
using System.Data.Common;

namespace DbHelper
{
    /// <summary>
    /// 参数化帮助类,提供最简单的参数化帮助
    /// </summary>
    public class ParameterHelper:DbParameter
    {
        private DbParameter dbParameter;
        private static DbProviderFactory _dbProvider;
        static internal void RegisterDbProvider(DbProviderFactory dbProvider)
        {
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// 获取确切类型的DbParameter
        /// </summary>
        /// <returns></returns>
        internal DbParameter GetDbParameter()
        {
            return dbParameter;
        }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="name">要映射的参数的名称。</param>
        /// <param name="value">设置参数的值。</param>
        public ParameterHelper(string name,object value)
        {
            dbParameter = _dbProvider.CreateParameter();
            this.ParameterName = name;
            this.Value = value;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public ParameterHelper()
        {
            dbParameter = _dbProvider.CreateParameter();
        }

        /// <summary>
        /// 获取或设置参数的 System.Data.SqlDbType。
        /// </summary>
        public override  DbType DbType { get => dbParameter.DbType; set => dbParameter.DbType = value; }

        /// <summary>
        /// 获取或设置一个值，该值指示参数是只可输入的参数、只可输出的参数、双向参数还是存储过程返回值参数。
        /// </summary>
        public override ParameterDirection Direction { get =>dbParameter.Direction; set => dbParameter.Direction=value; }

        /// <summary>
        /// 要映射的参数的名称。
        /// </summary>
        public override string ParameterName { get => dbParameter.ParameterName; set => dbParameter.ParameterName = value; }

        /// <summary>
        /// 获取或设置列中的数据的最大大小（以字节为单位）
        /// </summary>
        public override int Size { get =>dbParameter.Size; set => dbParameter.Size=value; }

        /// <summary>
        /// 获取或设置映射到 System.Data.DataSet 并且用于加载或返回 System.Data.SqlClient.SqlParameter.Value的源列的名称
        /// </summary>
        public override string SourceColumn { get => dbParameter.SourceColumn; set => dbParameter.SourceColumn=value; }

        /// <summary>
        /// 设置或获取一个值，该值指示源列是否可以为 null。 通过此操作，System.Data.SqlClient.SqlCommandBuilder 能够为可以为
        ///     null 的列正确地生成 Update 语句。
        /// </summary>
        public override bool SourceColumnNullMapping { get => dbParameter.SourceColumnNullMapping; set => dbParameter.SourceColumnNullMapping=value; }

        /// <summary>
        /// 获取或设置要在加载 System.Data.DataRowVersion 时使用的 System.Data.SqlClient.SqlParameter.Value。
        /// </summary>
        public override DataRowVersion SourceVersion { get => dbParameter.SourceVersion; set => dbParameter.SourceVersion=value; }

        /// <summary>
        /// 获取或设置参数的值。
        /// </summary>
        public override object Value { get => dbParameter.Value; set => dbParameter.Value = value; }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsNullable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override void ResetDbType()
        {
            dbParameter.ResetDbType();
        }
    }

}
