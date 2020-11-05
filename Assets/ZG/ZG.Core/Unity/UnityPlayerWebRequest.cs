
using System;
using System.Collections;
using System.Text;
using Hamster.ZG.Http.Protocol;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Hamster.ZG
{
    public class ReceivedData
    {
        public string result;
    }
    public class WriteDataSender
    {
        public string password;
        public string instruction = "writeData";
        public string spreadSheetID;
        public string sheetID;
        public string key;
        public string[] value;

        public WriteDataSender(string spreadSheetID, string sheetID, string key, string[] value)
        {
            password = password = ZGSetting.ScriptPassword;
            this.spreadSheetID = spreadSheetID;
            this.sheetID = sheetID;
            this.key = key;
            this.value = value;
        }
    }

    public class CreateDefaultTableSender
    {
        public string password;
        public string instruction = "createDefaultTable";
        public string folderID;
        public string fileName;

        public CreateDefaultTableSender(string folderID, string fileName)
        {
            password = ZGSetting.ScriptPassword;
            this.folderID = folderID;
            this.fileName = fileName;
        }
    }
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
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReceivedData>(x);
                if(result.result == "update" || result.result== "create")
                {
                    Debug.Log(x);
                    onWrited?.Invoke();
                }
                else
                {
                    Debug.LogError(x);
                    onWrited?.Invoke();
                } 
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
                        var value = JsonConvert.DeserializeObject<Hamster.ZG.Http.Protocol.GetFolderInfo>(x);
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
                        var value = JsonConvert.DeserializeObject<Hamster.ZG.Http.Protocol.GetTableResult>(x);
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
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest(); 
                if(webRequest.error == null)
                {
                    callback?.Invoke(webRequest.downloadHandler.text);
                    reqProcessing = false;
                }
                else
                {
                    Debug.LogError(webRequest.error);
                    reqProcessing = false;
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
            yield return request.SendWebRequest(); 
            if (request.error == null)
            {
                callback?.Invoke(request.downloadHandler.text);
                reqProcessing = false;
            }
            else
            {
                Debug.LogError(request.error);
                reqProcessing = false;
            }
        }
    }
}