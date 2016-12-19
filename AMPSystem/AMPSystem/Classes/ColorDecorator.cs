using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    class ColorDecorator:ISubject
    {
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Day { get; set; }
        public ITimeTableItem Item { get; set; }
        public IEnumerator<IObserver> Observers { get; set; }

        /*Adds the ones who will be listening to my changes*/
        public void Add(IObserver observer)
        {
        }
        /*Remove one of my listeners*/
        public void Remove(IObserver observer)
        {
        }
        /*Notifies all the listeners*/
        public void Notify()
        {
        }
    }
}
