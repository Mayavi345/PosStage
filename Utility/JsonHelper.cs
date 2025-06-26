using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Utilities
{
    public class JsonHelper<T>
    {
        public string FileName { get; }

        public JsonHelper(string fileName)
        {
            FileName = fileName;
        }

        public void CreateJson(T defaultData, bool WriteIndented = true, Action notExistCallback = null)
        {

            string tempfileName = $"{FileName}.json";


            var options = new JsonSerializerOptions()
            {
                WriteIndented = WriteIndented // 換行與縮排
            };
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string jsonString = JsonSerializer.Serialize(defaultData, options);

            if (!File.Exists(tempfileName))
            {
                File.WriteAllText(tempfileName, jsonString);
                notExistCallback?.Invoke();
            }

        }
        public void SaveJson(T defaultData, bool WriteIndented = true)
        {

            string tempfileName = $"{FileName}.json";
            var options = new JsonSerializerOptions()
            {
                WriteIndented = WriteIndented // 換行與縮排
            };
            options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            string jsonString = JsonSerializer.Serialize(defaultData, options);
            File.WriteAllText(tempfileName, jsonString);

        }
        public T LoadJson()
        {
            string tempfileName = $"{FileName}.json";
            string jsonString = File.ReadAllText(tempfileName);
            return JsonSerializer.Deserialize<T>(jsonString);
        }


    }
}
