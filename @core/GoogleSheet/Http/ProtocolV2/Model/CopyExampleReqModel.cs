using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.Protocol.v2.Req
{
    public class CopyExampleReqModel : Model
    {
        public CopyExampleReqModel()
        {
            this.instruction = (int)EInstruction.COPY_EXAMPLE;
        }
    }
}
