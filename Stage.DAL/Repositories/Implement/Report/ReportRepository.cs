using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Stage.DAL.Repositories.Implement
{
    public interface IReportRepository
    {
        DataTable GetOrderDetailReportData(DateTime dateStart, DateTime dateEnd);
    }
    public class ReportRepository : IReportRepository
    {
        public ReportRepository()
        {
            MainConfigService.Instance.InitConfig();
        }

        DataTable IReportRepository.GetOrderDetailReportData(DateTime dateStart, DateTime dateEnd)
        {
            return GetOrderDetailReportData(dateStart, dateEnd);
        }
        public static DataTable GetOrderDetailReportData(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                string connectionString = MainConfigService.Instance.ConnectionString;
                var SQLString = "GetViewProductData";
                SqlConnection sqlconnection = new SqlConnection(connectionString);
                DataSet dataset = new DataSet();
                SqlCommand sqlcommand = new SqlCommand(SQLString);
                sqlcommand.CommandType = CommandType.StoredProcedure;

                sqlcommand.Parameters.AddWithValue("@StartTime", dateStart);
                sqlcommand.Parameters.AddWithValue("@EndTime", dateEnd);

                sqlcommand.Connection = sqlconnection;

                using (sqlcommand.Connection)
                {
                    sqlcommand.Connection.Open();
                    object result = sqlcommand.ExecuteScalar();

                    SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                    sqldataadapter.Fill(dataset);
                }
                return dataset.Tables[0];
            }
            catch (Exception ex) {
                return default;
            }
                
        }
    }
}
