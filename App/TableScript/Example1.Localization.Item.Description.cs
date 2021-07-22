
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
using Hamster.UG;
using System;
using System.Collections.Generic;
using System.IO;
using Hamster.UG.Type;
using System.Reflection;
using Hamster.UG.IO.FileReader;
using UGS.Protocol.v2.Res;


namespace Example1.Localization.Item
{
    [Hamster.UG.Attribute.TableStruct]
    public partial class Description : ITable
    { 

        public delegate void OnLoadedFromGoogleSheets(List<Description> loadedList, Dictionary<string, Description> loadedDictionary);

        static bool isLoaded = false;
        static string spreadSheetID = "1IKvUgU7i-MGnWQP_GES5DbXTd_oD2sreqe1SS_BzCeA"; // it is file id
        static string sheetID = "876985649"; // it is sheet id
        static FileReader reader = new FileReader();

/* Your Loaded Data Storage. */
        public static Dictionary<string, Description> DescriptionMap = new Dictionary<string, Description>(); 
        public static List<Description> DescriptionList = new List<Description>();   

/* Fields. */

		public String localeID;
		public String EN;
		public String KR;
  

#region fuctions

/*Write To GoogleSheet!*/

        public static void Write(Description data, System.Action<WriteObjectResult> onWriteCallback = null)
        { 
            TypeMap.Init();
            FieldInfo[] fields = typeof(Description).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var datas = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var type = fields[i].FieldType;
                string writeRule = null;
                if(type.IsEnum)
                {
                    writeRule = TypeMap.EnumMap[type.Name].Write(fields[i].GetValue(data));
                }
                else
                {
                    writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
                } 
                datas[i] = writeRule; 
            }  
             GoogleDriveWebRequesterV2.Instance.WriteObject(new UGS.Protocol.v2.Req.WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);
        } 
         

/*Load Data From Google Sheet! Working fine with runtime&editor*/

        public static void LoadFromGoogle(System.Action<List<Description>, Dictionary<string, Description>> onLoaded, bool updateCurrentData = false)
        {      
            TypeMap.Init();
            IHttpProtcol webInstance = GoogleDriveWebRequesterV2.Instance as IHttpProtcol;
 
            if(updateCurrentData)
            {
                DescriptionMap?.Clear();
                DescriptionList?.Clear(); 
            }
            List<Description> callbackParamList = new List<Description>();
            Dictionary<string,Description> callbackParamMap = new Dictionary<string, Description>();
            webInstance.ReadSpreadSheet(new UGS.Protocol.v2.Req.ReadSpreadSheetReqModel(spreadSheetID), OnError, (data) => {
            FieldInfo[] fields = typeof(Example1.Localization.Item.Description).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
              if (data != null)
                        {
                            var result = data;
                            var table= result.jsonObject; 
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
                                       try
                                       {
                                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                                             fields[j].SetValue(instance, readedValue);
                                       }
                                       catch
                                       {
                                        var type = typeInfos[j].type;
                                            type = type.Replace("Enum<", null);
                                            type = type.Replace(">", null);

                                             var readedValue = TypeMap.EnumMap[type].Read(typeValuesCList[j][i]);
                                             fields[j].SetValue(instance, readedValue); 
                                      }
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
                 Console.WriteLine("Description is already loaded! if you want reload then, forceReload parameter set true");
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
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSpreadSheetResult>(text);
                var table= result.jsonObject; 
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
                                    try{
                                        var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                        var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                                        fields[j].SetValue(instance, readedValue);
                                       }
                                      catch{
                                        var type = typeInfos[j].type;
                                            type = type.Replace("Enum<", null);
                                            type = type.Replace(">", null);

                                             var readedValue = TypeMap.EnumMap[type].Read(typeValuesCList[j][i]);
                                             fields[j].SetValue(instance, readedValue);
            
                                          }
                              }

                         //Add Data to Container
                        DescriptionList.Add(instance);
                        DescriptionMap.Add(instance.localeID, instance);
                  
                       
                         } 
                }
       isLoaded = true;
            }
      
        }
 


#endregion

 
    public void Upload()
    {
        Write(this);
    }
 
 
    public static void OnError(System.Exception e){
         throw e;
    }
 
    }
}
        