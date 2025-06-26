using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Nlog;

namespace Utilities
{
    public class HttpHelper
    {

        public HttpHelper()
        {

        }
        public static ResponseAPIModel<T> GetHttpRequest<T>(string url)
        {
            ResponseAPIModel<T> responseModel = new ResponseAPIModel<T>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {    // 發送 POST 請求
                    HttpResponseMessage response = httpClient.GetAsync(url).Result;
                    responseModel.Code = response.StatusCode.ToString();
                    responseModel.Message = response.ToString();

                    if (response.IsSuccessStatusCode)
                    {
                        // 解析回應內容為指定類型 TResponse
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        responseModel.Data = JsonConvert.DeserializeObject<T>(responseContent);
                        responseModel.IsSuccess = true;

                        return responseModel;
                    }
                    else
                    {
                        responseModel.Code = response.StatusCode.ToString();
                        responseModel.Message = response.ToString();
                        responseModel.IsSuccess = false;

                        return responseModel;
                    }
                }
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                responseModel.Message = e.ToString();
                responseModel.IsSuccess = false;
                return responseModel;
            }
        }
        //public static IResponseModel<T> GetResponseModelBase<T>(string url)
        //{
        //    IResponseModel<T> responseModel = new ResponseAPIModel<T>();
        //    try
        //    {
        //        responseModel = GetHttpRequest<T>(url);

        //        // 檢查響應是否成功
        //        if (responseModel.IsSuccess)
        //        {
        //            return responseModel;
        //        }
        //        else
        //        {
        //            return responseModel;
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        responseModel.Message = e.Message;
        //        LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);

        //        return responseModel;
        //    }
        //}
        public static ResponseAPIModel<T> SendHttpRequest<T, TRequest>(string url, TRequest requestData)
        {
            ResponseAPIModel<T> responseModel = new ResponseAPIModel<T>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // 將請求資料序列化為 JSON 字串
                    string jsonData = JsonConvert.SerializeObject(requestData);

                    // 建立 HTTP 內容
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // 發送 POST 請求
                    HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                    responseModel.Code = response.StatusCode.ToString();
                    responseModel.Message = response.ToString();

                    if (response.IsSuccessStatusCode)
                    {
                        // 解析回應內容為指定類型 TResponse
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        responseModel.Data = JsonConvert.DeserializeObject<T>(responseContent);
                        responseModel.IsSuccess = true;
                        return responseModel;
                    }
                    else
                    {
                        responseModel.Code = response.StatusCode.ToString();
                        responseModel.Message = response.ToString();
                        responseModel.IsSuccess = false;
                        return responseModel;
                    }
                }
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                responseModel.Message = e.ToString();
                responseModel.IsSuccess = false;
                return responseModel;
            }
        }
        public static ResponseAPIModel<T> DeleteHttpRequest<T, TRequest>(string baseUrl, TRequest requestData)
        {
            ResponseAPIModel<T> responseModel = new ResponseAPIModel<T>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // 定義參數
                    string param1 = requestData.ToString();

                    // 使用 string.Format 或字串相結合的方式構建完整的 URL
                    string url = $"{baseUrl}?id={param1}";
                    //string url = $"{baseUrl}?param1={param1}&param2={param2}";

                    // 發送 POST 請求
                    HttpResponseMessage response = httpClient.DeleteAsync(url).Result;
                    responseModel.Code = response.StatusCode.ToString();
                    responseModel.Message = response.ToString();

                    if (response.IsSuccessStatusCode)
                    {
                        // 解析回應內容為指定類型 TResponse
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        responseModel.Data = JsonConvert.DeserializeObject<T>(responseContent);
                        responseModel.IsSuccess = true;
                        return responseModel;
                    }
                    else
                    {
                        responseModel.Code = response.StatusCode.ToString();
                        responseModel.Message = response.ToString();
                        responseModel.IsSuccess = false;
                        return responseModel;
                    }
                }
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                responseModel.Message = e.ToString();
                responseModel.IsSuccess = false;
                return responseModel;
            }
        }
        public static bool SendUpdateHttpRequest<TRequest, TResponse>(string url, TRequest requestData, out TResponse responseResult)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // 將請求資料序列化為 JSON 字串
                    string jsonData = JsonConvert.SerializeObject(requestData);

                    // 建立 HTTP 內容
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // 發送 POST 請求
                    HttpResponseMessage response = httpClient.PutAsync(url, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // 解析回應內容為指定類型 TResponse
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        responseResult = JsonConvert.DeserializeObject<TResponse>(responseContent);
                        return true;
                    }
                    else
                    {
                        responseResult = default(TResponse);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                // 處理異常情況
                responseResult = default(TResponse);
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return false;
            }
        }
    }
}
