using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamsterGoogleSpreadSheet.ZG.ZG.Core.Http.ProtocolV2.Req
{
    public class ReadSpreadSheetReqModel : Model
    {
        public string fileId;

        public ReadSpreadSheetReqModel(string fileId)
        {
            this.instruction = (int)EInstruction.READ_SPREADSHEET;
            this.fileId = fileId;
        }
    }
}
