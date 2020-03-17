using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cqp.tian.bgtz.me.Code.Enum
{
    public class Book
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string Code { get; set; }

        public List<Group> Group { get; set; }
    }
}
