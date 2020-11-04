using Example2.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    /// <summary>
    /// WeaponShop Create
    /// </summary>
    public void CreateWeaponShopUI()
    {
        UnityGoogleSheet.Load<Weapons>();
        foreach(var weaponData in Weapons.WeaponsList)
        {
            CreateWeaponProductUI(weaponData);
        }
    }








    void CreateWeaponProductUI(Weapons v) { }
}
