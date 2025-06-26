using Stage.Data.Models.Implement_Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PosStage.MVVM.Models.Implement_Model
{
    public class ProductModel : IOrderProduct
    {
        public int ProductId { get; set; }
        public double Price { get; set; }
        /// <summary>
        /// 這裡的數量是庫存數量
        /// </summary>
        public int Count { get; set; }
        public int State { get; set; }
        public string Name { get; set; }
        public CategoryModel Categories { get; set; }
        public int ImageId { get; set; }
        public int OrderId { get; set; }
        public double ToltalPrice { get; set; }
        public int Quantity { get; set; }


        public ProductModel()
        {
        }
        public ProductModel(ProductModel productModel)
        {
            MapData(productModel);
        }
        public void UpdateTotalPrice()
        {
            ToltalPrice = Price * Quantity;
        }

        public void MapData(ProductModel productModel)
        {
            ProductId = productModel.ProductId;
            Price = productModel.Price;
            Count = productModel.Count;
            State = productModel.State;
            Name = productModel.Name;
            Categories = productModel.Categories;
            ImageId = productModel.ImageId;
            ToltalPrice = productModel.ToltalPrice;
            Quantity = productModel.Quantity;
        }

        private PB3001_Product _product = new PB3001_Product();
        public PB3001_Product ConvertProduct()
        {
            _product.ProductId = ProductId;
            _product.Price = Price;
            _product.Count = Count;
            _product.State = State;
            _product.OrderId = OrderId;
            return _product;
        }
        private PB3002_ProductDetail _productDetail = new PB3002_ProductDetail();

        public PB3002_ProductDetail ConvertProductDetail()
        {
            _productDetail.ProductId = ProductId;
            _productDetail.Name = Name;
            _productDetail.CategoryId = Categories.CategoryId;
            _productDetail.ImageId = ImageId;
            return _productDetail;
        }
        public static ProductModel MapProductModel(PB3001_Product products, PB3002_ProductDetail detail, PB3003_Categories categories)
        {
            return new ProductModel
            {
                ProductId = products.ProductId,
                Price = products.Price,
                Count = products.Count,
                State = products.State,
                OrderId = products.OrderId??0,
                Name = detail.Name,
                Categories = new CategoryModel(categories),
                ImageId = detail.ImageId ?? 0,
            };
        }
        public static ProductModel MapProductModel(PB3001_Product products, PB3002_ProductDetail detail, PB1002_CompletedOrdersDetails orders, PB3003_Categories categories)
        {
            return new ProductModel
            {
                ProductId = products.ProductId,
                Price = orders.Price,
                Count = products.Count,
                State = products.State,
                Name = detail.Name,
                Categories = new CategoryModel(categories),
                ImageId = detail.ImageId ?? 0,
                OrderId = products.OrderId ?? 0,
                Quantity = orders.Quantity,
                ToltalPrice = orders.Price * orders.Quantity
            };
        }
    }
}
