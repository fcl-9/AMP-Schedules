using AMPSystem.Classes.TimeTableItems;

namespace AMPSystem.Classes.Filters
{
    public class TypeCreator
    {
        /// <summary>
        /// Ensures that as you add new TimeTableItems you can add them here without changing the TypeF class.
        /// </summary>
        /// <param name="typeToReturn"></param>
        /// <returns></returns>
        public object CreateTypeOf(string typeToReturn)
        {
            switch (typeToReturn)
            {
                case "Lesson":
                    return typeof(Lesson);
                case "EvaluationMoment":
                    return typeof(EvaluationMoment);
                case "OfficeHours":
                    return typeof(OfficeHours);
                default:
                    throw new System.InvalidOperationException("Your trying get type of an object tha doesn't exist");
            }
        }
    }
}