using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using PosStage.MVVM.Models;
using Stage.BLL.BLL;
using Utilities;
using Stage.BLL.BLL.Service;
using Utilities.Nlog;

namespace Stage.Backstage.Common
{
    //TODO 前後台各有一個，之後合併，建立前後台共用的程式參考
    public class ImageSourceProcess : Singleton<ImageSourceProcess>
    {
        private Dictionary<int, ImageSource> _imageDic = new Dictionary<int, ImageSource>();
        IProductService _productService;

        public ImageSource ImageEmpty => _imageEmpty;
        private ImageSource _imageEmpty;
        public void Init()
        {
            _imageDic = new Dictionary<int, ImageSource>();
            _productService = MainSystemService.Instance.ProductService;
            InitImageDic();
            _imageEmpty = GetEmptyImageSource();
        }
        private const string EmptyImagePath = "pack://application:,,,/Images/empty.jpg";

        public static System.Windows.Media.ImageSource GetImageSource(string fileName)
        {
            if (fileName == string.Empty)
            {
                return GetEmptyImageSource();
            }
            else
            {
            }
            System.Windows.Media.Imaging.BitmapImage image = new System.Windows.Media.Imaging.BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(fileName);
            image.EndInit();
            return image;
        }
        public static byte[] GetImageByte(string path)
        {
            Image img = Image.FromFile(path);
            Bitmap resizedImage = new Bitmap(img, new Size(256, 256));
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                resizedImage.Save(stream, ImageFormat.Jpeg);
                bytes = stream.ToArray();
                return bytes;
            }
        }
        public static ImageSource GetBitmapImage(byte[] bytes)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
            }
            return bitmapImage;
        }
        private static ImageSource GetEmptyImageSource()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(EmptyImagePath);
            image.EndInit();
            return image;
        }
        private void InitImageDic()
        {
            _productService = MainSystemService.Instance.ProductService;
            var respoen = _productService.GetImageList();
            if (respoen.IsSuccess)
            {
                List<PB4001_ImageData> list = respoen.Data;
                foreach (var item in list)
                {
                    ImageSource bitmapImage = GetBitmapImage(item.Image);
                    _imageDic.Add(item.Id, bitmapImage);
                }
            }
        }
        public ImageSource GetImage(int id)
        {
            if (_imageDic.ContainsKey(id))
            {
                return _imageDic[id];
            }
            else
            {
                return GetEmptyImageSource();
            }
        }

        internal void AddNewImage(int id, ImageSource imageData)
        {
            if (_imageDic.TryAdd(id, imageData))
            {
            }
            else
            {
                LogManagerSingleton.Instance.PrintLog("Add image fail",NLog.LogLevel.Error);
            }
        }
    }

}
