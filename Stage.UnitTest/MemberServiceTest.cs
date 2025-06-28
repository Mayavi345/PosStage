using Microsoft.EntityFrameworkCore;
using Moq;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Backstage.ViewModel;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.DAL;
using Stage.DAL.Repositories.Implement;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Stage.UnitTest
{
    public class MemberServiceTest
    {
        private IMemberService _memberService;
        MemberDbInMemoryContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _dbContext = new MemberDbInMemoryContext();
            var repository = new MemberRepositoryInMemory { _dbContext = _dbContext };
            _memberService = new MemberService(repository);
        }
        [Test]
        public void AddAndDelete()
        {
            MB1001_Member mB1001_Member = GetTestMamberData();
            var response = _memberService.Add(mB1001_Member);
            var tempMember = (MB1001_Member)response.Data;
            _memberService.Delete(tempMember.Id);
            Assert.IsTrue(response.IsSuccess);
        }
        [Test]
        public void GetMemberModelList()
        {
            var response = _memberService.GetMemberModelList();
            Assert.IsTrue(response.IsSuccess);

        }
        [Test]
        public void CalculateMemberLevel_UseMockRepository()
        {
            var levels = new List<MB1002_ConsumptionLevelRule>
            {
                new MB1002_ConsumptionLevelRule{ MemberLevel=1, ConsumptionPrice=500 },
                new MB1002_ConsumptionLevelRule{ MemberLevel=2, ConsumptionPrice=1000 },
                new MB1002_ConsumptionLevelRule{ MemberLevel=3, ConsumptionPrice=2000 }
            };

            var repoMock = new Mock<IMemberRepository>();
            repoMock.Setup(r => r.GetLevelList()).Returns(levels);
            IMemberService service = new MemberService(repoMock.Object);
            var result = service.CalculateMemberLevel(600);

            Assert.AreEqual(1, result.MemberLevel);
        }
        //[Test]
        //public void AddMemberOrderInfo()
        //{
        //    MB1003_MemberOrderInfo memberOrderInfo = GetTestMemberOrderInfo();
        //    if (_memberService is MemberService)
        //    {
        //        //有實際服務才執行測試
        //        var response = _memberService.AddMemberOrderInfo(memberOrderInfo);
        //        Assert.IsTrue(response.IsSuccess);
        //        //復原
        //        var newItem = _dbContext.MB1003_MemberOrderInfo.Where(x => x.OrderNumber == memberOrderInfo.OrderNumber).FirstOrDefault();
        //        _dbContext.MB1003_MemberOrderInfo.Remove(newItem);
        //    }
        //    else
        //        Assert.IsTrue(true);

      
        //}
        [Test]
        public void GetLevelList()
        {
            var response = _memberService.GetLevelList();
            Assert.IsTrue(response.IsSuccess);
        }

        [Test]
        public void GetAllMemberOrderInfo()
        {
            var response = _memberService.GetAllMemberOrderInfo();
            Assert.IsTrue(response.IsSuccess);
        }
        //[Test]
        //public void GetMemberOrderInfo()
        //{
        //    var orderList = _memberService.GetAllMemberOrderInfo().Data;
        //    var response = _memberService.GetMemberOrderInfo(orderList.FirstOrDefault().OrderNumber);
        //    Assert.IsTrue(response.IsSuccess);
        //}

        [Test]
        public void GetMemberConsumptionReportModel()
        {
            var response = _memberService.GetMemberConsumptionReportModel();
            Assert.IsTrue(response.IsSuccess);
        }

        private MB1003_MemberOrderInfo GetTestMemberOrderInfo()
        {
            var tempId = _dbContext.MB1001_Member.FirstOrDefault().Id;
            MB1003_MemberOrderInfo item = new MB1003_MemberOrderInfo()
            {
                OrderNumber = "199901010001",
                MemberId = tempId,
                TotalPrice = 10,
                Timestamp = DateTime.Now
            };
            return item;
        }

        private MB1001_Member GetTestMamberData()
        {
            MB1001_Member member = new MB1001_Member()
            {
                Name = "TestMember",
                Gender = 1,
                PhoneNumber = "0987654321",
                MemberLevel = 1,
                IsDelete = false
            };
            return member;
        }

    }
}
