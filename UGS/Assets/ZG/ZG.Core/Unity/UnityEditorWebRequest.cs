
#if UNITY_2017_1_OR_NEWER 
#if UNITY_EDITOR
using Hamster.ZG; 
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine; 
namespace Hamster.ZG
{ 

    public class UnityEditorWebRequest : IZGRequester
    {
        public static UnityEditorWebRequest Instance
        { 
            get
            {
                if(instance == null)
                    instance = new UnityEditorWebRequest();

                return instance;
            }
        }
        private static UnityEditorWebRequest instance;
        public string baseURL
        {
            get
            {
                return ZGSetting.ScriptURL;
            }
        }

        public void CopyExamples(string folderID, System.Action<System.Exception> errCallback, Action<string> callback)
        {
            Instance.Get($"{baseURL}?password={ZGSetting.ScriptPassword}&instruction=copyExampleSheets&folderID={folderID}", errCallback,(x) =>
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CopyExampleResult>(x);
                Debug.Log(result.result);
                callback?.Invoke(result.createdFolderId);
            });
        }

        public void SearchGoogleDriveDirectory(string folderID, System.Action<System.Exception> errCallback,  System.Action<GetFolderInfo> callback)
        {
            Instance.Get($"{baseURL}?password={ZGSetting.ScriptPassword}&instruction=getFolderInfo&folderID={folderID}", errCallback, (x) =>
            {
                if (x == null)
                {
                    string err = "Cannot Receive GoogleDrive Directory Data! Please Check Your Setting. (HamsterLib->ZGS->Setting)" + x;
                    Debug.LogError(err);
                    errCallback?.Invoke(new System.Exception(err));
                }
                else
                {
                    try
                    {
                        var value = JsonConvert.DeserializeObject<GetFolderInfo>(x);
                        callback?.Invoke(value);
                    }
                    catch
                    {
                        callback?.Invoke(null);

                    }
                }
            });
        } 
        public void ReadGoogleSpreadSheet(string sheetID, System.Action<System.Exception> errCallback, Action<GetTableResult, string> callback)
        {
            Instance.Get($"{baseURL}?password={ZGSetting.ScriptPassword}&instruction=getTable&sheetID={sheetID}", errCallback, (x) =>
            {
                if (x == null)
                {
                    Debug.LogError($"Cannot Read Google Sheet! Please Check Your Setting. (HamsterLib->ZGS->Setting)\n\nzgs root : {ZGSetting.GoogleFolderID}\ntarget id :{sheetID}");
                }
                else
                {
                    try
                    {
                        var value = JsonConvert.DeserializeObject<GetTableResult>(x);
                        callback?.Invoke(value, x);
                    }
                    catch
                    {
                        callback?.Invoke(null, x);
                    }
                }
            });
        }

   

      
        public void WriteObject(string spreadSheetID, string sheetID, string key, string[] value, System.Action<System.Exception> errCallback = null, System.Action onWrited = null)
        {
            var data = new WriteDataSender(spreadSheetID, sheetID, key, value);
            var json = JsonConvert.SerializeObject(data);

            Instance.Post(json, errCallback, (x) =>
            { 
                onWrited?.Invoke();
            });
        }
        public void CreateDefaultTable(string folderID, string fileName, System.Action<System.Exception> errCallback, Action<string> callback)
        {
            var data = new CreateDefaultTableSender(folderID, fileName);
            var json = JsonConvert.SerializeObject(data);

            Instance.Post(json, errCallback, (x) =>
            {
                callback?.Invoke(x);
            });
        }
        private void Get(string url, System.Action<System.Exception> errCallback, Action<string> callback)
        {

            try
            {
                EditorUtility.DisplayProgressBar("Request From Google Script..", "Please Wait a Second..", 1);
                WebRequest request = WebRequest.Create(url);
                request.Timeout = 7500;
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
                    Debug.LogError(e);
                    EditorUtility.DisplayDialog("Http Request Failed", "Goold Script Request Failed, Please Check Setting!", "OK");
                    callback?.Invoke(null);
                    errCallback?.Invoke(e);
                }
                else if (e is System.UriFormatException)
                {
                    Debug.LogError(e);

                    EditorUtility.DisplayDialog("Invalid URI", "Please Check Uri Format in HamsterLib->ZGS->Setting", "OK");
                    callback?.Invoke(null);
                    errCallback?.Invoke(e);
                }
                else if (e is System.Collections.Generic.KeyNotFoundException)
                {
                    Debug.LogError(e);
                    EditorUtility.DisplayDialog("UGS Error", "Maybe, Google Spread Sheet Rules is Invalid or You not make Typedata or other error.." , "Hmm.. Ok");
                    callback?.Invoke(null);
                }
                else if (e is Hamster.ZG.Exception.TypeParserNotFoundException)
                {
                    Debug.LogError(e);
                    EditorUtility.DisplayDialog("UGS Error", e.Message, "Hmm.. Ok");
                    callback?.Invoke(null);
                }
                else
                {
                    Debug.LogError(e);
                    
                    EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                    callback?.Invoke(null);
                    errCallback?.Invoke(e);
                }


            }
        }

        private void Post(string json, System.Action<System.Exception> errCallback, Action<string> callback)
        {

            try
            {
                EditorUtility.DisplayProgressBar("Request From Google Script..", "Please Wait a Second..", 1);
                WebRequest request = WebRequest.Create(baseURL);
                request.Method = "POST";
                request.Timeout = 7500;
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

 
    }
}
#endif
#endif