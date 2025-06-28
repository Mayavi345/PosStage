using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using PosStage.DAL;
using PosStage.MVVM.Models;
using Utilities;

namespace Stage.DAL.Repositories.Implement
{
    public class MemberRepositoryInMemory : MemberRepositoryEFBase, IMemberRepository
    {
        public override void InitDbContext()
        {
            _dbContext = new MemberDbInMemoryContext();
        }

        bool IMemberRepository.Add(MB1001_Member newItem)
        {
            return Add(newItem);
        }

        IResponseModel<MB1003_MemberOrderInfo> IMemberRepository.AddMemberOrderInfo(MB1003_MemberOrderInfo memberOrderInfo)
        {
            return AddMemberOrderInfo(memberOrderInfo);
        }

        void IMemberRepository.Delete(int memberId)
        {
            Delete(memberId);
        }

        List<MB1003_MemberOrderInfo> IMemberRepository.GetAllMemberOrderInfo()
        {
            return GetAllMemberOrderInfo();
        }

        MB1002_ConsumptionLevelRule IMemberRepository.GetConsumptionLevel(double Consumption)
        {
            return GetConsumptionLevel(Consumption);
        }

        List<MB1002_ConsumptionLevelRule> IMemberRepository.GetLevelList()
        {
            return GetLevelList();
        }

        MemberModel IMemberRepository.GetMember(string phoneNumber)
        {
            return GetMember(phoneNumber);
        }

        List<MemberModel> IMemberRepository.GetMemberModelList()
        {
            return GetMemberModelList();
        }

        bool IMemberRepository.Update(MB1001_Member item)
        {
            return Update(item);
        }

        List<MemberConsumptionReportModel> IMemberRepository.GetMemberConsumptionReportModel()
        {
            return GetMemberConsumptionReportModel();
        }

        MemberModel IMemberRepository.GetMemberOrderInfo(string orderNumber)
        {
            return GetMemberOrderInfo(orderNumber);
        }
    }
}
