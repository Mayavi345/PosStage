using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using Stage.Data.Models;
using Stage.Data.Models.DataModel;
using System.Text.Json;
using PosStage.MVVM.Models;

namespace Stage.WebAPI
{
    [Route("api/members")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberSerivce;
        public MemberController()
        {
            //切換DB，使用是Member Repository Sqlite 
            MainSystemService.Instance.InitSystem(EDataDb.MsSqlServer,EServiceType.SQL);
            _memberSerivce = MainSystemService.Instance.MemberSerivce;

        }

        [HttpGet]
        public ActionResult<List<MemberModel>> Get()
        {
            var response = _memberSerivce.GetMemberModelList();
            if (response.IsSuccess == false)
            {
                return NotFound();
            }
            return response.Data;
        }
        [HttpPost("GetMemberModel")]
        public ActionResult<MemberModel> GetMemberModel([FromBody] string phoneNumber)
        {
            //TODO 思考更落實的API 回傳值方式
            var member = _memberSerivce.GetMemberModel(phoneNumber).Data;
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }
        [HttpGet("GetAllMemberOrderInfo")]
        public ActionResult<List<MB1003_MemberOrderInfo>> GetAllMemberOrderInfo()
        {
            var response = _memberSerivce.GetAllMemberOrderInfo();
            if (response.IsSuccess == false)
            {
                return NotFound();
            }
            return response.Data;
        }

        [HttpPost]
        public ActionResult<MB1001_Member> Post([FromBody] MB1001_Member data)
        {
            MB1001_Member newItem;
            var response = _memberSerivce.Add(data);
            newItem = (MB1001_Member)response.Data;

            if (response.IsSuccess)
                return Ok(newItem);
            else
                return NotFound();
        }
        [HttpDelete()]
        public ActionResult<MB1001_Member> Delete([FromQuery] int id)
        {
            var response = _memberSerivce.Delete(id);
            if (response.IsSuccess)
                return Ok();
            else
                return NotFound();
        }
        [HttpPut]
        public IActionResult Put([FromBody] MB1001_Member member)
        {
            bool state = _memberSerivce.Update(member);
            if (state)
                return Ok();
            else
                return NotFound();
        }

        [HttpPost("AddMemberOrderInfo")]
        public ActionResult<MB1001_Member> AddMemberOrderInfo([FromBody] MB1003_MemberOrderInfo data)
        {
            var response = _memberSerivce.AddMemberOrderInfo(data);
            if (response.IsSuccess)
                return Ok(response.Data);
            else
                return NotFound();
        }
        [HttpGet("GetLevelList")]
        public ActionResult<List<MB1002_ConsumptionLevelRule>> GetLevelList()
        {
            var response = _memberSerivce.GetLevelList();
            if (response == null) {
                return NotFound();
            }
            if (response.IsSuccess == false)
            {
                return NotFound();
            }
            return response.Data;
        }
        [HttpGet("GetMemberConsumptionReportModel")]
        public ActionResult<List<MemberConsumptionReportModel>> GetMemberConsumptionReportModel()
        {
            var response = _memberSerivce.GetMemberConsumptionReportModel();
            if (response.IsSuccess == false)
            {
                return NotFound();
            }
            return response.Data;
        }
        [HttpPost("GetMemberOrderInfo")]
        public ActionResult<MemberModel> GetMemberOrderInfo([FromBody] string orderNumber)
        {
            //TODO 思考更落實的API 回傳值方式
            var member = _memberSerivce.GetMemberOrderInfo(orderNumber).Data;
            if (member == null)
            {
                return NotFound();
            }
            return member;
        }
    }
}
