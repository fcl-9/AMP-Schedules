using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class OrCompositeFilter : IFilter
    {
        public ICollection<IFilter> Filters { get; set; }
        public Timetable Timatable { get; set; }

        public void Add(IFilter aFilter)
        {
            Filters.Add(aFilter);
        }

        public void Remove(IFilter aFilter)
        {
            Filters.Remove(aFilter);
        }
        
        public void ApplyFilter(string aName)
        {
            throw new System.NotImplementedException();
        }
    }
}