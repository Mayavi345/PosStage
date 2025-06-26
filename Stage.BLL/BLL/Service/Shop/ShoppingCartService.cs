using System.Collections.Generic;
using PosStage.MVVM.Models.Implement_Model;

namespace PosStage
{
    public interface IShoppingCartService
    {
        void AddItem(IOrderProduct itemName, int quantity);
        void ClearCart();
        bool ContainItem(IOrderProduct itemName);
        List<IOrderProduct> GetCartContents();
        int GetTotalItems();
        double GetTotalPrice();
        void RemoveItem(IOrderProduct itemName);
        void UpdateItem(IOrderProduct itemName, int newQuantity);
    }

    public class ShoppingCartService : IShoppingCartService
    {
        private List<IOrderProduct> _cartItems;

        public ShoppingCartService()
        {
            _cartItems = new List<IOrderProduct>();
        }

        public void AddItem(IOrderProduct itemName, int quantity)
        {
            //攤平資料，原本的數量需要攤平顯示，因此數量改為1
            itemName.Quantity = 1;
            _cartItems.Add(itemName);

        }
        public bool ContainItem(IOrderProduct itemName)
        {
            var item = _cartItems.FirstOrDefault(itemName);
            if (item != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public void RemoveItem(IOrderProduct itemName)
        {
            if (ContainItem(itemName))
            {
                _cartItems.Remove(itemName);
            }

        }

        public void UpdateItem(IOrderProduct itemName, int newQuantity)
        {
            var item = _cartItems.FirstOrDefault(itemName);
            if (item != null)
            {
                item.Quantity = newQuantity;
            }

        }

        public List<IOrderProduct> GetCartContents()
        {
            return new List<IOrderProduct>(_cartItems);
        }
        public double GetTotalPrice()
        {
            double totalPrice = 0;
            foreach (var item in _cartItems)
            {
                totalPrice += item.Price;
            }
            return totalPrice;
        }
        public int GetTotalItems()
        {
            int totalItems = 0;
            foreach (var item in _cartItems)
            {
                totalItems += item.Quantity;
            }
            return totalItems;
        }

        public void ClearCart()
        {
            _cartItems.Clear();
        }
    }
}
