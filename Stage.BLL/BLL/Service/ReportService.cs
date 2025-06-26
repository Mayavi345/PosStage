using Stage.DAL.Repositories.Implement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage.BLL.BLL.Service
{
    public interface IReportService
    {
        DataTable GetOrderDetailReportData(DateTime startDate, DateTime endDate);
    }
    public class ReportService : IReportService
    {
        IReportRepository _repository;
        public ReportService()
        {
            _repository = new ReportRepository();
        }

        DataTable IReportService.GetOrderDetailReportData(DateTime startDate, DateTime endDate)
        {
            return _repository.GetOrderDetailReportData(startDate, endDate);
        }
    }
}
