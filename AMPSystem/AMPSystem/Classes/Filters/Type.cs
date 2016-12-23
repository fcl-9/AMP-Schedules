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

        public string FilterAttribute { get; set; }

        public bool ApplyFilter()
        {
            throw new System.NotImplementedException();
        }
    }
}