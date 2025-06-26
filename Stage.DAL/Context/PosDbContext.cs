using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Configuration;
using Utilities;
using System.Windows;
using System;
using PosStage.MVVM.Models;
using Stage.DAL;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Options;
using Stage.Data.Models.DataModel;
using Utilities.Nlog;

namespace PosStage.DAL
{

    public class PosDbContext : DbContext
    {
        private const string NotFindJsonText = "找不到setting.json，系統已自動生成";

        public DbSet<PB2001_Employee> PB2001_Employee { get; set; }
        public DbSet<PB3001_Product> PB3001_Product { get; set; }
        public DbSet<PB3002_ProductDetail> PB3002_ProductDetail { get; set; }
        public DbSet<PB1001_CompletedOrders> PB1001_CompletedOrders { get; set; }
        public DbSet<PB1002_CompletedOrdersDetails> PB1002_CompletedOrdersDetails { get; set; }
        public DbSet<PB3003_Categories> PB3003_Categories { get; set; }
        public DbSet<PB2002_Role> PB2002_Role { get; set; }
        public DbSet<PB2003_UserRole> PB2003_UserRole { get; set; }
        public DbSet<PB4001_ImageData> PB4001_ImageData { get; set; }

        public PosDbContext(DbContextOptions<PosDbContext> options)
        {

        }
        public PosDbContext()
        {
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