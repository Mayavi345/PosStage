using Microsoft.EntityFrameworkCore;
using PosStage.MVVM.Models;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using Utilities;
using Utilities.Nlog;

namespace Stage.DAL.Repositories.Implement
{
    public abstract class MemberRepositoryEFBase
    {
        public MemberDbcontextbase _dbContext;

        public abstract void InitDbContext();
        public MemberRepositoryEFBase()
        {
            InitDbContext();
        }
        public bool Add(MB1001_Member newItem)
        {
            try
            {
                var isExist = _dbContext.MB1001_Member.Any(x => x.PhoneNumber == newItem.PhoneNumber);
                if (isExist)
                {
                    var currentItem = _dbContext.MB1001_Member.Where(x => x.Id == newItem.Id).FirstOrDefault();
                    Update(currentItem);
                    return true;
                }
                _dbContext.MB1001_Member.Add(newItem);
                _dbContext.SaveChanges();
                return true;

            }
            catch (Exception e)
            {
                return false;
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
            }
        }
        public IResponseModel<MB1003_MemberOrderInfo> AddMemberOrderInfo(MB1003_MemberOrderInfo memberOrderInfo)
        {
            IResponseModel<MB1003_MemberOrderInfo> responseModel = new ResponseModel<MB1003_MemberOrderInfo>();
            try
            {
                var isExist = _dbContext.MB1003_MemberOrderInfo.Any(x => x.OrderNumber == memberOrderInfo.OrderNumber);
                if (isExist)
                {
                    _dbContext.MB1003_MemberOrderInfo.Update(memberOrderInfo);
                    //理論上不會出現重複訂單，針對測試案例防呆
                    responseModel.Message = ($"訂單已經存在:{memberOrderInfo.OrderNumber}");
                    responseModel.Data = memberOrderInfo;
                    responseModel.IsSuccess = true;
                    return responseModel;
                }
                _dbContext.MB1003_MemberOrderInfo.Add(memberOrderInfo);
                _dbContext.SaveChanges();
                responseModel.Data = memberOrderInfo;
                responseModel.IsSuccess = true;
                return responseModel;
            }
            catch (Exception e)
            {
                memberOrderInfo = null;
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                responseModel.Data = memberOrderInfo;
                responseModel.IsSuccess = false;
                return responseModel;
            }
        }

        public void Delete(int memberId)
        {
            try
            {
                var currentItem = _dbContext.MB1001_Member.Where(x => x.Id == memberId).FirstOrDefault();
                currentItem.IsDelete = true;
                _dbContext.MB1001_Member.Update(currentItem);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
            }
        }

        public List<MB1003_MemberOrderInfo> GetAllMemberOrderInfo()
        {
            try
            {
                return _dbContext.MB1003_MemberOrderInfo.ToList();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return null;
            }
        }

        public MB1002_ConsumptionLevelRule GetConsumptionLevel(double Consumption)
        {
            try
            {
                return _dbContext.MB1002_ConsumptionLevelRule.Where(x => x.ConsumptionPrice >= Consumption)
                                .Where(x => x.ConsumptionPrice <= Consumption).FirstOrDefault();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return null;
            }
        }

        public List<MB1002_ConsumptionLevelRule> GetLevelList()
        {
            try
            {
                return _dbContext.MB1002_ConsumptionLevelRule.ToList();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return null;
            }
        }

        public MemberModel GetMember(string phoneNumber)
        {
            try
            {
                var query = from member in _dbContext.MB1001_Member
                            join ConsumptionLevel in _dbContext.MB1002_ConsumptionLevelRule
                            on member.MemberLevel equals ConsumptionLevel.MemberLevel
                            where member.IsDelete == false
                            where member.PhoneNumber == phoneNumber
                            select new MemberModel
                            {
                                Id = member.Id,
                                ConsumptionLevel = ConsumptionLevel,
                                Name = member.Name,
                                Gender = MemberModel.GetGender(member.Gender),
                                PhoneNumber = member.PhoneNumber,

                            };
                var result = query.FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return null;
            }
        }

        public List<MemberConsumptionReportModel> GetMemberConsumptionReportModel()
        {
            try
            {
                //實作練習EF 的SQL Raw
                string sql =
$@"
SELECT          MB1001_Member.Name AS MemberName, 
                CASE WHEN MB1001_Member.Gender = 1 THEN '男性' ELSE '女性' END AS MemberGender,
                MB1002_ConsumptionLevelRule.Name AS ConsumptionName, 
                SUM( MemberOrderInfo.TotalPrice) AS OrderTotalPrice
				,CONVERT(datetime2, MemberOrderInfo.Timestamp) AS OrderDate
FROM            MB1001_Member 
INNER JOIN
                    MB1002_ConsumptionLevelRule ON  MB1001_Member.MemberLevel = MB1002_ConsumptionLevelRule.MemberLevel 
INNER JOIN
                    MB1003_MemberOrderInfo AS MemberOrderInfo  ON MB1001_Member.Id = MemberOrderInfo.MemberId 
GROUP BY 
    MB1001_Member.Name,
    MB1001_Member.Gender,
    MB1002_ConsumptionLevelRule.Name
	,CONVERT(datetime2, MemberOrderInfo.Timestamp) 
;"
;
                //var result = _dbContext.Database.ExecuteSqlRaw(sql);
                List<MemberConsumptionReportModel> result = _dbContext.MemberConsumptionReportModel.FromSqlRaw(sql).ToList();

                return result;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return null;
            }
        }

        public List<MemberModel> GetMemberModelList()
        {
            try
            {
                var query = from member in _dbContext.MB1001_Member
                            join ConsumptionLevel in _dbContext.MB1002_ConsumptionLevelRule
                            on member.MemberLevel equals ConsumptionLevel.MemberLevel
                            where member.IsDelete == false
                            select new MemberModel
                            {
                                Id = member.Id,
                                ConsumptionLevel = ConsumptionLevel,
                                Name = member.Name,
                                Gender = MemberModel.GetGender(member.Gender),
                                PhoneNumber = member.PhoneNumber,

                            };
                var results = query.ToList();
                return results;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return null;
            }
        }

        public bool Update(MB1001_Member item)
        {
            try
            {
                MB1001_Member currentItem = _dbContext.MB1001_Member.Where(x => x.Id == item.Id).FirstOrDefault();
                currentItem.Name = item.Name;
                currentItem.Gender = item.Gender;
                currentItem.PhoneNumber = item.PhoneNumber;
                currentItem.MemberLevel = item.MemberLevel;
                currentItem.IsDelete = false;

                _dbContext.MB1001_Member.Update(currentItem);


                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return false;

            }
        }
        public MemberModel GetMemberOrderInfo(string orderNumber)
        {
            try
            {
                var query = from member in _dbContext.MB1001_Member
                            join ConsumptionLevel in _dbContext.MB1002_ConsumptionLevelRule
                            on member.MemberLevel equals ConsumptionLevel.MemberLevel
                            join MemberOrderInfo in _dbContext.MB1003_MemberOrderInfo
                            on member.Id equals MemberOrderInfo.MemberId
                            where member.IsDelete == false
                            where MemberOrderInfo.OrderNumber == orderNumber
                            select new MemberModel
                            {
                                Id = member.Id,
                                ConsumptionLevel = ConsumptionLevel,
                                Name = member.Name,
                                Gender = MemberModel.GetGender(member.Gender),
                                PhoneNumber = member.PhoneNumber,
                            };
                var result = query.FirstOrDefault();
                return result;
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return null;
            }
        }
    }
}