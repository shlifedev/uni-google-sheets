using Hamster.ZG.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.IO
{ 
    public class CodeGenerator : ICodeGenerator
    {
        private SheetInfo sheetInfo;
        private string readBasePath = "";
        private string generateForm = @"@assemblys
namespace @namespace
{
    public class @class
    {
        public static Dictionary<@keyType, @class> @classMap = new Dictionary<@keyType, @class>();
        public static List<@class> @classList = new List<@class>();  
@types 
@loadFunction 
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
                    builder.AppendLine($"\tpublic {GetCSharpRepresentation(TypeMap.StrMap[types[i]], true)} {names[i]};");
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
            builder.Append("public void Load(){}");
            generateForm = generateForm.Replace("@loadFunction", builder.ToString());
        }


        public string Generate()
        {
            string _namespace = sheetInfo.sheetFileName;
            string _className = sheetInfo.sheetName;
            WriteAssembly(new string[] { "System", "System.Collections.Generic", "System.IO" });
            WriteNamespace(_namespace);
            WriteClassReplace(_className);
            WriteTypes(sheetInfo.sheetTypes, sheetInfo.sheetVariableNames);
            WriteLoadFunction(); 
            return GenerateForm;
        }

    }
}

