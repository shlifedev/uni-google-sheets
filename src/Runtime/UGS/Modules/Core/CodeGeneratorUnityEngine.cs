#if UNITY_2017_1_OR_NEWER || UNITY_BUILD
using GoogleSheet;
using GoogleSheet.Type;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UGS.IO
{


    public class CodeGeneratorUnityEngine : ICodeGenerator
    {
        private SheetInfo sheetInfo;

        public string CommonLoad() => $@"

    public static (List<@class> list, Dictionary<@keyType, @class> map) CommonLoad(Dictionary<string, Dictionary<string, List<string>>> jsonObject, bool forceReload){{
            Dictionary<@keyType, @class> Map = new Dictionary<@keyType, @class>();
            List<@class> List = new List<@class>();     
            TypeMap.Init();
            FieldInfo[] fields = typeof(@class).GetFields(BindingFlags.Public | BindingFlags.Instance);
            List<(string original, string propertyName, string type)> typeInfos = new List<(string, string, string)>(); 
            List<List<string>> rows = new List<List<string>>();
            var sheet = jsonObject[""@class""];

            foreach (var column in sheet.Keys)
            {{
                string[] split = column.Replace("" "", null).Split(':');
                         string column_field = split[0];
                string   column_type = split[1];

                typeInfos.Add((column, column_field, column_type));
                          List<string> typeValues = sheet[column];
                rows.Add(typeValues);
            }}

          // 실제 데이터 로드
                    if (rows.Count != 0)
                    {{
                        int rowCount = rows[0].Count;
                        for (int i = 0; i < rowCount; i++)
                        {{
                            @class instance = new @class();
                            for (int j = 0; j < typeInfos.Count; j++)
                            {{
                                try
                                {{
                                    var typeInfo = TypeMap.StrMap[typeInfos[j].type];
                                    //int, float, List<..> etc
                                    string type = typeInfos[j].type;
                                    if (type.StartsWith("" < "") && type.Substring(1, 4) == ""Enum"" && type.EndsWith("">""))
                                    {{
                                         Debug.Log(""It's Enum"");
                                    }}

                                    var readedValue = TypeMap.Map[typeInfo].Read(rows[j][i]);
                                    fields[j].SetValue(instance, readedValue);

                                }}
                                catch (Exception e)
                                {{
                                    if (e is UGSValueParseException)
                                    {{
                                        Debug.LogError(""<color=red> UGS Value Parse Failed! </color>"");
                                        Debug.LogError(e);
                                        return (null, null);
                                    }}

                                    //enum parse
                                    var type = typeInfos[j].type;
                                    type = type.Replace(""Enum<"", null);
                                    type = type.Replace("">"", null);

                                    var readedValue = TypeMap.EnumMap[type].Read(rows[j][i]);
                                    fields[j].SetValue(instance, readedValue); 
                                }}
                              
                            }}
                            List.Add(instance); 
                            Map.Add(instance.{sheetInfo.sheetVariableNames[0]}, instance);
                        }}
                        if(isLoaded == false || forceReload)
                        {{ 
                            @classList = List;
                            @classMap = Map;
                            isLoaded = true;
                        }}
                    }} 
                    return (List, Map); 
}}


";
        private string DefaultForm = @"
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
        static UnityFileReader reader = new UnityFileReader();

/* Your Loaded Data Storage. */
    
        public static Dictionary<@keyType, @class> @classMap = new Dictionary<@keyType, @class>();  
        public static List<@class> @classList = new List<@class>();   

        /// <summary>
        /// Get @class List 
        /// Auto Load
        /// </summary>
        public static List<@class> GetList()
        {{
           if (isLoaded == false) Load();
           return @classList;
        }}

        /// <summary>
        /// Get @class Dictionary, keyType is your sheet A1 field type.
        /// - Auto Load
        /// </summary>
        public static Dictionary<@keyType, @class>  GetDictionary()
        {{
           if (isLoaded == false) Load();
           return @classMap;
        }}

    

/* Fields. */
@types  

#region fuctions

@loadFunction 
@loadFromGoogleFunction   
@CommonLoad 
@writeFunction  

 


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
    public static void OnError(System.Exception e){
         UnityGoogleSheet.OnTableError(e);
    }
 
    }
}
        ";
        public string GenerateForm { get => this.DefaultForm; }
        public CodeGeneratorUnityEngine(SheetInfo info)
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
                            Debug.Log("<color=#00ff00><b>-------UGS IMPORTANT ERROR DEBUG---------</b></color>");
                            string debugTypes = string.Join("  ", sheetInfo.sheetTypes);
                            Debug.LogError($"<color=white><b>Error Sheet Name => </b></color>{sheetInfo.sheetFileName}.{sheetInfo.sheetName}");
                            Debug.LogError($"<color=white><b>Your type list => </b></color> => {debugTypes}");
                            Debug.LogError($"<color=#00ff00><b>error field =>:</b></color> {targetField} : {sheetInfo.sheetTypes[i]}");
                            throw new TypeParserNotFoundException("Type Parser Not Found, You made your own type parser? check custom type document on gitbook document.");
                        }
                        builder.AppendLine($"\t\tpublic {outType.Namespace}.{GetCSharpRepresentation(TypeMap.StrMap[types[i]], true)} {fieldNames[i]};");
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
                DefaultForm = DefaultForm.Replace("@types", builder.ToString());
                DefaultForm = DefaultForm.Replace("@keyType", types[0]);
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
           
#if UNITY_EDITOR
if(Application.isPlaying == false)
{{
                UnityPlayerWebRequest.Instance.WriteObject(new WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

}}
else
{{
            UnityPlayerWebRequest.Instance.WriteObject(new  WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

}}
#endif

#if !UNITY_EDITOR
   UnityPlayerWebRequest.Instance.WriteObject(new  WriteObjectReqModel(spreadSheetID, sheetID, datas[0], datas), OnError, onWriteCallback);

#endif
        }} 
        ";

            DefaultForm = DefaultForm.Replace("@writeFunction", writeFunction);
        }
        private void WriteNamespace(string @namespace, bool[] sheetInfoIsEnum, string[] sheetInfoTypeName)
        {
            DefaultForm = DefaultForm.Replace("@namespace", @namespace);
        }
        private void WriteClassReplace(string @class)
        {
            DefaultForm = DefaultForm.Replace("@class", @class);
        }
        private void WriteLoader(string @path)
        {
            DefaultForm = DefaultForm.Replace("@assemblys", null);
        }


        private void WriteSpreadSheetData(string spreadID, string sheetID)
        {
            DefaultForm = DefaultForm.Replace("@spreadSheetID", spreadID);
            DefaultForm = DefaultForm.Replace("@sheetID", sheetID);
        }

        private void WriteAssembly(string[] assemblies, string[] types = null, bool[] isEnum = null)
        {

            if (assemblies != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var assembly in assemblies)
                    builder.AppendLine($"using {assembly};");
                builder.AppendLine("@assemblys");
                DefaultForm = DefaultForm.Replace("@assemblys", builder.ToString());

            }

            /* Enum */
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
                DefaultForm = DefaultForm.Replace("@assemblys", builder.ToString());

            }
            else
            {
                DefaultForm = DefaultForm.Replace("@assemblys", null);
            }
        }




        private void WriteCommonLoad()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(CommonLoad());
            DefaultForm = DefaultForm.Replace("@CommonLoad", builder.ToString());
        }
        private void WriteLoadFromGoogleFunction()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($@"
        public static void LoadFromGoogle(System.Action<List<@class>, Dictionary<@keyType, @class>> onLoaded, bool updateCurrentData = false)
        {{      
                IHttpProtcol webInstance = null;
    #if UNITY_EDITOR
                if (Application.isPlaying == false)
                {{
                    webInstance = UnityEditorWebRequest.Instance as IHttpProtcol;
                }}
                else 
                {{
                    webInstance = UnityPlayerWebRequest.Instance as IHttpProtcol;
                }}
    #endif
    #if !UNITY_EDITOR
                     webInstance = UnityPlayerWebRequest.Instance as IHttpProtcol;
    #endif
          
 
                var mdl = new ReadSpreadSheetReqModel(spreadSheetID);
                webInstance.ReadSpreadSheet(mdl, OnError, (data) => {{
                    var loaded = CommonLoad(data.jsonObject, updateCurrentData); 
                    onLoaded?.Invoke(loaded.list, loaded.map);
                }});
        }}

            ");

            DefaultForm = DefaultForm.Replace("@loadFromGoogleFunction", builder.ToString());
        }
        private void WriteLoadFunction()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($@"
        public static void Load(bool forceReload = false)
        {{
            if(isLoaded && forceReload == false)
            {{
#if UGS_DEBUG
                 Debug.Log(""@class is already loaded! if you want reload then, forceReload parameter set true"");
#endif
                 return;
            }}

            string text = reader.ReadData(""@namespace""); 
            if (text != null)
            {{
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<ReadSpreadSheetResult>(text);
                CommonLoad(result.jsonObject, forceReload); 
                if(!isLoaded)isLoaded = true;
            }}
      
        }}
");
            DefaultForm = DefaultForm.Replace("@loadFunction", builder.ToString());
        }


        public string Generate()
        {
            string _namespace = sheetInfo.sheetFileName;
            string _className = sheetInfo.sheetName;
            TypeMap.Init();
            WriteCommonLoad();

            WriteLoadFunction();
            WriteLoadFromGoogleFunction();

            WriteAssembly(new string[] {
                "GoogleSheet.Protocol.v2.Res",
                "GoogleSheet.Protocol.v2.Req",
                "UGS", "System",
                "UGS.IO","GoogleSheet",
                "System.Collections.Generic",
                "System.IO",
                "GoogleSheet.Type",
                "System.Reflection",
                "UnityEngine"}, sheetInfo.sheetTypes, sheetInfo.isEnumChecks);
            WriteNamespace(_namespace, sheetInfo.isEnumChecks, sheetInfo.sheetTypes);
            WriteSpreadSheetData(sheetInfo.spreadSheetID, sheetInfo.sheetID);
            WriteTypes(sheetInfo.sheetTypes, sheetInfo.sheetVariableNames, sheetInfo.isEnumChecks);
            WriteWriteFunction(_className);
            WriteClassReplace(_className);
            return GenerateForm;
        }

    }
}

#endif
