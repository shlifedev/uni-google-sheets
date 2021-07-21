 
using Hamster.ZG.IO;
using Hamster.ZG.IO.FileReader;
using Hamster.ZG.Type;
using UGS.Protocol.v2.Req;
using UGS.Protocol.v2.Res;
using System;
using Hamster.ZG.IO.FileWriter;

namespace Hamster.ZG
{
    public static class GoogleSheet
    {
        
        private static bool _credentialCalled = false;
        public static void Initialize(string scriptURL, string password)
        {
            GoogleDriveWebRequesterV2.Instance.Credential(scriptURL, password);
            HamsterGoogleSheet.Init(new GSParser(), new FileReader()); 
        }
        public static void GetDriveDirectory(string folderID, System.Action<GetDriveFolderResult> callback)
        {
            GoogleDriveWebRequesterV2.Instance.GetDriveDirectory(new GetDriveDirectoryReqModel(folderID), OnError, callback);
        }

        public static void ReadSpreadSheet(string fileID, System.Action<ReadSpreadSheetResult> callback)
        {
            GoogleDriveWebRequesterV2.Instance.ReadSpreadSheet(new ReadSpreadSheetReqModel(fileID), OnError, callback);
        }
 
        public static void Generate(string fileID)
        {
            ReadSpreadSheet(fileID, x => {
                HamsterGoogleSheet.DataParser.ParseSheet(x, true, true, new FileWriter());
            });
        }
        public static void OnError(System.Exception e)
        {

        } 
    }

    public static class HamsterGoogleSheet
    {
    
        private static IParser _dataParser;
        private static IFileReader _dataReader;
        public static IParser DataParser { get => _dataParser; } 
        public static IFileReader DataReader { get => _dataReader;}


     
        public static void Init(IParser parser, IFileReader reader)
        { 
            TypeMap.Init();  
            _dataParser = parser;
            _dataReader = reader;
        } 
    }
}
