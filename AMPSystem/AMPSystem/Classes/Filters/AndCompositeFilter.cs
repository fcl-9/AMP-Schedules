using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes.Filters
{
    public class AndCompositeFilter : IFilter
    {
        /// <summary>
        ///     Contructor.
        /// </summary>
        public AndCompositeFilter()
        {
            Manager = TimeTableManager.Instance;
            Filters = new List<IFilter>();
        }

        public ICollection<IFilter> Filters { get; set; }
        public TimeTableManager Manager { get; set; }

        /// <summary>
        ///     Applies all filters to the timetable items
        /// </summary>
        public void ApplyFilter()
        {
            foreach (var filter in Filters)
                filter.ApplyFilter();
        }

        /// <summary>
        ///     Changes when a user selects a filter.
        /// </summary>
        /// <param name="aFilter"></param>
        public void Add(IFilter aFilter)
        {
            Filters.Add(aFilter);
        }

        /// <summary>
        ///     Changes when a user removes a filter
        /// </summary>
        /// <param name="aFilter"></param>
        public void Remove(IFilter aFilter)
        {
            Filters.Remove(aFilter);
        }
    }
}