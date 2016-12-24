using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Classes;
using AMPSystem.Interfaces;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ABC");
            DataReader dataReader = new FileData();
            Repository dataHandler = new Repository();
            dataHandler.DataReader = dataReader;
            dataHandler.GetCourses("../../Cadeiras");
            dataHandler.GetRooms("../../Salas");
            dataHandler.GetSchedule("../../Dados");
            dataHandler.GetTeachers("../../Teacher");
            foreach (var dataHandlerItem in dataHandler.Items)
            {
                Console.WriteLine(dataHandlerItem.StartTime);
                Console.WriteLine(dataHandlerItem.EndTime);
                foreach (var room in dataHandlerItem.Rooms)
                {
                    Console.WriteLine(room.Name);
                    Console.WriteLine(room.Floor);
                }
                if (dataHandlerItem is Lesson)
                {
                    foreach (var course in ((Lesson)dataHandlerItem).Courses)
                    {
                        Console.WriteLine(course.Name);
                    }
                }
            }
            //Console.WriteLine(dataHandler.Items.First().StartTime);
            //Console.WriteLine(dataHandler.Items.First().EndTime);
            //Console.WriteLine(dataHandler.Items.First().Rooms.First().Number);
            //Console.WriteLine(dataHandler.Items.First().Rooms.First().Floor);
            //Console.WriteLine(((Lesson)dataHandler.Items.First()).Courses.First().Name);
            //Console.WriteLine(((Lesson)dataHandler.Items.First()).Courses.First().Years.First());
            Console.ReadKey();
        }
    }
}
