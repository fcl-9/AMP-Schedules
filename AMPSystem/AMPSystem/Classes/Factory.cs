using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMPSystem.Classes
{
    class Factory
    {
        #region Singleton
    
        private static Factory instance;

        private Factory() { }

        public static Factory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Factory();
                }
                return instance;
            }
        }
        #endregion
    }
}
