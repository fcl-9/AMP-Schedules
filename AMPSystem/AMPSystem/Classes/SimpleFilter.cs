using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class SimpleFilter : IFilter
    {
        public string Name { get; private set; }
        public Timetable Timatable { get; set; }

        public SimpleFilter(string filterName)
        {
            Name = filterName;
        }

        public void ApplyFilter(string aParameter)
        {
            throw new System.NotImplementedException();
        }
    }
}