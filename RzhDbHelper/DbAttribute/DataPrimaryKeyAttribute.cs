using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RzhDbHelper.DbAttribute
{
    /// <summary>
    /// 映射表字段主键
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataPrimaryKeyAttribute:Attribute
    {
        ///// <summary>
        ///// 表字段主键
        ///// </summary>
        //public string PrimaryKeyName { get; set; }

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="primeryKey">表字段主键名称</param>
        //public DataPrimaryKeyAttribute(string primeryKey)
        //{
        //    this.PrimaryKeyName = primeryKey.ToLower();
        //}
    }
}
