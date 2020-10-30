using Hamster.ZG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{

    public enum Language
    {
        EN,
        KR
    }
    static LocalizationManager instance;
    public static LocalizationManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<LocalizationManager>();
            }
            return instance;
        }
    }
     
    /// <summary>
    /// current language
    /// </summary>
    public Language currentLanguage;
     
    // Start is called before the first frame update
    void Awake()
    {
        LoadMethod1();
    }


    public void LoadMethod1()
    {
        Debug.Log("Load Method 1 (Reflection)");

        var subClasses = Hamster.ZG.Reflection.Utility.GetAllSubclassOf(typeof(ITable));
        foreach (var _class in subClasses)
        {
            //if namespace == localization
            if (_class.Namespace.Contains("Localization"))
            {
                //get Localization.Item.Class.Load() function by reflection!
                var loadFunction = _class.GetMethod("Load", System.Reflection.BindingFlags.Public| System.Reflection.BindingFlags.Static);

                // call Localization.Item.Class.Load();
                loadFunction.Invoke(null, new System.Object[] { false });
            }
        }
    }


    public void LoadMethod2()
    {
        Debug.Log("Load Method 2 (Manually)"); 
        Localization.Item.Name.Load();
        Localization.Item.Description.Load(); 
    }

    /// <summary>
    /// Get Item Description
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public string GetItemDescription(string itemID)
    {
        var localeMap = Localization.Item.Description.DescriptionMap;
        if (currentLanguage == Language.EN)
            return localeMap[itemID].EN;
        else if (currentLanguage == Language.KR)
            return localeMap[itemID].KR;

        return null;
    }

    /// <summary>
    /// Get Item Name
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public string GetItemName(string itemID)
    {
        var localeMap = Localization.Item.Name.NameMap;
        if(currentLanguage == Language.EN)
            return localeMap[itemID].EN;
        else if (currentLanguage == Language.KR)
            return localeMap[itemID].KR;

        return null;
    } 
}
