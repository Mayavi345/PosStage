using GalaSoft.MvvmLight;
using Stage.BLL.BLL;
using PosStage.MVVM.ViewModel;
using Stage.BLL.BLL.Service;

namespace Stage.Presentation.MVVM.MainPage.ViewModel
{
    public class MainProductPageViewModel : ObservableObject
    {
        public MainProductPageViewModel()
        {

            ProductViewModel = new ProductViewModel();
            OrderListViewModel = new OrderListViewModel();

            NavBarViewModel = new NavBarViewModel();
        }
        public void Init()
        {
            ProductViewModel.Init();
            OrderListViewModel.Init();
        }
        #region  Properties
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
        private ProductViewModel _productViewModel;
        public ProductViewModel ProductViewModel
        {
            get { return _productViewModel; }
            set
            {
                if (_productViewModel != value)
                {
                    _productViewModel = value;
                    RaisePropertyChanged(nameof(ProductViewModel));
                }
            }
        }
        private OrderListViewModel _orderListViewModel;
        public OrderListViewModel OrderListViewModel
        {
            get { return _orderListViewModel; }
            set
            {
                if (_orderListViewModel != value)
                {
                    _orderListViewModel = value;
                    RaisePropertyChanged(nameof(OrderListViewModel));
                }
            }
        }
        #endregion

    }
}
