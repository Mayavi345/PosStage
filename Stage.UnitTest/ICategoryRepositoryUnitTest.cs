using Stage.DAL.Repositories.Implement;
using System.Transactions;
using PosStage.DAL;
using PosStage.MVVM.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Stage.UnitTest
{
    public class ICategoryRepositoryUnitTest
    {
        private ICategoryRepository _categoryRepository;
        PosDbContext _posDbContext;
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction;
        [SetUp]
        public void Setup()
        {
            _posDbContext = new PosDbContext();
            _categoryRepository = new CategoryRepositoryEF();
        }

        [TearDown]
        public void TearDown()
        {

        }


        [Test]
        public void Add_AddsNewCategory()
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            //    // Arrange: 建立一個新的類別
            //    var newCategory = new PB3003_Categories { CategoryName = "Id1", IsEnable = true, IsDelete = false };
            //    // Act: 呼叫 Add 方法
            //    _categoryRepository.Add(newCategory);

            //    // Assert: 驗證結果，例如可以查詢資料庫，確認新類別已新增
            //    var addedCategory = _categoryRepository.Find(newCategory.CategoryId);
            //    Assert.IsNotNull(addedCategory);
            //    Assert.AreEqual("Id1", addedCategory.CategoryName);
            //}
        }

        [Test]
        public void Update_UpdatesCategory()
        {

            using (TransactionScope scope = new TransactionScope())
            {
                // Arrange: 建立一個現有的類別並加入資料庫
                var newCategory = new PB3003_Categories { CategoryName = "Id1", IsEnable = true, IsDelete = false };
                _categoryRepository.Add(newCategory);


                // 修改類別的屬性
                newCategory.CategoryName = "Updated Name";

                // Act: 呼叫 Update 方法
                _categoryRepository.Update(newCategory);

                // Assert: 驗證結果，例如查詢資料庫，確認類別已更新
                //var updatedCategory = _categoryRepository.Find(1);
                var updatedCategory = _posDbContext.PB3003_Categories.Where(x => x.CategoryName == "Updated Name").FirstOrDefault();
                Assert.IsNotNull(updatedCategory);
                Assert.AreEqual("Updated Name", updatedCategory.CategoryName);
            }
        }

        [Test]
        public void Delete_MarksCategoryAsDeleted()
        {
            //using (TransactionScope scope = new TransactionScope())
            //{
            //    /// Arrange: 建立一個現有的未刪除類別並新增到資料庫
            //    var newCategory = new PB3003_Categories {CategoryName = "Id1", IsEnable = true, IsDelete = false };
            //    _categoryRepository.Add(newCategory);

            //    // Act: 呼叫 Delete 方法
            //    _categoryRepository.Delete(newCategory);

            //    // Assert: 驗證結果，例如查詢資料庫，確認類別已標記為已刪除
            //    var deletedCategory = _categoryRepository.Find(1);
            //    Assert.IsNotNull(deletedCategory);
            //    Assert.IsTrue(deletedCategory.IsDelete);
            //}
        }
    }
}