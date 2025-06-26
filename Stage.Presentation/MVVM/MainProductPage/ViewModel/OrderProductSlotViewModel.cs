using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.BLL;
using Stage.BLL.BLL.Service;
using Stage.Presentation.MVVM.MemberLoader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PosStage.MVVM.Models.Implement_Model;
using Stage.BLL.BLL;
using Stage.Presentation.Common;

namespace PosStage.MVVM.ViewModel
{
    public class OrderProductSlotViewModel : ObservableObject
    {
        #region Field
        private IShopSerivce _shopService => MainDataCenter.Instance.ShopSerivce;

        #endregion
        #region Constructor
        public OrderProductSlotViewModel(IOrderProduct orderProduct)
        {
            OrderProductModel = orderProduct;
            Source = ImageSourceProcess.Instance.GetImage(orderProduct.ImageId);
        }
        #endregion
        #region  Properties
        private ImageSource _source;
        public ImageSource Source
        {
            get { return _source; }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    RaisePropertyChanged(nameof(Source));
                }
            }
        }
        private IOrderProduct _orderProductModel;
        public IOrderProduct OrderProductModel
        {
            get { return _orderProductModel; }
            set
            {
                if (_orderProductModel != value)
                {
                    _orderProductModel = value;
                    RaisePropertyChanged(nameof(OrderProductModel));
                }
            }
        }
        #endregion
        #region Public Method

        #endregion
        #region Commands

        private RelayCommand _addCountCommand;
        public RelayCommand AddCountCommand
        {
            get
            {
                if (_addCountCommand == null)
                {
                    _addCountCommand = new RelayCommand(AddCountAction);
                }
                return _addCountCommand;
            }
            set { _addCountCommand = value; }
        }

        private RelayCommand _decressCountCommand;

        public RelayCommand DecressCountCommand
        {
            get
            {
                if (_decressCountCommand == null)
                {
                    _decressCountCommand = new RelayCommand(DecressCountAction);
                }
                return _decressCountCommand;
            }
            set { _decressCountCommand = value; }
        }

        private RelayCommand _removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                if (_removeCommand == null)
                {
                    _removeCommand = new RelayCommand(RemoveAction);
                }
                return _removeCommand;
            }
            set { _removeCommand = value; }
        }
        private RelayCommand _loadRegisterMember;
        public RelayCommand LoadRegisterMember
        {
            get
            {
                if (_loadRegisterMember == null)
                {
                    _loadRegisterMember = new RelayCommand(LoadRegisterMemberAction);
                }
                return _loadRegisterMember;
            }
            set { _loadRegisterMember = value; }
        }
        #endregion
        #region Private Method
        private void LoadRegisterMemberAction()
        {
            MemberLoaderView memberLoaderView = new MemberLoaderView();
            memberLoaderView.DataContext = new MemberLoaderViewModel();
        }
        private void RemoveAction()
        {
            _shopService.RemoveOrderProduct(OrderProductModel);
        }
        private void UpdateCount()
        {
            _shopService.AddOrderProduct(OrderProductModel);
        }
        private void DecressCountAction()
        {
            if (OrderProductModel.Quantity > 0)
            {
                OrderProductModel.Quantity = OrderProductModel.Quantity - 1;
                UpdateCount();
            }
        }
        private void AddCountAction()
        {
            //進行UI顯示的處理
            if (OrderProductModel.Quantity >= 0)
            {
                OrderProductModel.Quantity = OrderProductModel.Quantity + 1;
                UpdateCount();
            }
        }
        #endregion
    }

}
