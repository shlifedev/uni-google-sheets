using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ZGSettingObject", menuName ="HamsterLib/ZG/SettingObject", order = 0)]
public class ZGSettingObject : ScriptableObject
{
    public string ScriptURL;
    public string ScriptPassword = "default";

    public string GoogleFolderID;

    public bool SavePathSyncToggle;



    [HideInInspector]
    public string CSPath = "Assets/ZGS/Scripts/ZGS.Struct";
    [HideInInspector]
    public string DataPath = "Assets/ZGS/Resources/ZGS.Data";
    [HideInInspector]
    public string RuntimeDataPath = "ZGS/ZGS.Data/";
 
}
