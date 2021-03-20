using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionUI : MonoBehaviour
{
    public Text itemName;
    public Text itemGradeAndType;
    public Text STR;
    public Text DEX;
    public Text INT;
    public Text LUK;


    public string GetItemName(Example2.Item.Weapons data)
    {
        if (data.Grade == 0)
        {
            return $"<color=#ffffff>{data.localeID}</color>";
        }
        if (data.Grade == 1)
        {
            return $"<color=#73FF98>{data.localeID}</color>";
        }
        if (data.Grade == 2)
        {
            return $"<color=#3B8AFF>{data.localeID}</color>";
        }
        if (data.Grade == 3)
        {
            return $"<color=#FF23ED>{data.localeID}</color>" ;
        }
        if (data.Grade == 4)
        {
            return $"<color=#FFA800>{data.localeID}</color>" ;;
        }
        return null;
    }
    public string GetItemGradeType(Example2.Item.Weapons data)
    {
        if (data.Grade == 0)
        {
            return $"<color=#ffffff>Normal </color>" + data.Type;
        }
        if (data.Grade == 1)
        {
            return $"<color=#73FF98>Rare </color>" + data.Type;
        }
        if (data.Grade == 2)
        {
            return $"<color=#3B8AFF>Unique </color>" + data.Type;
        }
        if (data.Grade == 3)
        {
            return $"<color=#FF23ED>Epic </color>" + data.Type;
        }
        if (data.Grade == 4)
        {
            return $"<color=#FFA800>Legendary </color>" + data.Type;
        }
        return null;
    }
    public void ShowUI(Example2.Item.Weapons data)
    {
        
        itemName.text = GetItemName(data);
        itemGradeAndType.text = GetItemGradeType(data);
        STR.text = $"STR\n{data.STR}";
        DEX.text = $"DEX\n{data.DEX}";
        INT.text = $"INT\n{data.INT}";
        LUK.text = $"LUK\n{data.LUK}";
        this.gameObject.SetActive(true);
    }
     
    public void HideUI()
    {
        this.gameObject.SetActive(false);
    }
}
