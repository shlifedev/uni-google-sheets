using Hamster.ZG.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.IO
{

    public class CodeGenerator: ICodeGenerator
    {
        private SheetInfo sheetInfo; 
        private string generateForm = @"
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGenerator.cs'  */
@assemblys
namespace @namespace
{
    [Hamster.ZG.Attribute.TableStruct]
    public partial class @class : ITable
    { 

        public delegate void OnLoadedFromGoogleSheets(List<@class> loadedList, Dictionary<@keyType, @class> loadedDictionary);

        static bool isLoaded = false;
        static string spreadSheetID = ""@spreadSheetID""; // it is file id
        static string sheetID = ""@sheetID""; // it is sheet id
        static FileReader reader = new FileReader();

/* Your Loaded Data Storage. */
        public static Dictionary<@keyType, @class> @classMap = new Dictionary<@keyType, @class>(); 
        public static List<@class> @classList = new List<@class>();   

/* Fields. */
@types  

#region fuctions

/*Write To GoogleSheet!*/
@writeFunction 

/*Load Data From Google Sheet! Working fine with runtime&editor*/
@loadFromGoogleFunction

/*Load From Cached Json. Require Generate Data.*/
@loadFunction 


#endregion

#region OdinInsepctorExtentions
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.Button(""UploadToSheet"")]
    public void Upload()
    {
        Write(this);
    }
#endif
#endregion
    }
}
        ";
        public string GenerateForm { get => this.generateForm; }
        public CodeGenerator(SheetInfo info)
        {
            this.sheetInfo = info;
        }

        private void WriteTypes(string[] types, string[] names)
        {
            if (types != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine();
                for (int i = 0; i < types.Length; i++)
                {
                    builder.AppendLine($"\t\tpublic {GetCSharpRepresentation(TypeMap.StrMap[types[i]], true)} {names[i]};");
                }
                generateForm = generateForm.Replace("@types", builder.ToString());
                generateForm = generateForm.Replace("@keyType", types[0]);
            } 
        }

        static string GetCSharpRepresentation(System.Type t, bool trimArgCount)
        {
            if (t.IsGenericType)
            {
                var genericArgs = t.GetGenericArguments().ToList();

                return GetCSharpRepresentation(t, trimArgCount, genericArgs);
            }

            return t.Name;
        }

        static string GetCSharpRepresentation(System.Type t, bool trimArgCount, List<System.Type> availableArguments)
        {
            if (t.IsGenericType)
            {
                string value = t.Name;
                if (trimArgCount && value.IndexOf("`") > -1)
                {
                    value = value.Substring(0, value.IndexOf("`"));
                }

                if (t.DeclaringType != null)
                    value = GetCSharpRepresentation(t.DeclaringType, trimArgCount, availableArguments) + "+" + value;
                string argString = "";
                var thisTypeArgs = t.GetGenericArguments();
                for (int i = 0; i < thisTypeArgs.Length && availableArguments.Count > 0; i++)
                {
                    if (i != 0) argString += ", ";
                    argString += GetCSharpRepresentation(availableArguments[0], trimArgCount);
                    availableArguments.RemoveAt(0);
                }
                if (argString.Length > 0)
                {
                    value += "<" + argString + ">";
                }

                return value;
            }

            return t.Name;
        }


        private void WriteWriteFunction(string @class)
        {
            string writeFunction =$@"
        public static void Write({@class} data, System.Action onWriteCallback = null)
        {{ 
            TypeMap.Init();
            FieldInfo[] fields = typeof({@class}).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var datas = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {{
                var type = fields[i].FieldType;
                var writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
                datas[i] = writeRule; 
            }}  
           
            GoogleDriveWebRequester.Instance.WriteObject(spreadSheetID, sheetID, datas[0], datas, onWriteCallback);

        }} 
        ";

            generateForm = generateForm.Replace("@writeFunction", writeFunction);
        }
        private void WriteNamespace(string @namespace)
        {
            generateForm = generateForm.Replace("@namespace", @namespace);
        }
        private void WriteClassReplace(string @class)
        {
            generateForm = generateForm.Replace("@class", @class);
        }
        private void WriteLoader(string @path)
        {
            generateForm = generateForm.Replace("@assemblys", null);
        }


        private void WriteSpreadSheetData(string spreadID, string sheetID)
        {
            generateForm = generateForm.Replace("@spreadSheetID", spreadID);
            generateForm = generateForm.Replace("@sheetID", sheetID);
        }

        private void WriteAssembly(string[] assemblys)
        {
            if (assemblys != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var data in assemblys)
                    builder.AppendLine($"using {data};");
                generateForm = generateForm.Replace("@assemblys", builder.ToString());
            }
        }

 

        private void WriteLoadFromGoogleFunction()
        {

      
            StringBuilder builder = new StringBuilder();
            builder.Append($@"
        public static void LoadFromGoogle(System.Action<List<@class>, Dictionary<@keyType, @class>> onLoaded, bool updateCurrentData = false)
        {{      
            TypeMap.Init();
            IZGRequester webInstance = GoogleDriveWebRequester.Instance as IZGRequester;
            if(updateCurrentData)
            {{
                @classMap?.Clear();
                @classList?.Clear(); 
            }}
            List<@class> callbackParamList = new List<@class>();
            Dictionary<@keyType,@class> callbackParamMap = new Dictionary<@keyType, @class>();
            webInstance.ReadGoogleSpreadSheet(spreadSheetID, (data, json) => {{
            FieldInfo[] fields = typeof(@namespace.@class).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
              if (json != null)
                        {{
                            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(json);
                            var table= result.tableResult; 
                            var sheet = table[""@class""];
                                foreach (var pNameAndTypeName in sheet.Keys)
                                {{
                                    var split = pNameAndTypeName.Replace("" "", null).Split(':');
                                    var propertyName = split[0];
                                    var type = split[1];
                                    typeInfos.Add((pNameAndTypeName, propertyName, type));
                                    var typeValues = sheet[pNameAndTypeName];
                                    typeValuesCList.Add(typeValues);
                                }} 
                            if (typeValuesCList.Count != 0)
                            {{
                                int rows = typeValuesCList[0].Count;
                                for (int i = 0; i < rows; i++)
                                {{
                                    @namespace.@class instance = new @namespace.@class();
                                    for (int j = 0; j < typeInfos.Count; j++)
                                    {{
                                        var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                        var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                                        fields[j].SetValue(instance, readedValue);
                                    }}
                                    //Add Data to Container
                                    callbackParamList.Add(instance);
                                    callbackParamMap .Add(instance.{sheetInfo.sheetVariableNames[0]}, instance);
                                    if(updateCurrentData)
                                    {{
                                       @classList.Add(instance);
                                       @classMap.Add(instance.{sheetInfo.sheetVariableNames[0]}, instance);
                                    }}
                                }} 
                            }}
                        }}

                      onLoaded?.Invoke(callbackParamList, callbackParamMap);
            }});
        }}

            ");

            generateForm = generateForm.Replace("@loadFromGoogleFunction", builder.ToString());
        }
        private void WriteLoadFunction()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($@"
        public static void Load(bool forceReload = false)
        {{
            if(isLoaded && forceReload == false)
            {{
                 Console.WriteLine(""@class is already loaded! if you want reload then, forceReload parameter set true"");
                 return;
            }}
            /* Clear When Try Load */
            @classMap?.Clear();
            @classList?.Clear(); 
            //Type Map Init
            TypeMap.Init();
            //Reflection Field Datas.
            FieldInfo[] fields = typeof(@namespace.@class).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
            //Load GameData.
            string text = reader.ReadData(""@namespace"");
            if (text != null)
            {{
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GetTableResult>(text);
                var table= result.tableResult; 
                var sheet = table[""@class""];
                    foreach (var pNameAndTypeName in sheet.Keys)
                    {{
                        var split = pNameAndTypeName.Replace("" "", null).Split(':');
                        var propertyName = split[0];
                        var type = split[1];
                        typeInfos.Add((pNameAndTypeName, propertyName, type));
                        var typeValues = sheet[pNameAndTypeName];
                        typeValuesCList.Add(typeValues);
                    }} 
                if (typeValuesCList.Count != 0)
                {{
                    int rows = typeValuesCList[0].Count;
                    for (int i = 0; i < rows; i++)
                    {{
                        @namespace.@class instance = new @namespace.@class();
                        for (int j = 0; j < typeInfos.Count; j++)
                        {{
                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                            fields[j].SetValue(instance, readedValue);
                        }}
                        //Add Data to Container
                        @classList.Add(instance);
                        @classMap.Add(instance.{sheetInfo.sheetVariableNames[0]}, instance);
                    }} 
                }}
            }}
            isLoaded = true;
        }}
");
            generateForm = generateForm.Replace("@loadFunction", builder.ToString());
        }


        public string Generate()
        {
            string _namespace = sheetInfo.sheetFileName;
            string _className = sheetInfo.sheetName;

            WriteLoadFunction();
            WriteLoadFromGoogleFunction();

            WriteAssembly(new string[] { "Hamster.ZG", "System", "System.Collections.Generic", "System.IO", "Hamster.ZG.Type", "System.Reflection" , "Hamster.ZG.IO.FileReader" });
            WriteNamespace(_namespace);
            WriteClassReplace(_className);
            WriteSpreadSheetData(sheetInfo.spreadSheetID, sheetInfo.sheetID);
            WriteTypes(sheetInfo.sheetTypes, sheetInfo.sheetVariableNames);
            WriteWriteFunction(_className);
            return GenerateForm;
        }

    }
}