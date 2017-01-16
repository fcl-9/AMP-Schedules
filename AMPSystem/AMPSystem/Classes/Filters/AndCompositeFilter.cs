using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.Filters
{
    public class AndCompositeFilter : IFilter
    {
        //List of filters you can apply to time table items
        public ICollection<IFilter> Filters { get; set; } 
        //Allow you to access to all timetableitems
        public TimeTableManager Manager { get; set; }

        /// <summary>
        /// Contructer initializes the filters.
        /// </summary>
        /// <param name="manager"></param>
        public AndCompositeFilter()
        {
            Manager = TimeTableManager.Instance; 
            Filters = new List<IFilter>();
        }

        /// <summary>
        /// Changes when a user selects a filter.
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

        /// <summary>
        /// Applies all filters to the timetable items
        /// </summary>
        public void ApplyFilter()
        {
            foreach (var filter in Filters)
            {
               filter.ApplyFilter();
            }    
        }
    }
}