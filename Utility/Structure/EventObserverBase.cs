using System;
using Utilities.Observer;

namespace Stage.Common
{
    /// <summary>
    /// 基礎事件(事件發生時進行通知)
    /// </summary>
    public class EventObserverBase: IObserver
    {
        public Action Action { get; set; }


        public void Update(object message)
        {
            Action?.Invoke();
        }
    }
}