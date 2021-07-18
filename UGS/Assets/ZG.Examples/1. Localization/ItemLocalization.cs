using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLocalization : MonoBehaviour
{
    public string itemID;
    public Text ItemName;
    public Text ItemDescription;


    public void Start()
    {
        SetitemText();
    }

    public void SetitemText()
    {
        ItemName.text = LocalizationManager.Instance.GetItemName(itemID);
        ItemDescription.text = LocalizationManager.Instance.GetItemDescription(itemID); 
    }
}
