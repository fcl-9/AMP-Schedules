using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class DailyView: IViewHandler
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CurrentDate { get; set; }
        
        public void CalculateTimeInterval()
        {
            StartDateTime = CurrentDate;
            EndDateTime = CurrentDate;
        }

        #region Observer Pattern
        public ICollection<ISubject> Subjects { get; set; }
        public ICollection<IObserver> Observers { get; set; }
        #endregion

        #region Observer
        public void Update(ISubject subject)
        {
            //Updates Controller for the Interface calls this method
            //CurrentDate = subject.CurrentDate;
            //CalculateTimeInterval();
            throw new System.NotImplementedException();
        }
        #endregion

        #region Subject
        public void Add(IObserver observer)
        {
            Observers.Add(observer);
        }

        public void Remove(IObserver observer)
        {
            Observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in Observers)
            {
                observer.Update(this);
            }
        }
        #endregion
    }
}
