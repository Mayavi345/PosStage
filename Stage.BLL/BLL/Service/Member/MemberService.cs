using Stage.DAL.Repositories.Implement;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using System.Collections.Generic;
using System.Reflection;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Utilities;
using Microsoft.AspNetCore.Mvc;
using Utilities.Nlog;

namespace Stage.BLL.BLL.Service
{
    public class MemberService : IMemberService
    {
        private IMemberRepository _memberRepository;
        private ICompletedOrdersService _completedOrdersService;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }
        IResponseModel<MB1001_Member> IMemberService.Add(MB1001_Member item)
        {
            IResponseModel<MB1001_Member> responseModel = new ResponseModel<MB1001_Member>();

            try
            {
                //TODO 之後思考這樣落實拿資料庫創的物件
                if (_memberRepository.Add(item))
                {
                    responseModel.IsSuccess = true;
                    responseModel.Data = item;
                }
                else
                {
                    responseModel.IsSuccess = false;
                    responseModel.Data = null;
                }
                return responseModel;
            }
            catch (Exception e)
            {
                responseModel.IsSuccess = false;
                responseModel.Data = null;
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return responseModel;
            }
        }

        IResponseModel<object> IMemberService.Delete(int newItem)
        {
            IResponseModel<object> responseModel = new ResponseModel<object>();
            try
            {
                responseModel.IsSuccess = true;
                _memberRepository.Delete(newItem);
                return responseModel;
            }
            catch (Exception e)
            {
                responseModel.IsSuccess = false;
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return responseModel;
            }
        }
        IResponseModel<List<MemberModel>> IMemberService.GetMemberModelList()
        {
            return BaseResponseModel(_memberRepository.GetMemberModelList);
        }
        private MB1002_ConsumptionLevelRule GetConsumptionLevel(double Consumption)
        {
            MB1002_ConsumptionLevelRule ConsumptionLevel = _memberRepository.GetConsumptionLevel(Consumption);
            return ConsumptionLevel;
        }
        #region ConvertToMemberModel
        //private MemberModel ConvertToMemberModel(Member member)
        //{
        //    MemberModel memberModel = new MemberModel();
        //    memberModel.Id = member.Id;
        //    memberModel.Name = member.Name;
        //    memberModel.Gender = member.Gender;
        //    memberModel.PhoneNumber = member.PhoneNumber;
        //    memberModel.Level = member.MemberLevel;
        //    return memberModel;
        //}

        #endregion
        IResponseModel<List<MB1002_ConsumptionLevelRule>> IMemberService.GetLevelList()
        {
            return BaseResponseModel(_memberRepository.GetLevelList);
            IResponseModel<List<MB1002_ConsumptionLevelRule>> responseModel = new ResponseModel<List<MB1002_ConsumptionLevelRule>>();
            try
            {
                responseModel.IsSuccess = true;
                responseModel.Data = _memberRepository.GetLevelList();
                return responseModel;
            }
            catch
            {
                responseModel.IsSuccess = false;
                responseModel.Data = null;
                return responseModel;
            }
        }
        IResponseModel<T> BaseResponseModel<T>(Func<T> getDataFunc)
        {
            IResponseModel<T> responseModel = new ResponseModel<T>();
            try
            {
                responseModel.IsSuccess = true;
                responseModel.Data = getDataFunc();
                return responseModel;
            }
            catch (Exception e)
            {
                responseModel.IsSuccess = false;
                responseModel.Data = default;
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return responseModel;
            }
        }
        bool IMemberService.Update(MB1001_Member member)
        {
            try
            {
                if (_memberRepository.Update(member))
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return false;
            }

        }
        IResponseModel<MB1003_MemberOrderInfo> IMemberService.AddMemberOrderInfo(MB1003_MemberOrderInfo memberOrderInfo)
        {
            return _memberRepository.AddMemberOrderInfo(memberOrderInfo);
        }

        IResponseModel<List<MB1003_MemberOrderInfo>> IMemberService.GetAllMemberOrderInfo()
        {
            return BaseResponseModel(_memberRepository.GetAllMemberOrderInfo);
        }

        IResponseModel<MemberModel> IMemberService.GetMemberModel(string phoneNumber)
        {
            IResponseModel<MemberModel> responseModel = new ResponseModel<MemberModel>();

            MemberModel currentMember = _memberRepository.GetMember(phoneNumber);

            if (currentMember != null)
            {
                responseModel.IsSuccess = true;
                responseModel.Data = currentMember;
                responseModel.Message = "找到會員";
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.Message = "找不到該會員";
            }
            return responseModel;

        }

        MB1002_ConsumptionLevelRule IMemberService.CalculateMemberLevel(double allTotalPrice)
        {
            MB1002_ConsumptionLevelRule ConsumptionLevelRule = new MB1002_ConsumptionLevelRule();
            var response = BaseResponseModel(_memberRepository.GetLevelList);

            if (response.IsSuccess)
            {
                //理論上是有序的
                foreach (var levelRule in response.Data)
                {
                    if (allTotalPrice >= levelRule.ConsumptionPrice)
                        ConsumptionLevelRule = levelRule;
                    else
                        return response.Data[0];
                }
            }
            return ConsumptionLevelRule;
        }

        IResponseModel<List<MemberConsumptionReportModel>> IMemberService.GetMemberConsumptionReportModel()
        {
            return BaseResponseModel(_memberRepository.GetMemberConsumptionReportModel);
        }

        IResponseModel<MemberModel> IMemberService.GetMemberOrderInfo(string orderNumber)
        {
            IResponseModel<MemberModel> responseModel = new ResponseModel<MemberModel>();

            MemberModel currentMember = _memberRepository.GetMemberOrderInfo(orderNumber);

            if (currentMember != null)
            {
                responseModel.IsSuccess = true;
                responseModel.Data = currentMember;
                responseModel.Message = "找到會員";
            }
            else
            {
                responseModel.IsSuccess = false;
                responseModel.Message = "找不到該會員";
            }
            return responseModel;
        }
    }

}
