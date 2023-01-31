
using GoogleSheet.IO;
using GoogleSheet.IO.FileReader;
using GoogleSheet.IO.FileWriter;
using GoogleSheet.Protocol.v2.Req;
using GoogleSheet.Protocol.v2.Res;
using GoogleSheet.Type;
using System;
using System.Collections.Generic;

namespace GoogleSheet
{
    public static class GoogleSheetV2
    {

        public static void Initialize(string scriptURL, string password)
        {
            ScriptRequester.Instance.Credential(scriptURL, password);
            GoogleSpreadSheets.Init(new GSParser(), new FileReader());
        }

        /// <summary>
        /// Get Drive Directory Infos 
        /// </summary>
        /// <param name="folderID"></param>
        /// <param name="callback"></param>
        public static void GetDriveDirectory(string folderID, System.Action<GetDriveFolderResult> callback)
        {
            ScriptRequester.Instance.GetDriveDirectory(new GetDriveDirectoryReqModel(folderID), OnError, callback);
        }

        /// <summary>
        /// Read Srpead Sheet Data (It's not data load function)
        /// </summary>
        /// <param name="fileID"></param>
        /// <param name="callback"></param>
        public static void ReadSpreadSheet(string fileID, System.Action<ReadSpreadSheetResult> callback)
        {
            ScriptRequester.Instance.ReadSpreadSheet(new ReadSpreadSheetReqModel(fileID), OnError, callback);
        }

        /// <summary>
        /// Write To Spreadsheet 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="writeCallback"></param>
        public static void Write<T>(T value, Action<WriteObjectResult> writeCallback = null) where T : ITable
        {
            var @class = typeof(T);
            var writeFunction = @class.GetMethod("Write", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            writeFunction?.Invoke(null, new object[] { value, writeCallback });
        }

        /// <summary>
        /// Load From Google
        /// </summary>
        /// <typeparam name="Key">Your Object Unique Index Key (In GoogleSheet A1~A999999)</typeparam>
        /// <typeparam name="Value"></typeparam>
        /// <param name="callback"></param>
        /// <param name="updateData"></param>
        public static void LoadFromGoogle<Key, Value>(System.Action<List<Value>, Dictionary<Key, Value>> callback, bool updateData = false)
        where Value : ITable
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));


            var @class = typeof(Value);
            var loadFunction = @class.GetMethod("LoadFromGoogle", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (loadFunction != null)
                loadFunction.Invoke(null, new System.Object[] { callback, updateData });
        }


        /// <summary>
        /// Code Generator In Directory Files.
        /// </summary>
        /// <param name="folderId"></param>
        public static void GenerateDriveDirectory(string folderId)
        {
            ScriptRequester.Instance.GetDriveDirectory(new GetDriveDirectoryReqModel("folderId"), OnError, x =>
            {
                int idx = 0;
                foreach (var v in x.fileType)
                {
                    Console.WriteLine("Wait Generate for " + x.fileName[idx] + "...");
                    if (v == 2)
                    {
                        var sheetId = x.fileId[idx];
                        Generate(sheetId);
                    }
                    idx++;
                }
            });
        }

        /// <summary>
        /// Generate Your Table Data 
        /// </summary>
        /// <param name="csharpGenerate"> generate script, runtime not work this </param>
        /// <param name="jsonGenerate">generate json</param>
        public static void Generate<T>() where T : ITable
        {
            var targetTable = typeof(T);
            var field = targetTable.GetField("spreadSheetID", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            var sSheetID = (string)field.GetValue(null);
            Generate(sSheetID);
        }

        /// <summary>
        /// Load Data From Local Cached Json.
        /// </summary>
        public static void Load<T>() where T : ITable
        {
            var @class = typeof(T);
            var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (loadFunction != null)
                loadFunction.Invoke(null, new object[] { false });
        }

        /// <summary>
        /// Generate .cs .json 
        /// </summary>
        /// <param name="fileID"></param>
        public static void Generate(string fileID)
        {
            ReadSpreadSheet(fileID, x =>
            {
                GoogleSpreadSheets.DataParser.ParseSheet(x, true, true, new FileWriter());
            });
        }

        /// <summary>
        /// Generate .cs .json 
        /// </summary>
        /// <param name="fileID"></param>
        public static void Generate(string fileID, System.Action callback)
        {
            ReadSpreadSheet(fileID, x =>
            {
                GoogleSpreadSheets.DataParser.ParseSheet(x, true, true, new FileWriter());
                callback?.Invoke();
            });
        }

        /// <summary>
        /// Load All Your Generated Table.
        /// </summary>
        public static void LoadAllData()
        {
            var subClasses = GoogleSheet.Reflection.Utility.GetAllSubclassOf(typeof(ITable));
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
        /// Find Namespace And Load All
        /// </summary>
        /// <param name="namespace"></param>
        public static void LoadByNamespaceContains(string @namespace)
        {
            var subClasses = GoogleSheet.Reflection.Utility.GetAllSubclassOf(typeof(ITable));
            foreach (var @class in subClasses)
            {
                if (@class.Namespace != null && @class.Namespace.Contains(@namespace))
                {
                    var loadFunction = @class.GetMethod("Load", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    loadFunction?.Invoke(null, new System.Object[] { false });
                }
            }
        }


        /// <summary>
        /// Err Handling Callback.
        /// </summary>
        /// <param name="e"></param>
        public static void OnError(System.Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static class GoogleSpreadSheets
    {

        private static IParser _dataParser;
        private static IFileReader _dataReader;
        public static IParser DataParser { get => _dataParser; }
        public static IFileReader DataReader { get => _dataReader; }



        public static void Init(IParser parser, IFileReader reader)
        {
            TypeMap.Init();
            _dataParser = parser;
            _dataReader = reader;
        }
    }
}
