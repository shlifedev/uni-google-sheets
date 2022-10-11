using GoogleSheet.IO;
using GoogleSheet.Protocol.v2.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet
{
    public interface IParser
    {
        void ParseSheet(ReadSpreadSheetResult sheetJsonData, bool generateCs, bool generateJson, IFIleWriter writer);
    }
}
