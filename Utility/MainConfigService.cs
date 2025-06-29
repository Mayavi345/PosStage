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
            ApiServerIP = Environment.GetEnvironmentVariable("POS_APISERVER_IP") ?? string.Empty;
            ConnectionString = Environment.GetEnvironmentVariable("POS_DB_CONNECTIONSTRING") ?? string.Empty;

            if (string.IsNullOrEmpty(ApiServerIP) || string.IsNullOrEmpty(ConnectionString))
            {
                notExistCallback?.Invoke();
            }
        }
    }
}
