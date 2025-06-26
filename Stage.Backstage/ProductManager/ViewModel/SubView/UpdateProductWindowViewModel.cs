using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage.Backstage.ProductManager.ViewModel
{
    public class UpdateProductWindowViewModel : ObservableObject
    {
        private UpdateProductViewModel _updateProductViewModel;

        public UpdateProductWindowViewModel(Action refreshGridView)
        {
            UpdateProductViewModel = new UpdateProductViewModel(refreshGridView);
        }

        public UpdateProductViewModel UpdateProductViewModel
        {
            get { return _updateProductViewModel; }
            set
            {
                if (_updateProductViewModel != value)
                {
                    _updateProductViewModel = value;
                    RaisePropertyChanged(nameof(UpdateProductViewModel));
                }
            }
        }
    }
}
