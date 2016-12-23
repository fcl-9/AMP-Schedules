using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class Type : SimpleFilter
    {
        public string filterName { get; set; }

        public Type(string filterName)
        {
            this.filterName = filterName;
        }

        public bool ApplyFilter(TimeTableManager manager)
        {
            if (this.filterName == manager)
            {
            }
        }

        public string FilterAttribute { get; set; }
    }
}