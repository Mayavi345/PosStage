using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using PosStage.MVVM.Models;
using Utilities;

namespace Stage.DAL.Repositories.Implement
{
    public interface IMemberRepository
    {
        bool Add(MB1001_Member newItem);
        IResponseModel<MB1003_MemberOrderInfo> AddMemberOrderInfo(MB1003_MemberOrderInfo memberOrderInfo1);
        void Delete(int memberId);
        List<MB1003_MemberOrderInfo> GetAllMemberOrderInfo();
        MB1002_ConsumptionLevelRule GetConsumptionLevel(double Consumption);
        List<MB1002_ConsumptionLevelRule> GetLevelList();
        MemberModel GetMember(string phoneNumber);
        List<MemberModel> GetMemberModelList();
        /// <summary>
        /// 取得會員消費的總額
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Update(MB1001_Member member);
        List<MemberConsumptionReportModel> GetMemberConsumptionReportModel();
        MemberModel GetMemberOrderInfo(string orderNumber);
    }
}
