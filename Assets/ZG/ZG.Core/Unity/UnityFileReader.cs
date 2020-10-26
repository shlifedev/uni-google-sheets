using Hamster.ZG.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hamster.ZG
{
    public class UnityFileReader : IFileReader
    {
        public string ReadData(string fileName)
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                Debug.Log(fileName);
                var textasset = Resources.Load<TextAsset>("ZGS.Data/"+fileName);
                if (textasset != null)
                {
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
}