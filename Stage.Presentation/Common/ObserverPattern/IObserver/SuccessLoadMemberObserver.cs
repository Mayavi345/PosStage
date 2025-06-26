using Stage.Data.Models;
using System;
using Utilities.Observer;

namespace Stage.Presentation.Common
{
    public class SuccessLoadMemberObserver : IObserver
    {
        public MemberModel MemberModel => _currentMember;
        private MemberModel _currentMember;
        public SuccessLoadMemberObserver(Action actionSuccess, Action actionFail)
        {
            ActionSuccess = actionSuccess;
            ActionFail = actionFail;
        }

        public Action ActionSuccess { get; }
        public Action ActionFail { get; }

        public void Update(object message)
        {
            _currentMember = message as MemberModel;
            if (_currentMember == null)
                ActionFail?.Invoke();
            else
            {
                ActionSuccess?.Invoke();
            }
        }
    }
}
