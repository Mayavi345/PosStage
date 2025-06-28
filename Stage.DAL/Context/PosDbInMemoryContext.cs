using Microsoft.EntityFrameworkCore;
using PosStage.MVVM.Models;
using Stage.Data.Models.DataModel;

namespace PosStage.DAL
{
    public class PosDbInMemoryContext : PosDbContext
    {
        public PosDbInMemoryContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("PosInMemoryDb");
        }
    }
}
