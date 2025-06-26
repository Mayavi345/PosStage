using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Utilities
{
    public class MainConfigService : Singleton<MainConfigService>
    {
        public string ConnectionString { get; private set; }
        public string ApiServerIP { get; private set; }

        public void InitConfig(Action notExistCallback = null)
        {
            SettingConfigHelper settingConfig =
                new SettingConfigHelper("Setting", notExistCallback);

            ApiServerIP = settingConfig.ApiServerIP;
            ConnectionString = settingConfig.Connectstring(); ;
        }
    }
}
