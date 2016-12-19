using System.Collections.Generic;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
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