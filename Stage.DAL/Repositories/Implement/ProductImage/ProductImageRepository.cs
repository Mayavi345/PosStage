using Microsoft.EntityFrameworkCore;
using PosStage.DAL;
using PosStage.MVVM.Models;

namespace Stage.DAL.Repositories.Implement
{
    public interface IProductImageRepository
    {
        int AddImages(PB4001_ImageData item);
        void DeleteProductImagesByProductId(int productId);
        PB4001_ImageData GetImage(int id);
        List<PB4001_ImageData> GetImageList();
    }

    public class ProductImageRepository : IProductImageRepository
    {
        private readonly PosDbContext _dbContext;

        public ProductImageRepository()
        {
            _dbContext = new PosDbContext();
        }

        public int AddImages(PB4001_ImageData item)
        {

            if (_dbContext.PB4001_ImageData.Any(x => x.FileName == item.FileName))
            {
                var currentitem = _dbContext.PB4001_ImageData.Where(x => x.FileName == item.FileName).FirstOrDefault();
                return currentitem.Id;
            }
            else
            {
                // 如果有指定 Id 且 DB 已存在，就當作 Update
                if (item.Id != 0 && _dbContext.PB4001_ImageData.Any(x => x.Id == item.Id))
                {
                    _dbContext.PB4001_ImageData.Update(item);
                }
                else
                {
                    // 先從 DB 拿目前最大的 Id，若沒資料就回傳 0
                    var maxId = _dbContext.PB4001_ImageData
                                          .Select(x => (int?)x.Id)
                                          .Max() ?? 0;

                    // 給予下一個 Id
                    item.Id = maxId + 1;
                    _dbContext.PB4001_ImageData.Add(item);
                }
                _dbContext.SaveChanges();
                return item.Id;
            }
        }


        public void DeleteProductImagesByProductId(int productId)
        {
            var productImages = _dbContext.PB4001_ImageData.Where(img => img.Id == productId);
            foreach (var img in productImages)
                _dbContext.PB4001_ImageData.Remove(img);
            _dbContext.SaveChanges();
        }
        PB4001_ImageData IProductImageRepository.GetImage(int id)
        {
            return _dbContext.PB4001_ImageData.Where(p => p.Id == id).FirstOrDefault();
        }
        public List<PB4001_ImageData> GetImageList()
        {
            return _dbContext.PB4001_ImageData.ToList();
        }
    }

}
