using System;
using System.Collections.Generic;
using System.Diagnostics;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class TypeF : ISimpleFilter<Object>
    {
        public Object FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }
        public TypeCreator GeneratesTypes { get; set; }
        public TypeF(string filterName, TimeTableManager manager)
        {
            GeneratesTypes = new TypeCreator();
            Manager = manager;
            this.FilterAttribute = GeneratesTypes.CreateTypeOf(filterName);
        }

        public void ApplyFilter()
        {
            for (int i = Manager.TimeTable.CounTimeTableItems() - 1; i >= 0; i--)
            {
                if (Manager.TimeTable.ItemList[i].GetType() != FilterAttribute)
                {
                    Debug.Write(FilterAttribute);
                    Manager.TimeTable.RemoveTimeTableItem(i);
                }
            }
        }
    }
}