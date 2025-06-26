using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using PosStage.DAL;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using Stage.Data.Models.Implement_Model;
using Utilities;
using Utilities.Nlog;

namespace Stage.DAL.Repositories.Implement
{
    public interface ICompletedOrdersRepository
    {
        bool AddNewCompletedOrders(PB1001_CompletedOrders product, List<ProductModel> productModelList);
        /// <summary>
        /// 新增已結訂單
        /// </summary>
        /// <param name="product"></param>
        /// <param name="productModelList"></param>
        /// <param name="memberId"></param>
        /// <param name=""></param>
        /// <returns></returns>
        bool AddNewCompletedOrders(PB1001_CompletedOrders product, List<ProductModel> productModelList, MemberModel memberModel);

        List<PB1001_CompletedOrders> GetAllCompletedOrders();
        List<PB1002_CompletedOrdersDetails> GetCompletedOrdersById(string productId);
        void UpdateCompletedOrders(PB1001_CompletedOrders changedProduct);
        void AddNewOrderDetail(PB1002_CompletedOrdersDetails item);
        void AddNewOrderDetail(List<PB1002_CompletedOrdersDetails> item);

        IResponseModel<List<ProductModel>> GetSelectOrderDetailProduct(string orderNumber);
        List<PB1001_CompletedOrders> GetTimeRangeOrderProduct(DateTime StartDate, DateTime EndDate);
        List<PB1001_CompletedOrders> GetOrderList(string orderNumber);

        IResponseModel<double> GetCurrentMemberTotalPrice(int id);
    }
    public class CompletedOrdersRepositoryEF : ICompletedOrdersRepository
    {
        private PosDbContext _dbContext;
        private string ApiServerIP => MainConfigService.Instance.ApiServerIP;
        public CompletedOrdersRepositoryEF()
        {
            //TODO 改DI
            _dbContext = new PosDbContext();
        }
        public void AddNewOrderDetail(PB1002_CompletedOrdersDetails item)
        {
            _dbContext.PB1002_CompletedOrdersDetails.Add(item);
            _dbContext.SaveChanges();
        }
        public void AddNewOrderDetail(List<PB1002_CompletedOrdersDetails> items)
        {
            _dbContext.PB1002_CompletedOrdersDetails.AddRange(items);
            _dbContext.SaveChanges();
        }
        public List<PB1001_CompletedOrders> GetAllCompletedOrders()
        {
            return _dbContext.PB1001_CompletedOrders.ToList();
        }
        public List<PB1002_CompletedOrdersDetails> GetCompletedOrdersById(string orderNumber)
        {
            return _dbContext.PB1002_CompletedOrdersDetails.Where(x => x.OrderNumber == orderNumber).ToList();
        }

        public IResponseModel<List<ProductModel>> GetSelectOrderDetailProduct(string orderNumber)
        {
            IResponseModel<List<ProductModel>> responseModel = new ResponseModel<List<ProductModel>>();

            try
            {
                var allProduct = from products in _dbContext.PB3001_Product
                                 join detail in _dbContext.PB3002_ProductDetail on products.ProductId equals detail.ProductId
                                 join orders in _dbContext.PB1002_CompletedOrdersDetails on products.ProductId equals orders.ProductID
                                 join categories in _dbContext.PB3003_Categories on detail.CategoryId equals categories.CategoryId
                                 where orderNumber == orders.OrderNumber
                                 select ProductModel.MapProductModel(products, detail, orders, categories);
                var results = allProduct.ToList();

                responseModel.Data = results;
                responseModel.IsSuccess = true;
                return responseModel;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                responseModel.IsSuccess = false;
                responseModel.Message = e.Message;
                return responseModel;
                throw;
            }
        }


        bool ICompletedOrdersRepository.AddNewCompletedOrders
            (PB1001_CompletedOrders newCompletedOrder, List<ProductModel> productModelList, MemberModel member)
        {
            try
            {
                //新增產品
                _dbContext.PB1001_CompletedOrders.Add(newCompletedOrder);
                //新增詳細產品資訊
                CreateNewOrderDetail(productModelList, newCompletedOrder.OrderNumber);

                //新增會員的點餐資訊 (Web Api)
                MB1003_MemberOrderInfo newMemberOrderInfo = new MB1003_MemberOrderInfo()
                {
                    MemberId = member.Id,
                    OrderNumber = newCompletedOrder.OrderNumber,
                    TotalPrice = newCompletedOrder.TotalPrice,
                    Timestamp = newCompletedOrder.Timestamp

                };
                if (AddOrderInfoOfMember(newMemberOrderInfo))
                {
                    if (UpdateMember(member.ConvertToMember()))
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                //API 確定做完，資料庫再存
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return false;
                throw;
            }

        }
        //TODO API應該要在Service 部分
        private bool UpdateMember(MB1001_Member member)
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

        bool ICompletedOrdersRepository.AddNewCompletedOrders(PB1001_CompletedOrders completedOrders, List<ProductModel> productModelList)
        {
            try
            {
                //新增產品
                _dbContext.PB1001_CompletedOrders.Add(completedOrders);
                //新增詳細產品資訊
                CreateNewOrderDetail(productModelList, completedOrders.OrderNumber);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return false;
            }
        }
        private void CreateNewOrderDetail(List<ProductModel> productModelList, string currentOrderNumber)
        {
            List<PB1002_CompletedOrdersDetails> CompletedOrdersDetailList = new List<PB1002_CompletedOrdersDetails>();
            foreach (var cartItem in productModelList)
            {
                PB1002_CompletedOrdersDetails completedOrdersDetail = new PB1002_CompletedOrdersDetails();
                completedOrdersDetail.ProductID = cartItem.ProductId;
                completedOrdersDetail.OrderNumber = currentOrderNumber;
                completedOrdersDetail.Price = cartItem.Price;
                completedOrdersDetail.Quantity = cartItem.Quantity;
                CompletedOrdersDetailList.Add(completedOrdersDetail);
            }
            AddNewOrderDetail(CompletedOrdersDetailList);
        }
        //TODO API應該要在Service 部分
        bool AddOrderInfoOfMember(MB1003_MemberOrderInfo newData)
        {
            var requestData = new
            {
                MemberId = newData.MemberId,
                OrderNumber = newData.OrderNumber,
                TotalPrice = newData.TotalPrice,
                Timestamp = newData.Timestamp
            };
            string url = ApiServerIP + "/api/members/AddMemberOrderInfo";

            var response = HttpHelper.SendHttpRequest<MB1003_MemberOrderInfo, object>(url, requestData);
            var success = response.IsSuccess;

            // 檢查響應是否成功
            if (success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //TODO API應該要在Service 部分
        IResponseModel<List<MB1003_MemberOrderInfo>> GetAllMemberOrderInfo()
        {
            var responseModel = HttpHelper.GetHttpRequest<List<MB1003_MemberOrderInfo>>(ApiServerIP + "/api/members/GetAllMemberOrderInfo");
            return responseModel;
            #region Old Http API
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    HttpHelper httpHelper = new HttpHelper();
            //    string url = ApiServerIP + "/api/members/GetAllMemberOrderInfo";

            //    bool success = httpHelper.GetHttpRequest(url, out memberOrderInfoList);
            //    // 檢查響應是否成功
            //    if (success)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        memberOrderInfoList = null;
            //        return false;
            //    }
            //}
            #endregion
        }
        public void UpdateCompletedOrders(PB1001_CompletedOrders changedProduct)
        {
            var oProduct = _dbContext.PB1001_CompletedOrders.Find(changedProduct.OrderNumber);
            MapCompleteOders(changedProduct, oProduct);
            _dbContext.SaveChanges();
        }

        private static void MapCompleteOders(PB1001_CompletedOrders changedProduct, PB1001_CompletedOrders oProduct)
        {
            oProduct.OrderNumber = changedProduct.OrderNumber;
            oProduct.TotalPrice = changedProduct.TotalPrice;
            oProduct.Timestamp = changedProduct.Timestamp;
            oProduct.OrderEmployeeId = changedProduct.OrderEmployeeId;
        }

        List<PB1001_CompletedOrders> ICompletedOrdersRepository.GetTimeRangeOrderProduct(DateTime StartDate, DateTime EndDate)
        {
            try
            {
                var result = _dbContext.PB1001_CompletedOrders.Where(x => x.Timestamp <= EndDate && x.Timestamp >= StartDate).ToList();
                return result;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                throw;
            }
        }
        List<PB1001_CompletedOrders> ICompletedOrdersRepository.GetOrderList(string orderNumber)
        {
            return _dbContext.PB1001_CompletedOrders.ToList();
        }
        IResponseModel<double> ICompletedOrdersRepository.GetCurrentMemberTotalPrice(int id)
        {
            IResponseModel<double> responseModel = new ResponseModel<double>();
            var response = GetAllMemberOrderInfo();
            if (response.IsSuccess)
            {
                List<MB1003_MemberOrderInfo> memberOrderInfoList = response.Data;
                var totalPrice = memberOrderInfoList.Where(x => x.MemberId == id).Sum(x => x.TotalPrice);
                ////查詢該會員所有的消費金額
                //var query = (from memberOrderInfo in memberOrderInfoList
                //             join completedOrders in _dbContext.PB1001_CompletedOrders on memberOrderInfo.OrderNumber equals completedOrders.OrderNumber
                //             where memberOrderInfo.MemberId == id
                //             group completedOrders by memberOrderInfo.MemberId into groupedOrders
                //             select new
                //             {
                //                 MemberId = groupedOrders.Key,
                //                 TotalPriceSum = groupedOrders.Sum(o => o.TotalPrice)
                //             }.TotalPriceSum);

                //var allTotalPrice = query.FirstOrDefault();

                responseModel.Data = totalPrice;
                responseModel.IsSuccess = true;
                return responseModel;
            }
            else
            {
                responseModel.Data = 0;
                responseModel.IsSuccess = false;
                responseModel.Message = response.Message;
                return responseModel;
                throw new Exception();
            }
        }
    }
}
