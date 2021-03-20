
using System.Collections.Generic;

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