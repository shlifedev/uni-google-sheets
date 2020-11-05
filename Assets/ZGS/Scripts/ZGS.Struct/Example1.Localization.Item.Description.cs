
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

namespace Example1.Localization.Item
{
    [Hamster.ZG.Attribute.TableStruct]
    public class Description : ITable
    { 

        public delegate void OnLoadedFromGoogleSheets(List<Description> loadedList, Dictionary<string, Description> loadedDictionary);

        static bool isLoaded = false;
        static string spreadSheetID = "1_4ZAVZxieF3WWWyMP1focWuxVlm7j8NL0uJ0wy8rewM"; // it is file id
        static string sheetID = "876985649"; // it is sheet id
        static UnityFileReader reader = new UnityFileReader();

/* Your Loaded Data Storage. */
        public static Dictionary<string, Description> DescriptionMap = new Dictionary<string, Description>(); 
        public static List<Description> DescriptionList = new List<Description>();   

/* Fields. */

		public String localeID;
		public String EN;
		public String KR;
  

#region fuctions

/*Write To GoogleSheet!*/

        public static void Write(Description data, System.Action onWriteCallback = null)
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
            UnityEditorWebRequest.Instance.WriteObject(spreadSheetID, sheetID, datas[0], datas, onWriteCallback);
}
else
{
            UnityPlayerWebRequest.Instance.WriteObject(spreadSheetID, sheetID, datas[0], datas, onWriteCallback);
}
#endif
        } 
         

/*Load Data From Google Sheet! Working fine with runtime&editor*/

        public static void LoadFromGoogle(System.Action<List<Description>, Dictionary<string, Description>> onLoaded, bool updateCurrentData = false)
        {      
            TypeMap.Init();
            IZGRequester webInstance = null;
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                webInstance = UnityEditorWebRequest.Instance as IZGRequester;
            }
            else
            {
                webInstance = UnityPlayerWebRequest.Instance as IZGRequester;
            }
#endif
#if !UNITY_EDITOR
                 webInstance = UnityPlayerWebRequest.Instance as IZGRequester;
#endif
            if(updateCurrentData)
            {
                DescriptionMap?.Clear();
                DescriptionList?.Clear(); 
            }
            List<Description> callbackParamList = new List<Description>();
            Dictionary<string,Description> callbackParamMap = new Dictionary<string, Description>();
            webInstance.ReadGoogleSpreadSheet(spreadSheetID, (data, json) => {
            FieldInfo[] fields = typeof(Example1.Localization.Item.Description).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
              if (json != null)
                        {
                            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(json);
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
                                    Example1.Localization.Item.Description instance = new Example1.Localization.Item.Description();
                                    for (int j = 0; j < typeInfos.Count; j++)
                                    {
                                        var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                        var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                                        fields[j].SetValue(instance, readedValue);
                                    }
                                    //Add Data to Container
                                    callbackParamList.Add(instance);
                                    callbackParamMap .Add(instance.localeID, instance);
                                    if(updateCurrentData)
                                    {
                                       DescriptionList.Add(instance);
                                       DescriptionMap.Add(instance.localeID, instance);
                                    }
                                } 
                            }
                        }

                      onLoaded?.Invoke(callbackParamList, callbackParamMap);
            });
        }

            

/*Load From Cached Json. Require Generate Data.*/

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
            FieldInfo[] fields = typeof(Example1.Localization.Item.Description).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData("Example1.Localization.Item");
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
                        Example1.Localization.Item.Description instance = new Example1.Localization.Item.Description();
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
 

#endregion

    }
}
        