using System;
using AMPSystem.Interfaces;


namespace AMPSystem.Classes
{
    class Alert
    {
        public DateTime Hour { get; set; }
        public string Type { get; set; }
        public ITimeTableItem Item { get; set; }
    }
}
