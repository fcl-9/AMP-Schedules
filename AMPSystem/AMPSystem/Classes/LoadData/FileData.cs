using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class FileData : DataReader
    {
        // This class is used to fetch data from sample text files

        public string RequestTeachers()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"App_Data/Teacher");
        }

        public string RequestUserCourses(string username)
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"App_Data/Course/" + username);
        }

        public string RequestCourses()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"App_Data/Cadeiras");
        }

        public string RequestRooms()
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"App_Data/Salas");
        }

        public string RequestSchedule(string username)
        {
            return System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"App_Data/Schedule/" + username);
        }
    }
}
