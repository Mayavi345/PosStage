using GalaSoft.MvvmLight;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Backstage.Common;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.Data.Models.Implement_Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Stage.Backstage.ProductManager.ViewModel
{
    //TODO 現有ProductModel無法使用ImageSource，因此用這裡來處理圖片相關事務
    public class ProductDataGridSingleViewModel : ObservableObject
    {

        private ProductModel _productModel;

        public ProductDataGridSingleViewModel(ProductModel productModel)
        {
            _productModel = productModel;
            MapProductModel(productModel);
        }

        private void MapProductModel(ProductModel productModel)
        {
            ProductImage = ImageSourceProcess.Instance.GetImage(productModel.ImageId);
        }
        public ProductModel ProductModel => _productModel;
 
        private ImageSource? _productImage;
        public ImageSource? ProductImage
        {
            get { return _productImage; }
            set
            {
                if (_productImage != value)
                {
                    _productImage = value;
                    RaisePropertyChanged(nameof(ProductImage));
                }
            }
        }

    }
}
