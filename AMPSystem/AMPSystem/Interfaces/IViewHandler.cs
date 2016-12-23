using System;

namespace AMPSystem.Interfaces
{
    public interface IViewHandler: IObserver, ISubject
    {
        DateTime StartDateTime { get; set; }
        DateTime EndDateTime { get; set; }
        DateTime CurrentDate { get; set; }

        void CalculateTimeInterval();
    }
}