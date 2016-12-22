using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class SimpleFilter : IFilter
    {
        public Timetable Timatable { get; set; }

        public void ApplyFilter(string aParameter)
        {
            throw new System.NotImplementedException();
        }
    }
}