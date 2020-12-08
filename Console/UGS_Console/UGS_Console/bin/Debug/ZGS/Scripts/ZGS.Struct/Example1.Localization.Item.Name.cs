
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGenerator.cs'  */
using Hamster.ZG;
using System;
using System.Collections.Generic;
using System.IO;
using Hamster.ZG.Type;
using System.Reflection;
using Hamster.ZG.IO.FileReader;

namespace Example1.Localization.Item
{
    [Hamster.ZG.Attribute.TableStruct]
    public partial class Name : ITable
    { 

        public delegate void OnLoadedFromGoogleSheets(List<Name> loadedList, Dictionary<string, Name> loadedDictionary);

        static bool isLoaded = false;
        static string spreadSheetID = "1usG2ox2jkDi3OtbCMNTIVCKsFTyZr5M0dKxW9tO2tWo"; // it is file id
        static string sheetID = "0"; // it is sheet id
        static FileReader reader = new FileReader();

/* Your Loaded Data Storage. */
        public static Dictionary<string, Name> NameMap = new Dictionary<string, Name>(); 
        public static List<Name> NameList = new List<Name>();   

/* Fields. */

		public String localeID;
		public String EN;
		public String KR;
  

#region fuctions

/*Write To GoogleSheet!*/

        public static void Write(Name data, System.Action onWriteCallback = null)
        { 
            TypeMap.Init();
            FieldInfo[] fields = typeof(Name).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var datas = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                var type = fields[i].FieldType;
                var writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
                datas[i] = writeRule; 
            }  
           
            GoogleDriveWebRequester.Instance.WriteObject(spreadSheetID, sheetID, datas[0], datas, onWriteCallback);

        } 
         

/*Load Data From Google Sheet! Working fine with runtime&editor*/

        public static void LoadFromGoogle(System.Action<List<Name>, Dictionary<string, Name>> onLoaded, bool updateCurrentData = false)
        {      
            TypeMap.Init();
            IZGRequester webInstance = GoogleDriveWebRequester.Instance as IZGRequester;
            if(updateCurrentData)
            {
                NameMap?.Clear();
                NameList?.Clear(); 
            }
            List<Name> callbackParamList = new List<Name>();
            Dictionary<string,Name> callbackParamMap = new Dictionary<string, Name>();
            webInstance.ReadGoogleSpreadSheet(spreadSheetID, (data, json) => {
            FieldInfo[] fields = typeof(Example1.Localization.Item.Name).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
              if (json != null)
                        {
                            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(json);
                            var table= result.tableResult; 
                            var sheet = table["Name"];
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
                                    Example1.Localization.Item.Name instance = new Example1.Localization.Item.Name();
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
                                       NameList.Add(instance);
                                       NameMap.Add(instance.localeID, instance);
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
                 Console.WriteLine("Name is already loaded! if you want reload then, forceReload parameter set true");
                 return;
            }
            /* Clear When Try Load */
            NameMap?.Clear();
            NameList?.Clear(); 
            //Type Map Init
            TypeMap.Init();
            //Reflection Field Datas.
            FieldInfo[] fields = typeof(Example1.Localization.Item.Name).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData("Example1.Localization.Item");
            if (text != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(text);
                var table= result.tableResult; 
                var sheet = table["Name"];
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
                        Example1.Localization.Item.Name instance = new Example1.Localization.Item.Name();
                        for (int j = 0; j < typeInfos.Count; j++)
                        {
                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                            fields[j].SetValue(instance, readedValue);
                        }
                        //Add Data to Container
                        NameList.Add(instance);
                        NameMap.Add(instance.localeID, instance);
                    } 
                }
            }
            isLoaded = true;
        }
 


#endregion

#region OdinInsepctorExtentions
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button("UploadToSheet")]
    public void Upload()
    {
        Write(this);
    }
#endif
#endregion
    }
}
        