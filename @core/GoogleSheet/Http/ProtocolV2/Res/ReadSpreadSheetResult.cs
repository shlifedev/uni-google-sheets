using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Protocol.v2.Res
{
    public partial class ReadSpreadSheetResult : Response 
    {
        public Dictionary<string, Dictionary<string, List<string>>> jsonObject;
        public string spreadSheetName;
        public string spreadSheetID;
        public List<string> sheetIDList;
        public List<EFileType> tableTypes;  
    }
}
