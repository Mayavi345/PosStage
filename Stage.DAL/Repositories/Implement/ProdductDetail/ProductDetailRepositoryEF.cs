using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PosStage.DAL;
using PosStage.MVVM.Models;
using Utilities.Nlog;

namespace Stage.DAL.Repositories.Implement
{
    public interface IProductDetailRepository
    {
        void AddNewProduct(PB3002_ProductDetail item);
        void DeleteProductById(int item);
        List<PB3002_ProductDetail> GetAllItems();
        List<int> GetCategories();
        PB3002_ProductDetail GetItemById(int id);
        void UpdateProductDetail(PB3002_ProductDetail changedItem);
    }
    public class ProductDetailRepositoryEF : IProductDetailRepository
    {
        private PosDbContext _dbContext;
        public List<PB3002_ProductDetail> productDatilList;

        public ProductDetailRepositoryEF()
        {
            //TODO 改DI
            _dbContext = new PosDbContext();
            productDatilList = new List<PB3002_ProductDetail>();
        }
        public List<PB3002_ProductDetail> GetAllItems()
        {
            return productDatilList = _dbContext.PB3002_ProductDetail.ToList();
        }
        public PB3002_ProductDetail GetItemById(int id)
        {
            return _dbContext.PB3002_ProductDetail.Find(id);
        }
        public List<int> GetCategories()
        {
            return _dbContext.PB3002_ProductDetail.Select(x => x.CategoryId).Distinct().ToList();
        }

        public void AddNewProduct(PB3002_ProductDetail item)
        {
            _dbContext.PB3002_ProductDetail.Add(item);
            _dbContext.SaveChanges();
        }
        public void DeleteProductById(int id)
        {
            try
            {
                var currentItem = _dbContext.PB3002_ProductDetail.Where(x => x.ProductId == id).ToList().FirstOrDefault();
                _dbContext.PB3002_ProductDetail.Update(currentItem);
                _dbContext.SaveChanges();

            }
            catch(Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw;
            }
        }

        public void UpdateProductDetail(PB3002_ProductDetail changedItem)
        {
            var existingProductDetail = _dbContext.PB3002_ProductDetail.Where(x => x.ProductId == changedItem.ProductId).FirstOrDefault();
            if (existingProductDetail != null)
            {
                MapProductDetail(changedItem, existingProductDetail);
                _dbContext.PB3002_ProductDetail.Update(existingProductDetail);
                _dbContext.SaveChanges();
            }

        }

        private static void MapProductDetail(PB3002_ProductDetail changedItem, PB3002_ProductDetail? existingProductDetail)
        {
            existingProductDetail.ProductId = changedItem.ProductId;
            existingProductDetail.Name = changedItem.Name;
            existingProductDetail.CategoryId = changedItem.CategoryId;
            existingProductDetail.ImageId = changedItem.ImageId;
        }
    }
}
