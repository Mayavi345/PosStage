using Microsoft.Reporting.WinForms;
using Stage.BLL.BLL.Service;
using Stage.ReportViewCore.Service;
using PosStage.MVVM.Models;
using System.Collections.Generic;
using Stage.DAL;
using Stage.BLL.BLL;

namespace Stage.ReportViewCore
{
    public class ConsumptionReportManager : ReportViewManagerBase
    {
        IMemberService _memberService;

        public ConsumptionReportManager(ReportViewer myReportViewr) : base(myReportViewr)
        {
            _memberService = MainSystemService.Instance.MemberSerivce;
        }

        public override void RefreshReportView()
        {
            base.RefreshReportView();
            myReportView.Reset();
            List<MemberConsumptionReportModel> memberCunsumptionList = GetMemberList();
            ReportHelper.UpdateRepoertViewData(myReportView, "MemberCunsumptionDataSet", memberCunsumptionList, "Stage.ReportViewCore.MemberConsumptionReport.rdlc");
        }
        private List<MemberConsumptionReportModel> GetMemberList()
        {
            var response = _memberService.GetMemberConsumptionReportModel();
            if (response.IsSuccess)
            {
                List<MemberConsumptionReportModel> memberConsumptionReportModelsList = response.Data;
                return memberConsumptionReportModelsList;
            }
            else
            {
                System.Windows.MessageBox.Show(response.Message);
                return null;
            }
        }
    }

}
