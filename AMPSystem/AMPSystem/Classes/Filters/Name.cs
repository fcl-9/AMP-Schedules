using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Name: ISimpleFilter<string>
    {
        public string FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }

        public Name(string nameFilter)
        {
            FilterAttribute = nameFilter;
        }

        public bool ApplyFilter()
        {
            throw new NotImplementedException();
        }
    }
}