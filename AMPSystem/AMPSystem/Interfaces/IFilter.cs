using AMPSystem.Classes;

namespace AMPSystem.Interfaces
{
    public interface IFilter
    {
        TimeTableManager Manager { get; set; }
        /// <summary>
        /// Filter Applyer
        /// </summary>
        void ApplyFilter();
    }
}