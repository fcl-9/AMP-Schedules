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

        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Day { get; set; }
        public IEnumerator<IObserver> Observers { get; set; }
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
