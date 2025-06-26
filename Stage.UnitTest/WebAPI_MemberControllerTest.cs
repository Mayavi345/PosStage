using Microsoft.EntityFrameworkCore;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Backstage.ViewModel;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.DAL;
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
    public class WebAPI_MemberControllerTest
    {
        private IMemberService _memberService;
        MemberDbMSSqlContext _dbContext;

        [SetUp]
        public void Setup()
        {
            MainSystemService.Instance.InitSystem(EDataDb.MsSqlServer, EServiceType.SQL);
            _dbContext = new MemberDbMSSqlContext();
            _memberService = MainSystemService.Instance.MemberSerivce;
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
        public void UpdateMember()
        {
            var memberList = _memberService.GetMemberModelList();
            MemberModel oringinMember = memberList.Data[0];
            MemberModel memberModel = new MemberModel(oringinMember);

            memberModel.PhoneNumber = "1234567890";
            _memberService.Update(memberModel.ConvertToMember());

            var newMember = _dbContext.MB1001_Member.Where(x => x.PhoneNumber == "1234567890").FirstOrDefault();
            Assert.IsNotNull(newMember);
            Assert.AreEqual(memberModel.Name, newMember.Name);

            //復原
            memberModel.PhoneNumber = oringinMember.PhoneNumber;
            _memberService.Update(memberModel.ConvertToMember());
        }
        [Test]
        public void AddMemberOrderInfo()
        {
            MB1003_MemberOrderInfo memberOrderInfo = GetTestMemberOrderInfo();
            var response = _memberService.AddMemberOrderInfo(memberOrderInfo);
            Assert.IsTrue(response.IsSuccess);

            //復原
            var newItem = _dbContext.MB1003_MemberOrderInfo.Where(x => x.OrderNumber == memberOrderInfo.OrderNumber).FirstOrDefault();
            _dbContext.MB1003_MemberOrderInfo.Remove(newItem);
        }
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
        [Test]
        public void GetMemberOrderInfo()
        {
            var orderList = _memberService.GetAllMemberOrderInfo().Data;
            var response = _memberService.GetMemberOrderInfo(orderList.FirstOrDefault().OrderNumber);
            Assert.IsTrue(response.IsSuccess);
        }
        [Test]
        public void GetMemberModel()
        {
            var memberList = _memberService.GetMemberModelList();
            MemberModel oringinMember = memberList.Data[0];
            var response = _memberService.GetMemberModel(oringinMember.PhoneNumber);
            Assert.IsTrue(response.IsSuccess);
        }
        [Test]
        public void GetMemberConsumptionReportModel()
        {
            var response = _memberService.GetMemberConsumptionReportModel();
            Assert.IsTrue(response.IsSuccess);
        }
        [Test]
        public void CalculateMemberLevel()
        {
            var resultLevel1 = _memberService.CalculateMemberLevel(0);
            Assert.IsTrue(resultLevel1.MemberLevel==0);

            var resultLevel2 = _memberService.CalculateMemberLevel(500);
            Assert.IsTrue(resultLevel2.MemberLevel == 1);

            var resultLevel3 = _memberService.CalculateMemberLevel(2000);
            Assert.IsTrue(resultLevel3.MemberLevel ==2);

            var resultLevel4 = _memberService.CalculateMemberLevel(5000);
            Assert.IsTrue(resultLevel4.MemberLevel == 3);
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
