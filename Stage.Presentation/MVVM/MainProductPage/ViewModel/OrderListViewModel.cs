using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.Presentation.Common;
using Stage.Presentation.MVVM.MemberLevelInfo;
using Stage.Presentation.MVVM.MemberLoader;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PosStage.MVVM.Models;
using Stage.Data.Models;
using Stage.BLL;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using Utilities;
using PosStage.MVVM.Models.Implement_Model;
using System.Linq;
using Stage.ReportViewCore;
using UIComponent.Controls;

namespace PosStage.MVVM.ViewModel
{
    public class OrderListViewModel : ObservableObject
    {
        #region Field
        private IShopSerivce _shopService => MainDataCenter.Instance.ShopSerivce;

        private ICompletedOrdersService _completedOrdersService;
        private IMemberService _memberService;
        private MemberLoaderView _memberLoaderView;
        private SuccessLoadMemberObserver _successLoadMemberObserver;
        private MemberModel _loadMemberModel;
        private bool _isLoadMemberAlready;
        #endregion
        #region Constructor
        public OrderListViewModel()
        {
            this._completedOrdersService = MainSystemService.Instance.CompletedOrdersService;
            this._memberService = new MemberServiceWebAPI();
            OrderProductViewModelList = new ObservableCollection<OrderProductSlotViewModel>();

            MainDataCenter.Instance.RefreshCartSubject.RegisterObserver(new RefreshCartObserver(RefreshCartData));

            MemberLevelInfoViewModel = new MemberLevelInfoViewModel(MemberLevelInfoRemove);
            _successLoadMemberObserver = new SuccessLoadMemberObserver(SuccessAction, FailAction);
            MainDataCenter.Instance.MemberInfoSubject.RegisterObserver(_successLoadMemberObserver);
        }
        public void Init()
        {

        }
        #endregion
        #region Properties

        private bool _isCheckOutEnable => _shopService.ProductCount == 0 ? false : true;
        public bool IsCheckOutEnable
        {
            get { return _isCheckOutEnable; }
            set
            {
                RaisePropertyChanged(nameof(IsCheckOutEnable));
            }
        }

        private MemberLevelInfoViewModel _memberLevelInfoViewModel;
        /// <summary>
        /// 顯示的會員資料
        /// </summary>
        public MemberLevelInfoViewModel MemberLevelInfoViewModel
        {
            get { return _memberLevelInfoViewModel; }
            set
            {
                if (_memberLevelInfoViewModel != value)
                {
                    _memberLevelInfoViewModel = value;
                    RaisePropertyChanged(nameof(MemberLevelInfoViewModel));
                }
            }
        }
        private ObservableCollection<OrderProductSlotViewModel> _productViewModelList;
        /// <summary>
        /// 當前的餐點列表
        /// </summary>
        public ObservableCollection<OrderProductSlotViewModel> OrderProductViewModelList
        {
            get { return _productViewModelList; }
            set
            {
                if (_productViewModelList != value)
                {
                    _productViewModelList = value;
                    RaisePropertyChanged(nameof(OrderProductViewModelList));
                }
            }
        }
        private IOrderProduct _orderProduct;
        public IOrderProduct OrderProduct
        {
            get { return _orderProduct; }
            set
            {
                if (_orderProduct != value)
                {
                    _orderProduct = value;
                    RaisePropertyChanged(nameof(OrderProduct));
                }
            }
        }

        private string _totalCartCount;
        public string TotalCartCount
        {
            get { return _totalCartCount; }
            set
            {
                if (_totalCartCount != value)
                {
                    _totalCartCount = value;
                    RaisePropertyChanged(nameof(TotalCartCount));
                }
            }
        }
        private string _totalCartAmount;
        public string TotalCartPrice
        {
            get { return _totalCartAmount; }
            set
            {
                if (_totalCartAmount != value)
                {
                    _totalCartAmount = value;
                    RaisePropertyChanged(nameof(TotalCartPrice));
                }
            }
        }
        #endregion
        #region Cammand
        private RelayCommand _checkOutCommand;
        public RelayCommand CheckOutCommand
        {
            get
            {
                if (_checkOutCommand == null)
                {
                    _checkOutCommand = new RelayCommand(CheckOutAction);
                }
                return _checkOutCommand;
            }
            set { _checkOutCommand = value; }
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
        #region Public Method

        #endregion
        #region Private Method
        private void LoadRegisterMemberAction()
        {
            SingletonWindowManager.ShowSingletonWindow<MemberLoaderView>(new MemberLoaderViewModel());
            _memberLoaderView = (MemberLoaderView)SingletonWindowManager.GetWindow<MemberLoaderView>();

            _memberLoaderView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        private void CheckOutAction()
        {
            var allCartContexts = MainDataCenter.Instance.ShoppingCart.GetCartContents().Cast<ProductModel>().ToList();

            if (allCartContexts.Count == 0)
            {
                string text = TextResourceCenter.OrderFailNotHaveProductText;
                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(text);
            }
            else
            {
                string currentOrderNumber = string.Empty;
                //有會員
                if (_isLoadMemberAlready == true || _loadMemberModel != null)
                {
                    MB1003_MemberOrderInfo memberOrderInfo;
                    var response = _completedOrdersService.AddProductOrder(allCartContexts, _loadMemberModel.PhoneNumber);
                    currentOrderNumber = response.Data;
                    if (response.IsSuccess)
                    {
                        string text = TextResourceCenter.OrderSuccessText + currentOrderNumber;
                        ShowMessage(text);
                    }
                    else
                    {
                        ShowMessage(response.Message);
                    }
                }
                //沒會員
                else
                {
                    if (_completedOrdersService.AddProductOrder(allCartContexts, out currentOrderNumber))
                    {
                        string text = TextResourceCenter.OrderSuccessText + currentOrderNumber;
                        ShowMessage(text);
                    }
                }

                ClearOrderList();
                ClearMemberLevelInfo();
            }
        }
        private static void ShowMessage(string text)
        {
            MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(text);
        }
        private void ClearMemberLevelInfo()
        {
            MemberLevelInfoViewModel.IsShow = false;
            _isLoadMemberAlready = false;
        }

        private void ClearOrderList()
        {
            OrderProductViewModelList.Clear();
            MainDataCenter.Instance.ShoppingCart.ClearCart();
            _shopService.RefrshOrderList();
        }
        private void SuccessAction()
        {
            MemberLevelInfoViewModel.IsShow = true;
            _isLoadMemberAlready = true;
            _loadMemberModel = _successLoadMemberObserver.MemberModel;
            _memberLoaderView.Close();
        }
        private void FailAction()
        {
            _isLoadMemberAlready = false;
        }
        private void RefreshCartData()
        {
            RaisePropertyChanged(nameof(IsCheckOutEnable));
            TotalCartCount = _shopService.ProductCount.ToString();
            TotalCartPrice = _shopService.TotalPrice.ToString();
            RefreshOrderListProductView();
        }
        private void RefreshOrderListProductView()
        {
            OrderProductViewModelList.Clear();
            //取出購物車所有選擇的商品
            foreach (var item in _shopService.SelectedProductDic)
            {
                UpdateOrderProduct(item);

                ////取出數量超過1的，要把它全部攤平顯示
                //if (item.Quantity > 1)
                //{
                //    //TODO 可以在優化效能
                //    for (int i = 0; i < item.Quantity; i++)
                //    {
                //        ShowProduct(item);
                //    }
                //}
                //if (item.Quantity == 1)
                //{
                //    ShowProduct(item);
                //}
            }
        }

        private void UpdateOrderProduct(IOrderProduct item)
        {
            var orderProduct = new OrderProductSlotViewModel(item);
            OrderProductViewModelList.Add(orderProduct);
        }

        private void MemberLevelInfoRemove()
        {
            ClearMemberLevelInfo();
        }
        #endregion
    }
}
