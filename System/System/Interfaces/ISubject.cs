using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace System
{
    public interface ISubject:IDecorator
    {
        IEnumerator<IObserver> Observers { get; set; }
        void Add(IObserver observer);
        void Remove(IObserver observer);
        void Notify();
    }
}