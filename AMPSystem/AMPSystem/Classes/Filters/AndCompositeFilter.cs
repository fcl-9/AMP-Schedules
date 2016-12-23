using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class AndCopositeFilter : IFilter
    {
        public ICollection<IFilter> Filters { get; set; } //List of filters you can apply to time table items
        public TimeTableManager Manager { get; set; }
        //Allow you to access to all timetableitems

        public AndCopositeFilter()
        {
            Filters = new List<IFilter>();
        }

        /// <summary>
        /// Changes when a user selects a filter 
        /// </summary>
        /// <param name="aFilter"></param>
        public void Add(IFilter aFilter)
        {
            Filters.Add(aFilter);
        }

        /// <summary>
        /// Changes when a user removes a filter
        /// </summary>
        /// <param name="aFilter"></param>
        public void Remove(IFilter aFilter)
        {
            Filters.Remove(aFilter);
        }

        public void ApplyFilter(string aName)
        {
            //Base
        }

        public bool ApplyFilter()
        {
            throw new System.NotImplementedException();
        }
    }
}