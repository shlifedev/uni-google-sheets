using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGS.Protocol.v2.Res
{
    public class CreateExampleResult : Response
    {
        /// <summary>
        /// true = updated
        /// false = created new
        /// </summary>
        public string createdFolderId;
    }
}
