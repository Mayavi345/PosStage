using Stage.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage.Backstage.Common
{
    public class RefreshProductViewObserver : EventObserverBase
    {
        public RefreshProductViewObserver(Action action)
        {
            Action = action;
        }
    }

}
