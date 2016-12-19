using System;
using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class MonthlyView:IViewHandler
    {
        public IEnumerator<ISubject> Existingsubjects { get; set; }

        public void RenderView(int viewType)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}