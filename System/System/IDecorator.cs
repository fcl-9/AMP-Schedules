﻿using System.Security.Cryptography.X509Certificates;

namespace System
{
    public interface IDecorator: ITimeTableItem
    {
        ITimeTableItem item { get; set; }
    }
}