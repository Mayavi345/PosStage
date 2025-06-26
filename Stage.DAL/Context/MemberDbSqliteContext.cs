using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stage.Data.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using PosStage.MVVM.Models;
using Utilities.Nlog;

namespace Stage.DAL
{
    public class MemberDbSqliteContext : MemberDbcontextbase
    {
        private const string ConnetString =
            @"Data Source=Member.db";

        // 在資料庫建立時執行的方法
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 使用 HasData 方法初始化資料庫中的產品資料
            modelBuilder.Entity<MB1001_Member>().HasData(GetMember());
            modelBuilder.Entity<MB1002_ConsumptionLevelRule>().HasData(GetConsumptionLevelRule());
            modelBuilder.Entity<MB1003_MemberOrderInfo>().HasData(GetMemberOrderInfo());

            modelBuilder.Entity<MB1001_Member>().HasIndex(b => b.PhoneNumber).IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        private MB1003_MemberOrderInfo[] GetMemberOrderInfo()
        {
            return new MB1003_MemberOrderInfo[] {
           };
        }

        private MB1002_ConsumptionLevelRule[] GetConsumptionLevelRule()
        {
            return new MB1002_ConsumptionLevelRule[] {
               new MB1002_ConsumptionLevelRule{ MemberLevel = 1,ConsumptionPrice=0,Name="無"},
               new MB1002_ConsumptionLevelRule{ MemberLevel =2,ConsumptionPrice=500,Name="銅"},
               new MB1002_ConsumptionLevelRule{  MemberLevel = 3,ConsumptionPrice=2000,Name="銀"},
               new MB1002_ConsumptionLevelRule{ MemberLevel = 4,ConsumptionPrice=5000,Name="金"},

           };
        }

        private MB1001_Member[] GetMember()
        {
            return new MB1001_Member[] {
               new MB1001_Member{ Id = 1,Name="Jack",Gender=1,PhoneNumber="09123456789",MemberLevel=0 ,IsDelete=false},
               new MB1001_Member{ Id = 2,Name="admin",Gender=1,PhoneNumber="09123456781",MemberLevel=0 ,IsDelete=false},
               new MB1001_Member{ Id = 3,Name="Rose",Gender=2,PhoneNumber="09123456782",MemberLevel=0 ,IsDelete=false},
           };
        }
        public MemberDbSqliteContext()
        {
            Database.EnsureCreated();

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try

            {   // 創建服務集合 (ServiceCollection)
                ServiceCollection services = new ServiceCollection();
                // 設置 DbContext 服務，使用 SQLite 資料庫
                optionsBuilder.UseSqlite(ConnetString);

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
