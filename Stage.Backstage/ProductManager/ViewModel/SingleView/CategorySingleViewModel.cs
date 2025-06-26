using PosStage.MVVM.Models.Implement_Model;
using Stage.Backstage.ViewModel;
using Stage.Data.Models.Implement_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIComponent;
using Utilities;

namespace Stage.Backstage.ProductManager.ViewModel
{
    public class CategorySingleViewModel : SingleViewFormViewModelBase
    {
        public CategorySingleViewModel()
        {
            CategoryModel = new CategoryModel();
        }

        private CategoryModel _categoryModel;
        public CategoryModel CategoryModel
        {
            get { return _categoryModel; }
            set
            {
                _categoryModel = value;
                RaisePropertyChanged(nameof(CategoryName));

                RaisePropertyChanged(nameof(CategoryModel));
            }
        }
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + TextResourceCenter.RequiredText_InputCategoryText)]
        public string CategoryName
        {
            get { return CategoryModel.CategoryName; }
            set
            {
                CategoryModel.CategoryName = value;
                RaisePropertyChanged(nameof(CategoryName));
            }
        }
        public override List<Action> ValidateActions { get; set; }

        public override void ClearData()
        {
            //TODO new優化
            CategoryModel = new CategoryModel();
            _errorMessageHelper.ClearErrors(this, "CategoryName");
        }

        public override void InitSingleViewData<T>(T data)
        {
            var tempData = data as CategoryModel;

            CategoryModel = new CategoryModel();
            CategoryModel.MapData(tempData);

            RaisePropertyChanged(nameof(CategoryName));
            RaisePropertyChanged(nameof(CategoryModel));
        }

        public override void InitValidate()
        {
            ValidateActions = new List<Action>();
            ValidateActions.Add(() =>
            {
                var value = CategoryName;
                var propertyName = "CategoryName";
                var validationContext = this;
                _errorMessageHelper.ValidateProperty(value, propertyName, validationContext);
            });
        }
    }
}
