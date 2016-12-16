using System.Collections.Generic;

namespace System
{
    public interface IObserver
    {
        IEnumerator<ISubject> Existingsubjects { get; set; }

        /*This Method will be called when something changes in this object brothers*/
        void Update();
    }
}