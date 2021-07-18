using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReleasePipeline  
{
    [MenuItem("HamsterLib/Expoter/ExportUGS")]
    static void ExportUGS()
    {
      
        var settingObj = Resources.Load("ZGSettingObject") as ZGSettingObject;
        var gfid = settingObj.GoogleFolderID;
        var sp = settingObj.ScriptPassword;
        var surl = settingObj.ScriptURL;
        settingObj.ScriptPassword = null;
        settingObj.GoogleFolderID = null;
        settingObj.ScriptURL = null; 
        var exportedPackageAssetList = new List<string>();
        var assets = AssetDatabase.FindAssets(null, new[] { "Assets/ZG" });
        foreach (var asset in assets)
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);  
            exportedPackageAssetList.Add(path);
        }
         
        AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(), "ugs.unitypackage",
           ExportPackageOptions.Recurse); 
    }

    [MenuItem("HamsterLib/Expoter/ExportUGSExample")]
    static void ExportUGSExample()
    { 
        var settingObj = Resources.Load("ZGSettingObject") as ZGSettingObject;
        var gfid = settingObj.GoogleFolderID;
        var sp = settingObj.ScriptPassword;
        var surl = settingObj.ScriptURL;
        settingObj.ScriptPassword = null;
        settingObj.GoogleFolderID = null;
        settingObj.ScriptURL = null;
        var exportedPackageAssetList = new List<string>();
        var assets = AssetDatabase.FindAssets(null, new[] { "Assets/ZGS" , "Assets/ZG.Examples"});
        foreach (var asset in assets)
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);
            exportedPackageAssetList.Add(path);
        }

        settingObj.ScriptPassword = gfid;
        settingObj.GoogleFolderID = sp;
        settingObj.ScriptURL = surl;

        AssetDatabase.ExportPackage(exportedPackageAssetList.ToArray(), "ugs_example.unitypackage",
         ExportPackageOptions.Recurse);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
