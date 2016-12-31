using System;
using AMPSystem.Interfaces;


namespace AMPSystem.Classes
{
    public class Alert
    {
        public int Id { get; set; }
        public DateTime AlertTime { get; set; }
        public ITimeTableItem Item { get; set; }

        private static int _id;

        public Alert(DateTime alertTime, ITimeTableItem tableItem)
        {
            AlertTime = alertTime;
            Item = tableItem;
            InformItem();
        }

        public Alert(int id, DateTime alertTime, ITimeTableItem tableItem)
        {
            Id = id;
            AlertTime = alertTime;
            Item = tableItem;
            InformItem();
        }

        private void InformItem()
        {
            Item.Alerts.Add(this);
        }
    }
}
