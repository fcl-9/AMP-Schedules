using System;
using System.Collections.Generic;

namespace AMPSystem.Interfaces
{
    public interface IObserver
    {
        ICollection<ISubject> Subjects { get; set; }

        /// <summary>
        /// Method called when something changes in this object brothers.
        /// </summary>
        void Update(ISubject subject);
    }
}