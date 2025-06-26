using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage.Presentation.MVVM.MemberLoader
{
    public class MemberLoaderViewModel : ObservableObject
    {
        private IMemberService _memberSerivce;

        public MemberLoaderViewModel()
        {
            _memberSerivce = MainSystemService.Instance.MemberSerivce;

        }

        private string _phoneNumber;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                RaisePropertyChanged(nameof(PhoneNumber));
            }
        }

        private RelayCommand _loadMemberCommand;

        public RelayCommand LoadMemberCommand
        {
            get
            {
                if (_loadMemberCommand == null)
                {
                    _loadMemberCommand = new RelayCommand(LoadMemberCommandAction);
                }
                return _loadMemberCommand;
            }
            set { _loadMemberCommand = value; }
        }

        private void LoadMemberCommandAction()
        {
            MemberModel currentMember = _memberSerivce.GetMemberModel(PhoneNumber).Data;
            MainDataCenter.Instance.MemberInfoSubject.NotifyObservers(currentMember);
        }
    }
}
