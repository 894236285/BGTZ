using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace directorchen.cqp.me.UI.Model
{
    public class SQLiteColumn
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool PrimaryKey { get; set; }

        /// <summary>
        /// 是否自增列
        /// </summary>
        public bool AutoIncrement { get; set; }

        /// <summary>
        /// 是否不能为空
        /// </summary>
        public bool NotNull { get; set; }
    }
}
