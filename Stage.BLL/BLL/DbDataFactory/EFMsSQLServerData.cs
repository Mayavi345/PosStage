using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stage.BLL.BLL.Service;
using Stage.DAL.Repositories.Implement;
using Utilities;
using Utilities.Observer;
using PosStage.DAL;
using Utilities.Nlog;

namespace Stage.BLL.BLL
{
    public class EFMsSQLServerData : DbDataBase
    {

        private string ConnectionString;

        public EFMsSQLServerData(string connectionString)
        {
            this.ConnectionString = connectionString;
        }


        public override void InitService(EServiceType eServiceType)
        {
            EmployeeService = new EmployeeService(EmployeeRepository);
            ProductService = new ProductService(ProductsRepository, ProductDetailRepository, CategoryRepository);
            CompletedOrdersService = new CompletedOrdersService(CompletedOrdersRepository);
            switch (eServiceType)
            {
                case EServiceType.SQL:
                    MemberSerivce = new MemberService(MemberRepository);
                    break;
                case EServiceType.Api:
                    MemberSerivce = new MemberServiceWebAPI();
                    break;
            }

        }
        public override void InitRepository()
        {
            try
            {
                // 創建服務集合 (ServiceCollection)
                ServiceCollection services = new ServiceCollection();
                var optionsBuilder = new DbContextOptionsBuilder<PosDbContext>();
                using var dbContext = new PosDbContext(optionsBuilder.Options);
                string connectionString = ConnectionString;
                services.AddDbContext<PosDbContext>(options =>
                {
                    optionsBuilder.UseSqlServer(connectionString);
                });
                // 建立 ServiceProvider，用於處理依賴注入
                _serviceProvider = services.BuildServiceProvider();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);

                string messageText = e.Message;
            }

            EmployeeRepository = new EmployeeRepositoryEF();
            ProductsRepository = new ProductsRepositoryEF();
            ProductDetailRepository = new ProductDetailRepositoryEF();
            CategoryRepository = new CategoryRepositoryEF();
            CompletedOrdersRepository = new CompletedOrdersRepositoryEF();
            //MemberRepository = new MemberRepositoryEF();
            MemberRepository = new MemberRepositoryEF();
        }

    }

}
