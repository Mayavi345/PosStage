using Stage.BLL.BLL;
using Stage.Data.Models;
using Stage.Presentation.MVVM.MemberLevelInfo;
using Utilities.Observer;

namespace Stage.Presentation.Common
{
    public class RegisterMemberObserver : IObserver
    {
        public MemberLevelInfoViewModel _memberLevelInfoViewModel;

        public RegisterMemberObserver(MemberLevelInfoViewModel memberLevelInfoViewModel)
        {
            _memberLevelInfoViewModel = memberLevelInfoViewModel;
        }
        public void Update(object message)
        {
            MemberModel currentMember = message as MemberModel;
            _memberLevelInfoViewModel.SetMemberData(currentMember);
        }
    }
}
