using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http
{
    public class QueryGetTable : Query
    {  
        public string sheetID;

        public QueryGetTable(string sheetID)
        {
            this.sheetID = sheetID;
        }
        public override string Execute()
        {  
            return requester.Get(baseURL+$"?instruction=getTable&sheetID={sheetID}", this.MethodString, this);
        }
    }
}
