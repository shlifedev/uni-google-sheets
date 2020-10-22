using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.Http
{
    public class QueryGetFolderInfo : Query
    {  
        public string folderID = "";
        public QueryGetFolderInfo(string folderID)
        {
            this.folderID = folderID;
        }
        public override string Execute()
        {  
            return requester.Get(baseURL+ $"?instruction=getFolderInfo&folderID={this.folderID}", this.MethodString, this);
        }
    }
}
