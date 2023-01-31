
#if UNITY_2017_1_OR_NEWER || UNITY_BUILD
#if UNITY_EDITOR || UNITY_BUILD
using GoogleSheet;
using GoogleSheet.Protocol.v2.Req;
using GoogleSheet.Protocol.v2.Res;
using System;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace UGS
{

    public class UnityEditorWebRequest : IHttpProtcol
    {
        public static UnityEditorWebRequest Instance
        {
            get
            {

                if (instance == null)
                    instance = new UnityEditorWebRequest();

                return instance;
            }
        }
        private static UnityEditorWebRequest instance;
        public string baseURL
        {
            get
            {
                return UGSettingObjectWrapper.ScriptURL;
            }
        }




        private void Get<T>(string url, System.Action<System.Exception> errCallback, Action<T> callback) where T : Response
        {
            try
            {
                EditorUtility.DisplayProgressBar("UGS 스크립트 요청중..", "몇 초 소요될 수 있습니다..", 1);
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 300000;
                request.Credentials = CredentialCache.DefaultCredentials;
                WebResponse response = request.GetResponse();
                var statusCode = ((HttpWebResponse)response).StatusCode;
                string responseFromServer = "";
                if (statusCode == HttpStatusCode.RequestTimeout)
                {
                    EditorUtility.DisplayDialog("Timeout", "시간이 초과되었습니다. 세팅정보를 다시 확인해주세요.", "ok");
                    errCallback?.Invoke(new System.Exception("TimeOut!"));
                }
                if (statusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        var res = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseFromServer);
                        if (res.hasError())
                        {
                            throw new UGSWebError(res.error.message);
                        }
                        else
                        {
                            callback?.Invoke(res);
                        }
                    }
                }
                else
                    errCallback?.Invoke(new System.Exception("Internal Error"));


                response.Close();
                EditorUtility.ClearProgressBar();
            }
            catch (System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                if (e is WebException)
                {
                    var we = e as WebException;
                }
                else if (e is System.Net.Http.HttpRequestException)
                {
                    EditorUtility.DisplayDialog("Http Request Failed", "Goold Script Request Failed, Please Check Setting!", "OK");
                }
                else if (e is System.UriFormatException)
                {
                    EditorUtility.DisplayDialog("Invalid URI", "Please Check Script URL Format in HamsterLib->UGS->Setting", "OK");
                }
                else if (e is System.Collections.Generic.KeyNotFoundException)
                {
                    EditorUtility.DisplayDialog("UGS Error", "Maybe, Google Spread Sheet Rules is Invalid or You not make Typedata or other error..", "Hmm.. Ok");
                }
                else if (e is TypeParserNotFoundException)
                {
                    EditorUtility.DisplayDialog("UGS Error", e.Message, "Hmm.. Ok");
                }
                else if (e is System.IndexOutOfRangeException)
                {

                }
                else
                {
                    EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                }


                errCallback?.Invoke(e);
                Debug.LogError(e);
            }
        }

        private void Post(string json, System.Action<System.Exception> errCallback, Action<string> callback)
        {

            try
            {
                EditorUtility.DisplayProgressBar("UGS 스크립트 요청중..", "몇 초 소요될 수 있습니다..", 1);
                WebRequest request = WebRequest.Create(baseURL);
                request.Method = "POST";
                request.Timeout = 300000;
                byte[] data = Encoding.UTF8.GetBytes(json);
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
                    EditorUtility.DisplayDialog("Timeout", "ZegoGoogleSheet Initialize Failed! Try Check Setting Window.", "ok");
                    callback?.Invoke(null);
                }
                if (statusCode == HttpStatusCode.OK)
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                        responseFromServer = reader.ReadToEnd();
                        callback?.Invoke(responseFromServer);
                    }
                }
                else
                {
                    Debug.Log(statusCode);
                    callback?.Invoke(null);
                }
                response.Close();
                EditorUtility.ClearProgressBar();
            }
            catch (System.Exception e)
            {
                EditorUtility.ClearProgressBar();
                if (e is WebException)
                {
                    var we = e as WebException;
                    Debug.Log(we.Status);
                    callback?.Invoke(null);
                    errCallback?.Invoke(e);
                }
                else if (e is System.Net.Http.HttpRequestException)
                {
                    Debug.LogError(e.Message);
                    EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                    callback?.Invoke(null);
                    errCallback?.Invoke(e);
                }
                else
                {
                    Debug.LogError(e.Message);
                    EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                    callback?.Invoke(null);
                    errCallback?.Invoke(e);
                }
            }
        }
        public void CopyFolder(CopyFolderReqModel mdl, Action<System.Exception> errResponse, Action<CreateExampleResult> callback)
        {
            var url = baseURL + HttpUtils.ToQueryString(mdl, UGSettingObjectWrapper.ScriptPassword);
            Instance.Get<CreateExampleResult>(url, errResponse, (x) =>
            {
                callback?.Invoke(x);
            });
        }

        public void GetDriveDirectory(GetDriveDirectoryReqModel mdl, Action<System.Exception> errResponse, Action<GetDriveFolderResult> callback)
        {
            var url = baseURL + HttpUtils.ToQueryString(mdl, UGSettingObjectWrapper.ScriptPassword);
            Instance.Get<GetDriveFolderResult>(url, errResponse, (x) =>
            {
                callback?.Invoke(x);
            });
        }

        public void ReadSpreadSheet(ReadSpreadSheetReqModel mdl, Action<System.Exception> errResponse, Action<ReadSpreadSheetResult> callback)
        {
            var url = baseURL + HttpUtils.ToQueryString(mdl, UGSettingObjectWrapper.ScriptPassword);
            Instance.Get<ReadSpreadSheetResult>(url, errResponse, (x) =>
            {
                callback?.Invoke(x);
            });
        }

        public void WriteObject(WriteObjectReqModel mdl, Action<System.Exception> errResponse, Action<WriteObjectResult> callback)
        {
            var url = baseURL + HttpUtils.ToQueryString(mdl, UGSettingObjectWrapper.ScriptPassword);
            Instance.Get<WriteObjectResult>(url, errResponse, (x) =>
            {
                callback?.Invoke(x);
            });
        }

        public void CreateDefaultSheet(CreateDefaultReqModel mdl, Action<System.Exception> errResponse, Action<CreateDefaultSheetResult> callback)
        {
            var url = baseURL + HttpUtils.ToQueryString(mdl, UGSettingObjectWrapper.ScriptPassword);
            Instance.Get<CreateDefaultSheetResult>(url, errResponse, (x) =>
            {
                callback?.Invoke(x);
            });
        }

        public void CopyExample(CopyExampleReqModel mdl, Action<System.Exception> errResponse, Action<CreateExampleResult> callback)
        {
            var url = baseURL + HttpUtils.ToQueryString(mdl, UGSettingObjectWrapper.ScriptPassword);
            Instance.Get<CreateExampleResult>(url, errResponse, (x) =>
            {
                callback?.Invoke(x);
            });
        }
    }
}
#endif
#endif