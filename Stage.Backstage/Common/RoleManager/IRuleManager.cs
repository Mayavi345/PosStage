using System;

namespace Stage.Backstage.ViewModel
{
    public interface IRuleManager
    {
        void RegisterObserver(Action<bool> action);
    }
}