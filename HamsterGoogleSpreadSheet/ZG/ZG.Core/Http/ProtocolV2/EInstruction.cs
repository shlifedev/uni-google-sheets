using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HamsterGoogleSpreadSheet.ZG.ZG.Core.Http.ProtocolV2
{ 
    public enum EInstruction
    {
        SEARCH_GOOGLE_DRIVE = 0,
        READ_SPREADSHEET = 1,
        WRITE_OBJECT = 2,
        CREATE_DEFAULT_TABLE = 101,
        COPY_EXAMPLE = 102
    }
}
