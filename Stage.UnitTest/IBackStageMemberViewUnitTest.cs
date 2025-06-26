using GalaSoft.MvvmLight.Command;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Backstage.ViewModel;
using Stage.BLL.BLL;
using Stage.DAL.Repositories.Implement;
using System.Transactions;

namespace Stage.UnitTest
{
    public class IBackStageMemberViewUnitTest
    {
        private EmployeeManagerViewModel _employeeManagerViewModel;
        PosDbContext _posDbContext;
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction;
        [SetUp]
        public void Setup()
        {
            MainSystemService.Instance.InitSystem(EDataDb.MsSqlServer, EServiceType.SQL);
            _posDbContext = new PosDbContext();
            _employeeManagerViewModel = new EmployeeManagerViewModel();
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        public void Add_Employee()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                // Arrange: 建立一個新的類別
                RelayCommand<EmployeeModel> addCommand = _employeeManagerViewModel.AddEmployeeViewModel.AddCommand;
                EmployeeModel employeeModel = GetTestEmployeeModel();

                _employeeManagerViewModel.AddEmployeeViewModel.IsPassValidate = true;
                // Act: 呼叫 Add 方法
                addCommand.Execute(employeeModel);

                // Assert: 驗證結果，確認新類別已新增
                var addedCategory = _posDbContext.PB2001_Employee.Where(x => x.Name == employeeModel.Name).FirstOrDefault();
                Assert.IsNotNull(addedCategory);
            }
        }

        private EmployeeModel GetTestEmployeeModel()
        {
            EmployeeModel employeeModel = new EmployeeModel()
            {
                Name = "Test123",
                Account = "Test123",
                Password = "Test123",
                Gender = new GenderModel()
                {
                    Id = 0,
                    Name = "男性"
                },
                IsDelete = false,
                Role = new RoleViewModel()
                {
                    RoleId = 1,
                    RoleName = "銅"
                },
                ImageId = 1,
            };
            return employeeModel;

        }
    }
}