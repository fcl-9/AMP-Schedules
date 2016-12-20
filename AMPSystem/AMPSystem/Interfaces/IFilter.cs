using AMPSystem.Classes;

namespace AMPSystem.Interfaces
{
    public interface IFilter
    {
        Timetable Timatable { get; set; }

        void ApplyFilter(string aName);
    }
}