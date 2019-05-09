using DbHelper.Factory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace DbHelper
{
    public class DbContext
    {
        #region 私有字段

        ///// <summary>
        ///// 事务对象
        ///// </summary>
        private DbCommand transCommand;

        /// <summary>
        /// 存储参数
        /// </summary>
        private List<DbParameter> parameters = new List<DbParameter>();

        /// <summary>
        /// 数据源适配对象
        /// </summary>
        private DbProviderFactory DbProvider;

        #endregion

        #region 公共属性

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStr { get; set; }

        /// <summary>
        /// 返回最后一次获取到的错误信息
        /// </summary>
        public string ErroMsg { get; private set; }

        #endregion

        #region 构造器

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStr">连接字符串</param>
        /// <param name="selectName">在配置文件中所选择的connectionStrings节点下的name</param>
        public DbContext(string connectionStr, DataBaseType dbType= DataBaseType.None, string selectName = "")
        {
            this.ConnectionStr = connectionStr;
            switch (dbType)
            {
                case DataBaseType.None:
                    RegisterDb(selectName);
                    break;
                case DataBaseType.SqlServer:
                    DbProvider = new SqlServerFactory();
                    break;
                case DataBaseType.Oracle:
                    DbProvider = new OracleFactory();
                    break;
                case DataBaseType.PostgreSql:
                    DbProvider = new NpgsqlFactory();
                    break;
            }
           
            ParameterHelper.RegisterDbProvider(DbProvider);
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="conStrBuilder">连接字符串对象</param>
        /// <param name="dbType">对应数据库类型</param>
        /// <param name="selectName">配置文件对应数据库名称</param>
        public DbContext(DbConnectionStringBuilder conStrBuilder, DataBaseType dbType = DataBaseType.None, string selectName = "") 
            :this(conStrBuilder.ToString(),dbType,selectName) { }

       
        public DbContext() : this("")
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="selectName">在配置文件中所选择的connectionStrings节点下的name</param>
        public DbContext(string selectName) : this("", DataBaseType.None, selectName)
        {

        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 更改连接字符串
        /// </summary>
        /// <param name="stringBuilder"></param>
        public void ChangeConnectionStr(DbConnectionStringBuilder stringBuilder)
        {
            this.ConnectionStr = stringBuilder.ToString();
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameter">参数对象</param>
        public void AddParameter(DbParameter parameter)
        {
            if (parameter is ParameterHelper)
            {
                AddParameter((ParameterHelper)parameter);
            }
            else
            {
                this.parameters.Add(parameter);
            }
        }

        /// <summary>
        /// 提供最简单的参数化添加
        /// </summary>
        /// <param name="parameter"></param>
        public void AddParameter(ParameterHelper parameter)
        {
            AddParameter(parameter.GetDbParameter());
        }

        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="parameters">参数对象集合</param>
        public void AddParameters(IEnumerable<DbParameter> parameters)
        {
            this.parameters.AddRange(parameters);
        }

        /// <summary>
        /// 增加,删除,更新
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string query, CommandType commandType = CommandType.Text)
        {
            DbCommand cmd = null;
            try
            {
                cmd = GetDbCommand(query, commandType);

                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return false;
            }
            finally
            {
                DisposeCommand(cmd);
                parameters.Clear();
            }
        }

        /// <summary>
        /// 获取单个的值
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public object ExcuteSacler(string query, CommandType commandType = CommandType.Text)
        {
            DbCommand cmd = null;
            try
            {
                cmd = GetDbCommand(query, commandType);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return null;
            }
            finally
            {
                DisposeCommand(cmd);
                parameters.Clear();
            }
        }

        /// <summary>
        /// 调用有无返回值的存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        public void ExcuteProcedure(string proName)
        {
             this.ExcuteSacler(proName, CommandType.StoredProcedure);
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="proName">存储过程或函数名</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns>返回调用之后的返回值</returns>
        public object ExcuteProcedure(string proName, CommandType commandType)
        {
            return this.ExcuteSacler(proName, commandType);
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="tableName">设置查询出来的表名称</param>
        /// <returns></returns>
        public DataTable GetDataTable(string query, string tableName = "")
        {
            DbDataAdapter dataAdapter = null;
            DbCommand command = null;
            try
            {
                dataAdapter = DbProvider.CreateDataAdapter();
                command = GetDbCommand(query);
                DataTable dt = new DataTable(tableName);

                dataAdapter.SelectCommand = command;

                dataAdapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return new DataTable();
            }
            finally
            {
                DisposeCommand(command);
                dataAdapter?.Dispose();
            }
        }

        /// <summary>
        /// 更新表格
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="dt">需要更新的表</param>
        /// <returns></returns>
        public bool UpdateTable(string query, DataTable dt)
        {
            return UpdateTable(query, dt, false, ConflictOption.CompareAllSearchableValues);
        }

        /// <summary>
        /// 更新表格
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="dt">需要更新的表</param>
        /// <param name="SetAllValues">指定 update 语句中是包含所有列值还是仅包含更改的列值。</param>
        /// <param name="conflictOption">指定将如何检测和解决对数据源的相互冲突的更改。</param>
        /// <returns>对表进行更新操作</returns>
        public bool UpdateTable(string query, DataTable dt, bool SetAllValues, ConflictOption conflictOption)
        {
            DbDataAdapter dataAdapter = null;
            DbCommand command = null;
            try
            {
                dataAdapter = DbProvider.CreateDataAdapter();
                command = GetDbCommand(query);
                dataAdapter.SelectCommand = command;
                DbCommandBuilder commandBuilder = DbProvider.CreateCommandBuilder();
                commandBuilder.DataAdapter = dataAdapter;
                commandBuilder.SetAllValues = SetAllValues;
                commandBuilder.ConflictOption = conflictOption;
                dataAdapter.Update(dt);
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return false;
            }
            finally
            {
                DisposeCommand(command);
                dataAdapter?.Dispose();
            }
        }

        /// <summary>
        /// 获取指定对象集合
        /// </summary>
        /// <typeparam name="T">需要获取的对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public List<T> GetList<T>(string sql)
        {
            List<T> values = new List<T>();
            try
            {
                T t = default(T);
                DataTable dt = GetDataTable(sql);
                int count = dt.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    t = GetObjectInfo<T>(dt, i);
                    values.Add(t);
                }
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
            }

            return values;
        }

        /// <summary>
        /// 获取对象集合
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        public List<object[]> GetObjects(string sql)
        {
            DbCommand command = null;
            DbDataReader reader = null;
            List<object[]> values = new List<object[]>();
            try
            {
                command = GetDbCommand(sql);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object[] objs = new object[reader.FieldCount];
                        reader.GetValues(objs);
                        values.Add(objs);
                    }
                }
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
            }
            finally
            {
                reader.Close();
                DisposeCommand(command);
            }
            return values;
        }

        /// <summary>
        /// 增加一个事务处理
        /// </summary>
        /// <param name="sql">sql处理语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public bool TransactionAdd(string sql, CommandType commandType = CommandType.Text)
        {
            CreateTransCommand(sql);
            transCommand.CommandType = commandType;
            try
            {
                transCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception exc)
            {
                ErroMsg = exc.Message;
                TransctionRollBack();
                return false;
            }
            finally
            {
                parameters.Clear();
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void TransctionCommit()
        {
            if (null != transCommand)
            {
                transCommand.Transaction.Commit();
                transCommand.Connection.Close();
                transCommand.Dispose();
                transCommand = null;
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void TransctionRollBack()
        {
            if (null != transCommand)
            {
                transCommand.Transaction.Rollback();
                transCommand.Connection.Close();
                transCommand = null;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 创建一个事务适配器
        /// </summary>
        private void CreateTransCommand(string sql)
        {
            if (null == transCommand)
            {
                try
                {
                    transCommand = GetDbCommand(sql);
                }
                catch (Exception exc)
                {
                    ErroMsg = exc.Message;
                    transCommand.Dispose();
                    transCommand = null;
                }
                transCommand.Transaction = transCommand.Connection.BeginTransaction();

            }
        }

        /// <summary>
        /// 注册数据库对象
        /// </summary>
        /// <param name="selectName"></param>
        private void RegisterDb(string selectName)
        {
            var provider = ConfigurationManager.ConnectionStrings[selectName];
            try
            {
                if (string.IsNullOrWhiteSpace(this.ConnectionStr))
                {
                    ConnectionStr = provider.ConnectionString;
                }
                string _providerName = provider.ProviderName;
                DbProvider = DbProviderFactories.GetFactory(_providerName);
            }
            catch
            {
                //如果配置文件失败,且又没有连接字符串,则在此处使用默认的PostgreSql
                if (string.IsNullOrWhiteSpace(this.ConnectionStr))
                    ConnectionStr = "Server=192.168.18.136;Port=5866;Database=tymap;User Id=tymap;Password=123456;";
                DbProvider = new NpgsqlFactory();
            }
        }


        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T">需要获取的对象</typeparam>
        /// <param name="dt">对象的表</param>
        /// <param name="index">当前需要获取的表中的行索引</param>
        /// <returns></returns>
        private T GetObjectInfo<T>(DataTable dt, int index)
        {
            T t = default(T);
            T tobj = Activator.CreateInstance<T>();//创建一个对象
            PropertyInfo[] pr = tobj.GetType().GetProperties();//获取对象的所有公共属性
            int columnCount = dt.Columns.Count;
            //循环获取
            foreach (PropertyInfo item in pr)
            {
                for (int i = 0; i < columnCount; i++)
                {
                    //判断当前的属性是否实现了对应的特性
                    var attr = item.GetCustomAttribute<DataPropertyAttribute>();
                    string currentName = item.Name.ToLower();
                    if (attr != null)
                    {
                        currentName = attr.DataName;
                    }
                    //如果当前循环到的对象属性名称与当前数据表中的列名一致
                    if (currentName.Equals(dt.Columns[i].ColumnName.ToLower()))
                    {
                        //将当前的行列值设置到对象中
                        var value = dt.Rows[index][i];
                        if (value != DBNull.Value)
                            item.SetValue(tobj, value, null);
                        else
                            item.SetValue(tobj, null, null);
                    }
                }
            }
            t = tobj;
            return t;
        }

        /// <summary>
        /// 获取DbCommand对象
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        private DbCommand GetDbCommand(string query, CommandType commandType = CommandType.Text)
        {
            DbConnection connection = GetDbConnection();
            DbCommand command;
            try
            {
                connection.Open();
                command = connection.CreateCommand();
                command.CommandText = query;
                command.CommandType = commandType;
                if (parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
            }
            catch
            {
                connection.Close();
                connection.Dispose();
                throw;
            }
            return command;
        }

        /// <summary>
        /// 获取DbConnection
        /// </summary>
        /// <returns></returns>
        private DbConnection GetDbConnection()
        {
            var connection = DbProvider.CreateConnection();
            
            connection.ConnectionString = ConnectionStr;
            return connection;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="command"></param>
        private void DisposeCommand(DbCommand command)
        {
            command?.Connection?.Close();
            command?.Connection?.Dispose();
            command?.Dispose();
        }
        #endregion


    }
}
