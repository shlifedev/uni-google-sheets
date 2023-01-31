using UnityEditor;
using UnityEngine;

public static class EditorPrefsManager
{
    public static T Get<T>(string key)
    {
        key = Application.dataPath + "_" + key;
        object value = null;
        if (typeof(T) == typeof(string)) value = EditorPrefs.GetString(key, null);
        if (typeof(T) == typeof(int)) value = EditorPrefs.GetInt(key, 0);
        if (typeof(T) == typeof(float)) value = EditorPrefs.GetFloat(key, 0);
        if (typeof(T) == typeof(bool)) value = EditorPrefs.GetBool(key, false);
        return (T)value;
    }

    public static void Set<T>(string key, object value)
    {
        key = Application.dataPath + "_" + key;
        if (typeof(T) == typeof(string)) EditorPrefs.SetString(key, (string)value);
        if (typeof(T) == typeof(int)) EditorPrefs.SetInt(key, (int)value);
        if (typeof(T) == typeof(float)) EditorPrefs.SetFloat(key, (float)value);
        if (typeof(T) == typeof(bool)) EditorPrefs.SetBool(key, (bool)value);
    }

    public static void Toggle(string key)
    {
        key = Application.dataPath + "_" + key;
        EditorPrefs.SetBool(key, !EditorPrefs.GetBool(key));
    }

}
