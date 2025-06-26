using GalaSoft.MvvmLight;
using Stage.BLL.BLL;

namespace Stage.Backstage.ViewModel
{
    public class PageViewModelBase : ObservableObject
    {
        private IRuleManager _ruleManager;
        public PageViewModelBase()
        {
            _ruleManager = new RuleManager();
            _ruleManager.RegisterObserver(UpdateRole);

        }
        private void UpdateRole(bool state)
        {
            IsManagerRole = state;
        }
        private bool _isManagerRole;
        public bool IsManagerRole
        {
            get { return _isManagerRole; }
            set
            {
                if (_isManagerRole != value)
                {
                    _isManagerRole = value;
                    RaisePropertyChanged(nameof(IsManagerRole));
                }
            }
        }
    }

}
