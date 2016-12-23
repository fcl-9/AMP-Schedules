using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace AMPSystem.Interfaces
{
    public interface ISubject
    {
        ICollection<IObserver> Observers { get; set; }

        void Add(IObserver observer);
        void Remove(IObserver observer);
        void Notify();
    }
}