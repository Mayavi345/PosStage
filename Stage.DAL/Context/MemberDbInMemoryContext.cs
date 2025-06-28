using Microsoft.EntityFrameworkCore;
using PosStage.MVVM.Models;
using Stage.Data.Models.DataModel;

namespace Stage.DAL
{
    public class MemberDbInMemoryContext : MemberDbcontextbase
    {
        public DbSet<MB1003_MemberOrderInfo> MB1003_MemberOrderInfo { get; set; }

        public MemberDbInMemoryContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("MemberInMemoryDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MB1001_Member>().HasData(GetMembers());
            modelBuilder.Entity<MB1002_ConsumptionLevelRule>().HasData(GetLevelRules());
            modelBuilder.Entity<MB1003_MemberOrderInfo>()
                    .HasData(GetOrderInfos());
            base.OnModelCreating(modelBuilder);
        }

        private static MB1003_MemberOrderInfo[] GetOrderInfos()
        {
            return new MB1003_MemberOrderInfo[]
            {
                new MB1003_MemberOrderInfo
                {
                    OrderNumber = "ORD2025001",
                    MemberId    = 1,
                    TotalPrice  = 150.0,
                    Timestamp   = DateTime.Parse("2025-06-01T10:00:00")
                },
                new MB1003_MemberOrderInfo
                {
                    OrderNumber = "ORD2025002",
                    MemberId    = 2,
                    TotalPrice  = 300.0,
                    Timestamp   = DateTime.Parse("2025-06-15T14:30:00")
                }
            };
        }

        private static MB1001_Member[] GetMembers()
        {
            return new MB1001_Member[]
            {
                new MB1001_Member{ Id = 1, Name="Jack", Gender=1, PhoneNumber="09123456789", MemberLevel=0, IsDelete=false },
                new MB1001_Member{ Id = 2, Name="admin", Gender=1, PhoneNumber="09123456781", MemberLevel=0, IsDelete=false },
                new MB1001_Member{ Id = 3, Name="Rose", Gender=2, PhoneNumber="09123456782", MemberLevel=0, IsDelete=false },
            };
        }

        private static MB1002_ConsumptionLevelRule[] GetLevelRules()
        {
            return new MB1002_ConsumptionLevelRule[]
            {
                new MB1002_ConsumptionLevelRule{ MemberLevel = 1, ConsumptionPrice=0, Name="無"},
                new MB1002_ConsumptionLevelRule{ MemberLevel = 2, ConsumptionPrice=500, Name="銅"},
                new MB1002_ConsumptionLevelRule{ MemberLevel = 3, ConsumptionPrice=2000, Name="銀"},
                new MB1002_ConsumptionLevelRule{ MemberLevel = 4, ConsumptionPrice=5000, Name="金"},
            };
        }
    }
}
