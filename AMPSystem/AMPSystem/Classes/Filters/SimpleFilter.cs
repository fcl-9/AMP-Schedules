using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    interface ISimpleFilter<T> : IFilter
    {
        T FilterAttribute { get; set; }

    }
}