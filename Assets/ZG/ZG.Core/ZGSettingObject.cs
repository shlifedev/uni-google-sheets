using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="ZGSettingObject", menuName ="HamsterLib/ZG/SettingObject", order = 0)]
public class ZGSettingObject : ScriptableObject
{
    public string CSPath = "Assets/ZGS/Scripts/ZGS.Struct";
    public string DataPath = "Assets/ZGS/Resources/ZGS.Data";

 
    public string RuntimeDataPath = "ZGS/ZGS.Data/";
    
}
