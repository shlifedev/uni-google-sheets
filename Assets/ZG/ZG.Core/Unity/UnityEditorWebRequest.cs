﻿
 
#if UNITY_EDITOR
using Hamster.ZG;
using Hamster.ZG.Http;
using Hamster.ZG.Http.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine;

public abstract class ZGWebReqeust
{
    public abstract void GetFolderFiles(string folderID, System.Action<Hamster.ZG.Http.Protocol.GetFolderInfo> callback);
    public abstract void GetTableData(string sheetID, System.Action<Hamster.ZG.Http.Protocol.GetTableResult, string> callback);
    public abstract void WriteData(string spreadSheetID, string sheetID, string key, string[] value);

    //  public abstract void WriteValue(string sheetID, string key, string value);
}

public class UnityEditorWebRequest : ZGWebReqeust
{
    public static UnityEditorWebRequest Instance = new UnityEditorWebRequest();
    public string baseURL
    {
        get 
        {
            return ZGSetting.ScriptURL;
        }
    } 
    public override void GetFolderFiles(string folderID, System.Action<GetFolderInfo> callback)
    {  
        Instance.Get($"{baseURL}?instruction=getFolderInfo&folderID={folderID}", (x) => {
            if(x == null)
            {
                Debug.LogError("Cannot Receive Data From URL : " + baseURL);
                Debug.LogError("Cannot Receive Data From Folder ID : " + folderID);
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
                    callback?.Invoke(value, x);
                }
                catch
                {
                    callback?.Invoke(null, x);
                }
            }
        });
    }
     
    public class WriteDataSender
    {
        public string instruction = "writeData";
        public string spreadSheetID;
        public string sheetID;
        public string key;
        public string[] value;

        public WriteDataSender(string spreadSheetID, string sheetID, string key, string[] value)
        {
            this.spreadSheetID = spreadSheetID;
            this.sheetID = sheetID;
            this.key = key;
            this.value = value;
        }
    }
    public override void WriteData(string spreadSheetID, string sheetID, string key, string[] value)
    {
        var data = new WriteDataSender(spreadSheetID, sheetID, key, value);
        var json = JsonConvert.SerializeObject(data);

        Instance.Post(json, (x) => 
        { 
            Debug.Log(x);
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
        catch (Exception e)
        {
            if (e is WebException)
            {
                var we = e as WebException;
                Debug.Log(we.Status);
                callback?.Invoke(null);
            }
            else if (e is System.Net.Http.HttpRequestException)
            {
                Debug.LogError(e.Message);
                EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                callback?.Invoke(null);
            }
            else
            {
                Debug.LogError(e.Message);
                EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                callback?.Invoke(null);
            }
        }
    }

    private void Post(string json, Action<string> callback)
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
        catch (Exception e)
        {
            if (e is WebException)
            {
                var we = e as WebException;
                Debug.Log(we.Status);
                callback?.Invoke(null);
            }
            else if (e is System.Net.Http.HttpRequestException)
            {
                Debug.LogError(e.Message);
                EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                callback?.Invoke(null);
            }
            else
            {
                Debug.LogError(e.Message);
                EditorUtility.DisplayDialog("Please Check Setting!", e.Message, "OK");
                callback?.Invoke(null);
            }
        }
    }
}

#endif