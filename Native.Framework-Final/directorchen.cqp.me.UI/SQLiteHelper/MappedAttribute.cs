using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace directorchen.cqp.me.UI.SQLiteHelper
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MappedAttribute : Attribute
    {
        public bool IsMapped { get; set; }
    }
}
