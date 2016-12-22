using AMPSystem.Classes;

namespace AMPSystem.Interfaces
{
    public interface IFilter
    {
        Timetable TimatableInstance { get; set; }

        void ApplyFilter(string aName);
    }
}