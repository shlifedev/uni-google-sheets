
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
using GoogleSheet.;
using System;
using System.Collections.Generic;
using System.IO;
using GoogleSheet.Type;
using System.Reflection;
using GoogleSheet.IO.FileReader;
using GoogleSheet.Protocol.v2.Res;


namespace DefaultTable
{
    [GoogleSheet.Attribute.TableStruct]
    public partial class Data : ITable
    { 

        public delegate void OnLoadedFromGoogleSheets(List<Data> loadedList, Dictionary<int, Data> loadedDictionary);

        static bool isLoaded = false;
        static string spreadSheetID = "1SgkBh-HngzRYL3Vl9PrxbMQ_vY89f0hx_BGpgXP4Ffc"; // it is file id
        static string sheetID = "0"; // it is sheet id
        static FileReader reader = new FileReader();

/* Your Loaded Data Storage. */
        public static Dictionary<int, Data> DataMap = new Dictionary<int, Data>(); 
        public static List<Data> DataList = new List<Data>();   

/* Fields. */

		public Int32 index;
		public Int32 intValue;
		public String strValue;
  

#region fuctions

/*Write To GoogleSheet!*/

        public static void Write(Data data, System.Action<WriteObjectResult> onWriteCallback = null)
        { 
            TypeMap.Init();
            FieldInfo[] fields = typeof(Data).GetFields(BindingFlags.Public | BindingFlags.Instance);
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
             GoogleDriveWebRequesterV2.Instance.WriteObject(new GoogleSheet.Protocol.v2.Req.WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);
        } 
         

/*Load Data From Google Sheet! Working fine with runtime&editor*/

        public static void LoadFromGoogle(System.Action<List<Data>, Dictionary<int, Data>> onLoaded, bool updateCurrentData = false)
        {      
            TypeMap.Init();
            IHttpProtcol webInstance = GoogleDriveWebRequesterV2.Instance as IHttpProtcol;
 
            if(updateCurrentData)
            {
                DataMap?.Clear();
                DataList?.Clear(); 
            }
            List<Data> callbackParamList = new List<Data>();
            Dictionary<int,Data> callbackParamMap = new Dictionary<int, Data>();
            webInstance.ReadSpreadSheet(new GoogleSheet.Protocol.v2.Req.ReadSpreadSheetReqModel(spreadSheetID), OnError, (data) => {
            FieldInfo[] fields = typeof(DefaultTable.Data).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
              if (data != null)
                        {
                            var result = data;
                            var table= result.jsonObject; 
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
                                    DefaultTable.Data instance = new DefaultTable.Data();
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
                                    callbackParamMap .Add(instance.index, instance);
                                    if(updateCurrentData)
                                    {
                                       DataList.Add(instance);
                                       DataMap.Add(instance.index, instance);
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
                 Console.WriteLine("Data is already loaded! if you want reload then, forceReload parameter set true");
                 return;
            }
            /* Clear When Try Load */
            DataMap?.Clear();
            DataList?.Clear(); 
            //Type Map Init
            TypeMap.Init();
            //Reflection Field Datas.
            FieldInfo[] fields = typeof(DefaultTable.Data).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData("DefaultTable");
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSpreadSheetResult>(text);
                var table= result.jsonObject; 
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
                                DefaultTable.Data instance = new DefaultTable.Data();
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
                        DataList.Add(instance);
                        DataMap.Add(instance.index, instance);
                  
                       
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
        