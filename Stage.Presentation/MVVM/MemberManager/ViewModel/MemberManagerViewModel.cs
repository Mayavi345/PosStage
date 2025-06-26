using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.Data.Models;
using Stage.Presentation.MVVM.MemberManager.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosStage.MVVM.Models.Implement_Model;
using static Stage.Presentation.MVVM.MemberManager.MemberManagerViewModel;
using CodingNinja.Wpf.ObjectModel;
using PosStage.MVVM.ViewModel;
using UIComponent;

namespace Stage.Presentation.MVVM.MemberManager
{
    public class MemberManagerViewModel : ObservableObject
    {
        private IMemberService _memberSerivce;
        public delegate void RefreshDGCallBack();
        private RefreshDGCallBack _refreshDGCallBack;

        public MemberManagerViewModel()
        {
            _memberSerivce = MainSystemService.Instance.MemberSerivce;
            MemberDGList = new WpfObservableRangeCollection<MemberModel>();
            _refreshDGCallBack = LoadMemberDGList;
            _addMemberViewModel = new AddMemberViewModel(_refreshDGCallBack);
            _updateMemberViewModel = new UpdateMemberViewModel(_refreshDGCallBack);

            NavBarViewModel = new NavBarViewModel();
        }

        public void LoadMemberDGList()
        {
            Utilities.IResponseModel<List<MemberModel>> response = _memberSerivce.GetMemberModelList();
            if (response.IsSuccess)
            {
                //TODO 優化效能，先前修改資料型態會有問題 "System.NotSupportedException: 不支援範圍動作" 。
                MemberDGList = new WpfObservableRangeCollection<MemberModel>(response.Data);
            }
            else
            {
                throw new Exception();
            }
        }

        #region Properties
        private NavBarViewModel _navBarViewModel;
        public NavBarViewModel NavBarViewModel
        {
            get { return _navBarViewModel; }
            set
            {
                if (_navBarViewModel != value)
                {
                    _navBarViewModel = value;
                    RaisePropertyChanged(nameof(NavBarViewModel));
                }
            }
        }
        private AddMemberViewModel _addMemberViewModel;
        public AddMemberViewModel AddMemberViewModel
        {
            get { return _addMemberViewModel; }
            set
            {
                if (_addMemberViewModel != value)
                {
                    _addMemberViewModel = value;
                    RaisePropertyChanged(nameof(AddMemberViewModel));
                }
            }
        }
        private UpdateMemberViewModel _updateMemberViewModel;
        public UpdateMemberViewModel UpdateMemberViewModel
        {
            get { return _updateMemberViewModel; }
            set
            {
                if (_updateMemberViewModel != value)
                {
                    _updateMemberViewModel = value;
                    RaisePropertyChanged(nameof(UpdateMemberViewModel));
                }
            }
        }
        private WpfObservableRangeCollection<MemberModel> _memberDGList;
        public WpfObservableRangeCollection<MemberModel> MemberDGList
        {
            get => _memberDGList;
            set
            {
                _memberDGList = value;
                RaisePropertyChanged(nameof(MemberDGList));
            }
        }
        #endregion
        #region Command
        private RelayCommand<object> _editCommand;

        public RelayCommand<object> EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand<object>(EditCommandAction);
                }
                return _editCommand;
            }
            set { _editCommand = value; }
        }



        private RelayCommand<object> _deleteCommand;

        public RelayCommand<object> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<object>(DeleteCommandAction);
                }
                return _deleteCommand;
            }
            set { _deleteCommand = value; }
        }

        #endregion
        #region Internael
        private void EditCommandAction(object obj)
        {
            var member = (obj as MemberModel);
            UpdateMemberViewModel.IsUpdateEditEnable = true;
            UpdateMemberViewModel.MemberSingleViewModel.InitSingleViewData(member);
        }
        private void DeleteCommandAction(object obj)
        {
            UIMessageBoxOKCancel.Instance.ShowDialog(() =>
            {
                Delete(obj);
            }, () => { });

        }

        private void Delete(object obj)
        {
            var member = (obj as MemberModel);

            var response = _memberSerivce.Delete(member.Id);
            if (response.IsSuccess)
            {
                _refreshDGCallBack?.Invoke();
            }
            else
            {
                MainSystemService.Instance.ShowMessageBox(response.Message);
            }
        }

        #endregion

    }
}
