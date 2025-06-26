using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosStage.DAL;
using PosStage.MVVM.Models;

namespace Stage.DAL.Repositories.Implement
{
    public interface ICategoryRepository
    {
        void Add(PB3003_Categories item);
        void Delete(PB3003_Categories para);
        PB3003_Categories Find(int categoryId);
        List<PB3003_Categories> GetAllCategories();
        List<PB3003_Categories> GetCategories(bool IsOnlyShowEnable = true);

        void Update(PB3003_Categories item);
    }
    public class CategoryRepositoryEF : ICategoryRepository
    {
        private PosDbContext _dbContext;
        public CategoryRepositoryEF()
        {
            _dbContext = new PosDbContext();
        }
        public List<PB3003_Categories> GetAllCategories()
        {
            return _dbContext.PB3003_Categories.Where(x => x.IsDelete == false).ToList();
        }
        public void Add(PB3003_Categories item)
        {
            _dbContext.PB3003_Categories.Add(item);
            _dbContext.SaveChanges();
        }

        void ICategoryRepository.Update(PB3003_Categories item)
        {
            PB3003_Categories? currentItem = _dbContext.PB3003_Categories.Where(x => x.CategoryId == item.CategoryId).FirstOrDefault();
            currentItem.CategoryName = item.CategoryName;
            currentItem.IsEnable = item.IsEnable;
            currentItem.OrderId = item.OrderId;
            _dbContext.PB3003_Categories.Update(currentItem);
            _dbContext.SaveChanges();
        }
        void ICategoryRepository.Delete(PB3003_Categories item)
        {
            var currentItem = _dbContext.PB3003_Categories.Where(x => x.CategoryId == item.CategoryId).FirstOrDefault();
            currentItem.IsDelete = true;
            _dbContext.PB3003_Categories.Update(currentItem);
            _dbContext.SaveChanges();
        }

        PB3003_Categories ICategoryRepository.Find(int categoryId)
        {
            return _dbContext.PB3003_Categories.Where(x => x.CategoryId == categoryId).FirstOrDefault();

        }

        List<PB3003_Categories> ICategoryRepository.GetCategories(bool IsOnlyShowEnale)
        {
            if (IsOnlyShowEnale)
            {
                return _dbContext.PB3003_Categories.Where(x => x.IsDelete == false && x.IsEnable == true)
                    .OrderBy(x => !x.OrderId.HasValue)
                    .ThenBy(x => x.OrderId)
                    .ToList();
            }
            else
            {
                return _dbContext.PB3003_Categories.Where(x => x.IsDelete == false)
                     .OrderBy(x => !x.OrderId.HasValue)
                    .ThenBy(x => x.OrderId)
                    .ToList();

            }
        }
    }
}
