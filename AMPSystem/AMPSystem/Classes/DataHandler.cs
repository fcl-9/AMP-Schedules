using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMPSystem.Classes
{ 
    public abstract class DataHandler
    {
        public abstract string RequestData(string path);

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
