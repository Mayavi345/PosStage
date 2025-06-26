using Microsoft.EntityFrameworkCore;
using PosStage.DAL;
using PosStage.MVVM.Models;
using Stage.Data.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Nlog;
using Utilities;

namespace Stage.DAL
{
    public class MemberDbMSSqlContext : MemberDbcontextbase
    {
        public MemberDbMSSqlContext(DbContextOptions<MemberDbMSSqlContext> options)
        {

        }
        public MemberDbMSSqlContext() { 
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                MainConfigService.Instance.InitConfig();
                string connectionString = MainConfigService.Instance.ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
            catch (Exception e)
            {
                string errorText = "OnConfiguring: " + e.Message;
                MainDALSystem.Instance.UIMessageBox.NotifyObservers(errorText);
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
            }
        }
    }
}
