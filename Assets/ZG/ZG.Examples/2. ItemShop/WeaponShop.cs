using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    public RectTransform contentTransform;
    public GameObject productPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        Example2.Item.Weapons.Load();
        foreach (var data in Example2.Item.Weapons.WeaponsList)
        {
            var productObj = Instantiate(productPrefab);
            productObj.transform.SetParent(contentTransform, false);
            productObj.GetComponent<WeaponShopProudctModel>().SetData(data);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
