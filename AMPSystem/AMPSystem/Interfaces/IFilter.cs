using AMPSystem.Classes;

namespace AMPSystem.Interfaces
{
    public interface IFilter
    {
        TimeTableManager Manager { get; set; }
        bool ApplyFilter();
    }
}