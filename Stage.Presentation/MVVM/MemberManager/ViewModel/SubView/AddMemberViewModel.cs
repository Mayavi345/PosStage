using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NLog.Targets;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIComponent.BindingModel;
using PosStage.MVVM.Models;
using static Stage.Presentation.MVVM.MemberManager.MemberManagerViewModel;
using System.Windows.Markup;
using Utilities;

namespace Stage.Presentation.MVVM.MemberManager.ViewModel
{
    public class AddMemberViewModel : ObservableObject
    {
        #region Field
        private Action<object> _addCommandAction;
        private IMemberService _memberSerivce;
        private Action<object> _updateCancelCommandAction;
        private RefreshDGCallBack _refreshDGCallBack;
        private List<Func<bool>> _checkValidateFuncList;

        #endregion
        #region Constructor
        public AddMemberViewModel(RefreshDGCallBack refreshDGCallBack)
        {
            _addCommandAction = AddAction;
            _memberSerivce = MainSystemService.Instance.MemberSerivce;
            MemberSingleViewModel = new MemberSingleViewModel(_memberSerivce);

            _refreshDGCallBack = refreshDGCallBack;
        }
        #endregion
        #region Properties

        private MemberSingleViewModel _memberSingleViewModel;
        public MemberSingleViewModel MemberSingleViewModel
        {
            get => _memberSingleViewModel;
            set
            {
                _memberSingleViewModel = value;
                RaisePropertyChanged(nameof(MemberSingleViewModel));
            }
        }
        #endregion
        #region Command

        private RelayCommand<object> _addCommand;

        public RelayCommand<object> AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand<object>(_addCommandAction);
                }
                return _addCommand;
            }
            set { _addCommand = value; }
        }
        private RelayCommand<object> _updateCancelCommand;

        public RelayCommand<object> UpdateCancelCommand
        {
            get
            {
                if (_updateCancelCommand == null)
                {
                    _updateCancelCommand = new RelayCommand<object>(_updateCancelCommandAction);
                }
                return _updateCancelCommand;
            }
            set { _updateCancelCommand = value; }
        }



        #endregion
        #region Public Method

        #endregion
        #region Private Method

        private void AddAction(object obj)
        {

            if (!MemberSingleViewModel.CheckIsValidate())
            {
                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.FiledValidateFail);
                return;
            }
            else
            {
                var member = (obj as MemberModel);
                MB1001_Member tempMember;

                List<Func<bool>> checkValidateFuncList = GetValidateFuncList();
                foreach (var item in checkValidateFuncList)
                {
                    var result = item?.Invoke();
                    if (result == false)
                    {
                        var messageBoxText = TextResourceCenter.CreateMemberFail;
                        MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(messageBoxText);
                    }
                }

                var response = _memberSerivce.Add(member.ConvertToMember());
                tempMember = (MB1001_Member)response.Data;

                if (response.IsSuccess)
                {

                    var messageBoxText = TextResourceCenter.CreateMemberSuccess + tempMember.Name;
                    MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(messageBoxText);
                    _refreshDGCallBack?.Invoke();
                }
                else
                {
                    var messageBoxText = TextResourceCenter.CreateMemberFail + response.Message;
                    MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(messageBoxText);

                }
                MemberSingleViewModel.ClearData();
            }
        }



        private List<Func<bool>> GetValidateFuncList()
        {
            _checkValidateFuncList = new List<Func<bool>>();
            Func<bool> func = CheckPhoneNumber;
            _checkValidateFuncList.Add(func);
            return _checkValidateFuncList;
        }

        private bool CheckPhoneNumber()
        {

            //TODO 檢查電話號碼
            return true;
        }

        #endregion
    }
}
