using AMPSystem.Classes;

namespace AMPSystem.Interfaces
{
    public interface IFilter
    {
        TimeTableManager Manager { get; set; }

        /// <summary>
        /// Filter applier.
        /// </summary>
        void ApplyFilter();
    }
}