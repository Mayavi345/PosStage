using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.BLL;
using Stage.BLL.BLL.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PosStage.MVVM.Models.Implement_Model;
using Utilities.Nlog;

namespace PosStage.MVVM.ViewModel
{
    public class ProductSlotViewModel : ObservableObject
    {
        #region Field
        private IShopSerivce _shopService;
        #endregion
        #region Constructor
        public ProductSlotViewModel(IShopSerivce shopService)
        {
            _shopService = shopService;
        }
        #endregion
        #region Properties
        private ProductModel _product;
        public ProductModel Product
        {
            get { return _product; }
            set
            {
                if (_product != value)
                {
                    _product = value;
                    RaisePropertyChanged(nameof(Product));
                }
            }
        }
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
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    RaisePropertyChanged(nameof(Quantity));
                }
            }
        }

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
        private RelayCommand _addCartCommand;
        public RelayCommand AddCartCommand
        {
            get
            {
                if (_addCartCommand == null)
                {
                    _addCartCommand = new RelayCommand(AddCartAction);
                }
                return _addCartCommand;
            }
            set { _addCartCommand = value; }
        }
        #endregion
        #region Public Method
        public static BitmapImage GetProductImage(string name)
        {
            try
            {
                string pathString = $"pack://application:,,,/Images/{name}.png";
                return new BitmapImage(new Uri(pathString, UriKind.RelativeOrAbsolute));
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);

                return null;
            }
        }
        #endregion
        #region Private Method
        private void AddCountAction()
        {

            //進行UI顯示的處理
            if (Product.Quantity >= 0)
            {
                Product.Quantity += 1;
                UpdateCount();
                RaisePropertyChanged(nameof(Product));
            }
        }
        private void UpdateCount()
        {
            Product.UpdateTotalPrice();
        }
        private void AddCartAction()
        {
            _shopService.AddProduct(Product);
        }

        private void DecressCountAction()
        {
            //進行UI顯示的處理
            if (Product.Quantity > 1)
            {
                Product.Quantity -= 1;
                UpdateCount();
                RaisePropertyChanged(nameof(Product));
            }
        }
        #endregion
    }
}
