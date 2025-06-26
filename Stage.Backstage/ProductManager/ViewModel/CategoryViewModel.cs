using GalaSoft.MvvmLight;
using Stage.BLL.BLL;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PosStage.MVVM.Models.Implement_Model;
using PosStage.MVVM.Models;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Windows;
using Microsoft.Extensions.ObjectPool;
using Utilities;
using Stage.Data.Models.Implement_Model;
using Utilities.Nlog;
using Stage.Backstage.ProductManager.ViewModel;
using Stage.BLL.BLL.Service;
using UIComponent;

namespace Stage.Backstage
{
    public class CategoryViewModel : ObservableObject
    {
        private IProductService _productsService;
        private static DefaultObjectPool<CategoryModel> categoriesPool;
        private CategoryModel _emptyCategory;

        public CategoryViewModel()
        {
            this._productsService = MainSystemService.Instance.ProductService;

            _emptyCategory = new CategoryModel();
            AddCategorySingleViewModel = new CategorySingleViewModel();
            EditCategorySingleViewModel = new CategorySingleViewModel();
            InitPool();

            AddCommand = new RelayCommandBase(AddCommandAction).Command;
            EditCommand = new RelayCommandBase(EditCommandAction).Command;
            UpdateCommand = new RelayCommandBase(UpdateCommandAction).Command;
            UpdateCancelCommand = new RelayCommandBase(UpdateCancelCommandAction).Command;
            DeleteCommand = new RelayCommandBase(DeleteCommandAction).Command;
        }
        public void Init()
        {
            RefreshGrid();
        }
        private void InitPool()
        {
            var policy = new DefaultPooledObjectPolicy<CategoryModel>();
            categoriesPool = new DefaultObjectPool<CategoryModel>(policy);
        }

        #region Properties

        private ObservableCollection<CategoryModel> _categoryDGList;
        public ObservableCollection<CategoryModel> CategoryDGList
        {
            get { return _categoryDGList; }
            set
            {
                if (_categoryDGList != value)
                {
                    _categoryDGList = value;
                    RaisePropertyChanged(nameof(CategoryDGList));
                }
            }
        }
        private CategorySingleViewModel _addCategorySingleViewModel;
        public CategorySingleViewModel AddCategorySingleViewModel
        {
            get { return _addCategorySingleViewModel; }
            set
            {
                if (_addCategorySingleViewModel != value)
                {
                    _addCategorySingleViewModel = value;
                    RaisePropertyChanged(nameof(AddCategorySingleViewModel));
                }
            }
        }
        private CategorySingleViewModel _editCategorySingleViewModel;
        public CategorySingleViewModel EditCategorySingleViewModel
        {
            get { return _editCategorySingleViewModel; }
            set
            {
                if (_editCategorySingleViewModel != value)
                {
                    _editCategorySingleViewModel = value;
                    RaisePropertyChanged(nameof(EditCategorySingleViewModel));
                }
            }
        }
        //private CategoryModel _addCategory;
        //public CategoryModel AddCategory
        //{
        //    get { return _addCategory; }
        //    set
        //    {
        //        if (_addCategory != value)
        //        {
        //            _addCategory = value;
        //            RaisePropertyChanged(nameof(AddCategory));
        //        }
        //    }
        //}
        //private CategoryModel _editCategory;

        //public CategoryModel EditCategory
        //{
        //    get { return _editCategory; }
        //    set
        //    {
        //        if (_editCategory != value)
        //        {
        //            _editCategory = value;
        //            RaisePropertyChanged(nameof(EditCategory));
        //        }
        //    }
        //}
        private bool _isUpdateEditEnable;
        public bool IsUpdateEditEnable
        {
            get { return _isUpdateEditEnable; }
            set
            {
                if (_isUpdateEditEnable != value)
                {
                    _isUpdateEditEnable = value;
                    RaisePropertyChanged(nameof(IsUpdateEditEnable));
                }
            }
        }
        #endregion

        #region Command
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateCancelCommand { get; }

        #endregion
        public void RefreshGrid()
        {
            CategoryDGList = new ObservableCollection<CategoryModel>(_productsService.GetCategories(false));
        }
        #region Internal

        private void AddCommandAction(object obj)
        {
            if (!_addCategorySingleViewModel.CheckIsValidate())
            {
                MainSystemService.Instance.ShowMessageBox(TextResourceCenter.FiledValidateFail);
                return;
            }
            else
            {
                var addViewModel = obj as CategoryModel;
                if (addViewModel.CategoryName == string.Empty)
                    MessageBox.Show(TextResourceCenter.DataIsNotAllowNull);
                else
                {
                    //addProductViewModel.IsDelete = false;
                    _productsService.Add(addViewModel);
                    RefreshGrid();
                }
                CategoryModel tempPoolObj = GetEmptyObj();
                AddCategorySingleViewModel.CategoryModel = tempPoolObj;
                MainDataCenter.Instance.RefreshProductView.NotifyObservers(() => { });
            }
        }

        private CategoryModel GetEmptyObj()
        {
            var tempPoolObj = categoriesPool.Get();
            tempPoolObj.MapData(_emptyCategory);
            return tempPoolObj;
        }

        private void EditCommandAction(object obj)
        {
            try
            {
                var para = obj as CategoryModel;
                var tempPoolObj = categoriesPool.Get();

                IsUpdateEditEnable = true;

                tempPoolObj.CategoryId = para.CategoryId;
                tempPoolObj.CategoryName = para.CategoryName;
                tempPoolObj.IsEnable = para.IsEnable;


                EditCategorySingleViewModel.InitSingleViewData(tempPoolObj);
                categoriesPool.Return(tempPoolObj);

            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        private void UpdateCommandAction(object obj)
        {

            try
            {
                if (!EditCategorySingleViewModel.CheckIsValidate())
                {
                    MainSystemService.Instance.ShowMessageBox(TextResourceCenter.FiledValidateFail);
                    return;
                }
                else
                {
                    var para = obj as CategoryModel;
                    IsUpdateEditEnable = false;

                    _productsService.Update(para);
                    RefreshGrid();

                    var tempPoolObj = categoriesPool.Get();
                    tempPoolObj.MapData(_emptyCategory);
                    EditCategorySingleViewModel.CategoryModel = tempPoolObj;
                    MainDataCenter.Instance.RefreshProductView.NotifyObservers(() => { });
                }
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        private void UpdateCancelCommandAction(object obj)
        {
            CategoryModel tempPoolObj = GetEmptyObj();
            EditCategorySingleViewModel.CategoryModel = tempPoolObj;

            IsUpdateEditEnable = false;
        }

        private void DeleteCommandAction(object obj)
        {
            UIMessageBoxOKCancel.Instance.ShowDialog(() =>
            {
                var para = obj as CategoryModel;
                _productsService.Delete(para);
                RefreshGrid();
                MainDataCenter.Instance.RefreshProductView.NotifyObservers(() => { });
            }, () => { });
        }

        #endregion
    }

}
