using Hamster.ZG.IO;
using HamsterGoogleSpreadSheet.ZG.ZG.Core.Http.ProtocolV2.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG
{
    public interface IParser
    {
        void ParseSheet(ReadSpreadSheetResult sheetJsonData, bool generateCs, bool generateJson, IFIleWriter writer);
    }
}
