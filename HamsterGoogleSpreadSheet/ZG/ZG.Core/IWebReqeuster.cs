using Hamster.ZG;
using System;

public interface IZGRequester
{ 
    void SearchGoogleDriveDirectory(string folderID, System.Action<System.Exception> errCallback, System.Action<GetFolderInfo> callback);
    
    void ReadGoogleSpreadSheet(string sheetID, System.Action<System.Exception> errCallback, System.Action<GetTableResult, string> callback);
    
    void WriteObject(string spreadSheetID, string sheetID, string key, string[] value, System.Action<System.Exception> errCallback,  System.Action onWrited = null);

    void CreateDefaultTable(string folderID, string fileName, System.Action<System.Exception> errCallback, Action<string> callback);

    void CopyExamples(string folderID, System.Action<System.Exception> errCallback, Action<string> callback); 
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