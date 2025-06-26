using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
using PosStage.MVVM.Models.Implement_Model;
using static Stage.Presentation.MVVM.MemberManager.MemberManagerViewModel;
using Utilities;

namespace Stage.Presentation.MVVM.MemberManager.ViewModel
{
    public class UpdateMemberViewModel : ObservableObject
    {
        private IMemberService _memberSerivce;
        private RefreshDGCallBack _refreshDGCallBack;


        public UpdateMemberViewModel(RefreshDGCallBack refreshDGCallBack)
        {
            _memberSerivce = MainSystemService.Instance.MemberSerivce;
            MemberSingleViewModel = new MemberSingleViewModel(_memberSerivce);
            _refreshDGCallBack = refreshDGCallBack;
        }
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

        private bool _isUpdateEditEnable;
        public bool IsUpdateEditEnable
        {
            get { return _isUpdateEditEnable; }
            set
            {
                if (_isUpdateEditEnable != value)
                {
                    _isUpdateEditEnable = value;
                    RaisePropertyChanged(nameof(IsUpdateEditEnable));
                }
            }
        }
        #endregion

        #region Command
        private RelayCommand<object> _updateCommand;
        public RelayCommand<object> UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand<object>(UpdateAction);
                }
                return _updateCommand;
            }
            set { _updateCommand = value; }
        }
        private RelayCommand<object> _updateCancelCommand;
        public RelayCommand<object> UpdateCancelCommand
        {
            get
            {
                if (_updateCancelCommand == null)
                {
                    _updateCancelCommand = new RelayCommand<object>(UpdateCancelCommandAction);
                }
                return _updateCancelCommand;
            }
            set { _updateCancelCommand = value; }
        }
        #endregion

        #region Internal
        private void UpdateCancelCommandAction(object obj)
        {
            UpdateFinish();
        }

        private void UpdateFinish()
        {
            IsUpdateEditEnable = false;
            MemberSingleViewModel.ClearData();

        }

        private void UpdateAction(object obj)
        {
            var member = (obj as MemberModel);
            if (!MemberSingleViewModel.CheckIsValidate())
            {
                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.FiledValidateFail);
                return;
            }
            else
            {
                if (_memberSerivce.Update(member.ConvertToMember()))
                {
                    _refreshDGCallBack?.Invoke();
                    UpdateFinish();
                    MainSystemService.Instance.ShowMessageBox("成功");
                }
                else {
                    MainSystemService.Instance.ShowMessageBox("更新失敗");
                }
            }

        }

        #endregion
    }
}
