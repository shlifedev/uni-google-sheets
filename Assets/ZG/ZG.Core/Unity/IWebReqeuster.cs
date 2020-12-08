using Hamster.ZG;
using System;

public interface IZGRequester
{ 
    void SearchGoogleDriveDirectory(string folderID, System.Action<GetFolderInfo> callback);
    
    void ReadGoogleSpreadSheet(string sheetID, System.Action<GetTableResult, string> callback);
    
    void WriteObject(string spreadSheetID, string sheetID, string key, string[] value, System.Action onWrited = null);

    void CreateDefaultTable(string folderID, string fileName, Action<string> callback);

    void CopyExamples(string folderID, Action<string> callback); 
}

public class WriteDataSender
{
    public string password;
    public string instruction = "writeData";
    public string spreadSheetID;
    public string sheetID;
    public string key;
    public string[] value;

#if UNITY_2017_1_OR_NEWER
    public WriteDataSender(string spreadSheetID, string sheetID, string key, string[] value)
    {
        password = password = ZGSetting.ScriptPassword;
        this.spreadSheetID = spreadSheetID;
        this.sheetID = sheetID;
        this.key = key;
        this.value = value;
    }
#endif
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
#if UNITY_2017_1_OR_NEWER
    public CreateDefaultTableSender(string folderID, string fileName)
    {
        password = ZGSetting.ScriptPassword;
        this.folderID = folderID;
        this.fileName = fileName;
    }
#endif


    public CreateDefaultTableSender(string password, string folderID, string fileName)
    {
        this.password = password;
        this.folderID = folderID;
        this.fileName = fileName;
    }
}