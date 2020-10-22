using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http.Protocol
{
    public class GetTableResult : Result
    {
        /// <summary>
        /// key : sheet
        /// value (type/values)
        ///   - key : type string 
        ///   - value : data
        /// </summary>
        public Dictionary<string, Dictionary<string, List<string>>> tableResult;  
        public List<string> sheetIDList;
        public string spreadSheetID;
        public string spreadSheetName;
    }
}
