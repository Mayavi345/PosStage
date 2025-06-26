using GalaSoft.MvvmLight;
using Stage.BLL.BLL;
using Stage.Data.Models;
using Stage.Presentation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Utilities;

namespace Stage.Presentation.MVVM.MemberLevelInfo
{
    public class MemberLevelInfoViewModel : ObservableObject
    {
        public ICommand RemoveCommand { get; set; }
        public delegate void MemberLevelInfoRemoveCallback();
        private MemberLevelInfoRemoveCallback _memberRemoveCallback;
        public MemberLevelInfoViewModel(MemberLevelInfoRemoveCallback memberLevelInfoRemoveCallback)
        {
            _memberRemoveCallback = memberLevelInfoRemoveCallback;
            MainDataCenter.Instance.MemberInfoSubject.RegisterObserver(new RegisterMemberObserver(this));
            IsShow = false;
            RemoveCommand = new RelayCommandBase(RemoveCommandAction).Command;

        }

        #region  Properties
        private bool _isShow;
        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    RaisePropertyChanged(nameof(IsShow));
                }
            }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set
            {
                if (_gender != value)
                {
                    _gender = value;
                    RaisePropertyChanged(nameof(Gender));
                }
            }
        }
        private string _level;
        public string Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    RaisePropertyChanged(nameof(Level));
                }
            }
        }
        #endregion

        public void SetMemberData(MemberModel currentMember)
        {
            if (currentMember == null)
                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.MemberDataIsNull);
            else
            {
                Name = currentMember.Name;
                Gender = currentMember.Gender.Name;
                Level = currentMember.ConsumptionLevel.Name;
            }
        }
        private void RemoveCommandAction(object obj)
        {
            _memberRemoveCallback?.Invoke();
        }
    }
}
