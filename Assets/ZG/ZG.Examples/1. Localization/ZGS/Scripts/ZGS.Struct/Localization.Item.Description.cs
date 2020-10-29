
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
using Hamster.ZG;
using Hamster.ZG.Http;
using System;
using System.Collections.Generic;
using System.IO;
using Hamster.ZG.Http.Protocol;
using Hamster.ZG.Type;
using System.Reflection;
using UnityEngine;

namespace Localization.Item
{
    [Hamster.ZG.Attribute.TableStruct]
    public class Description : ITable
    { 
        static bool isLoaded = false;
        public static string spreadSheetID = "18CCNohygzagd79mnYBKr0lQDnNiAomhscdnaocUj9Xo"; // it is file id
        public static string sheetID = "876985649"; // it is sheet id
        public static Dictionary<string, Description> DescriptionMap = new Dictionary<string, Description>(); 
        public static List<Description> DescriptionList = new List<Description>();  
        public static UnityFileReader reader = new UnityFileReader();

		public String localeID;
		public String EN;
		public String KR;
 

        public static void Write(Description data)
        { 
            TypeMap.Init();
            FieldInfo[] fields = typeof(Description).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var datas = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var type = fields[i].FieldType;
                var writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
                datas[i] = writeRule; 
            }  
           
#if UNITY_EDITOR
if(Application.isPlaying == false)
{
            UnityEditorWebRequest.Instance.POST_WriteData(spreadSheetID, sheetID, datas[0], datas);
}
else
{
            UnityPlayerWebRequest.Instance.POST_WriteData(spreadSheetID, sheetID, datas[0], datas);
}
#endif
        } 
        

        public static void Load(bool forceReload = false)
        {
            if(isLoaded && forceReload == false)
            {
                 Debug.Log("Description is already loaded! if you want reload then, forceReload parameter set true");
                 return;
            }
            /* Clear When Try Load */
            DescriptionMap?.Clear();
            DescriptionList?.Clear(); 
            //Type Map Init
            TypeMap.Init();
            //Reflection Field Datas.
            FieldInfo[] fields = typeof(Localization.Item.Description).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData("Localization.Item");
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(text);
                var table= result.tableResult; 
                var sheet = table["Description"];
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
                        Localization.Item.Description instance = new Localization.Item.Description();
                        for (int j = 0; j < typeInfos.Count; j++)
                        {
                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                            fields[j].SetValue(instance, readedValue);
                        }
                        //Add Data to Container
                        DescriptionList.Add(instance);
                        DescriptionMap.Add(instance.localeID, instance);
                    } 
                }
            }
            isLoaded = true;
        }
 
    }
}
        