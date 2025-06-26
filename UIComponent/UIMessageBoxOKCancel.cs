using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Utilities;

namespace UIComponent
{
    public class UIMessageBoxOKCancel : Singleton<UIMessageBoxOKCancel>
    {
        public void ShowDialog(Action OkAction, Action FailAction, string text = TextResourceCenter.ConfirmIsDelete)
        {
            MessageBoxResult result = MessageBox.Show(text, "", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                OkAction?.Invoke();
            }
            else
            {
                FailAction?.Invoke();

            }
        }
    }
}
