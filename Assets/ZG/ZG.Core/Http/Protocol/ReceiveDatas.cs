using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public enum ReceivedResult
{
    Success = 0,
    Error = 1,
    Auth = 2
}
public enum WriteResult
{
    Create = 0,
    Update = 1
}
public class ReceivedData
{
    public string instruction;
    public ReceivedResult result;
}
public class WriteObjectResult : ReceivedData
{
    public WriteResult writeResult;
}

public class CreateDefaultTableResult : ReceivedData
{
    public string fileID;
    public string fileName;
    public int fileType;
    public string url;
}

public class GetTableResult : ReceivedData
{
    /// <summary>
    /// key : sheet
    /// value (type/values)
    ///   - key : type string 
    ///   - value : data
    /// </summary> 
    public List<int> TableTypes;
    public Dictionary<string, Dictionary<string, List<string>>> tableResult;  
    public List<string> sheetIDList;
    public string spreadSheetID;
    public string spreadSheetName;
}

public class GetFolderInfo : ReceivedData
{
    public List<string> fileID;
    public List<string> fileName;
    public List<int> fileType;
    public List<string> url;
}

public class CopyExampleResult : ReceivedData
{
    public string createdFolderId;
}
