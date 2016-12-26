using System;
using System.Diagnostics;
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
            for (int i = Manager.TimeTable.CounTimeTableItems() - 1; i>= 0; i-- )
            {
                if (Manager.TimeTable.ItemList[i].Name != FilterAttribute)
                {
                    Manager.TimeTable.RemoveTimeTableItem(i);
                }
            }


            //    foreach (var item in Manager.TimeTable.ItemList)
            //{
            //    Debug.Write(item);
            //    if (item.Name )
            //    {
            //        Manager.TimeTable.RemoveTimetableItem(item);
            //    }
            //}
        }
    }
}