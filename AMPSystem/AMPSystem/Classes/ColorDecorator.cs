using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    class ColorDecorator : IDecorator, ISubject
    {
        public string Color { get; set; }
        public ITimeTableItem Item { get; set; }
        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<Room> Rooms { get; set; }
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public ICollection<IObserver> Observers { get; set; }
        public void Add(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Remove(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Notify()
        {
            throw new NotImplementedException();
        }
    }
}
