using System;
using System.Diagnostics;

namespace AMPSystem.Classes.Filters
{
    public class TypeF : ISimpleFilter<object>
    {
        public object FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }
        public TypeCreator GeneratesTypes { get; set; }

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="filterName"></param>
        /// <param name="manager"></param>
        public TypeF(string filterName, TimeTableManager manager)
        {
            GeneratesTypes = new TypeCreator();
            Manager = manager;
            FilterAttribute = GeneratesTypes.CreateTypeOf(filterName);
        }

        public void ApplyFilter()
        {
            for (var i = Manager.CountTimeTableItems() - 1; i >= 0; i--)
            {
                if (Manager.TimeTable.ItemList == null || Manager.TimeTable.ItemList[i].GetType() == (Type) FilterAttribute)
                    continue;
                Debug.Write(FilterAttribute);
                Manager.RemoveTimeTableItem(i);
            }
        }
    }
}