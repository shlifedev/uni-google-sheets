

#if UNITY_EDITOR || UNITY_BUILD 
using GoogleSheet.Core.IO.Generator;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UGS.Editor
{
    public class EnumGeneratorGUI : EditorWindow
    {
        public static EnumGeneratorGUI Instance
        {
            get
            {
                if (instance == null) instance = (EnumGeneratorGUI)EditorWindow.GetWindow(typeof(EnumGeneratorGUI));
                return instance;
            }
            set => instance = value;
        }

        bool useValidation = true;
        public string enumNameField { get; private set; }
        public string enumNameFieldData => (GetNameSpace() == null) ? GetName() : enumNameField;
        static EnumGeneratorGUI instance;
        // [MenuItem("HamsterLib/UGS/Utils/EnumReaderGenerator", priority = -99)]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            Instance = (EnumGeneratorGUI)EditorWindow.GetWindow(typeof(EnumGeneratorGUI));
            Instance.Show();
#pragma warning disable CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            Instance.title = "EnumReader Generator";
#pragma warning restore CS0618 // 형식 또는 멤버는 사용되지 않습니다.
            Instance.minSize = new Vector2(455, 150);
            Instance.maxSize = new Vector2(455, 150);

        }

        Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                   SingleOrDefault(assembly => assembly.GetName().Name == name);
        }

        public string GetName()
        {
            string[] split = enumNameField.Split('.');
            if (split.Length == 0)
            {
                return enumNameField;
            }
            else
            {
                return split[split.Length - 1];
            }
        }
        public string GetNameSpace()
        {
            if (string.IsNullOrWhiteSpace(enumNameField))
                return null;
            string[] split = enumNameField.Split('.');

            if (split == null || split.Length <= 1)
            {
                return null;
            }
            int cnt = split.Length;
            int index = 0;
            string compleatedNameSpace = null;
            foreach (var field in split)
            {
                if (index + 1 == cnt)
                {
                    var filter = compleatedNameSpace.Remove(compleatedNameSpace.Length - 1, 1);
                    compleatedNameSpace = filter;
                    break;
                }
                compleatedNameSpace += field + ".";
                index++;

            }
            return compleatedNameSpace;
        }

        public bool Validation()
        {
            //Set Unity Default Assembly
            Assembly assem = typeof(UnityEngine.GameObject).Assembly;

            foreach (var value in GetAssemblyByName("Assembly-CSharp").GetTypes())
            {
                if (value.Name == GetName())
                {
                    Debug.Log(GetNameSpace() + "," + value.Namespace);
                }
                if (GetNameSpace() == value.Namespace)
                {
                    if (value.Name == GetName())
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        public void OnGUI()
        {
            EditorGUILayout.LabelField("---- Generator ----");
            enumNameField = EditorGUILayout.TextField("EnumName", enumNameField);

            var generator = new EnumGenerator(GetNameSpace(), GetName(), (gen) =>
            {
                var path = UnityEditor.EditorUtility.SaveFilePanel("save a your own enum converter", Application.dataPath, enumNameField + "Type", "cs");
                var dir = System.IO.Path.GetDirectoryName(path);
                System.IO.File.WriteAllText(path, gen);
            });


            if (GUILayout.Button("Generate Enum Parser"))
            {
                if (Validation() && useValidation)
                {
                    generator.Generate();
                }
                else if (!useValidation)
                {
                    generator.Generate();
                }
                else
                {
                    EditorUtility.DisplayDialog("Generate Failed by Validation Error", enumNameField + " Type is not exist in Assembly.Chsarp!", "OK");
                }
            }



            //EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("---- Validator Options ----");
            EditorGUILayout.LabelField("If you use assembly defenition, disable this checkbox.");
            useValidation = EditorGUILayout.Toggle("use validation", useValidation);
        }

    }
}
#endif
