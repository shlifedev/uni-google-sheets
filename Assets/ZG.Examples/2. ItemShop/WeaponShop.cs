using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WeaponShop : MonoBehaviour
{
    public static WeaponShop instance;
    public ItemDescriptionUI descriptionUI;
    public RectTransform contentTransform;
    public GameObject productPrefab; 
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;  
        UnityGoogleSheet.Load<Example2.Item.Weapons>();
        CreateShop();
    }
     
    public void CreateShop()
    { 
        // Destroy already created
        for (int i = 0; i < contentTransform.childCount; i++)
          Destroy(contentTransform.transform.GetChild(i).gameObject);
       
        // Sort
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
