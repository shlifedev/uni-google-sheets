using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponShopProudctModel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public Example2.Item.Weapons data;
    public Image proudctImage;
    public Text  proudctName;
    public Text  priceBtnText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (WeaponShop.instance.descriptionUI.gameObject.activeInHierarchy == false)
        {
            WeaponShop.instance.descriptionUI.ShowUI(data); 
        } 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WeaponShop.instance.descriptionUI.HideUI();
    }

    public void SetData(Example2.Item.Weapons data)
    {
        this.data = data;

        Color color = new Color(1,1,1,1);
        switch(data.Grade)
        {
            case 0:
                break;
            case 1:
                color = new Color(0.77f,1, 0.72f);
                break;
            case 2:
                color = new Color(0, 0.70f, 1);
                break;
            case 3:
                color = new Color(1,0, 0.75f);
                break;
            case 4:
                color = new Color(1, 0.74f, 0);
                break;

        }
        proudctName.color = color;
        proudctName.text = data.localeID; 
        priceBtnText.text = "$"+String.Format("{0:#,###}", data.Price);
    }


}
