using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utilities.Observer;

namespace UIComponent
{
    public class UIMessageObserver : IObserver
    {
        public void Update(object message)
        {
            if (message is string typedMessage)
            {
                MessageBox.Show(typedMessage);
            }
            else
            {
            }
        }
    }
}
