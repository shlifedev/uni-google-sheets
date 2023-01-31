using GoogleSheet.Protocol.v2.Req;
using GoogleSheet.Protocol.v2.Res;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace GoogleSheet
{
    public static class HttpUtils
    {
        public static string ToQueryString(object data, string password = null)
        {
            var fields = (from p in data.GetType().GetFields()
                          where p.GetValue(data) != null
                          select p).ToList();



            var properties = from p in data.GetType().GetFields()
                             where p.GetValue(data) != null
                             select p.Name + "=" + System.Uri.EscapeUriString(p.GetValue(data).ToString());

            return "?" + String.Join("&", properties.ToArray()) + $"&password={System.Uri.EscapeUriString(password)}";
        }
    }
    public class ScriptRequester : IHttpProtcol
    {
        public static ScriptRequester Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScriptRequester();

                return instance;
            }
        }
        static ScriptRequester instance;

        public void Credential(string appsScriptUrl, string password)
        {
            _baseURL = appsScriptUrl;
            _password = password;
        }
        private static string _baseURL = "";
        private static string _password = "";


        public static bool IsCredential() { return _baseURL != null; }
        private void Post<T>(string json, Action<System.Exception> errCallback, Action<T> callback) where T : Response
        {
            try
            {

                //credential
                JObject jo = JObject.Parse(json);
                jo.Add("password", _password);
                json = jo.ToString();
                var reqJson = jo.ToString();

                Console.WriteLine(reqJson);

                WebRequest request = WebRequest.Create(_baseURL);
                request.Method = "POST";
                request.Timeout = 15000;
                byte[] data = Encoding.UTF8.GetBytes(reqJson);
                request.ContentType = "application/json";
                request.ContentLength = data.Length;

                Stream ds = request.GetRequestStream();
                ds.Write(data, 0, data.Length);
                ds.Close();


                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                var statusCode = ((HttpWebResponse)response).StatusCode;
                string responseFromServer = "";

                if (statusCode == HttpStatusCode.RequestTimeout)
                {
                    Console.WriteLine("Timeout - UGS Initialize Failed! Try Check Setting Window.");
                    callback?.Invoke(null);
                }

                if (statusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        var resObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseFromServer);
                        if (resObject != null && !resObject.hasError())
                        {
                            callback?.Invoke(resObject);
                            return;
                        }
                        else
                        {
                            if (resObject == null) throw new System.Exception("Response data is null");
                            if (resObject.hasError()) throw new UGSWebError(resObject.error.message);
                        }
                    }
                }
                else
                {
                    throw new System.Exception("HTTP STATUS ERROR");
                }
                response.Close();
            }
            catch (System.Exception e)
            {
                errCallback?.Invoke(e);
            }
        }
        private void Get<T>(string url, Action<System.Exception> errCallback, Action<T> callback) where T : Response
        {
            try
            {
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 30000;
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                var statusCode = ((HttpWebResponse)response).StatusCode;
                string responseFromServer = "";
                if (statusCode == HttpStatusCode.RequestTimeout)
                {
                    callback?.Invoke(null);
                }

                if (statusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        Console.WriteLine(url);
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        Console.WriteLine("get response => " + responseFromServer);
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseFromServer);
                        if (data != null && !data.hasError())
                        {
                            callback?.Invoke(data);
                        }
                        else
                        {
                            if (data == null) throw new System.Exception("Response data is null");
                            if (data.hasError()) throw new UGSWebError(data.error.message);
                        }
                    }
                }
                else
                {
                    throw new System.Exception("Http Status Error");
                }
                response.Close();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message + "\n" + e.StackTrace);
                errCallback?.Invoke(e);
            }
        }



        public void GetDriveDirectory(GetDriveDirectoryReqModel mdl, Action<System.Exception> errCallback, Action<GetDriveFolderResult> callback)
        {
            string url = $"{_baseURL}{HttpUtils.ToQueryString(mdl, _password)}";
            Get<GetDriveFolderResult>(url, errCallback, (result) =>
            {
                if (result != null) callback?.Invoke(result);
            });
        }

        public void ReadSpreadSheet(ReadSpreadSheetReqModel mdl, Action<System.Exception> errCallback, Action<ReadSpreadSheetResult> callback)
        {
            string url = $"{_baseURL}{HttpUtils.ToQueryString(mdl, _password)}";
            Get<ReadSpreadSheetResult>(url, errCallback, (result) =>
            {
                if (result != null) callback?.Invoke(result);
            });
        }

        public void CreateDefaultSheet(CreateDefaultReqModel mdl, Action<System.Exception> errCallback, Action<CreateDefaultSheetResult> callback)
        {
            string url = $"{_baseURL}{HttpUtils.ToQueryString(mdl, _password)}";
            Get<CreateDefaultSheetResult>(url, errCallback, (result) =>
            {
                if (result != null) callback?.Invoke(result);
            });
        }

        public void CopyExample(CopyExampleReqModel mdl, Action<System.Exception> errCallback, Action<CreateExampleResult> callback)
        {
            string url = $"{_baseURL}{HttpUtils.ToQueryString(mdl, _password)}";
            Console.WriteLine(url);
            Get<CreateExampleResult>(url, errCallback, (result) =>
            {
                if (result != null) callback?.Invoke(result);
            });
        }

        public void WriteObject(WriteObjectReqModel mdl, Action<System.Exception> errResponse, Action<GoogleSheet.Protocol.v2.Res.WriteObjectResult> callback)
        {
            var req = Newtonsoft.Json.JsonConvert.SerializeObject(mdl);
            Post<GoogleSheet.Protocol.v2.Res.WriteObjectResult>(req, errResponse, (result) =>
            {
                if (result != null) callback?.Invoke(result);
            });
        }

        public void CopyFolder(CopyFolderReqModel mdl, Action<System.Exception> errResponse, Action<CreateExampleResult> callback)
        {
            throw new NotImplementedException();
        }
    }
}