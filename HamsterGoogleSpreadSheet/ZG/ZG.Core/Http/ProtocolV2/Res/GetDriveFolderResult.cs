using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamsterGoogleSpreadSheet.ZG.ZG.Core.Http.ProtocolV2.Res
{
    public class GetDriveFolderResult : Response
    {
        public List<string> fileId;
        public List<string> fileName;
        public List<int> fileType;
        public List<string> url; 
    }
}
