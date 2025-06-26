using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stage.BLL.BLL.Service;
using Stage.DAL.Repositories.Implement;
using Utilities.Observer;
using PosStage.DAL;

namespace Stage.BLL.BLL
{
    public class EFSQLiteData : DbDataBase
    {

        private string _connectionString;

        public EFSQLiteData( string connectionString)
        {
            this._connectionString = connectionString;
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
            EmployeeRepository = new EmployeeRepositoryEF();
            ProductsRepository = new ProductsRepositoryEF();
            ProductDetailRepository = new ProductDetailRepositoryEF();
            CategoryRepository = new CategoryRepositoryEF();
            CompletedOrdersRepository = new CompletedOrdersRepositoryEF();
            MemberRepository = new MemberRepositorySqlite();

        }

    }

}
