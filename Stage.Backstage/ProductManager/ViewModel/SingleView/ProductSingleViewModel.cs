using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.ObjectPool;
using PosStage.MVVM.Models.Implement_Model;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.Data.Models.Implement_Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UIComponent;
using Utilities;
using Utilities.Nlog;
using PosStage.MVVM.Models;
using Stage.Backstage.Common;

namespace Stage.Backstage.ProductManager.ViewModel
{
    public class ProductSingleViewModel : SingleViewFormViewModelBase
    {
        private IProductService _productService;
        private IProductSettingManager _productSettingManager => MainDataCenter.Instance.ProductSettingManager;

        public ProductSingleViewModel()
        {
            _productService = MainSystemService.Instance.ProductService;
            ProductModel = new ProductModel();

            OpenFileDialog = new RelayCommand(OpenFileDialogAction);
            ImageData = ImageSourceProcess.Instance.ImageEmpty;
        }


        public void InitData()
        {
            if (!_productSettingManager.CategoryList.Any())
                _productSettingManager.LoadData();
            CategoryList = _productSettingManager.CategoryList;
            //更新ComboBox
            SelectedItem = CategoryList[0];
        }

        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入名稱")]
        public string Name
        {
            get { return ProductModel.Name; }
            set
            {
                ProductModel.Name = value;
                _errorMessageHelper.ClearErrors(this, "Name");
                RaisePropertyChanged(nameof(Name));
            }
        }
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入價格")]
        public double Price
        {
            get { return ProductModel.Price; }
            set
            {
                ProductModel.Price = value;
                RaisePropertyChanged(nameof(Price));
            }
        }
        [Required(ErrorMessage = TextResourceCenter.WarmTextEmoji + "請輸入數量")]
        public int Count
        {
            get { return ProductModel.Quantity; }
            set
            {
                ProductModel.Quantity = value;
                RaisePropertyChanged(nameof(Count));

            }
        }
        public ProductModel ProductModel
        {
            get { return _productSettingManager.SelectProductModel; }
            set
            {
                _productSettingManager.SetSelectProductModel(value);
                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(Price));
                RaisePropertyChanged(nameof(Count));

                RaisePropertyChanged(nameof(ProductModel));
            }
        }
        private IComboBoxGenericItem<CategoryModel> _selectedItem;
        public IComboBoxGenericItem<CategoryModel> SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                _productSettingManager.SelectProductModel.Categories = _selectedItem.Value;
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }
        private ObservableCollection<IComboBoxGenericItem<CategoryModel>>? _categoryList;
        public ObservableCollection<IComboBoxGenericItem<CategoryModel>>? CategoryList
        {
            get { return _categoryList; }
            set
            {
                _categoryList = value;
                RaisePropertyChanged(nameof(CategoryList));
            }
        }

        public ICommand OpenFileDialog { get; set; }
        private ImageSource _imageData;
        public ImageSource ImageData
        {
            get { return _imageData; }
            set
            {
                _imageData = value;
                RaisePropertyChanged(nameof(ImageData));
            }
        }
        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChanged(nameof(ImagePath));
            }
        }
        private void OpenFileDialogAction()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "Choose your photo";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.apng;*.avif;*.gif;*.jfif;*.pjpeg";
            openFileDialog.ShowDialog();
            ImagePath = openFileDialog.FileName;
            ImageData = ImageSourceProcess.GetImageSource(openFileDialog.FileName);
            // ProductModel.ProductId = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
            RaisePropertyChanged(nameof(ProductModel));
        }
        public bool AddImage(out int number)
        {
            if (ImagePath != null)
            {
                number = AddImages(new string[] { ImagePath });
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }
        private int AddImages(string[] pathes)
        {
            int id = 0;
            if (pathes.Length > 0)
            {
                // byte[] buff;
                for (int i = 0; i < pathes.Length; i++)
                {
                    if (File.Exists(pathes[i]))
                    {
                        byte[] imageByte = ImageSourceProcess.GetImageByte(pathes[i]);
                        var tempProductImage = new PB4001_ImageData
                        {
                            FileName = Path.GetFileNameWithoutExtension(pathes[i]),
                            FileExtension = Path.GetExtension(pathes[i]),
                            Image = imageByte,
                            Size = imageByte.Length,
                        };
                        ImageData = ImageSourceProcess.GetBitmapImage(imageByte);
                        id = _productService.Add(tempProductImage);
                        ImageSourceProcess.Instance.AddNewImage(id, ImageData);
                    }
                }
            }
            ProductModel.ImageId = id;
            return id;
        }
        public override List<Action> ValidateActions { get; set; }

        public override void ClearData()
        {
            ProductModel = _productSettingManager.EmptyProductModel;
            SelectedItem = CategoryList[0];
            _errorMessageHelper.ClearAllError();
            ImageData = ImageSourceProcess.Instance.ImageEmpty;
        }

        public override void InitSingleViewData<T>(T data)
        {
            try
            {
                //var productModelPoolObj = pool.Get();
                //productModelPoolObj.MapData(selectProductModel);
                //ProductModel = productModelPoolObj;
                InitData();
                ProductModel = _productSettingManager.SelectProductModel;
                SelectedItem = _productSettingManager.GetCurrentCategory();
                ImageData = GetImage(ProductModel.ImageId);
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }

        public ImageSource GetImage(int id)
        {
            var respoen = _productService.GetImage(id);
            if (respoen.IsSuccess)
            {
                PB4001_ImageData imageData = respoen.Data;
                return ImageSourceProcess.GetBitmapImage(imageData.Image);
            }
            else
            {
                return ImageSourceProcess.Instance.ImageEmpty;
            }

        }
        public override void InitValidate()
        {
            ValidateActions = new List<Action>();
            ValidateActions.Add(() =>
            {
                _errorMessageHelper.ValidateProperty(Name, "Name", this);
            });
            ValidateActions.Add(() =>
            {
                _errorMessageHelper.ValidateProperty(Price, "Price", this);
            });
            ValidateActions.Add(() =>
            {
                _errorMessageHelper.ValidateProperty(Count, "Count", this);
            });
        }

    }

}
