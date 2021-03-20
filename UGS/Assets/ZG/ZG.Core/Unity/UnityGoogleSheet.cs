using Hamster.ZG;
using Hamster.ZG.IO.FileReader;
using Hamster.ZG.IO.FileWriter;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

#if UNITY_2017_1_OR_NEWER
using UnityEngine;
#endif
public class UnityGoogleSheet
{
#if UNITY_EDITOR 
    public static void TestFunction()
    {
        UnityGoogleSheet.LoadAllData();
    }
#endif
    public static bool Init = false;
#if UNITY_2017_1_OR_NEWER
    public static void Initalize()
    {
        //Unity
        if (Init == false)
            ZeroGoogleSheet.Init(new UnityGSParser(), new UnityFileReader()); 
        Init = true;
    }
#endif

#if !UNITY_2017_1_OR_NEWER && !UNITY_EDITOR
    public static void Initalize(string baseURL, string password)
    { 
        //C# 
        if (Init == false)
            ZeroGoogleSheet.Init(new GSParser(), new FileReader());

        GoogleDriveWebRequester.Instance.baseURL = baseURL;
        GoogleDriveWebRequester.Instance.password = password;

        Init = true;
    }
#endif

    /// <summary>
    /// Write Your Table Data To GoogleSheet
    /// </summary> 
    public static void Write<T>(T value, Action writeCallback = null) where T : ITable
    {
#if UNITY_2017_1_OR_NEWER
        Initalize();
#endif
        var @class = typeof(T); 
        var writeFunction = @class.GetMethod("Write", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
        if (writeFunction != null)
        {
            writeFunction?.Invoke(null, new object[] { value , writeCallback });
        }

    }

    /// <summary>
    /// Generate Your Table Data 
    /// </summary>
    /// <param name="csharpGenerate"> generate script, runtime not work this </param>
    /// <param name="jsonGenerate">generate json</param>
    public static void Generate<T>(bool csharpGenerate, bool jsonGenerate) where T : ITable
    {
#if UNITY_2017_1_OR_NEWER
        Initalize();
#endif
        var targetTable = typeof(T);
     
        var field = targetTable.GetField("spreadSheetID", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        var sSheetID = (string)field.GetValue(null);
        Generate(sSheetID, csharpGenerate, jsonGenerate);

    }

#if!UNITY_2017_1_OR_NEWER && !UNITY_EDITOR
    public static void GenerateSheetInFolder(string folderId, bool csharpGenerate, bool jsonGenerate)
    {
        GoogleDriveWebRequester.Instance.SearchGoogleDriveDirectory(folderId, x => {
            int idx = 0;
           
            foreach(var v in x.fileType)
            {
                Console.WriteLine("Wait Generate for " + x.fileName[idx] +"...");
                if (v == 2)
                {
                    var sheetId = x.fileID[idx];
                    Generate(sheetId, csharpGenerate, jsonGenerate);
                }
                idx++;
            }
        });

    }
#endif
    /// <su
    public static void Generate(string spreadSheetId, bool csharpGenerate, bool jsonGenerate)
    {

#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            UnityPlayerWebRequest.Instance.ReadGoogleSpreadSheet(spreadSheetId, (x, json) => {
                ZeroGoogleSheet.DataParser.ParseSheet(json, csharpGenerate, jsonGenerate, new UnityFileWriter());
            });
        }
        else
        {
            UnityEditorWebRequest.Instance.ReadGoogleSpreadSheet(spreadSheetId, (x, json) => {
                ZeroGoogleSheet.DataParser.ParseSheet(json, csharpGenerate, jsonGenerate, new UnityFileWriter());
            });
        }
#elif UNITY_2017_1_OR_NEWER
        UnityPlayerWebRequest.Instance.ReadGoogleSpreadSheet(spreadSheetId, (x, json) => {
                    ZeroGoogleSheet.DataParser.ParseSheet(json, false, jsonGenerate, new UnityFileWriter());
                }); 
#else
        //C# 코드작성
        GoogleDriveWebRequester.Instance.ReadGoogleSpreadSheet(spreadSheetId, (x, json) => { 
            ZeroGoogleSheet.DataParser.ParseSheet(json, csharpGenerate, jsonGenerate, new FileWriter()); 
        });
#endif
    } 
    /// <summary>
    /// Load Data From GoogleSheet  
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    /// <param name="callback"></param>
    /// <param name="updateData"></param>
    public static void LoadFromGoogle<Key, Value>([NotNull] System.Action<List<Value>, Dictionary<Key, Value>> callback, bool updateData = false)  
    where Value : ITable
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));
#if UNITY_2017_1_OR_NEWER
        Initalize();
#endif
        var @class = typeof(Value); 
        //Get Load Method
        var loadFunction = @class.GetMethod("LoadFromGoogle", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
        //Call Load Method
        if (loadFunction != null)
            loadFunction.Invoke(null, new System.Object[] { callback, updateData }); 
  
    }


    /// <summary>
    /// Load All Your Generated Table.
    /// </summary>
    public static void Load<T>() where T : ITable
    {
#if UNITY_2017_1_OR_NEWER
        Initalize();
#endif
        var @class = typeof(T);
         
        //Get Load Method
        var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
        //Call Load Method
        if (loadFunction != null)
            loadFunction.Invoke(null, new object[] { false });
    }

    /// <summary>
    /// Load All Your Generated Table.
    /// </summary>
    public static void LoadAllData()
    {
#if UNITY_2017_1_OR_NEWER
        Initalize();
#endif
        var subClasses = Hamster.ZG.Reflection.Utility.GetAllSubclassOf(typeof(ITable));
        foreach (var @class in subClasses)
        {
            //Get Load Method
            var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
            //Call Load Method
            if (loadFunction != null)
                loadFunction.Invoke(null, new System.Object[] { false });
        }
    }

    /// <summary>
    /// Load All Your Generated Table.
    /// </summary>
    public static void LoadByNamespaceContains(string @namespace)
    {
#if UNITY_2017_1_OR_NEWER
        Initalize();
#endif
        var subClasses = Hamster.ZG.Reflection.Utility.GetAllSubclassOf(typeof(ITable));
        foreach (var @class in subClasses)
        {
            if (@class.Namespace != null && @class.Namespace.Contains(@namespace))
            {
                var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
                loadFunction?.Invoke(null, new System.Object[] { false });
            }
        }
    }


}