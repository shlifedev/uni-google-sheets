using Hamster.ZG.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.IO
{

    public class CodeGeneratorUnityEngine : ICodeGenerator
    {
        private SheetInfo sheetInfo;
        private string readBasePath = "";
        private string generateForm = @"
/*     ===== Do not touch this. Auto Generated Code. =====    */
/*     If you want custom code generation modify this => 'CodeGeneratorUnityEngine.cs'  */
@assemblys
namespace @namespace
{
    [Hamster.ZG.Attribute.TableStruct]
    public class @class : ITable
    {
        public static string spreadSheetID = ""@spreadSheetID""; // it is file id
        public static string sheetID = ""@sheetID""; // it is sheet id
        public static Dictionary<@keyType, @class> @classMap = new Dictionary<@keyType, @class>(); 
        public static List<@class> @classList = new List<@class>();  
        public static UnityFileReader reader = new UnityFileReader();
@types 
@writeFunction
@loadFunction 
    }
}
        ";
        public string GenerateForm { get => this.generateForm; }
        public CodeGeneratorUnityEngine(SheetInfo info)
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
        public static void Write({@class} data)
        {{ 
            //FieldInfo[] fields = typeof({@class}).GetFields(BindingFlags.Public | BindingFlags.Instance);
            //var datas = new string[fields.Length];
            //for (int i = 0; i < fields.Length; i++)
            //{{
            //    var type = fields[i].FieldType;
            //    var writeRule = TypeMap.Map[type].Write(fields[i].GetValue(data));
            //    datas[i] = writeRule; 
            //}} 
            //if(Application.isPlaying) 
            //    Request.Init(ZeroGoogleSheetUnity.Engine.UnityEngineWebRequester.Instance); 
            //Request.Execute(new QueryAppendRow(spreadSheetID, sheetID, datas));
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

        private void WriteLoadFunction()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($@"
        public static void Load()
        {{
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
        }}
");
            generateForm = generateForm.Replace("@loadFunction", builder.ToString());
        }


        public string Generate()
        {
            string _namespace = sheetInfo.sheetFileName;
            string _className = sheetInfo.sheetName;

            WriteLoadFunction();
            WriteAssembly(new string[] { "Hamster.ZG.Http","System", "System.Collections.Generic", "System.IO", "Hamster.ZG.Http.Protocol", "Hamster.ZG.Type", "System.Reflection", "UnityEngine" });
            WriteNamespace(_namespace);
            WriteClassReplace(_className);
            WriteSpreadSheetData(sheetInfo.spreadSheetID, sheetInfo.sheetID);
            WriteTypes(sheetInfo.sheetTypes, sheetInfo.sheetVariableNames);
            WriteWriteFunction(_className);
            return GenerateForm;
        }

    }
}

