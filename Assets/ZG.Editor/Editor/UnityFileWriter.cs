using Hamster.ZG.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnityFileWriter : IFIleWriter
{

 
    public void WriteCS(string writePath, string content)
    {

#if UNITY_EDITOR
        var csPath = EditorPrefs.GetString("UNITY_FILE_WRITER_CS_PATH", null); 
        if (string.IsNullOrEmpty(csPath))
        {  
            System.IO.Directory.CreateDirectory("Assets/ZGS/Scripts/ZGS.Struct");
            System.IO.File.WriteAllText("Assets/ZGS/Scripts/ZGS.Struct/" + writePath + ".cs", content);
            AssetDatabase.Refresh();
        } 
#else
        Debug.Log("C# code auto-generate editor only support!");
#endif
    }

    /// <summary>
    /// write json resources folder.
    /// </summary>
    /// <param name="writePath"></param>
    /// <param name="content"></param>
    public void WriteData(string writePath, string content)
    {
#if UNITY_EDITOR
        Debug.Log("UnityFile Writer :: Editor Mode(Debug)");
        if (Application.isEditor){ 
            var dataPath = EditorPrefs.GetString("UNITY_FILE_WRITER_DATA_PATH", null);

            if (string.IsNullOrEmpty(dataPath))
            {
                System.IO.Directory.CreateDirectory("Assets/ZGS/Resources/ZGS.Data");
                System.IO.File.WriteAllText("Assets/ZGS/Resources/ZGS.Data/" + writePath + ".json", content);
                AssetDatabase.Refresh();
            }
        }
    }
#elif !UNITY_EDITOR

        Debug.Log("UnityFile Writer :: Engine Mode(Runtime)");
        if (!Application.isEditor){
            string filePath = "file:///"+Application.persistentDataPath +$"/{writePath}.json";
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            System.IO.File.WriteAllText(filePath, content);
        }
 
    }
#endif
}