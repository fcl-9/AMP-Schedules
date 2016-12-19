using System;
using AMPSystem.Interfaces;


namespace AMPSystem.Classes
{
    class AlertDecorator : IDecorator
    {
        public DateTime AlertStarTime { get; set; }
        public DateTime AlertEndTime { get; set; }
        public DateTime AlertDay { get; set; }
        public ITimeTableItem Item { get; set; }

        DateTime ITimeTableItem.StarTime
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DateTime ITimeTableItem.EndTime
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        DateTime ITimeTableItem.Day
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
