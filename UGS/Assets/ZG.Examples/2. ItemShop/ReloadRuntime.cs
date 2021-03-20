using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadRuntime : MonoBehaviour
{
    public void Reload()
    {
        Example2.Item.Weapons.LoadFromGoogle((list, mao)=> { 
            Debug.Log("Reload From Runtime!");
            var weaponShop = FindObjectOfType<WeaponShop>();
            weaponShop.CreateShop();
        }, true);
    }
}
