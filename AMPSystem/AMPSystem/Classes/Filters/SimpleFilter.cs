using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    internal interface ISimpleFilter<T> : IFilter
    {
        T FilterAttribute { get; set; }
    }
}