using System.CodeDom;

namespace AMPSystem.Classes
{
    public class TypeCreator
    {
        //This way you add new TimeTableItems you can add them here without changing the TypeF Class.
       public object CreateTypeOf(string typeToReturn)
        {
            if (typeToReturn == "Lesson")
            {
                return typeof(Lesson);
            }
            else if (typeToReturn == "EvaluationMoment")
            {
                return typeof(EvaluationMoment);
            }
            else if (typeToReturn == "OfficeHours")
            {
                return typeof(OfficeHours);
            }
            else
            {
                //This should throw an exception cause there is no object that matches the string sent
                throw new System.InvalidOperationException("Your trying get type of an object tha doesn't exist");
            }

        }
    }
}