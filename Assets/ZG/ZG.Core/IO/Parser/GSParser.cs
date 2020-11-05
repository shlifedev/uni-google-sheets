 
using Hamster.ZG.IO;
using Hamster.ZG.IO.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG
{
    public class GSParser : IParser
    {
        public void ParseSheet(string sheetJsonData, bool generateCs, bool generateJson, IFIleWriter writer)
        {
            GetTableResult getTableResult = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(sheetJsonData);
            foreach (var sheet in getTableResult.tableResult)
            {
                string[] sheetInfoTypes = null;
                string[] sheetInfoNames = null; 
                ///generate json data
                if (generateJson)
                { 
                    var result = GenerateData(getTableResult);
                    writer?.WriteData(getTableResult.spreadSheetName+"."+ sheet.Key, result);
                } 
                if (generateCs)
                {
                    sheetInfoTypes = new string[sheet.Value.Count()];
                    sheetInfoNames = new string[sheet.Value.Count()];
                    int i = 0;
                    foreach (var type in sheet.Value)
                    {
                        var id = type.Key; 
                        var split = id.Replace(" ", null).Split(':');
                        sheetInfoTypes[i] = split[1];
                        sheetInfoNames[i] = split[0];
                        i++;
                    }
                    SheetInfo info = new SheetInfo();
                    info.sheetFileName = getTableResult.spreadSheetName;
                    info.sheetName = sheet.Key;
                    info.sheetTypes = sheetInfoTypes;
                    info.sheetVariableNames = sheetInfoNames;

                    var result = GenerateCS(info);
                    writer?.WriteCS(info.sheetFileName + "." + info.sheetName, result);
                }
            }
        }
        private string GenerateData(GetTableResult tableResult)
        {
            DataGenerator dataGen = new DataGenerator(tableResult);
            var result = dataGen.Generate();  
            return result;
        }

        private string GenerateCS(SheetInfo info)
        {
            CodeGenerator sheetGenerator = new CodeGenerator(info);
            var result = sheetGenerator.Generate();
            return result;
        }


    }

}


