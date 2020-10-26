
using System;
using System.Collections;
using System.Text;
using Hamster.ZG.Http.Protocol;
using UnityEngine;
using UnityEngine.Networking;

namespace Hamster.ZG
{
    public class UnityPlayerWebRequest : MonoBehaviour, IZGRequester
    {
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


        public void POST_CreateDefaultTable(string folderID, string fileName, Action<string> callback)
        {
          
        }

        public void GET_ReqFolderFiles(string folderID, Action<GetFolderInfo> callback)
        {
             
        }

        public void GET_TableData(string sheetID, Action<GetTableResult, string> callback)
        {
             
        }

        public void POST_WriteData(string spreadSheetID, string sheetID, string key, string[] value)
        {
      
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
                }
                else
                {
                    Debug.LogError(webRequest.error);
                }
            }
        }
        IEnumerator Post(string url, string json, Action<string> callback)
        { 
            var request = new UnityWebRequest (url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest(); 
            if (request.error == null)
            {
                callback?.Invoke(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }
}