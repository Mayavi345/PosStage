using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Data.Models.Implement_Model;
using Utilities;

namespace Stage.DAL.Repositories.Implement
{
    public interface IProductsRepository
    {
        void AddNewProductAndDetail(PB3001_Product product, PB3002_ProductDetail item);
        void AddNewProduct(PB3001_Product product);

        void DeleteProductById(int productId);
        int GetNewProductId();
        PB3001_Product GetProductById(int productId);
        void UpdateProduct(PB3001_Product changedProduct);
        List<ProductModel> GetAllProducts(bool ShowEnable);
        List<ProductModel> GetSelectProducts(int[] productId);
        List<string> GetCategorieNameList();
        List<ProductModel> GetProductsByCategoryName(string selectedCategoryName);
    }
    public class ProductsRepositoryEF : IProductsRepository
    {
        private PosDbContext _dbContext;

        public ProductsRepositoryEF()
        {
            //TODO 改DI
            _dbContext = new PosDbContext();
        }
        public PB3001_Product GetProductById(int productId)
        {
            return _dbContext.PB3001_Product.Find(productId);
        }


        public void AddNewProduct(PB3001_Product product)
        {
            _dbContext.PB3001_Product.Add(product);
            _dbContext.SaveChanges();
        }
        public void DeleteProductById(int id)
        {
            var currentItem = _dbContext.PB3001_Product.Where(x => x.ProductId == id).FirstOrDefault();
            currentItem.IsDelete = true;
            _dbContext.PB3001_Product.Update(currentItem);
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(PB3001_Product changedProduct)
        {
            var product = _dbContext.PB3001_Product.Find(changedProduct.ProductId);
            MapProduct(changedProduct, product);
            _dbContext.SaveChanges();
        }

        private static void MapProduct(PB3001_Product changedProduct, PB3001_Product? setValue)
        {
            setValue.Price = changedProduct.Price;
            setValue.ProductId = changedProduct.ProductId;
            setValue.Count = changedProduct.Count;
            setValue.State = changedProduct.State;
            setValue.OrderId=changedProduct.OrderId;
        }

        int IProductsRepository.GetNewProductId()
        {
            var allProducts = _dbContext.PB3001_Product.ToList();
            return allProducts.Max(x => x.ProductId) + 1;
        }

        List<ProductModel> IProductsRepository.GetAllProducts(bool ShowEnable)
        {
            var query = from products in _dbContext.PB3001_Product.AsNoTracking()
                        join detail in _dbContext.PB3002_ProductDetail.AsNoTracking() on products.ProductId equals detail.ProductId
                        join categories in _dbContext.PB3003_Categories.AsNoTracking() on detail.CategoryId equals categories.CategoryId
                        where (!ShowEnable && products.State == TextHelper.BoolToInt(true)) || ShowEnable
                        where products.IsDelete == false
                        orderby !products.OrderId.HasValue, products.OrderId
                        select ProductModel.MapProductModel(products, detail, categories);
            var results = query.ToList();

            return results;
        }
        List<ProductModel> IProductsRepository.GetSelectProducts(int[] productId)
        {
            var allProduct = from products in _dbContext.PB3001_Product
                             join detail in _dbContext.PB3002_ProductDetail on products.ProductId equals detail.ProductId
                             join categories in _dbContext.PB3003_Categories on detail.CategoryId equals categories.CategoryId
                             where productId.Contains(products.ProductId)
                             where products.IsDelete == false
                             orderby !products.OrderId.HasValue, products.OrderId
                             select ProductModel.MapProductModel(products, detail, categories);

            var results = allProduct.ToList();

            return results;
        }

        List<string> IProductsRepository.GetCategorieNameList()
        {
            var query = (from category in _dbContext.PB3003_Categories
                         join productDetail in _dbContext.PB3002_ProductDetail on category.CategoryId equals productDetail.CategoryId
                         orderby category.OrderId.HasValue
                         select category.CategoryName).Distinct().ToList();
            //return _productDetailRepository.GetCategories();
            return query;
        }

        List<ProductModel> IProductsRepository.GetProductsByCategoryName(string selectedCategoryName)
        {
            var query = from products in _dbContext.PB3001_Product
                        join detail in _dbContext.PB3002_ProductDetail on products.ProductId equals detail.ProductId
                        let categories = (from category in _dbContext.PB3003_Categories
                                          where category.CategoryName == selectedCategoryName
                                          select category).FirstOrDefault()
                        where products.State == TextHelper.BoolToInt(true)
                        where detail.CategoryId == categories.CategoryId
                        where products.IsDelete == false
                        orderby !products.OrderId.HasValue, products.OrderId
                        select ProductModel.MapProductModel(products, detail, categories);

            var results = query.ToList();
            return results;
        }

        void IProductsRepository.AddNewProductAndDetail(PB3001_Product product, PB3002_ProductDetail detailItem)
        {
            _dbContext.PB3001_Product.Add(product);
            _dbContext.SaveChanges();
            _dbContext.PB3002_ProductDetail.Add(detailItem);
            _dbContext.SaveChanges();

        }
    }

}