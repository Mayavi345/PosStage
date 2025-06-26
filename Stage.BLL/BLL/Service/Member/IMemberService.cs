using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using PosStage.MVVM.Models;
using Utilities;

namespace Stage.BLL.BLL.Service
{
    public interface IMemberService
    {
        IResponseModel<MB1001_Member> Add(MB1001_Member item);
        bool Update(MB1001_Member member);
        IResponseModel<object> Delete(int id);
        IResponseModel<List<MemberModel>> GetMemberModelList();
        IResponseModel<MB1003_MemberOrderInfo> AddMemberOrderInfo(MB1003_MemberOrderInfo memberOrderInfo);
        IResponseModel<List<MB1002_ConsumptionLevelRule>> GetLevelList();
        IResponseModel<List<MB1003_MemberOrderInfo>> GetAllMemberOrderInfo();
        IResponseModel<MemberModel> GetMemberOrderInfo(string orderNumber);
        IResponseModel<MemberModel> GetMemberModel(string phoneNumber);
        MB1002_ConsumptionLevelRule CalculateMemberLevel(double allTotalPrice);
        IResponseModel<List<MemberConsumptionReportModel>> GetMemberConsumptionReportModel();
    }

}
