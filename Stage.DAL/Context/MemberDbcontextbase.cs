using Microsoft.EntityFrameworkCore;
using Stage.Data.Models.DataModel;
using PosStage.MVVM.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stage.DAL
{
    public class MemberDbcontextbase: DbContext
    {
        public DbSet<MB1001_Member> MB1001_Member { get; set; }
        public DbSet<MB1002_ConsumptionLevelRule> MB1002_ConsumptionLevelRule { get; set; }
        public DbSet<MB1003_MemberOrderInfo> MB1003_MemberOrderInfo { get; set; }
        public DbSet<MemberConsumptionReportModel> MemberConsumptionReportModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberConsumptionReportModel>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
}
