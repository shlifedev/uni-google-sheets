using System;

public interface IZGRequester
{
    void GET_ReqFolderFiles(string folderID, System.Action<Hamster.ZG.Http.Protocol.GetFolderInfo> callback);
    void GET_TableData(string sheetID, System.Action<Hamster.ZG.Http.Protocol.GetTableResult, string> callback);
    void POST_WriteData(string spreadSheetID, string sheetID, string key, string[] value);
    void POST_CreateDefaultTable(string folderID, string fileName, Action<string> callback);
}