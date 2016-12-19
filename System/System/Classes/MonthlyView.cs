using System.Collections.Generic;

namespace System
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