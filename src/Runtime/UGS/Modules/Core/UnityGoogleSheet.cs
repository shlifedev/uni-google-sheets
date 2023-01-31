using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using GoogleSheet.Protocol.v2.Req;
using GoogleSheet.Protocol.v2.Res;
using GoogleSheet;
using UGS.IO;
using GoogleSheet.Reflection;
#if UNITY_2017_1_OR_NEWER || UNITY_BUILD
using UnityEngine;
#endif

namespace UGS
{
    public static class UGSBackupManager
    {
        public struct BackupPlan
        {
            public System.DateTime BackupDateTime;
            public string OriginFilePath;
            public string OriginFileContent;
            public bool IsTable; // true is csharp file.

            public BackupPlan(DateTime backupDateTime, string originFilePath, string originFileContent, bool isTable)
            {
                BackupDateTime = backupDateTime;
                OriginFilePath = originFilePath;
                OriginFileContent = originFileContent;
                IsTable = isTable;
            }
        }
        static List<BackupPlan> BackupList = new List<BackupPlan>();
        public static void AddBackupPlan(string path)
        {
            if (System.IO.File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);
                var extention = System.IO.Path.GetExtension(path);
                var backupPlan = new BackupPlan(System.DateTime.Now, path, content, extention.Contains("json"));
                BackupList.Add(backupPlan);
                //  Debug.Log($"UGS Backup Data => {path}, it is {extention} file.");
            }
            else
            {
                Debug.LogError("UGS Can't add backup plan, path not found =>" + path);
            }
        }
    }
}
namespace UGS
{
    public class UnityGoogleSheet
    {

#if UNITY_EDITOR
        public static void TestFunction()
        {
            UnityGoogleSheet.LoadAllData();
        }
#endif
        public static bool Init = false;
#if UNITY_2017_1_OR_NEWER || UNITY_BUILD
        public static void Initalize()
        {
            //Unity
            if (Init == false)
                GoogleSpreadSheets.Init(new UnityGSParser(), new UnityFileReader());
            Init = true;
        }
#endif

#if (!UNITY_2017_1_OR_NEWER || UNITY_BUILD) && !UNITY_EDITOR
    public static void Initalize(string baseURL, string password)
    { 
        //C# 
        if (Init == false)
            HamsterGoogleSheet.Init(new GSParser(), new FileReader());

        GoogleDriveWebRequester.Instance.baseURL = baseURL;
        GoogleDriveWebRequester.Instance.password = password;

        Init = true;
    }
#endif



#if UNITY_EDITOR
        public static void CopyFolder(string id, System.Action<string> callback)
        {
            if (Application.isEditor)
            {
                UnityEditorWebRequest.Instance.CopyFolder(new CopyFolderReqModel(id),
                    e => { throw e; }
                   , x =>
                    {
                        callback?.Invoke(x.createdFolderId);
                    });
            }
        }
#endif
        /// <summary>
        /// Write Your Table Data To GoogleSheet
        /// </summary> 
        public static void Write<T>(T value, Action<WriteObjectResult> writeCallback = null) where T : ITable
        {
#if UNITY_2017_1_OR_NEWER
            Initalize();
#endif
            var @class = typeof(T);
            var writeFunction = @class.GetMethod("Write", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (writeFunction != null)
            {
                writeFunction?.Invoke(null, new object[] { value, writeCallback });
            }

        }

        /// <summary>
        /// If you call this function frequently, This Method is not good for performance. (Reflection&Unboxing)
        /// Instead Of Your Generated Script 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>


        public static List<T> GetList<T>() where T : ITable
        {
            var @class = typeof(T);
            var isLoadedField = @class.GetField("isLoaded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var isLoaded = (bool)isLoadedField.GetValue(null);
            if (!isLoaded)
                UnityGoogleSheet.LoadAllData();

            var className = @class.Name;
            var targetName = $"{className}List";
            var target = @class.GetField(targetName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            return target.GetValue(null) as List<T>;
        }

        public static Dictionary<Key, Value> GetDictionary<Key, Value>() where Value : ITable
        {
            var @class = typeof(Value);
            var isLoadedField = @class.GetField("isLoaded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var isLoaded = (bool)isLoadedField.GetValue(null);
            if (!isLoaded)
                UnityGoogleSheet.LoadAllData();

            var className = @class.Name;
            var targetName = $"{className}Map";
            var target = @class.GetField(targetName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            return target.GetValue(null) as Dictionary<Key, Value>;
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
            var sSheetID = TableUtils.GetSpreadSheetID<T>();
            Generate(sSheetID, csharpGenerate, jsonGenerate);

        }

#if !UNITY_2017_1_OR_NEWER && !UNITY_EDITOR
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

        public static void OnReadError(System.Exception e)
        {
            Debug.LogError("UGS Data Generate :: Google Networking Error \n\n " + e);

        }
        /// <su
        public static void Generate(string spreadSheetId, bool csharpGenerate, bool jsonGenerate)
        {

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                UnityEditorWebRequest.Instance.ReadSpreadSheet(new ReadSpreadSheetReqModel(spreadSheetId), OnReadError, (x) =>
                {
                    GoogleSpreadSheets.DataParser.ParseSheet(x, csharpGenerate, jsonGenerate, new UnityFileWriter());
                });
            }
            else
            {
                UnityEditorWebRequest.Instance.ReadSpreadSheet(new ReadSpreadSheetReqModel(spreadSheetId), OnReadError, (x) =>
                {
                    GoogleSpreadSheets.DataParser.ParseSheet(x, csharpGenerate, jsonGenerate, new UnityFileWriter());
                });
            }
#endif

#if !UNITY_EDITOR
        Debug.LogError("Currently Cannot Generate In Builded Runtime.");
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
            var loadFunction = @class.GetMethod("LoadFromGoogle", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            //Call Load Method
            if (loadFunction != null)
                loadFunction.Invoke(null, new System.Object[] { callback, updateData });

        }


        public static void OnTableError(System.Exception e)
        {
            Debug.LogError(e);
        }

        /// <summary>
        /// Load All Your Generated Table.
        /// </summary>
        public static void Load<T>() where T : ITable
        {
#if UNITY_2017_1_OR_NEWER || UNITY_BUILD
            Initalize();
#endif
            var @class = typeof(T);

            //Get Load Method
            var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
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
            var subClasses = Utility.GetAllSubclassOf(typeof(ITable));
            foreach (var @class in subClasses)
            {
                //Get Load Method
                var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
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
#if UNITY_2017_1_OR_NEWER || UNITY_BUILD
            Initalize();
#endif
            var subClasses = Utility.GetAllSubclassOf(typeof(ITable));
            foreach (var @class in subClasses)
            {
                if (@class.Namespace != null && @class.Namespace.Contains(@namespace))
                {
                    var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    loadFunction?.Invoke(null, new System.Object[] { false });
                }
            }
        }

    }

}
