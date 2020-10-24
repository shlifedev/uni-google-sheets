
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

namespace GameTable.Item
{
    [Hamster.ZG.Attribute.TableStruct]
    public class Armor : ITable
    {
        public static string spreadSheetID = "1NSoUhE8YLchn8CnPPrtenff9DeyFck6EAjYA1UeZObU"; // it is file id
        public static string sheetID = "560083213"; // it is sheet id
        public static Dictionary<int, Armor> ArmorMap = new Dictionary<int, Armor>(); 
        public static List<Armor> ArmorList = new List<Armor>();  
        public static UnityFileReader reader = new UnityFileReader();

		public Int32 index;
		public String itemLocalizationID;
		public List<String> jobList;
		public Int32 armor;
		public Int32 requireLv;
		public String uiDesc;
 

        public static void Write(Armor data)
        { 
            //FieldInfo[] fields = typeof(Armor).GetFields(BindingFlags.Public | BindingFlags.Instance);
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
            ArmorMap?.Clear();
            ArmorList?.Clear(); 
            //Type Map Init
            TypeMap.Init();
            //Reflection Field Datas.
            FieldInfo[] fields = typeof(GameTable.Item.Armor).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData("GameTable.Item");
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(text);
                var table= result.tableResult; 
                var sheet = table["Armor"];
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
                        GameTable.Item.Armor instance = new GameTable.Item.Armor();
                        for (int j = 0; j < typeInfos.Count; j++)
                        {
                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                            fields[j].SetValue(instance, readedValue);
                        }
                        //Add Data to Container
                        ArmorList.Add(instance);
                        ArmorMap.Add(instance.index, instance);
                    } 
                }
            }
        }
 
    }
}
        