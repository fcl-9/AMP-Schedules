using System.Collections.Generic;

namespace System
{
    public class WeeklyView:IViewHandler
    {
        public IEnumerator<ISubject> Existingsubjects { get; set; }

        public void RenderView(int viewType)
        {
        }

        public void Update()
        {
        }
    }
}