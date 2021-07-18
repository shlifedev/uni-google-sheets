#if UNITY_2017_1_OR_NEWER 
using Hamster.ZG.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Hamster.ZG
{
    public class UnityFileWriter : IFIleWriter
    {


        public void WriteCS(string writePath, string content)
        {
            ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject"); 
#if UNITY_EDITOR
            System.IO.Directory.CreateDirectory(setting.CSPath); 
            string targetPath = setting.CSPath + "/" + writePath + ".cs";

            if (System.IO.File.Exists(targetPath))
            {
                UGSBackupManager.AddBackupPlan(targetPath);
                Debug.Log("백업 " + targetPath);
            }

            System.IO.File.WriteAllText(targetPath, content);
            AssetDatabase.Refresh();
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
            UnityEditorWriteData(writePath, content);
        }

        public void UnityEditorWriteData(string writePath, string content)
        {
            ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
#if UNITY_EDITOR
            System.IO.Directory.CreateDirectory(setting.DataPath);
            string targetPath = setting.DataPath + "/" + writePath + ".json";
            if (System.IO.File.Exists(targetPath))
            {
                UGSBackupManager.AddBackupPlan(targetPath); 
            }
            System.IO.File.WriteAllText(targetPath, content);
            AssetDatabase.Refresh();
#endif 
            //#else
            //            System.IO.Directory.CreateDirectory(setting.RuntimeDataPath);
            //            System.IO.File.WriteAllText(setting.RuntimeDataPath + "/" + writePath + ".json", content);
            //            AssetDatabase.Refresh();
            //#endif
        }
    }
}
#endif