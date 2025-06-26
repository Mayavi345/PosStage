using Azure;
using Azure.Core;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Utilities;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.DAL.Repositories.Implement;
using Utilities.Observer;
using System.Diagnostics.Metrics;
using Utilities.Nlog;

namespace Stage.BLL.BLL.Service
{
    public class MemberServiceWebAPI : IMemberService
    {
        private string ApiServerIP => GetConfigIP();
        private MessageSubject _observerUIMessageBox => MainSystemService.Instance.ObserverUIMessageBox;

        public MemberServiceWebAPI()
        {

        }
        private static string GetConfigIP()
        {
            return MainConfigService.Instance.ApiServerIP;
        }

        IResponseModel<MB1001_Member> IMemberService.Add(MB1001_Member item)
        {
            ResponseAPIModel<MB1001_Member> responseModel = new ResponseAPIModel<MB1001_Member>();
            using (HttpClient httpClient = new HttpClient())
            {
                MB1001_Member requestData = new MB1001_Member()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Gender = item.Gender,
                    PhoneNumber = item.PhoneNumber,
                    MemberLevel = item.MemberLevel,
                    IsDelete = item.IsDelete,
                };
                string url = ApiServerIP + "/api/members/";

                responseModel = HttpHelper.SendHttpRequest<MB1001_Member, object>(url, requestData);
                return responseModel;
            }
        }
        IResponseModel<MB1003_MemberOrderInfo> IMemberService.AddMemberOrderInfo(MB1003_MemberOrderInfo newData)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var requestData = new
                {
                    MemberId = newData.MemberId,
                    OrderNumber = newData.OrderNumber,
                    TotalPrice = newData.OrderNumber,
                    Timestamp = newData.Timestamp
                };
                string url = ApiServerIP + "/api/members/AddMemberOrderInfo";
                var response = HttpHelper.SendHttpRequest<MB1003_MemberOrderInfo, object>(url, requestData);

                return response;
            }
        }
        IResponseModel<object> IMemberService.Delete(int id)
        {
            string url = ApiServerIP + "/api/members/";
            var response = HttpHelper.DeleteHttpRequest<object, object>(url, id);
            return response;

        }
        IResponseModel<List<MB1003_MemberOrderInfo>> IMemberService.GetAllMemberOrderInfo()
        {
            return GetResponseModelBase<List<MB1003_MemberOrderInfo>>("/api/members/GetAllMemberOrderInfo");
        }
        IResponseModel<List<MB1002_ConsumptionLevelRule>> IMemberService.GetLevelList()
        {
            return GetLevelList();
        }

        private IResponseModel<List<MB1002_ConsumptionLevelRule>> GetLevelList()
        {
            return GetResponseModelBase<List<MB1002_ConsumptionLevelRule>>("/api/members/GetLevelList");
        }

        IResponseModel<List<MemberModel>> IMemberService.GetMemberModelList()
        {
            return GetResponseModelBase<List<MemberModel>>("/api/members");
        }
        IResponseModel<T> GetResponseModelBase<T>(string apiRouter)
        {
            IResponseModel<T> responseModel = new ResponseAPIModel<T>();
            try
            {
                string url = ApiServerIP + apiRouter;
                responseModel = HttpHelper.GetHttpRequest<T>(url);

                // 檢查響應是否成功
                if (responseModel.IsSuccess)
                {
                    return responseModel;
                }
                else
                {
                    return responseModel;
                }
            }
            catch (Exception e)
            {
                responseModel.Message = e.Message;
                _observerUIMessageBox.NotifyObservers(e.Message);
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);

                return responseModel;
            }
        }
        IResponseModel<MemberModel> IMemberService.GetMemberModel(string phoneNumber)
        {
            return GetMemberModel(phoneNumber);
        }
        private IResponseModel<MemberModel> GetMemberModel(string phoneNumber)
        {
            IResponseModel<MemberModel> responseModel = new ResponseAPIModel<MemberModel>();
            using (HttpClient httpClient = new HttpClient())
            {
                string url = ApiServerIP + "/api/members/GetMemberModel";
                MemberModel member = new MemberModel();
                responseModel = HttpHelper.SendHttpRequest<MemberModel, object>(url, phoneNumber);
                return responseModel;
            }
        }

        bool IMemberService.Update(MB1001_Member member)
        {
            var requestData = new
            {
                id = member.Id,
                Name = member.Name,
                Gender = member.Gender,
                PhoneNumber = member.PhoneNumber,
                MemberLevel = member.MemberLevel,
                IsDelete = member.IsDelete,
            };
            string url = ApiServerIP + "/api/members";
            string createItem;
            bool success = HttpHelper.SendUpdateHttpRequest(url, requestData, out createItem);
            // 檢查響應是否成功
            if (success)
            {
                return true;
            }
            else
            {
                createItem = null;
                return false;
            }
        }

        MB1002_ConsumptionLevelRule IMemberService.CalculateMemberLevel(double allTotalPrice)
        {
            MB1002_ConsumptionLevelRule ConsumptionLevelRule = new MB1002_ConsumptionLevelRule();
            var response = GetLevelList();
            if (response.IsSuccess)
            {
                //理論上是有序的
                foreach (var levelRule in response.Data)
                {
                    //比較當前的金額到哪一個等級 ，從低到高
                    if (allTotalPrice >= levelRule.ConsumptionPrice)
                    {
                        ConsumptionLevelRule = levelRule;
                    }
                    else
                    //如果比較到等級，就回傳
                        if (ConsumptionLevelRule != null)
                        return ConsumptionLevelRule;
                    else
                        return null;
                }
            }
            return ConsumptionLevelRule;
        }
        IResponseModel<List<MemberConsumptionReportModel>> IMemberService.GetMemberConsumptionReportModel()
        {
            return GetResponseModelBase<List<MemberConsumptionReportModel>>("/api/members/GetMemberConsumptionReportModel");
        }

        IResponseModel<MemberModel> IMemberService.GetMemberOrderInfo(string orderNumber)
        {
            IResponseModel<MemberModel> responseModel = new ResponseAPIModel<MemberModel>();
            using (HttpClient httpClient = new HttpClient())
            {
                string url = ApiServerIP + "/api/members/GetMemberOrderInfo";
                MemberModel member = new MemberModel();
                responseModel = HttpHelper.SendHttpRequest<MemberModel, object>(url, orderNumber);
                return responseModel;
            }
        }
      
    }

}
