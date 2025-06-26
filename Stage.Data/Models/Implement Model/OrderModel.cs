using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace PosStage.MVVM.Models.Implement_Model
{
    public class OrderModel : IFilterableObject
    {
        public string OrderNumber { get; set; }
        public DateTime Timestamp { get; set; }
        public double TotalAmount { get; set; }
        public string EmployeeName { get; set; }
        string IFilterableObject.FilterValue => OrderNumber;
    }
}
