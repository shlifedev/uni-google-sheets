﻿
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
                return setting.GoogleFolderID; 
            }
            set
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                setting.GoogleFolderID = value;
            }
        }
        public static string ScriptURL
        {
            get
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                return setting.ScriptURL;
            }
            set
            {
                ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
                setting.ScriptURL = value;
            }
        }
    }
}