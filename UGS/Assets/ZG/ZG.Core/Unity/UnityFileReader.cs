#if UNITY_2017_1_OR_NEWER
using Hamster.ZG.IO;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Hamster.ZG
{
    public class UnityFileReader : IFileReader
    {
        public string ReadData(string fileName)
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return EditorAssetLoad(fileName);
            }
            else
            {
                return RuntimeAssetLoad(fileName);
            }
#elif !UNITY_EDITOR
                return RuntimeAssetLoad(fileName);
#endif
        }

        public string EditorAssetLoad(string fileName)
        {
            var textasset = Resources.Load<TextAsset>("ZGS.Data/" + fileName);
            if (textasset != null)
            {
                return textasset.text;
            }
            else
            {
                return null;
            }
        }

        public string RuntimeAssetLoad(string fileName)
        {
            ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
            var savePath = Path.Combine(Application.persistentDataPath, setting.RuntimeDataPath);
            var fileSavePath = Path.Combine(savePath, fileName);
            if (!System.IO.Directory.Exists(savePath))
            {
                System.IO.Directory.CreateDirectory(savePath);
            }

            if (System.IO.File.Exists(fileSavePath + ".json"))
            {
                var savedAsset = System.IO.File.ReadAllText(fileSavePath + ".json");
                Debug.Log("load from " + savedAsset);
                return savedAsset;
            }
            else
            {
                var textasset = Resources.Load<TextAsset>("ZGS.Data/" + fileName);
                if (textasset == null)
                    return null;

                return textasset.text;
            }
        }
    }
}


#endif