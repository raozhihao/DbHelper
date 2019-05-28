
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace DbHelper
{
    /// <summary>
    /// 提供对数据库访问的上下文类
    /// </summary>
    public class DbContext : IDisposable
    {
        #region 私有字段


        /// <summary>
        /// 存储参数
        /// </summary>
        private List<DbParameter> parameters;

        /// <summary>
        /// 数据源适配对象
        /// </summary>
        private DbProviderFactory DbProvider;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection connection;

        /// <summary>
        /// 数据库命令对象
        /// </summary>
        private DbCommand command;

        private DbDataAdapter adapter;

        /// <summary>
        /// 事务命令对象
        /// </summary>
        private DbCommand transCommand;

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
        /// <param name="dbType">数据库类型,默认使用app.config中的设置</param>
        /// <param name="selectName">在配置文件中所选择的connectionStrings节点下的name</param>
        public DbContext(string connectionStr, DataBaseType dbType = DataBaseType.None, string selectName = "")
        {
            this.ConnectionStr = connectionStr;
            switch (dbType)
            {
                case DataBaseType.None:
                    RegisterDb(selectName);
                    break;
                case DataBaseType.SqlServer:
                    DbProvider = System.Data.SqlClient.SqlClientFactory.Instance;
                    break;
                case DataBaseType.Oracle:
                    DbProvider = Oracle.DataAccess.Client.OracleClientFactory.Instance;
                    break;
                case DataBaseType.PostgreSql:
                    DbProvider = Npgsql.NpgsqlFactory.Instance;
                    break;
            }

            connection = DbProvider.CreateConnection();
            connection.ConnectionString = ConnectionStr;

            parameters = new List<DbParameter>();
            ParameterHelper.RegisterDbProvider(DbProvider);
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="conStrBuilder">连接字符串对象</param>
        /// <param name="dbType">对应数据库类型</param>
        /// <param name="selectName">配置文件对应数据库名称</param>
        public DbContext(DbConnectionStringBuilder conStrBuilder, DataBaseType dbType = DataBaseType.None, string selectName = "")
            : this(conStrBuilder.ToString(), dbType, selectName) { }

        /// <summary>
        /// 构造器
        /// </summary>
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
            ChangeConnectionStr(stringBuilder.ToString());
        }

        /// <summary>
        /// 更改连接字符串
        /// </summary>
        /// <param name="newConnectionStr"></param>
        public void ChangeConnectionStr(string newConnectionStr)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            connection.ConnectionString = newConnectionStr;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameter">参数对象</param>
        public void AddParameter(DbParameter parameter)
        {
            DbParameter par = parameter;
            if (parameter is ParameterHelper)
            {
                //获取其确切类型
                par = ((ParameterHelper)parameter).GetDbParameter();
            }
            this.parameters.Add(par);
        }

        /// <summary>
        /// 提供最简单的参数化添加
        /// </summary>
        /// <param name="parameter"></param>
        public void AddParameter(ParameterHelper parameter)
        {
            //这里一定是要加入其父类,这里的parameter传进来的确切类型为ParameterHelper
            //但DbContext需要的却是其确切的类型,例如SqlParameter
            //所以这里使用ParameterHelper对象的GetDbParameter()方法获取其映射的确切类型
            this.parameters.Add(parameter.GetDbParameter());
        }

        /// <summary>
        /// 添加参数集合
        /// </summary>
        /// <param name="parameters">参数对象集合</param>
        public void AddParameters(params DbParameter[] parameters)
        {
            foreach (var item in parameters)
            {
                AddParameter(item);
            }
        }

        /// <summary>
        /// 增加,删除,更新
        /// </summary>
        /// <param name="query">sql语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string query, CommandType commandType = CommandType.Text)
        {
            try
            {
                SetCommand(query, commandType);
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {

                ErroMsg = ex.Message;
                return false;
            }
            finally
            {
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
            try
            {
                SetCommand(query, commandType);
                return command.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return null;
            }
            finally
            {
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
        /// 获取DataTable
        /// </summary>
        /// <param name="query">sql查询语句</param>
        /// <param name="dt">返回的数据集</param>
        /// <param name="tableName">为数据集命名</param>
        /// <returns></returns>
        public bool GetDataTable(string query, out DataTable dt, string tableName = "")
        {
            dt = new DataTable(tableName);
            try
            {
                SetCommand(query, CommandType.Text);
                if (adapter == null)
                {
                    adapter = DbProvider.CreateDataAdapter();
                }
                adapter.SelectCommand = command;

                adapter.Fill(dt);
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return false;
            }
            finally
            {
                parameters.Clear();
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
        /// <param name="setAllValues">指定 update 语句中是包含所有列值还是仅包含更改的列值。</param>
        /// <param name="conflictOption">指定将如何检测和解决对数据源的相互冲突的更改。</param>
        /// <returns>对表进行更新操作</returns>
        public bool UpdateTable(string query, DataTable dt, bool setAllValues, ConflictOption conflictOption)
        {
            try
            {
                command.CommandText = query;
                adapter.SelectCommand = command;
                DbCommandBuilder builder = DbProvider.CreateCommandBuilder();
                builder.DataAdapter = adapter;
                builder.SetAllValues = setAllValues;
                builder.ConflictOption = conflictOption;
                adapter.Update(dt);
                return true;
            }
            catch (Exception ex)
            {
                ErroMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取指定对象集合
        /// </summary>
        /// <typeparam name="T">需要获取的对象</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="values">返回的强类型集合</param>
        /// <returns></returns>
        public bool GetList<T>(string sql,out List<T> values)
        {
            values = new List<T>();
            DataTable dt;
            bool re = GetDataTable(sql, out dt,typeof(T).Name);
            if (re)
            {
                try
                {
                     values = ToList<T>(dt);
                }
                catch (Exception ex)
                {
                    ErroMsg = ex.Message;
                    re = false;
                }
            }

            return re;  
        }

        /// <summary>
        /// 根据DataTable创建一个强类型对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns>如果失败,将返回null</returns>
        public  List<T> ToList<T>(DataTable dt)
        {
           List<T> values = new List<T>();
         
            try
            {
                T t = default(T);
               
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
                values = null;
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
            DbDataReader reader = null;
            List<object[]> values = new List<object[]>();
            try
            {
                SetCommand(sql, CommandType.Text);
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
            }
            return values;
        }

        /// <summary>
        /// 将集合对象转为DataTable
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="collection">对象集合</param>
        /// <returns></returns>
        public DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        /// <summary>
        /// 增加一个事务处理
        /// </summary>
        /// <param name="sql">sql处理语句</param>
        /// <param name="commandType">指定如何解释命令字符串</param>
        /// <returns></returns>
        public bool TransactionAdd(string sql, CommandType commandType = CommandType.Text)
        {
            SetTransCommand(sql);
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
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 设置Command对象的一些值
        /// </summary>
        /// <param name="query"></param>
        /// <param name="text"></param>
        private void SetCommand(string query, CommandType text)
        {
            if (command == null)
            {
                command = DbProvider.CreateCommand();
                command.Connection = connection;
            }
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            command.CommandType = text;
            command.CommandText = query;
            if (parameters.Count > 0)
            {
                command.Parameters.Clear();
                command.Parameters.AddRange(parameters.ToArray());
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
                DbProvider = Npgsql.NpgsqlFactory.Instance; //new NpgsqlFactory();
            }
        }

        /// <summary>
        /// 设置事务属性
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="commandType"></param>
        private void SetTransCommand(string sql, CommandType commandType = CommandType.Text)
        {
            if (transCommand == null)
            {
                transCommand = DbProvider.CreateCommand();
                transCommand.Connection = connection;
            }
            transCommand.CommandText = sql;
            transCommand.CommandType = commandType;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            if (parameters.Count > 0)
            {
                transCommand.Parameters.Clear();
                transCommand.Parameters.AddRange(parameters.ToArray());
            }
            if (transCommand.Transaction == null)
            {
                transCommand.Transaction = transCommand.Connection.BeginTransaction();
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


        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    parameters.Clear();

                    ErroMsg = null;
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。
                transCommand?.Dispose();
                command?.Dispose();
                adapter?.Dispose();
                connection?.Close();
                connection?.Dispose();
                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DbContext() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        /// <summary>
        /// 释放对应资源
        /// </summary>
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
        #endregion


    }
}
