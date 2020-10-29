using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponShop : MonoBehaviour
{
    public RectTransform contentTransform;
    public GameObject productPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        Example2.Item.Weapons.Load(); 
        var sortData = Example2.Item.Weapons.WeaponsList.OrderBy(x=>x.Price);
        foreach (var data in sortData)
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
