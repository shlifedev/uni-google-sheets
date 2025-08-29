using GoogleSheet.Protocol.v2.Req;
using GoogleSheet.Protocol.v2.Res;

namespace GoogleSheet
{

    public interface IHttpProtcol
    {
        void GetDriveDirectory(GetDriveDirectoryReqModel mdl, System.Action<System.Exception> errResponse, System.Action<GetDriveFolderResult> callback);
        void ReadSpreadSheet(ReadSpreadSheetReqModel mdl, System.Action<System.Exception> errResponse, System.Action<ReadSpreadSheetResult> callback);
        void WriteObject(WriteObjectReqModel mdl, System.Action<System.Exception> errResponse, System.Action<GoogleSheet.Protocol.v2.Res.WriteObjectResult> callback);
        void CreateDefaultSheet(CreateDefaultReqModel mdl, System.Action<System.Exception> errResponse, System.Action<CreateDefaultSheetResult> callback);
        void CopyExample(CopyExampleReqModel mdl, System.Action<System.Exception> errResponse, System.Action<CreateExampleResult> callback);
        void CopyFolder(CopyFolderReqModel mdl, System.Action<System.Exception> errResponse, System.Action<CreateExampleResult> callback);
    }
    public class WriteDataSender
    {
        public string password;
        public string instruction = "writeData";
        public string spreadSheetID;
        public string sheetID;
        public string key;
        public string[] value;


        public WriteDataSender(string password, string spreadSheetID, string sheetID, string key, string[] value)
        {
            this.password = password;
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
        public CreateDefaultTableSender(string password, string folderID, string fileName)
        {
            this.password = password;
            this.folderID = folderID;
            this.fileName = fileName;
        }
    }
}