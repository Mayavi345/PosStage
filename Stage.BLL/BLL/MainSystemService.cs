using Stage.BLL.BLL.Service;
using Utilities;
using Utilities.Nlog;
using Utilities.Observer;

namespace Stage.BLL.BLL
{
    public class MainSystemService : Singleton<MainSystemService>
    {

        public MessageSubject ObserverUIMessageBox => _observerUIMessageBox;
        private MessageSubject _observerUIMessageBox;

        public ServiceFactory DataFactory => _dataFactory;
        private ServiceFactory _dataFactory;
        #region Serivce
        public IEmployeeService EmployeeService => _dataFactory.EmployeeService;
        public IProductService ProductService => _dataFactory.ProductService;
        public ICompletedOrdersService CompletedOrdersService => _dataFactory.CompletedOrdersService;
        public IMemberService MemberSerivce => _dataFactory.MemberSerivce;
        public IRoleService RoleService => _roleService;

        private RoleService _roleService;

        public IReportService ReportService => _reportService;
        private ReportService _reportService;
        #endregion

        public MainSystemService()
        {
            _observerUIMessageBox = new MessageSubject();
            _dataFactory = new ServiceFactory();
            _roleService = new RoleService();
            _reportService = new ReportService();
        }


        public void ShowMessageBox(string message)
        {
            Instance.ObserverUIMessageBox.NotifyObservers(message);
        }

        public void InitSystem(EDataDb eDataDb, EServiceType eServiceType)
        {
            LogManagerSingleton.Instance.PrintLog("", NLog.LogLevel.Info);

            _dataFactory.Build(eDataDb, eServiceType);

            MainConfigService.Instance.InitConfig(() =>
            {
                Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.FindNotSettingJsonFile);
            });
        }


    }

}
