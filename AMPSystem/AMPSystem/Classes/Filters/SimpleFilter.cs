using System;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    interface SimpleFilter : IFilter
    {
        string FilterAttribute { get; set; }
       
       
    }
}