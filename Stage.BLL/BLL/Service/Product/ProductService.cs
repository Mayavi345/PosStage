using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stage.DAL.Repositories.Implement;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Utilities.Nlog;
using Stage.Data.Models.Implement_Model;
using Utilities;
using Azure;

namespace Stage.BLL.BLL.Service
{
    public interface IProductService
    {
        #region ProductModel
        void Add(ProductModel productModel);
        void Update(ProductModel model);
        void DeleteProductById(ProductModel model);
        List<ProductModel> GetAllProducts(bool ShowEnable);
        List<ProductModel> GetSelectProducts(int[] productId);
        int GetNewProductId();
        #endregion
        #region Categorie
        List<string> GetCategorieNameList();
        List<CategoryModel> GetCategories(bool IsOnlyShowEnable);
        List<ProductModel> GetProductsByCategoryName(string selectedCategory);
        void Add(CategoryModel addProductViewModel);
        void Update(CategoryModel model);
        void Delete(CategoryModel para);
        #endregion
        #region ImageData
        int Add(PB4001_ImageData item);
        IResponseModel<PB4001_ImageData> GetImage(int Name);

        IResponseModel<List<PB4001_ImageData>> GetImageList();
        #endregion
    }
    public class ProductService : IProductService
    {
        private IProductsRepository _productsRepository;
        private IProductDetailRepository _productDetailRepository;
        private ICategoryRepository _categoryRepository;
        //TODO 整理
        private IProductImageRepository _productImageRepository;
        public ProductService(IProductsRepository productsRepository, IProductDetailRepository productDetailRepository, ICategoryRepository categoryRepository)
        {
            _productsRepository = productsRepository;
            _productDetailRepository = productDetailRepository;
            _categoryRepository = categoryRepository;
            _productImageRepository = new ProductImageRepository();
        }
        #region ProductModel
        List<ProductModel> IProductService.GetAllProducts(bool ShowEnable)
        {
            try
            {
                return _productsRepository.GetAllProducts(ShowEnable);

            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);

                throw;
            }
        }
        public List<ProductModel> GetSelectProducts(int[] productId)
        {
            try
            {
                return _productsRepository.GetSelectProducts(productId);
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);

                throw;
            }
        }    
        void IProductService.Add(ProductModel productModel)
        {
            _productsRepository.AddNewProductAndDetail(productModel.ConvertProduct(), productModel.ConvertProductDetail());
        }
        int IProductService.GetNewProductId()
        {
            return _productsRepository.GetNewProductId();
        }

        void IProductService.Update(ProductModel model)
        {
            _productDetailRepository.UpdateProductDetail(model.ConvertProductDetail());
            _productsRepository.UpdateProduct(model.ConvertProduct()); ;

        }

        void IProductService.DeleteProductById(ProductModel model)
        {
            _productDetailRepository.DeleteProductById(model.ConvertProductDetail().ProductId);
            _productsRepository.DeleteProductById(model.ConvertProduct().ProductId); ;
        }
        #endregion
        #region Categorie
        List<string> IProductService.GetCategorieNameList()
        {
            return _productsRepository.GetCategorieNameList();
        }

        List<ProductModel> IProductService.GetProductsByCategoryName(string selectedCategoryName)
        {
            return _productsRepository.GetProductsByCategoryName(selectedCategoryName);
        }
        List<CategoryModel> IProductService.GetCategories(bool IsOnlyShowEnable)
        {
            List<CategoryModel> categoryModelList = new List<CategoryModel>();
            var tempList = _categoryRepository.GetCategories(IsOnlyShowEnable);
            foreach (var item in tempList)
            {
                CategoryModel categoryModel = new CategoryModel(item);
                categoryModelList.Add(categoryModel);
            }
            return categoryModelList;
        }
        void IProductService.Add(CategoryModel item)
        {
            _categoryRepository.Add(item.ConvertToDBCategories());
        }

        void IProductService.Update(CategoryModel item)
        {
            _categoryRepository.Update(item.ConvertToDBCategories());
        }

        void IProductService.Delete(CategoryModel para)
        {
            _categoryRepository.Delete(para.ConvertToDBCategories());
        }
        #endregion
        #region ImageData
        int IProductService.Add(PB4001_ImageData item)
        {
            return _productImageRepository.AddImages(item);
        }

        IResponseModel<PB4001_ImageData> IProductService.GetImage(int id)
        {
            IResponseModel<PB4001_ImageData> response = new ResponseModel<PB4001_ImageData>();
            try
            {
                var file = _productImageRepository.GetImage(id);
                if (file != null)
                {
                    response.Data = file;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = null;
                    response.IsSuccess = false;
                    response.Message = "Image is null";
                }
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.IsSuccess = false;
                response.Message = e.Message;
                return response;
            }

        }

        IResponseModel<List<PB4001_ImageData>> IProductService.GetImageList()
        {
            IResponseModel<List<PB4001_ImageData>> response = new ResponseModel<List<PB4001_ImageData>>();
            try
            {
                var fileList = _productImageRepository.GetImageList();
                if (fileList != null)
                {
                    response.Data = fileList;
                    response.IsSuccess = true;
                }
                else
                {
                    response.Data = null;
                    response.IsSuccess = false;
                    response.Message = "Image List is null";
                }
                return response;
            }
            catch (Exception e)
            {
                response.Data = null;
                response.IsSuccess = false;
                response.Message = e.Message;
                return response;
            }
        }
        #endregion
    }

}
