using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Type : ISimpleFilter<object>
    {
        public object FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }

        public Type(object filterName)
        {
            this.FilterAttribute = filterName;
        }

        public bool ApplyFilter()
        {
            throw new NotImplementedException();
        }
    }
}