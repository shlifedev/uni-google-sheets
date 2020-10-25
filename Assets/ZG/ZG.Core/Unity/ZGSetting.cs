 
using UnityEngine;

public static class ZGSetting
{
 
    public static string GoogleFolderID
    {
        get
        {
#if UNITY_EDITOR
            return UnityEditor.EditorPrefs.GetString("GoogleFolderID", null);
#endif
            return PlayerPrefs.GetString("GoogleFolderID", null);
        }
        set
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString("GoogleFolderID", value);
#endif
            PlayerPrefs.SetString("GoogleFolderID", value);
        }
    }
    public static string ScriptURL
    {
        get
        {
#if UNITY_EDITOR
            return UnityEditor.EditorPrefs.GetString("ScriptURL", null);
#endif
            return PlayerPrefs.GetString("ScriptURL", null);
        }
        set
        {
#if UNITY_EDITOR
            UnityEditor.EditorPrefs.SetString("ScriptURL", value);
#endif
            PlayerPrefs.SetString("ScriptURL", value);
        }
    } 
}