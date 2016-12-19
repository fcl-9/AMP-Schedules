using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Classes;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ABC");
            DataHandler dataHandler = new TextHandler();
            dataHandler.ParseData("../../Dados");
            Console.ReadKey();
        }
    }
}
