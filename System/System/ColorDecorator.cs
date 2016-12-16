using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    class ColorDecorator:ISubject
    {
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Day { get; set; }
        public ITimeTableItem item { get; set; }
    }
}
