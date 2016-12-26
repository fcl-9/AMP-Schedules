using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class TypeF : ISimpleFilter<ITimeTableItem>
    {
        public ITimeTableItem FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }

        public TypeF(string filterName, TimeTableManager manager)
        {
            //TODO: Create an ITimeTableItem Object woth the string received
            Manager = manager;
            //this.FilterAttribute = filterName;
        }

        public void ApplyFilter()
        {
            foreach (var item in Manager.TimeTable.ItemList)
            {
                if (item is ITimeTableItem && FilterAttribute is ITimeTableItem)
                {
                    Manager.TimeTable.ItemList.Remove(item);
                }
            }
        }
    }
}