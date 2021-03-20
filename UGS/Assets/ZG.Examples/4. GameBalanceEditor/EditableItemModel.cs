using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableItemModel : MonoBehaviour
{
    public Example4.Item.Data editTargetData;
    public Text itemName;

    public void SelectThis()
    {
        FindObjectOfType<ItemEditor>().SelecItem(this);
    }
}
