using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Name: ISimpleFilter<string>
    {
        public string FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }

        public Name(string nameFilter,TimeTableManager manager)
        {
            Manager = manager;
            FilterAttribute = nameFilter;
        }

        public void ApplyFilter()
        {
            foreach (var item in Manager.TimeTable.ItemList)
            {
                if (item.Name != FilterAttribute)
                {
                    Manager.TimeTable.ItemList.Remove(item);
                }
            }
        }
    }
}