using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Timetable
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public IList<ITimeTableItem> ItemList { get; set; }

        
        public Timetable(DateTime startDateTime, DateTime endDateTime)
        {
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            ItemList = new List<ITimeTableItem>();
        }

        public void AddTimetableItem(ITimeTableItem item)
        {
            ItemList.Add(item);
        }

        //TODO: rpbably this needs to be removed. The method in bottom will remove the items by index
        public void RemoveTimeTableItem(ITimeTableItem item)
        {
            ItemList.Remove(item);
        }

        public void RemoveTimeTableItem(int position)
        {
            ItemList.RemoveAt(position);
        }

        public int CounTimeTableItems()
        {
            return ItemList.Count();
        }
    }
}
