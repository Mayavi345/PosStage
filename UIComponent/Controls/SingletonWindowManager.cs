using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UIComponent.Controls
{
    public class SingletonWindowManager
    {
        private static readonly Dictionary<Type, Window> OpenWindows = new Dictionary<Type, Window>();
        public static Window GetWindow<T>() where T : Window
        {
            Type windowType = typeof(T);
            return OpenWindows[windowType];

        }
        public static void ShowSingletonWindow<T>() where T : Window, new()
        {
            Type windowType = typeof(T);

            if (!OpenWindows.ContainsKey(windowType) || OpenWindows[windowType] == null)
            {
                // Create a new instance of the window if it doesn't exist
                Window window = new T();
                OpenWindows[windowType] = window;

                // Handle the Closed event to remove the window from the dictionary
                window.Closed += (sender, args) => OpenWindows[windowType] = null;

                window.Show();
            }
            else
            {
                // Bring the existing window to the foreground
                OpenWindows[windowType].Activate();
            }
        }
        public static void ShowSingletonWindow<T>(ObservableObject viewModel) where T : Window, new()
        {
            Type windowType = typeof(T);

            if (!OpenWindows.ContainsKey(windowType) || OpenWindows[windowType] == null)
            {
                // Create a new instance of the window if it doesn't exist
                Window window = new T();
                OpenWindows[windowType] = window;
                window.DataContext = viewModel;
                // Handle the Closed event to remove the window from the dictionary
                window.Closed += (sender, args) => OpenWindows[windowType] = null;

                window.Show();
            }
            else
            {
                // Bring the existing window to the foreground
                OpenWindows[windowType].Activate();
            }
        }
    }
}
