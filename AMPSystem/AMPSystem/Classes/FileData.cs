using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class FileData : DataReader
    {
        public string RequestData(string filePath)
        {
            //Read Data From Text File
            return System.IO.File.ReadAllText(filePath);
        }
    }
}
