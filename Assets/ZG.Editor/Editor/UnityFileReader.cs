using Hamster.ZG.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityFileReader : IFileReader
{
    /// <summary>
    /// 읽기의 경우에는 Write와달리 Resources에도 있을 수 있고, persistentDataPath에 있을수도 있으니
    /// 경우에맞춰서 관리. 에디터는 무조건 리소시즈에 있음.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string ReadData(string fileName)
    {
#if UNITY_EDITOR
        Debug.Log("UnityFile Reader :: Editor Mode(Debug)");
        if (Application.isEditor){
        Debug.Log(fileName);
        var textasset = Resources.Load<TextAsset>("ZGS.Data/"+fileName);
        if (textasset != null)
        {
            Debug.Log(textasset.text);
            return textasset.text;
        }
        else
        {
            Debug.Log("cannot found textasset");
            return null;
        } 
        }
#elif !UNITY_EDITOR
        Debug.Log("UnityFile Reader :: Engine Mode(Runtime)");
        if (!Application.isEditor)
        {
            if (System.IO.File.Exists(fileName))
            {
                var assetFromDownloadData = System.IO.File.ReadAllText(fileName+".json");
                if (!string.IsNullOrEmpty(assetFromDownloadData))
                {
                    Debug.Log("load from persistent");
                    return assetFromDownloadData;
                }
            }
            else
            {
                var asset = Resources.Load<TextAsset>("file:///"+Application.persistentDataPath +"/"+ fileName);
                if (asset != null)
                {
                    Debug.Log("load from Resources");
                    return asset.text;
                }
            }
            return null;
 
        }

        return null;
#endif

        return null;
    }
}