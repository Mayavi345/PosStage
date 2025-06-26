using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Stage.Backstage.ProductManager.ViewModel
{
    public class AddProductWindowsViewModel : ObservableObject
    {
        private AddProductViewModel _addProductViewModel;

        public AddProductWindowsViewModel(Action refreshGridView)
        {
            AddProductViewModel = new AddProductViewModel(refreshGridView);
        }

        public AddProductViewModel AddProductViewModel
        {
            get { return _addProductViewModel; }
            set
            {
                if (_addProductViewModel != value)
                {
                    _addProductViewModel = value;
                    RaisePropertyChanged(nameof(AddProductViewModel));
                }
            }
        }
       
    }
}
