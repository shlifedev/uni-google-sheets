﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGS.Protocol.v2.Res
{
    public class CreateDefaultSheetResult : Response
    {
        public string fileID;
        public string fileName;
        public string fileType;
        public string url;
    }
}
