using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Protocol.v2.Req
{
    public class CopyFolderReqModel : Model
    {
        public string folderId;

        public CopyFolderReqModel(string folderId)
        {
            this.folderId = folderId;
            this.instruction = (int)EInstruction.COPY_FOLDER;
        }
    }
}
