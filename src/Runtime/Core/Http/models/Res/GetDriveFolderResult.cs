using System.Collections.Generic;

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
