using System;

namespace DbHelper
{
    /// <summary>
    /// 原始数据库中的字段名称,当自定义类的名称不与数据库相同时使用
    /// </summary>
    public class DataPropertyAttribute : Attribute
    {
        /// <summary>
        /// 数据库原始字段名称
        /// </summary>
        public string DataName { get; set; }
        public DataPropertyAttribute(string dataName)
        {
            this.DataName = dataName.ToLower();
        }
      
    }
}
