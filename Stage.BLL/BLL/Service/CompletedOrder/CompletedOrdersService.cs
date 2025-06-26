using Stage.DAL.Repositories.Implement;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Data.Models.DataModel;
using Utilities.Nlog;
using Stage.Data.Models;
using Utilities;
using System.Diagnostics.Metrics;

namespace Stage.BLL.BLL.Service
{
    /// <summary>
    /// 結帳功能服務
    /// </summary>
    public interface ICompletedOrdersService
    {
        void AddNewOrderDetail(PB1002_CompletedOrdersDetails completedOrdersDetail);
        List<PB1001_CompletedOrders> GetAllCompletedOrders();
        List<PB1001_CompletedOrders> GetOrderListByDateRange(DateTime StartDate, DateTime EndDate);
        IResponseModel<List<ProductModel>> GetSelectOrderDetailProduct(string orderNumber);
        List<PB1001_CompletedOrders> GetOrderList(string orderNumber);
        bool AddProductOrder(List<ProductModel> productModelList, out string OrderNumber);
        IResponseModel<string> AddProductOrder(List<ProductModel> productModelList, string phoneNumber);

        string CreateNewOrderNumber();
    }
    public class CompletedOrdersService : ICompletedOrdersService
    {
        private ICompletedOrdersRepository _completedOrdersRepository;
        private IShopSerivce _shopSerivce => MainDataCenter.Instance.ShopSerivce;
        private IMemberService _memberService => MainSystemService.Instance.MemberSerivce;
        public CompletedOrdersService(ICompletedOrdersRepository completedOrdersRepository)
        {
            this._completedOrdersRepository = completedOrdersRepository;
        }

        void ICompletedOrdersService.AddNewOrderDetail(PB1002_CompletedOrdersDetails completedOrdersDetail)
        {
            _completedOrdersRepository.AddNewOrderDetail(completedOrdersDetail);
        }

        List<PB1001_CompletedOrders> ICompletedOrdersService.GetAllCompletedOrders()
        {
            return _completedOrdersRepository.GetAllCompletedOrders();
        }
        IResponseModel<List<ProductModel>> ICompletedOrdersService.GetSelectOrderDetailProduct(string orderNumber)
        {
            return _completedOrdersRepository.GetSelectOrderDetailProduct(orderNumber);
        }
        List<PB1001_CompletedOrders> ICompletedOrdersService.GetOrderListByDateRange(DateTime StartDate, DateTime EndDate)
        {
            return _completedOrdersRepository.GetTimeRangeOrderProduct(StartDate, EndDate);
        }

        List<PB1001_CompletedOrders> ICompletedOrdersService.GetOrderList(string orderNumber)
        {
            return _completedOrdersRepository.GetOrderList(orderNumber);

        }
        string ICompletedOrdersService.CreateNewOrderNumber()
        {
            return CreateNewOrderNumber();
        }
        bool ICompletedOrdersService.AddProductOrder(List<ProductModel> productModelList, out string orderNumber)
        {
            try
            {
                orderNumber = CreateNewOrderNumber();
                PB1001_CompletedOrders newCompletedOrder = GetNewCompletedOrder(_shopSerivce.TotalPrice, orderNumber);

                _completedOrdersRepository.AddNewCompletedOrders(newCompletedOrder, productModelList);
                return true;
            }
            catch (Exception e)
            {
                orderNumber = string.Empty;
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return false;
            }
        }
        IResponseModel<string> ICompletedOrdersService.AddProductOrder(List<ProductModel> productModelList, string phoneNumber)
        {
            IResponseModel<string> responseModel = new ResponseModel<string>();
            try
            {
                string orderNumber = string.Empty;
                orderNumber = CreateNewOrderNumber();
                responseModel.Data = orderNumber;

                //建立已結訂單資訊資料
                PB1001_CompletedOrders newCompletedOrder = GetNewCompletedOrder(_shopSerivce.TotalPrice, orderNumber);
                //取得該會員資料
                var memberRespon = _memberService.GetMemberModel(phoneNumber);
                MemberModel member;
                if (memberRespon.IsSuccess)
                    member = memberRespon.Data;
                else
                {
                    responseModel.IsSuccess = false;
                    return responseModel;
                }
                //取得會員消費紀錄總額
                var getCurrentMemberTotalPriceResponse = _completedOrdersRepository.GetCurrentMemberTotalPrice(member.Id);
                responseModel.Message = getCurrentMemberTotalPriceResponse.Message;

                double currentMemberTotalPrice = 0;
                if (getCurrentMemberTotalPriceResponse.IsSuccess)
                    currentMemberTotalPrice = getCurrentMemberTotalPriceResponse.Data;
                else
                {
                    responseModel.IsSuccess = false;
                    return responseModel;
                }
                //更新會員等級 (以之前的金額加上現在的金額進行查詢)
                MB1002_ConsumptionLevelRule newLevel = _memberService.CalculateMemberLevel(currentMemberTotalPrice + _shopSerivce.TotalPrice);
                member.ConsumptionLevel = newLevel;

                if (_completedOrdersRepository.AddNewCompletedOrders(newCompletedOrder, productModelList, member))
                {
                    responseModel.IsSuccess = true;
                    return responseModel;
                }
                else
                {
                    responseModel.IsSuccess = false;
                    return responseModel;
                }

            }
            catch (Exception e)
            {
                responseModel.Data = string.Empty;
                responseModel.IsSuccess = false;
                LogManagerSingleton.Instance.PrintLog(e.Message + ": " + responseModel.Message, NLog.LogLevel.Error);
                return responseModel;
            }
        }
        private PB1001_CompletedOrders GetNewCompletedOrder(double totalPrice, string currentOrderNumber)
        {
            //創立訂單號碼
            PB1001_CompletedOrders completedOrders = new PB1001_CompletedOrders();
            completedOrders.OrderNumber = currentOrderNumber;
            completedOrders.TotalPrice = totalPrice;
            completedOrders.Timestamp = DateTime.Now;
            completedOrders.OrderEmployeeId = MainSystemService.Instance.EmployeeService.CurrentEmployee.Id;
            return completedOrders;
        }
        private string CreateNewOrderNumber()
        {
            int id = _completedOrdersRepository.GetAllCompletedOrders().Count;
            //TODO 之後詳細規則
            return DateTime.Now.ToString("yyyyMMdd") + id.ToString().PadLeft(4, '0');
        }

    }
}
