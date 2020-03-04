using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace directorchen.cqp.me.UI.Model
{
    public class SQLiteTable
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 列的集合
        /// </summary>
        public List<SQLiteColumn> SQLiteColumns { get; set; }
    }
}
