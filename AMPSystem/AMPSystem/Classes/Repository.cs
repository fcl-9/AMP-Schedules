using System;
using Newtonsoft.Json.Linq;

namespace AMPSystem.Classes
{
    public abstract class Repository
    {
        public EvaluationMoment GetEvaluationMoment();
        public Lesson GetLessons();
        public OfficeHours GetOfficeHours();
        public string RequestData(string path);

        public void ParseData(string path)
        {
            string data = RequestData(path);
            Console.WriteLine(data);
            if (!string.IsNullOrEmpty(data))
            {
                var dataParsed = JObject.Parse(data);
                foreach (var item in dataParsed["Schedule"])
                {
                    var lessonName = item["LessonName"];
                    var startTime = item["StartTime"];
                    var endTime = item["EndTime"];
                    var lessonType = item["LessonType"];
                    var classRoom = item["ClassRoom"];
                    var teacher = item["Teacher"];
                    Console.WriteLine(lessonName);
                    Console.WriteLine(startTime);
                    Console.WriteLine(endTime);
                    Console.WriteLine(lessonType);
                    Console.WriteLine(classRoom);
                    Console.WriteLine(teacher);
                }
            }
        }
    }
}