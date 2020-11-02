
using Hamster.ZG;
using UnityEngine;

public class UnityGoogleSheet
{
#if UNITY_EDITOR

    [UnityEditor.MenuItem("W/S")]
    public static void TestFunction()
    {
        UnityGoogleSheet.LoadAllData();
    }
#endif
    public static bool Init = false;   
    public static void Initalize()
    {
        if (Init == false)
            ZeroGoogleSheet.Init(new UnityGSParser(), new UnityFileReader());

        Init = true;
    }
     
    /// <summary>
    /// Write Your Table Data To GoogleSheet
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public static void WriteToGoogleSheet<T>(T value) where T : ITable
    {
        Initalize();
        var _class = typeof(T); 
        var writeFunction = _class.GetMethod("Write", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
        if (writeFunction != null) 
            writeFunction?.Invoke(null, new object[] { value }); 
    }
    /// <summary>
    /// Generate Your Table Data 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="csharpGenerate"> generate script, runtime not work this </param>
    /// <param name="jsonGenerate">generate json</param>
    public static void Generate<T>(bool csharpGenerate, bool jsonGenerate) where T : ITable
    {
        Initalize();
        var targetTable = typeof(T);
        var field = targetTable.GetField("spreadSheetID", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        var fieldValue = (string)field.GetValue(null);

        if (field != null)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                UnityPlayerWebRequest.Instance.GET_TableData(fieldValue, (x, json) => {
                    ZeroGoogleSheet.DataParser.ParseSheet(json, csharpGenerate, jsonGenerate, new UnityFileWriter());
                });
            }
            else
            {
                UnityEditorWebRequest.Instance.GET_TableData(fieldValue, (x,json)=> {
                    ZeroGoogleSheet.DataParser.ParseSheet(json, csharpGenerate, jsonGenerate, new UnityFileWriter()); 
                });
            }
#else
                UnityPlayerWebRequest.Instance.GET_TableData(fieldValue, (x, json) => {
                    ZeroGoogleSheet.DataParser.ParseSheet(json, false, jsonGenerate, new UnityFileWriter());
                }); 
#endif
        }
    }
    /// <summary>
    /// Load All Your Generated Table.
    /// </summary>
    public static void LoadData<T>() where T : ITable
    {
        Initalize();
        var _class = typeof(T); 
            //Get Load Method
            var loadFunction = _class.GetMethod("Load", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
            //Call Load Method
            if (loadFunction != null)
                loadFunction.Invoke(null, new System.Object[] { false }); 
    }

    /// <summary>
    /// Load All Your Generated Table.
    /// </summary>
    public static void LoadAllData()
    {
        Initalize();
        var subClasses = Hamster.ZG.Reflection.Utility.GetAllSubclassOf(typeof(ITable));
        foreach (var _class in subClasses)
        {
            //Get Load Method
            var loadFunction = _class.GetMethod("Load", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
            //Call Load Method
            if (loadFunction != null)
                loadFunction.Invoke(null, new System.Object[] { false });
        }
    }

    /// <summary>
    /// Load All Your Generated Table.
    /// </summary>
    public static void LoadByNamespaceContains(string @namespace)
    {
        Initalize();
        var subClasses = Hamster.ZG.Reflection.Utility.GetAllSubclassOf(typeof(ITable));
        foreach (var _class in subClasses)
        {
            if (_class.Namespace.Contains(@namespace))
            {
                var loadFunction = _class.GetMethod("Load", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);
                loadFunction.Invoke(null, new System.Object[] { false });
            }
        }
    }


}