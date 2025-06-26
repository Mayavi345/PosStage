using Microsoft.Extensions.DependencyInjection;
using Stage.BLL.BLL.Service;
using Stage.DAL.Repositories.Implement;

namespace Stage.BLL.BLL
{
    //TODO 之後再來想怎麼優化
    public class DbDataBase
    {
        public IEmployeeService EmployeeService;
        public IProductService ProductService;
        public IMemberService MemberSerivce;
        public ICompletedOrdersService CompletedOrdersService;

        public IEmployeeRepository EmployeeRepository;
        public IProductsRepository ProductsRepository;
        public IProductDetailRepository ProductDetailRepository;
        public ICategoryRepository CategoryRepository;
        public ICompletedOrdersRepository CompletedOrdersRepository;
        public IMemberRepository MemberRepository;

        public ServiceProvider ServiceProvider => _serviceProvider;

        protected ServiceProvider _serviceProvider;
        public virtual void InitService(EServiceType eServiceType)
        {

        }
        public virtual void InitRepository()
        {

        }
    }


}
