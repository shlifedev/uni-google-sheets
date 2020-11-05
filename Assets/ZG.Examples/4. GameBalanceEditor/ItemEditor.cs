using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemEditor : MonoBehaviour
{
    public GameObject waitObj;
    public GameObject prefab;
    public RectTransform contentTransform;
    public InputField itemIndex;
    public InputField localeID;
    public InputField Type;
    public InputField Grade;
    public InputField STR;
    public InputField DEX;
    public InputField INT;
    public InputField LUK;
    public InputField IconName;
    public InputField Price;

    public void Awake()
    {
        waitObj.gameObject.SetActive(true);
        Example4.Item.Data.LoadFromGoogle((list,map)=> {
            Debug.Log("Loaded From Google | Item Count: : " + list.Count); 
            CreateEditor();
            waitObj.gameObject.SetActive(false);
        }, true);  
    }
    public void CreateEditor()
    {
        // Destroy already created
        for (int i = 0; i < contentTransform.childCount; i++)
            Destroy(contentTransform.transform.GetChild(i).gameObject);

        // Sort
        var sortData = Example4.Item.Data.DataList.OrderBy(x=>x.Price);
        foreach (var data in sortData)
        {
            var editable = Instantiate(prefab);
            editable.transform.SetParent(contentTransform, false);
            var editableItem = editable.GetComponent<EditableItemModel>();
            editableItem.editTargetData = data;


            Color color = new Color(1,1,1,1);
            switch (data.Grade)
            {
                case 0:
                    break;
                case 1:
                    color = new Color(0.77f, 1, 0.72f);
                    break;
                case 2:
                    color = new Color(0, 0.70f, 1);
                    break;
                case 3:
                    color = new Color(1, 0, 0.75f);
                    break;
                case 4:
                    color = new Color(1, 0.74f, 0);
                    break;

            } 
            editableItem.itemName.text = data.localeID;
            editableItem.itemName.color = color;
        }
    }
    public void SelecItem(EditableItemModel mdl)
    {
        itemIndex.text = mdl.editTargetData.itemIndex.ToString();
        localeID.text  = mdl.editTargetData.localeID;
        Type.text      = mdl.editTargetData.Type;
        Grade.text     = mdl.editTargetData.Grade.ToString();
        STR.text       = mdl.editTargetData.STR.ToString();
        DEX.text       = mdl.editTargetData.DEX.ToString();
        INT.text       = mdl.editTargetData.INT.ToString();
        LUK.text       = mdl.editTargetData.LUK.ToString();
        IconName.text  = mdl.editTargetData.IconName;
        Price.text     = mdl.editTargetData.Price.ToString();
    }
    public void UpdateGoogleSheet()
    {
        waitObj.gameObject.SetActive(true);
        UnityGoogleSheet.Write<Example4.Item.Data>(new Example4.Item.Data()
        {
            itemIndex = int.Parse(this.itemIndex.text),
            localeID = this.localeID.text,
            Type = this.Type.text,
            Grade = int.Parse(this.Grade.text),
            STR = int.Parse(this.STR.text),
            DEX = int.Parse(this.DEX.text),
            INT = int.Parse(this.INT.text),
            LUK = int.Parse(this.LUK.text),
            IconName = this.IconName.text,
            Price = int.Parse(this.Price.text)
        }
        
        , ()=> {
            Example4.Item.Data.LoadFromGoogle((list, map) => {
                waitObj.gameObject.SetActive(false);
                Debug.Log("Update Google Sheet!");
                CreateEditor();
            }, true); 
        }); 
    }
}
