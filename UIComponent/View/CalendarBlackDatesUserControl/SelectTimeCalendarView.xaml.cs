using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UIComponent.Controls;

namespace UIComponent.View
{
    /// <summary>
    /// SelectTimeCalendarView.xaml 的互動邏輯
    /// </summary>
    public partial class SelectTimeCalendarView : UserControl
    {
        public SelectTimeCalendarView()
        {
            InitializeComponent();


        }
        public ICommand ApplyCommand
        {
            get { return (ICommand)GetValue(ApplyCommandProperty); }
            set { SetValue(ApplyCommandProperty, value); }
        }
        // Using a DependencyProperty as the backing store for ApplyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ApplyCommandProperty =
            DependencyProperty.Register("ApplyCommand", typeof(ICommand), typeof(SelectTimeCalendarView), new PropertyMetadata());


        public ICommand ClearCommand
        {
            get { return (ICommand)GetValue(ClearCommandProperty); }
            set { SetValue(ClearCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClearCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClearCommandProperty =
            DependencyProperty.Register("ClearCommand", typeof(ICommand), typeof(SelectTimeCalendarView), new PropertyMetadata());
        public void ChangeDate()
        {
            DateRanges = new DateRange()
            {
                StartDate = StartDatePicker.SelectedDate ?? DateTime.MinValue,
                EndDate = EndDatePicker.SelectedDate ?? DateTime.MaxValue

            };

        }

        private void CheckButtonState()
        {
            if ((StartDatePicker.SelectedDate.HasValue) && (EndDatePicker.SelectedDate.HasValue) != false)
            {
                ApplyButton.IsEnabled = true;
            }
            else
            {
                ApplyButton.IsEnabled = false;
            }
        }

        public DateRange DateRanges
        {
            get { return (DateRange)GetValue(DateRangeProperty); }
            set { SetValue(DateRangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DateRange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateRangeProperty =
            DependencyProperty.Register("DateRanges", typeof(DateRange), typeof(SelectTimeCalendarView), new PropertyMetadata());



        public bool NoDataLockButton
        {
            get
            {
                return (bool)GetValue(NoDataLockButtonProperty);
            }
            set
            {

                SetValue(NoDataLockButtonProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for NoDataLockButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NoDataLockButtonProperty =
            DependencyProperty.Register("NoDataLockButton", typeof(bool), typeof(SelectTimeCalendarView), new PropertyMetadata(NoDataLockButtonCallBack));

        private static void NoDataLockButtonCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var selectTimeCalendarView = d as SelectTimeCalendarView;
            selectTimeCalendarView.LockButton();
        }
        public void LockButton()
        {
            if (NoDataLockButton == true)
            {
                ApplyButton.IsEnabled = false;
            }

        }
        public void Clear()
        {
            //TODO 改成MVVM，DependencyProperty實例化的時候似乎會被清除，導致無法正常使用。先每次都new
            DateRanges = new DateRange()
            {
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MaxValue,
            };
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
        }

        private void ApplyClick(object sender, RoutedEventArgs e)
        {
            ChangeDate();
            if (ApplyCommand?.CanExecute(null) == true)
            {
                ApplyCommand.Execute(null);

            }
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            Clear();
            if (ClearCommand?.CanExecute(null) == true)
            {
                ClearCommand.Execute(null);
            }
        }

        private void StartDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoDataLockButton)
            {
                CheckButtonState();
            }
        }

        private void EndDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoDataLockButton)
            {
                CheckButtonState();
            }
        }
    }
}
