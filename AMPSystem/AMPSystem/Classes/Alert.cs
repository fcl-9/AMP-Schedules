using System;
using AMPSystem.Interfaces;


namespace AMPSystem.Classes
{
    public class Alert
    {
        // By default, the Entity Framework interprets a property that's named ID or 
        // classnameID as the primary key.
        public int AlertID { get; set; }
        public TimeSpan Time { get; set; }
        public ITimeTableItem Item { get; set; }

        /// <summary>
        /// Empty construtor to create data into database.
        /// </summary>
        public Alert() { }
    }
}
