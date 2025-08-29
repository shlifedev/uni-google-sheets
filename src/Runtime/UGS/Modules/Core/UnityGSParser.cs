#if UNITY_2017_1_OR_NEWER || UNITY_BUILD  
using GoogleSheet;
using GoogleSheet.IO;
using GoogleSheet.IO.Generator;
using GoogleSheet.Protocol.v2.Res;
using System.Linq;

namespace UGS.IO
{
    /*
        백업 플
     */
    public class UnityGSParser : IParser
    {
        public void ParseSheet(ReadSpreadSheetResult sheetJsonData, bool generateCs, bool generateJson, IFIleWriter writer)
        {
            ReadSpreadSheetResult getTableResult = sheetJsonData;
            if (generateJson)
            {
                var result = GenerateData(getTableResult);
                writer?.WriteData(getTableResult.spreadSheetName, result);
            }


            int count = 0;
            foreach (var sheet in getTableResult.jsonObject)
            {
                string[] sheetInfoTypes = null;
                string[] sheetInfoNames = null;
                bool[] isEnum = null;
                ///generate json data 
                if (generateCs)
                {
                    sheetInfoTypes = new string[sheet.Value.Count()];
                    sheetInfoNames = new string[sheet.Value.Count()];
                    isEnum = new bool[sheet.Value.Count()];
                    int i = 0;
                    foreach (var type in sheet.Value)
                    {
                        var id = type.Key;
                        var split = id.Replace(" ", null).Split(':');
                        sheetInfoTypes[i] = split[1];
                        sheetInfoNames[i] = split[0];

                        if (split[1].Contains("Enum<"))
                        {
                            isEnum[i] = true;
                        }
                        else
                        {
                            isEnum[i] = false;
                        }

                        i++;
                    }
                    SheetInfo info = new SheetInfo();
                    info.spreadSheetID = getTableResult.spreadSheetID;
                    info.sheetID = getTableResult.sheetIDList[count];
                    info.sheetFileName = getTableResult.spreadSheetName;
                    info.sheetName = sheet.Key;
                    info.sheetTypes = sheetInfoTypes;
                    info.sheetVariableNames = sheetInfoNames;
                    info.isEnumChecks = isEnum;

                    var result = GenerateCS(info);
                    writer?.WriteCS(info.sheetFileName + "." + info.sheetName, result);
                }
                count++;
            }
        }
        private string GenerateData(ReadSpreadSheetResult tableResult)
        {
            DataGenerator dataGen = new DataGenerator(tableResult);
            var result = dataGen.Generate();
            return result;
        }

        private string GenerateCS(SheetInfo info)
        {
            CodeGeneratorUnityEngine sheetGenerator = new CodeGeneratorUnityEngine(info);
            var result = sheetGenerator.Generate();
            return result;
        }


    }

}


#endif