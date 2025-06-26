using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
 using static System.Net.Mime.MediaTypeNames;

namespace UIComponent.View
{

    public partial class CalendarBlackDatesUserControl : UserControl
    {
        public CalendarBlackDatesUserControl()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty DateRangesProperty =
       DependencyProperty.Register("DateRanges", typeof(DateRange), typeof(CalendarBlackDatesUserControl), new PropertyMetadata());

        public DateRange DateRanges
        {
            get { return (DateRange)GetValue(DateRangesProperty); }
            set { SetValue(DateRangesProperty, value); }
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null && e.AddedItems.Count != 0)
            {
                DateRanges = new DateRange();

                List<DateTime> calendars = new List<DateTime>();

                foreach (DateTime selectedDate in e.AddedItems)
                {
                    calendars.Add(selectedDate);
                }
                DateRange tempDateRange = new DateRange();

                var tempFirst = (calendars[0]);
                var tempEnd = calendars[calendars.Count - 1];

                bool isEarly = false;
                if (DateTime.Compare(tempFirst, tempEnd) == 1)    //t1 晚於 t2
                {
                    SetTimeRangeStartEndTime(tempDateRange, tempEnd, tempFirst );

                }
                else if (DateTime.Compare(tempFirst, tempEnd) == -1)//t1 早於 t2
                {
                    SetTimeRangeStartEndTime(tempDateRange, tempFirst, tempEnd);

                }
                else //t1等於t2，回傳值：0
                {
                    SetTimeRangeStartEndTime(tempDateRange, tempFirst, tempEnd);
                }
                DateRanges = tempDateRange;
            }

        }

        private static void SetTimeRangeStartEndTime(DateRange tempDateRange, DateTime tempFirst, DateTime tempEnd)
        {
            DateTime startTime = tempFirst;
            DateTime endTime = tempEnd;
            tempDateRange.StartDate = new DateTime(startTime.Date.Year, startTime.Date.Month, startTime.Date.Day, 0, 0, 0);
            tempDateRange.EndDate = new DateTime(endTime.Date.Year, endTime.Date.Month, endTime.Date.Day, 23, 59, 59);
        }
    }
}
