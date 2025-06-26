using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace State.CommonWPF
{
    /// <summary>
    /// 共同處理的View 系統中心
    /// </summary>
    public class CommonViewSystem : Singleton<CommonViewSystem>
    {
        public Stage.ReportViewCore.MainWindow ReportMainWindow => _reportMainWindow;
        private Stage.ReportViewCore.MainWindow _reportMainWindow = new Stage.ReportViewCore.MainWindow();
        public void Init()
        {
            _reportMainWindow = new Stage.ReportViewCore.MainWindow();
        }
    }
}
