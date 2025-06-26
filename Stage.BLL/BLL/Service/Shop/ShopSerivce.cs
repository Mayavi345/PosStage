using Stage.BLL;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosStage;
using PosStage.MVVM.Models.Implement_Model;

namespace Stage.BLL.BLL.Service
{
    public class ShopSerivce : IShopSerivce
    {
        #region Field
        private IShoppingCartService _shoppingCart { get; set; }
        /// <summary>
        /// 當前選擇的資料
        /// </summary>
        private IOrderProduct _selectedProduct;
        #endregion
        #region Constructor
        public ShopSerivce()
        {
            _shoppingCart = MainDataCenter.Instance.ShoppingCart;
        }
        #endregion
        #region Properties
        public List<IOrderProduct> SelectedProductDic => GetCartContents();
        public int ProductCount => GetTotalItems();
        public double TotalPrice => CalculateTotalPrice();

        #endregion

        #region Public
        public void AddProduct(IOrderProduct product)
        {
            if (product.Quantity > 0)
            {
           
                for (int i = 0; i < product.Quantity; i++)
                {
                    _selectedProduct = product;
                    var currentProduc = product;
                    _shoppingCart.AddItem(currentProduc, 1);
                }

                //if (_shoppingCart.ContainItem(currentProduc))
                //{
                //    _shoppingCart.UpdateItem(currentProduc, product.Quantity);
                //}
                //else
                //{
                //    _shoppingCart.AddItem(currentProduc, product.Quantity);
                //}
            }
            else if (product.Quantity == 0)
            {
                _selectedProduct = product;
                var currentProduc = product;
                _shoppingCart.AddItem(currentProduc, 1);
            }
            MainDataCenter.Instance.RefreshCartSubject.NotifyObservers(() => { });
        }
        public void RefrshOrderList()
        {
            MainDataCenter.Instance.RefreshCartSubject.NotifyObservers(() => { });
        }
        public void RemoveOrderProduct(IOrderProduct product)
        {
            _shoppingCart.RemoveItem(product);
            MainDataCenter.Instance.RefreshCartSubject.NotifyObservers(() => { });

        }
        public void AddOrderProduct(IOrderProduct orderProduct)
        {
            var tempCount = (orderProduct.Quantity);

            if (tempCount > 0)
            {
                var currentProduc = orderProduct;
                _shoppingCart.AddItem(currentProduc, tempCount);

                //if (_shoppingCart.ContainItem(currentProduc))
                //{
                //    _shoppingCart.UpdateItem(currentProduc, tempCount);
                //}
                //else
                //{
                //    _shoppingCart.AddItem(currentProduc, tempCount);
                //}
            }
            MainDataCenter.Instance.RefreshCartSubject.NotifyObservers(() => { });
        }
        #endregion
        #region Private Method
        private int GetTotalItems()
        {
            return _shoppingCart.GetTotalItems();
        }
        private List<IOrderProduct> GetCartContents()
        {
            return _shoppingCart.GetCartContents();
        }
        private double CalculateTotalPrice()
        {
            if (_shoppingCart.GetTotalItems() > 0)
            {
                var totalAmount = _shoppingCart.GetTotalPrice();
                return totalAmount;
            }
            else
                return 0;
        }
        #endregion      
    }
}
