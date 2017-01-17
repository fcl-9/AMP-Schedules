using AMPSystem.Interfaces;

namespace AMPSystem.Classes.Filters
{
    public class Name : ISimpleFilter<string>
    {
        /// <summary>
        ///     Construtor.
        /// </summary>
        /// <param name="nameFilter"></param>
        public Name(string nameFilter)
        {
            Manager = TimeTableManager.Instance;
            FilterAttribute = nameFilter;
        }

        public string FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }

        /// <summary>
        ///     Apply filter to items.
        /// </summary>
        public void ApplyFilter()
        {
            for (var i = Manager.CountTimeTableItems() - 1; i >= 0; i--)
                if (Manager.TimeTable.ItemList[i].Name != FilterAttribute)
                    Manager.RemoveTimeTableItem(i);
        }
    }
}