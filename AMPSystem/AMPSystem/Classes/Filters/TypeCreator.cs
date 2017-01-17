using System;
using AMPSystem.Classes.TimeTableItems;

namespace AMPSystem.Classes.Filters
{
    public class TypeCreator
    {
        /// <summary>
        ///     TypeF class doesn't change.
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
                    throw new InvalidOperationException("You are trying to get a type of an object tha doesn't exist");
            }
        }
    }
}