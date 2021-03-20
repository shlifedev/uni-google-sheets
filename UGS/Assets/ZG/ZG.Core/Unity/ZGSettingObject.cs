
#if UNITY_2017_1_OR_NEWER 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ZGSettingObject", menuName ="HamsterLib/ZG/SettingObject", order = 0)]
public class ZGSettingObject : ScriptableObject
{
    // Target Script URL
    public string ScriptURL;
    // Script Password
    public string ScriptPassword = "default"; 
    // Google Folder ID
    public string GoogleFolderID;  
     
    public bool SavePathSyncToggle;



    [HideInInspector]
    public string CSPath = "Assets/ZGS/Scripts/ZGS.Struct";
    [HideInInspector]
    public string DataPath = "Assets/ZGS/Resources/ZGS.Data";
    [HideInInspector]
    public string RuntimeDataPath = "ZGS/ZGS.Data/";
 
}
#endif