using AMPSystem.Classes;

namespace AMPSystem.Interfaces
{
    public interface IFilter
    {
        TimeTableManager Manager { get; set; }
        //Filter Applyer
        void ApplyFilter();
    }
}