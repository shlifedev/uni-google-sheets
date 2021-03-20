#if UNITY_2017_1_OR_NEWER 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text; 
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Hamster.ZG
{ 
    public class UnityPlayerWebRequest : MonoBehaviour, IZGRequester
    {
        public bool reqProcessing = false;
        public static UnityPlayerWebRequest Instance
        {
            get
            {
                if(instance == null)
                {
                    var data = new GameObject().AddComponent<UnityPlayerWebRequest>();
                    instance =data; 
                    data.gameObject.name = "UnityPlayerWebRequest";
                }
                return instance;
            }
        }
        private static UnityPlayerWebRequest instance;

       
        public string baseURL
        {
            get
            {
                return ZGSetting.ScriptURL;
            }
        }

        void Awake()
        {
            //singleton
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
        }
         
        public void CreateDefaultTable(string folderID, string fileName, Action<string> callback)
        {
            if (reqProcessing)
            {
                Debug.Log("already requested! wait response!");
                return;
            }
            else
            {
                reqProcessing = true;
            }
            var data = new CreateDefaultTableSender(folderID, fileName);
            var json = JsonConvert.SerializeObject(data);

            StartCoroutine(Post(json, (x) =>
            {
                Debug.Log(x);
            }));
        } 

        public void WriteObject(string spreadSheetID, string sheetID, string key, string[] value, System.Action onWrited = null)
        {
            if (reqProcessing)
            {
                Debug.Log("already requested! wait response!");
                return;
            }
            else
            {
                reqProcessing = true;
            }
            var data = new WriteDataSender(spreadSheetID, sheetID, key, value);
            var json = JsonConvert.SerializeObject(data);

            StartCoroutine(Post(json, (x) =>
            {
                try
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceivedData>(x);
                    if (result.result == ReceivedResult.Success)
                    {
                        Debug.Log(x);
                        onWrited?.Invoke();
                    }
                    else
                    {
                        Debug.LogError(x);
                        onWrited?.Invoke();
                    }
                }
                catch
                {
                    Debug.LogError("Write Failgure! =>\n\n" +x);
                    onWrited?.Invoke();
                }
            }));
        }
         
 
        public void CopyExamples(string folderID, Action<string> callback)
        { 
            StartCoroutine(Get($"{baseURL}?password={ZGSetting.ScriptPassword}&instruction=copyExampleSheets&folderID={folderID}", (x) =>
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CopyExampleResult>(x);
                Debug.Log(result.result);
                callback?.Invoke(result.createdFolderId);
            }));
        }

        public void SearchGoogleDriveDirectory(string folderID, Action<GetFolderInfo> callback)
        {
            if (reqProcessing)
            {
                Debug.Log("already requested! wait response!");
                return;
            } 
            else
            {
                reqProcessing = true;
            }
            StartCoroutine(Get($"{baseURL}?password={ZGSetting.ScriptPassword}&instruction=getFolderInfo&folderID={folderID}", x=> {
                if (x == null)
                { 
                    callback?.Invoke(null);
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
            }));

        }

        public void ReadGoogleSpreadSheet(string sheetID, Action<GetTableResult, string> callback)
        {
            if (reqProcessing)
            {
                Debug.Log("already requested! wait response!");
                return;
            }
            else
            {
                reqProcessing = true;
            }
            StartCoroutine(Get($"{baseURL}?password={ZGSetting.ScriptPassword}&instruction=getTable&sheetID={sheetID}", (x) =>
            {
                if (x == null)
                { 
                    callback?.Invoke(null, null);
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
            }));
        }
        IEnumerator Get(string uri, Action<string> callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.timeout = 60;
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest(); 
                if(webRequest.error == null)
                {
                    reqProcessing = false;
                    callback?.Invoke(webRequest.downloadHandler.text);
               
                }
                else
                {
                    reqProcessing = false;
                    Debug.LogError(webRequest.error); 
                }
            }
        }
        IEnumerator Post(string json, Action<string> callback)
        { 
            var request = new UnityWebRequest (baseURL, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = 60;
            yield return request.SendWebRequest(); 
            if (request.error == null)
            { 
                reqProcessing = false;
                callback?.Invoke(request.downloadHandler.text);
        
            }
            else
            {
                reqProcessing = false;
                Debug.LogError(request.error); 
            }
        }
    }
}
#endif