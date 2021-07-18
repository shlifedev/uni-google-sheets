#if UNITY_2017_1_OR_NEWER 
using System.Configuration;
using UnityEngine;
namespace Hamster.ZG
{
    public static class ZGSetting
    { 
        public static string GoogleFolderID
        {
            get
            { 
                
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject"); 
                if(setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                return setting.GoogleFolderID; 
            }
            set
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                if (setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                setting.GoogleFolderID = value;
            }
        }
        public static string ScriptURL
        {
            get
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                if (setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                return setting.ScriptURL;
            }
            set
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                if (setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                setting.ScriptURL = value;
            }
        }

        public static string ScriptPassword
        {
            get
            {

                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                if (setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                return setting.ScriptPassword;
            }
            set
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                if (setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                setting.ScriptPassword = value;
            }
        }



        public static bool SavePathSyncToggle
        {
            get
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                if (setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                return setting.SavePathSyncToggle;
            }
            set
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                if (setting == null)
                {
                    throw new System.Exception("Cannot Find ZG/Resources/ZGSettingObject.asset file, Setting Object is null!, please check or complate <color=#00ff00><b>HamsterLib -> UGS -> ZGSetting</b></color>");
                }
                setting.SavePathSyncToggle = value;
            }
        }
    }
}
#endif