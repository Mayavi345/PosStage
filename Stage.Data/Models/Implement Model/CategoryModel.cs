using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage.Data.Models.Implement_Model
{
    public class CategoryModel : IComboBoxGenericItem<CategoryModel>
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsEnable { get; set; }
        public int OrderId { get; set; }
        #region IComboBoxGenericItem
        public string DisplayValue { get => CategoryName; set { CategoryName = value; } }
        public CategoryModel Value { get => this; set { } }
        #endregion
        private PB3003_Categories _pB3003_Categories;
        public CategoryModel(PB3003_Categories pB3003_Categories)
        {
            _pB3003_Categories = pB3003_Categories;
            CategoryId = pB3003_Categories.CategoryId;
            CategoryName = pB3003_Categories.CategoryName;
            IsEnable = pB3003_Categories.IsEnable;
            OrderId = pB3003_Categories.OrderId ?? 0;
        }
        public CategoryModel()
        {
            _pB3003_Categories = new PB3003_Categories();
        }
        public PB3003_Categories ConvertToDBCategories()
        {
            _pB3003_Categories.CategoryId = CategoryId;
            _pB3003_Categories.CategoryName = CategoryName;
            _pB3003_Categories.IsEnable = IsEnable;
            _pB3003_Categories.IsDelete = false;
            _pB3003_Categories.OrderId = OrderId;
            return _pB3003_Categories;
        }
        public void MapData(CategoryModel productModel)
        {
            CategoryId = productModel.CategoryId;
            CategoryName = productModel.CategoryName;
            IsEnable = productModel.IsEnable;
            OrderId = productModel.OrderId;
        }
    }
}
