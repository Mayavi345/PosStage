using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIComponent.BindingModel
{
    public class ComboBoxItem<T> : ObservableObject
    {
        public string ID { get; set; }
        public T Object { get; set; }
        private string _DisplayName = "";
        public string DisplayName
        {
            get => _DisplayName;
            set
            {
                if (_DisplayName != value)
                {
                    _DisplayName = value;
                    this.RaisePropertyChanged(() => DisplayName);
                }
            }
        }
    }
}
