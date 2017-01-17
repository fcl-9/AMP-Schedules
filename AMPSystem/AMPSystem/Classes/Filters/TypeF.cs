using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.Filters
{
    public class TypeF : ISimpleFilter<object>
    {
        /// <summary>
        ///     Construtor.
        /// </summary>
        /// <param name="filterName"></param>
        public TypeF(string filterName)
        {
            GeneratesTypes = new TypeCreator();
            Manager = TimeTableManager.Instance;
            FilterAttribute = GeneratesTypes.CreateTypeOf(filterName);
        }

        public TypeCreator GeneratesTypes { get; set; }
        public object FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }

        /// <summary>
        ///     Apply filter
        /// </summary>
        public void ApplyFilter()
        {
            for (var i = Manager.CountTimeTableItems() - 1; i >= 0; i--)
            {
                if ((Manager.TimeTable.ItemList == null) ||
                    (Manager.TimeTable.ItemList[i].GetType() == (Type) FilterAttribute))
                    continue;
                Manager.RemoveTimeTableItem(i);
            }
        }
    }
}