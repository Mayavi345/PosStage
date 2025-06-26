using Stage.BLL.BLL.Service;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Data.Models.Implement_Model;
using System.Collections.ObjectModel;
using Microsoft.Extensions.ObjectPool;

namespace Stage.BLL.BLL
{
    public interface IProductSettingManager
    {
        void SetSelectProductModel(ProductModel productModel);
        ProductModel SelectProductModel { get; }
        IComboBoxGenericItem<CategoryModel> GetCurrentCategory();
        void LoadData();

        ObservableCollection<IComboBoxGenericItem<CategoryModel>> CategoryList { get; }
        ProductModel EmptyProductModel { get; }
    }
    public class ProductSettingManager : IProductSettingManager
    {
        private static DefaultObjectPool<ProductModel> pool;

        ProductModel _productModelEmpty;
        private IProductService _productService;

        private ProductModel _selectProductModel;

        ProductModel IProductSettingManager.SelectProductModel => _selectProductModel;

        ObservableCollection<IComboBoxGenericItem<CategoryModel>> IProductSettingManager.CategoryList => _categoryList;

        ProductModel IProductSettingManager.EmptyProductModel => EmptyProductModel();

        private ObservableCollection<IComboBoxGenericItem<CategoryModel>> _categoryList;

        public ProductSettingManager()
        {
            _productService = MainSystemService.Instance.ProductService;
            _categoryList = new ObservableCollection<IComboBoxGenericItem<CategoryModel>>();
            _productModelEmpty = new ProductModel();
            InitPool();

        }
        void IProductSettingManager.LoadData()
        {
            _categoryList = new ObservableCollection<IComboBoxGenericItem<CategoryModel>>(GetCategoryList());
        }
        private void InitPool()
        {
            var policy = new DefaultPooledObjectPolicy<ProductModel>();
            pool = new DefaultObjectPool<ProductModel>(policy);
        }
        void IProductSettingManager.SetSelectProductModel(ProductModel productModel)
        {
            _selectProductModel = productModel;
        }

        IComboBoxGenericItem<CategoryModel> IProductSettingManager.GetCurrentCategory()
        {
            if (_selectProductModel == null)
            {
                return null;
            }
            var currentList = _categoryList.Where(x => x.DisplayValue == _selectProductModel.Categories.DisplayValue).FirstOrDefault();
            if (currentList == null)
            {
                return null;
            }
            else
            {
                return currentList;
            }
        }

        private List<CategoryModel> GetCategoryList()
        {
            return _productService.GetCategories(true);
        }

        ProductModel EmptyProductModel()
        {
            ProductModel productModel = new ProductModel(_productModelEmpty);
            return productModel;
        }

   
    }
}
