
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

namespace Example3.CustomType
{
    [Hamster.ZG.Attribute.TableStruct]
    public class Data : ITable
    { 
        static bool isLoaded = false;
        public static string spreadSheetID = "1PUospYVYWzMfUXV2IGCov6RhFtsphkCg1EVZut37ZnY"; // it is file id
        public static string sheetID = "0"; // it is sheet id
        public static Dictionary<int, Data> DataMap = new Dictionary<int, Data>(); 
        public static List<Data> DataList = new List<Data>();  
        public static UnityFileReader reader = new UnityFileReader();

		public Int32 index;
		public MyCustomStruct data;
		public MyCustomStruct data2;
		public Vector3 data3;
 

        public static void Write(Data data)
        { 
            TypeMap.Init();
            FieldInfo[] fields = typeof(Data).GetFields(BindingFlags.Public | BindingFlags.Instance);
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
                 Debug.Log("Data is already loaded! if you want reload then, forceReload parameter set true");
                 return;
            }
            /* Clear When Try Load */
            DataMap?.Clear();
            DataList?.Clear(); 
            //Type Map Init
            TypeMap.Init();
            //Reflection Field Datas.
            FieldInfo[] fields = typeof(Example3.CustomType.Data).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData("Example3.CustomType");
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(text);
                var table= result.tableResult; 
                var sheet = table["Data"];
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
                        Example3.CustomType.Data instance = new Example3.CustomType.Data();
                        for (int j = 0; j < typeInfos.Count; j++)
                        {
                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                            fields[j].SetValue(instance, readedValue);
                        }
                        //Add Data to Container
                        DataList.Add(instance);
                        DataMap.Add(instance.index, instance);
                    } 
                }
            }
            isLoaded = true;
        }
 
    }
}
        