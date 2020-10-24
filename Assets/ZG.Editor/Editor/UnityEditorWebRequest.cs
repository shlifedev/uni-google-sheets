using Hamster.ZG;
using Hamster.ZG.Http;
using Hamster.ZG.Http.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;


public abstract class ZGWebReqeust
{
    public abstract void GetFolderFiles(string folderID, System.Action<GetFolderInfo> callback);
    public abstract void GetTableData(string sheetID, System.Action<GetTableResult, string> callback);


  //  public abstract void WriteValue(string sheetID, string key, string value);
}
public class UnityEditorWebRequest : ZGWebReqeust
{
    public static UnityEditorWebRequest Instance = new UnityEditorWebRequest();
    public string baseURL
    {
        get 
        {
            return "https://script.google.com/macros/s/AKfycbyOBVdYiUz6W1WJCHhV5SS4r0Bq3NIyCKW8ugVunsBD-4Bbn30U/exec";
        }
    } 
    public override void GetFolderFiles(string folderID, System.Action<GetFolderInfo> callback)
    { 
        Instance.Get($"{baseURL}?instruction=getFolderInfo&folderID={folderID}", (x) => {
            if(x == null)
            {
                Debug.LogError("cannot receive data");
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
        });
    }

    public override void GetTableData(string sheetID, Action<GetTableResult, string> callback)
    {
        Instance.Get($"{baseURL}?instruction=getTable&sheetID={sheetID}", (x) => {
            if (x == null)
            {
                Debug.LogError("cannot receive data");
                callback?.Invoke(null, null);
            }
            else
            {
                try
                {
                    var value = JsonConvert.DeserializeObject<Hamster.ZG.Http.Protocol.GetTableResult>(x);
                    callback?.Invoke(value,x);
                }
                catch
                {
                    callback?.Invoke(null,x); 
                }
            }
        });
    }

    private void Get(string url, Action<string> callback)
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
        catch(Exception e)
        {
            if(e is WebException)
            {
                var we = e as WebException;
                Debug.Log(we.Status);
                callback?.Invoke(null);
            }
        }
    }
}
