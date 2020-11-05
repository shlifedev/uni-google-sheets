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