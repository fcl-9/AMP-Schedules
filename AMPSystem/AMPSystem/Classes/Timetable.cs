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
        public DateTime TimeInterval { get; set; }
        public ICollection<ITimeTableItem> ItemList { get; set; }

        #region singleton
        private static Timetable  Schedule = new Timetable();

        private Timetable()
        {
            ItemList = new List<ITimeTableItem>();    
        }

        public static Timetable Instance
        {
            get { return Schedule; }
        }
        #endregion

        public void AddTimetableItem(ITimeTableItem item)
        {
            ItemList.Add(item);
        }

        public void RemoveTimetableItem(ITimeTableItem item)
        {
            ItemList.Remove(item);
        }
    }
}
