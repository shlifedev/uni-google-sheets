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
    [MenuItem("Test/Gogo")]
    public static void EditorWebRequest()
    { 
        ZeroGoogleSheet.Init(new GSParser());
        Instance.GetFolderFiles("1kvfx7v8K1fMWtpFGkmRr_DrAyjVfInRo", x=> {
            Instance.GetTableData(x.fileID[0], (v,b)=> { 
                 ZeroGoogleSheet.DataReader.ParseSheet(b,true,true, new UnityFileWriter());
            });
        });
    } 

    public override void GetFolderFiles(string folderID, System.Action<GetFolderInfo> callback)
    { 
        Instance.Get($"{baseURL}?instruction=getFolderInfo&folderID={folderID}", (x) => {
            var value = JsonConvert.DeserializeObject<Hamster.ZG.Http.Protocol.GetFolderInfo>(x);
 
            callback?.Invoke(value);
        });
    }

    public override void GetTableData(string sheetID, Action<GetTableResult, string> callback)
    {
        Instance.Get($"{baseURL}?instruction=getTable&sheetID={sheetID}", (x) => { 
            var value = JsonConvert.DeserializeObject<Hamster.ZG.Http.Protocol.GetTableResult>(x);
            callback?.Invoke(value, x);
        });
    }

    private void Get(string url, Action<string> callback)
    {
 
        EditorUtility.DisplayProgressBar("Request From Google Script..", "Please Wait a Second..", 1);
        WebRequest request = WebRequest.Create(url);
        request.Timeout = 3000;
        request.Credentials = CredentialCache.DefaultCredentials; 
        WebResponse response = request.GetResponse(); 
        var statusCode = ((HttpWebResponse)response).StatusCode;
        string responseFromServer = "";

      
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
            callback?.Invoke(null);
        }
        response.Close(); 
        EditorUtility.ClearProgressBar(); 
    } 
}
