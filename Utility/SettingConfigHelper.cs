using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class SettingConfigHelper
    {
        private JsonHelper<SettingConfig> _configJson;
        public string ApiServerIP => GetApiServerIP();
        public SettingConfigHelper(string configPath, Action notExistCallback = null)
        {
            _configJson = new JsonHelper<SettingConfig>(configPath);

            SettingConfig defaultData = new SettingConfig();
            defaultData.DbList = new List<DbData>();
            DbData dbDataA = new DbData()
            {
                ServerName = "(localdb)\\MSSQLLocalDB",
                Db = "posDb",
                Id = string.Empty,
                Password = string.Empty,
                ApiServerIP = "http://(localdb)\\MSSQLLocalDB:8080",
                IsSelected = "1",

            };
            DbData dbDataB = new DbData()
            {
                ServerName = "(localdb)\\MSSQLLocalDB",
                Db = "Pos_DB_2",
                Id = string.Empty,
                Password = string.Empty,
                ApiServerIP = "http://(localdb)\\MSSQLLocalDB:8080",
                IsSelected = "0",

            };
            defaultData.DbList.Add(dbDataA);
            defaultData.DbList.Add(dbDataB);

            _configJson.CreateJson(defaultData, true, () =>
            {
                notExistCallback?.Invoke();
            });
        }
        public SettingConfig SettingData()
        {
            return _configJson.LoadJson();
        }
        public DbData CurrentSelect()
        {
            DbData selectDb = _configJson.LoadJson().DbList.Where(x => x.IsSelected == "1").FirstOrDefault();
            return selectDb;
        }

        public void SetSelected(string Db)
        {
            var currentdata = SettingData();
            foreach (var item in currentdata.DbList)
            {
                if (item.Db == Db)
                {
                    item.IsSelected = "1";
                }
                else
                {
                    item.IsSelected = "0";
                }
            }
            _configJson.SaveJson(currentdata);
        }
        public string Connectstring()
        {
            var currentSelectData = SettingData().DbList.Where(x => x.IsSelected == "1").FirstOrDefault();
            return $"Data Source={currentSelectData.ServerName};Initial Catalog={currentSelectData.Db};User ID={currentSelectData.Id};password={currentSelectData.Password};Encrypt=True;TrustServerCertificate=true;";
        }
        public string GetApiServerIP()
        {
            var currentSelectData = SettingData().DbList.Where(x => x.IsSelected == "1").FirstOrDefault();
            return currentSelectData.ApiServerIP;
        }
    }
    public class DbData
    {
        public string ServerName { get; set; }
        public string Db { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public string ApiServerIP { get; set; }

        public string IsSelected { get; set; }
    }
    public class SettingConfig
    {
        public List<DbData> DbList { get; set; }

    }
}
