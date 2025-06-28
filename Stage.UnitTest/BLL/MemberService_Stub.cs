using PosStage.MVVM.Models;
using Stage.BLL.BLL.Service;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Stage.UnitTest.BLL
{
    internal class MemberService_Stub : IMemberService
    {
        // 假設已有這個通用回應模型
        private class StubResponseModel<T> : IResponseModel<T>
        {
            public bool IsSuccess { get; set; }
            public T Data { get; set; }
            public string Message { get; set; }
        }

        public IResponseModel<MB1001_Member> Add(MB1001_Member item)
        {
            // 直接回傳成功，並帶回剛剛傳入的 member
            return new StubResponseModel<MB1001_Member>
            {
                IsSuccess = true,
                Data = item,
                Message = "Stub: Add 成功"
            };
        }

        public IResponseModel<MB1003_MemberOrderInfo> AddMemberOrderInfo(MB1003_MemberOrderInfo memberOrderInfo)
        {
            return new StubResponseModel<MB1003_MemberOrderInfo>
            {
                IsSuccess = true,
                Data = memberOrderInfo,
                Message = "Stub: AddMemberOrderInfo 成功"
            };
        }

        public MB1002_ConsumptionLevelRule CalculateMemberLevel(double allTotalPrice)
        {
            // 回傳一個空的等級規則物件
            return new MB1002_ConsumptionLevelRule
            {

            };
        }

        public IResponseModel<object> Delete(int id)
        {
            return new StubResponseModel<object>
            {
                IsSuccess = true,
                Data = null,
                Message = $"Stub: 已刪除 id={id}"
            };
        }

        public IResponseModel<List<MB1003_MemberOrderInfo>> GetAllMemberOrderInfo()
        {
            return new StubResponseModel<List<MB1003_MemberOrderInfo>>
            {
                IsSuccess = true,
                Data = new List<MB1003_MemberOrderInfo>(),
                Message = "Stub: GetAllMemberOrderInfo"
            };
        }

        public IResponseModel<List<MB1002_ConsumptionLevelRule>> GetLevelList()
        {
            return new StubResponseModel<List<MB1002_ConsumptionLevelRule>>
            {
                IsSuccess = true,
                Data = new List<MB1002_ConsumptionLevelRule> { CalculateMemberLevel(0) },
                Message = "Stub: GetLevelList"
            };
        }

        public IResponseModel<List<MemberConsumptionReportModel>> GetMemberConsumptionReportModel()
        {
            return new StubResponseModel<List<MemberConsumptionReportModel>>
            {
                IsSuccess = true,
                Data = new List<MemberConsumptionReportModel>(),
                Message = "Stub: GetMemberConsumptionReportModel"
            };
        }

        public IResponseModel<MemberModel> GetMemberModel(string phoneNumber)
        {
            return new StubResponseModel<MemberModel>
            {
                IsSuccess = true,
                Data = new MemberModel { PhoneNumber = phoneNumber },
                Message = "Stub: GetMemberModel"
            };
        }

        public IResponseModel<List<MemberModel>> GetMemberModelList()
        {
            return new StubResponseModel<List<MemberModel>>
            {
                IsSuccess = true,
                Data = new List<MemberModel> { new MemberModel() },
                Message = "Stub: GetMemberModelList"
            };
        }

        public IResponseModel<MemberModel> GetMemberOrderInfo(string orderNumber)
        {
            return new StubResponseModel<MemberModel>
            {
                IsSuccess = true,
                Data = new MemberModel(),
                Message = "Stub: GetMemberOrderInfo"
            };
        }

        public bool Update(MB1001_Member member)
        {
            // 回傳 true 表示更新成功
            return true;
        }
    }
}