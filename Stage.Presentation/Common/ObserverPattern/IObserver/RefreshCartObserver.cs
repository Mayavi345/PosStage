using Stage.Common;
using System;
using Utilities.Observer;

namespace Stage.Presentation.Common
{
    public class RefreshCartObserver : EventObserverBase
    {
        public RefreshCartObserver(Action action)
        {
            Action = action;
        }
    }
    
}
