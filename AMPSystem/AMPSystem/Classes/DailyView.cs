using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMPSystem.Interfaces;

namespace AMPSystem.Classes
{
    public class DailyView:IViewHandler
    {
        public void RenderView(int viewType)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ISubject> Existingsubjects { get; set; }
        public void Update()
        {
        }
    }
}
