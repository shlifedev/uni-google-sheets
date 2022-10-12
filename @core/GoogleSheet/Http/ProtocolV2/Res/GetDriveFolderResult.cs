using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Protocol.v2.Res
{
    public partial class GetDriveFolderResult : Response
    {
        public List<string> fileId;
        public List<string> fileName;
        public List<int> fileType;
        public List<string> url; 
    }
}
