using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamsterGoogleSpreadSheet.ZG.ZG.Core.Http.ProtocolV2.Res
{
    public class WriteObjectResult : Response
    {
        /// <summary>
        /// true = updated
        /// false = created new
        /// </summary>
        public bool isUpdate;
    }
}
