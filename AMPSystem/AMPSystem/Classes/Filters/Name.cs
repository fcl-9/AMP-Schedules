namespace AMPSystem.Classes.Filters
{
    public class Name: ISimpleFilter<string>
    {
        public string FilterAttribute { get; set; }
        public TimeTableManager Manager { get; set; }

        /// <summary>
        /// Construtor.
        /// </summary>
        /// <param name="nameFilter"></param>
        /// <param name="manager"></param>
        public Name(string nameFilter)
        {
            Manager = TimeTableManager.Instance;
            FilterAttribute = nameFilter;
        }
        
        public void ApplyFilter()
        {
            for (var i = Manager.CountTimeTableItems() - 1; i>= 0; i-- )
            {
                if (Manager.TimeTable.ItemList[i].Name != FilterAttribute)
                {
                    Manager.RemoveTimeTableItem(i);
                }
            }
        }
    }
}