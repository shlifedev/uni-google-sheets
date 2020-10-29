using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShopProudctModel : MonoBehaviour
{
    public Example2.Item.Weapons data;
    public Image proudctImage;
    public Text  proudctName;

    public void SetData(Example2.Item.Weapons data)
    {
        this.data = data;
        proudctName.text = data.localeID;
    }


}
