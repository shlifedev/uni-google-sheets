using System;

public interface IZGRequester
{ 
    void SearchGoogleDriveDirectory(string folderID, System.Action<Hamster.ZG.Http.Protocol.GetFolderInfo> callback);
    
    void ReadGoogleSpreadSheet(string sheetID, System.Action<Hamster.ZG.Http.Protocol.GetTableResult, string> callback);
    
    void WriteObject(string spreadSheetID, string sheetID, string key, string[] value, System.Action onWrited = null);
    
    void CreateDefaultTable(string folderID, string fileName, Action<string> callback);
}