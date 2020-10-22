using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

public static class UnityEditorWebRequest 
{
    [MenuItem("Test/Gogo")]
    public static void EditorWebRequest()
    {
        Get("https://script.google.com/macros/s/AKfycbyOBVdYiUz6W1WJCHhV5SS4r0Bq3NIyCKW8ugVunsBD-4Bbn30U/exec?instruction=getFolderInfo&folderID=1kvfx7v8K1fMWtpFGkmRr_DrAyjVfInRo", (x)=> { });

    }


    
    static void Get(string url, Action<string> callback)
    {
        EditorUtility.DisplayProgressBar("Request From Google Script..", "Please Wait a Second..", 1);
        WebRequest request = WebRequest.Create(url); 
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
