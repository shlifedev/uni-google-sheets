using GoogleSheet.IO;
using GoogleSheet.Protocol.v2.Res;

namespace GoogleSheet
{
    public interface IParser
    {
        void ParseSheet(ReadSpreadSheetResult sheetJsonData, bool generateCs, bool generateJson, IFIleWriter writer);
    }
}
