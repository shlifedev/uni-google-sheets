using GoogleSheet.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleSheet.IO
{

    public class CodeGenerator : ICodeGenerator
    {
        private SheetInfo sheetInfo;

        private string generateForm = @"
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
@assemblys
namespace @namespace
{
    [GoogleSheet.Attribute.TableStruct]
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

 
    public void Upload()
    {
        Write(this);
    }
 
 
    public static void OnError(System.Exception e){
         throw e;
    }
 
    }
}
        ";
        public string GenerateForm { get => this.generateForm; }
        public CodeGenerator(SheetInfo info)
        {
            this.sheetInfo = info;
        }


        private void WriteTypes(string[] types, string[] fieldNames, bool[] isEnum)
        {
            if (types != null)
            {
                //  Debug.Log(isEnum.Length);
                StringBuilder builder = new StringBuilder();
                builder.AppendLine();
                for (int i = 0; i < types.Length; i++)
                {
                    if (isEnum[i] == false)
                    {
                        var targetType = types[i];
                        var targetField = fieldNames[i];
                        TypeMap.StrMap.TryGetValue(targetType, out System.Type outType);
                        if (outType == null)
                        {
                            Console.WriteLine("-------UGS IMPORTANT ERROR DEBUG---------");
                            string debugTypes = string.Join("  ", sheetInfo.sheetTypes);
                            Console.WriteLine($"Error Sheet Name => {sheetInfo.sheetFileName}.{sheetInfo.sheetName}");
                            Console.WriteLine($"Your type list=> {debugTypes}");
                            Console.WriteLine($"error field => {targetField} : {sheetInfo.sheetTypes[i]}");
                            throw new TypeParserNotFoundException("Type Parser Not Found.. You made your own type parser? check custom type document on gitbook document.");
                        }
                        builder.AppendLine($"\t\tpublic {GetCSharpRepresentation(TypeMap.StrMap[types[i]], true)} {fieldNames[i]};");
                    }
                    else
                    {
                        var str = types[i];
                        str = str.Replace("<", null);
                        str = str.Replace(">", null);
                        str = str.Replace(" ", null);
                        str = str.Remove(0, 4);
                        builder.AppendLine($"\t\tpublic {GetCSharpRepresentation(TypeMap.EnumMap[str].Type, true)} {fieldNames[i]};");
                    }
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
            string writeFunction = $@"
        public static void Write(@class data, System.Action<WriteObjectResult> onWriteCallback = null)
        {{ 
            TypeMap.Init();
            FieldInfo[] fields = typeof({@class}).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var datas = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {{
                var type = fields[i].FieldType;
                string writeRule = null;
                if(type.IsEnum)
                {{
                    writeRule = TypeMap.EnumMap[type.Name].Write(fields[i].GetValue(data));
                }}
                else
                {{
                    writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
                }} 
                datas[i] = writeRule; 
            }}  
             GoogleDriveWebRequesterV2.Instance.WriteObject(new GoogleSheet.Protocol.v2.Req.WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);
        }} 
        ";

            generateForm = generateForm.Replace("@writeFunction", writeFunction);
        }
        private void WriteNamespace(string @namespace, bool[] sheetInfoIsEnum, string[] sheetInfoTypeName)
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

        private void WriteAssembly(string[] assemblys, string[] types = null, bool[] isEnum = null)
        {
            if (assemblys != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var data in assemblys)
                    builder.AppendLine($"using {data};");
                builder.AppendLine("@assemblys");
                generateForm = generateForm.Replace("@assemblys", builder.ToString());

            }

            if (types != null && isEnum != null)
            {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < types.Length; i++)
                {
                    var type = types[i];
                    var _isEnum = isEnum[i];
                    type = type.Replace(" ", null);
                    type = type.Replace("Enum<", null);
                    type = type.Replace(">", null);
                    if (_isEnum && !string.IsNullOrEmpty(TypeMap.EnumMap[type].NameSpace))
                    {
                        builder.AppendLine($"using {TypeMap.EnumMap[type].NameSpace};");
                    }
                }
                generateForm = generateForm.Replace("@assemblys", builder.ToString());

            }
            else
            {
                generateForm = generateForm.Replace("@assemblys", null);
            }
        }



        private void WriteLoadFromGoogleFunction()
        {


            StringBuilder builder = new StringBuilder();
            builder.Append($@"
        public static void LoadFromGoogle(System.Action<List<@class>, Dictionary<@keyType, @class>> onLoaded, bool updateCurrentData = false)
        {{      
            TypeMap.Init();
            IHttpProtcol webInstance = GoogleDriveWebRequesterV2.Instance as IHttpProtcol;
 
            if(updateCurrentData)
            {{
                @classMap?.Clear();
                @classList?.Clear(); 
            }}
            List<@class> callbackParamList = new List<@class>();
            Dictionary<@keyType,@class> callbackParamMap = new Dictionary<@keyType, @class>();
            webInstance.ReadSpreadSheet(new GoogleSheet.Protocol.v2.Req.ReadSpreadSheetReqModel(spreadSheetID), OnError, (data) => {{
            FieldInfo[] fields = typeof(@namespace.@class).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string,string,string)>();
            List<List<string>> typeValuesCList = new List<List<string>>(); 
              if (data != null)
                        {{
                            var result = data;
                            var table= result.jsonObject; 
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
                                       try
                                       {{
                                            var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                            var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                                             fields[j].SetValue(instance, readedValue);
                                       }}
                                       catch
                                       {{
                                        var type = typeInfos[j].type;
                                            type = type.Replace(""Enum<"", null);
                                            type = type.Replace("">"", null);

                                             var readedValue = TypeMap.EnumMap[type].Read(typeValuesCList[j][i]);
                                             fields[j].SetValue(instance, readedValue); 
                                      }}
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
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSpreadSheetResult>(text);
                var table= result.jsonObject; 
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
                                    try{{
                                        var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                        var readedValue = TypeMap.Map[typeInfo].Read(typeValuesCList[j][i]); 
                                        fields[j].SetValue(instance, readedValue);
                                       }}
                                      catch{{
                                        var type = typeInfos[j].type;
                                            type = type.Replace(""Enum<"", null);
                                            type = type.Replace("">"", null);

                                             var readedValue = TypeMap.EnumMap[type].Read(typeValuesCList[j][i]);
                                             fields[j].SetValue(instance, readedValue);
            
                                          }}
                              }}

                         //Add Data to Container
                        @classList.Add(instance);
                        @classMap.Add(instance.{sheetInfo.sheetVariableNames[0]}, instance);
                  
                       
                         }} 
                }}
       isLoaded = true;
            }}
      
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

            WriteAssembly(new string[] { "GoogleSheet.",
                "System",
                "System.Collections.Generic",
                "System.IO",
                "GoogleSheet.Type",
                "System.Reflection",
                "GoogleSheet.IO.FileReader",
                "GoogleSheet.Protocol.v2.Res"}, sheetInfo.sheetTypes, sheetInfo.isEnumChecks);
            WriteNamespace(_namespace, sheetInfo.isEnumChecks, sheetInfo.sheetTypes);
            WriteSpreadSheetData(sheetInfo.spreadSheetID, sheetInfo.sheetID);
            WriteTypes(sheetInfo.sheetTypes, sheetInfo.sheetVariableNames, sheetInfo.isEnumChecks);
            WriteWriteFunction(_className);
            WriteClassReplace(_className);
            return GenerateForm;
        }

    }
}
