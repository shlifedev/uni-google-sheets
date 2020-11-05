 
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.IO.Generator
{
    public class DataGenerator : ICodeGenerator
    {
        GetTableResult info ;
        public DataGenerator(GetTableResult info)
        {
             this.info = info;
        }
        /// <summary>
        /// Vertical Data To Horizontal 
        /// </summary>
        /// <returns></returns>
        public string Generate()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(info);

            // verticla data to horizontal data, obsolute//


            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);
            //Newtonsoft.Json.JsonWriter writer = new JsonTextWriter(sw);
            //foreach (var table in info.tableResult)
            //{
            //    var className = table.Key; 
            //    var typeArray = table.Value.Keys.ToList<string>();
            //    if(typeArray.Count == 0)
            //        return null;
            //    var typeCount = table.Value.Keys.Count(); 
            //    var valueCount = table.Value[typeArray[0]].Count;  

            //    List<List<string>> values = new List<List<string>>();
            //    for(int typeIdx  = 0; typeIdx < typeCount; typeIdx++)
            //    {
            //        var type = typeArray[typeIdx];
            //        values.Add(table.Value[type]); 
            //    } 
            //    writer.WriteStartArray();
            //    for (int v = 0; v < valueCount; v++)
            //    {
            //        writer.WriteStartObject();
            //        for (int i = 0; i < typeCount; i++)
            //        {
            //            var type = typeArray[i];
            //            var split = type.Replace(" ", null).Split(':'); // split typeName : propertyName
            //            var value = values[i][v];
            //            writer.WritePropertyName(split[0]);
            //            writer.WriteValue(value); 
            //        }
            //        writer.WriteEndObject();
            //    }
            //    writer.WriteEndArray();
            //}

            //return sb.ToString();
        }
    }
}
