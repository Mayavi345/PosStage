using Stage.BLL.BLL.Service;
using Stage.DAL;
using Stage.DAL.Repositories.Implement;
using Utilities;
using Utilities.Nlog;

namespace Stage.BLL.BLL
{
    public class ServiceFactory
    {
        private DbDataBase _dbDataFactory;

        public IEmployeeService EmployeeService => _employeeService;
        private IEmployeeService _employeeService;
        public IProductService ProductService => _productService;
        private IProductService _productService;
        public ICompletedOrdersService CompletedOrdersService => _completedOrdersService;
        private ICompletedOrdersService _completedOrdersService;
        public IMemberService MemberSerivce => _memberSerivce;

        public IEmployeeRepository EmployeeRepository => _dbDataFactory.EmployeeRepository;
        public IProductsRepository ProductsRepository => _dbDataFactory.ProductsRepository;
        public IProductDetailRepository ProductDetailRepository => _dbDataFactory.ProductDetailRepository;
        public ICategoryRepository CategoryRepository => _dbDataFactory.CategoryRepository;
        public ICompletedOrdersRepository CompletedOrdersRepository => _dbDataFactory.CompletedOrdersRepository;
        public IMemberRepository MemberRepository => _dbDataFactory.MemberRepository;


        private IMemberService _memberSerivce;
        public DbDataBase Build(EDataDb eDataDb,EServiceType eServiceType)
        {
            LogManagerSingleton.Instance.PrintLog(eDataDb.ToString(), NLog.LogLevel.Info);
            switch (eDataDb)
            {
                case EDataDb.none:
                    return null;
                case EDataDb.MsSqlServer:
                    CrateMsSqlServer(eServiceType);
                    return _dbDataFactory;
                case EDataDb.Sqlite:
                    CrateSqlite(eServiceType);
                    return _dbDataFactory;
                default:
                    return null;
            }
       
        }
        private void CrateMsSqlServer(EServiceType eServiceType)
        {
            _dbDataFactory = new EFMsSQLServerData(MainConfigService.Instance.ConnectionString);
            _dbDataFactory.InitRepository();
            _dbDataFactory.InitService(eServiceType);

            PairDbService();
            MainDALSystem.Instance.Init();
        }



        private void CrateSqlite(EServiceType eServiceType)
        {
            _dbDataFactory = new EFSQLiteData(MainConfigService.Instance.ConnectionString);
            _dbDataFactory.InitRepository();
            _dbDataFactory.InitService(eServiceType);

            PairDbService();
            MainDALSystem.Instance.Init();
        }
        private void PairDbService()
        {
            _employeeService = _dbDataFactory.EmployeeService;
            _productService = _dbDataFactory.ProductService;
            _completedOrdersService = _dbDataFactory.CompletedOrdersService;
            _memberSerivce = _dbDataFactory.MemberSerivce;
        }
    }
    public enum EDataDb
    {
        none = 0,
        MsSqlServer = 1,
        Sqlite = 2
    }
    public enum EServiceType { 
        
        SQL=0,
        Api=1,
    }
}
