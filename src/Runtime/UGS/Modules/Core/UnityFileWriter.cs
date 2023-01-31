#if UNITY_2017_1_OR_NEWER || UNITY_BUILD
using GoogleSheet.IO;
using UnityEditor;
using UnityEngine;
namespace UGS.IO
{
    public class UnityFileWriter : IFIleWriter
    {


        public void WriteCS(string writePath, string content)
        {
            UGSettingObject setting = Resources.Load<UGSettingObject>("UGSettingObject");
#if UNITY_EDITOR
            System.IO.Directory.CreateDirectory(setting.GenerateCodePath);
            string targetPath = setting.GenerateCodePath + "/" + writePath + ".cs";

            if (System.IO.File.Exists(targetPath))
                UGSBackupManager.AddBackupPlan(targetPath);

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
            UGSettingObject setting = Resources.Load<UGSettingObject>("UGSettingObject");
#if UNITY_EDITOR
            System.IO.Directory.CreateDirectory(setting.DataPath);
            string targetPath = setting.DataPath + "/" + writePath + ".json";


            if (System.IO.File.Exists(targetPath))
                UGSBackupManager.AddBackupPlan(targetPath);

            if (setting.base64)
                content = UGS.Unused.Base64Utils.Encode(content);

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