using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageChanger : MonoBehaviour
{
 
    public void OnChangeValue(int value)
    {

        var lang = (LocalizationManager.Language)value;
        LocalizationManager.Instance.currentLanguage = lang;

        var items = FindObjectsOfType<ItemLocalization>();
        foreach(var item in items)
        {
            item.SetitemText();
        }
    } 
}
