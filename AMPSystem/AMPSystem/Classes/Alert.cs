using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Alert
    {
        /// <summary>
        ///     Construtor for database.
        /// </summary>
        /// <param name="alertTime"></param>
        /// <param name="tableItem"></param>
        public Alert(DateTime alertTime, ITimeTableItem tableItem)
        {
            AlertTime = alertTime;
            Item = tableItem;
            AddItem();
        }

        /// <summary>
        ///     Construtor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="alertTime"></param>
        /// <param name="tableItem"></param>
        public Alert(int id, DateTime alertTime, ITimeTableItem tableItem)
        {
            Id = id;
            AlertTime = alertTime;
            Item = tableItem;
            AddItem();
        }

        public int Id { get; set; }
        public DateTime AlertTime { get; set; }
        public ITimeTableItem Item { get; set; }

        private void AddItem()
        {
            Item.Alerts.Add(this);
        }
    }
}