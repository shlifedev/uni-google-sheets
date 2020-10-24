
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
using Hamster.ZG.Http;
using System;
using System.Collections.Generic;
using System.IO;
using Hamster.ZG.Http.Protocol;
using Hamster.ZG.Type;
using System.Reflection;
using UnityEngine;

namespace PlayerData
{
    [Hamster.ZG.Attribute.TableStruct]
    public class Data2 : ITable
    {
        public static string spreadSheetID = "1AKycOds6CnPYV1qIQz3S6Hk86DvJgtTLxnYISaLW2sY"; // it is file id
        public static string sheetID = "1359270438"; // it is sheet id
        public static Dictionary<string, Data2> Data2Map = new Dictionary<string, Data2>(); 
        public static List<Data2> Data2List = new List<Data2>();  
        public static UnityFileReader reader = new UnityFileReader();

		public String index;
		public Single speed;
		public Int32 damage;
		public Vector3 position;
 

        public static void Write(Data2 data)
        { 
            //FieldInfo[] fields = typeof(Data2).GetFields(BindingFlags.Public | BindingFlags.Instance);
            //var datas = new string[fields.Length];
            //for (int i = 0; i < fields.Length; i++)
            //{
            //    var type = fields[i].FieldType;
            //    var writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
            //    datas[i] = writeRule; 
            //} 
            //if(Application.isPlaying) 
            //    Request.Init(ZeroGoogleSheetUnity.Engine.UnityEngineWebRequester.Instance); 
            //Request.Execute(new QueryAppendRow(spreadSheetID, sheetID, datas));
        } 
        

        public static void Load()
        {
            /* Clear When Try Load */
            Data2Map?.Clear();
            Data2List?.Clear(); 
            //Type Map Init
            TypeMap.Init();
            //Reflection Field Datas.
            FieldInfo[] fields = typeof(PlayerData.Data2).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData("PlayerData");
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(text);
                var table= result.tableResult; 
                var sheet = table["Data2"];
                    foreach (var pNameAndTypeName in sheet.Keys)
                    {
                        var split = pNameAndTypeName.Replace(" ", null).Split(':');
                        var propertyName = split[0];
                        var type = split[1];
                        typeInfos.Add((pNameAndTypeName, propertyName, type));
                        var typeValues = sheet[pNameAndTypeName];
                        typeValuesCList.Add(typeValues);
                    } 
                if (typeValuesCList.Count != 0)
                {
                    int rows = typeValuesCList[0].Count;
                    for (int i = 0; i < rows; i++)
                    {
                        PlayerData.Data2 instance = new PlayerData.Data2();
                        for (int j = 0; j < typeInfos.Count; j++)
                        {
                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                            fields[j].SetValue(instance, readedValue);
                        }
                        //Add Data to Container
                        Data2List.Add(instance);
                        Data2Map.Add(instance.index, instance);
                    } 
                }
            }
        }
 
    }
}
        