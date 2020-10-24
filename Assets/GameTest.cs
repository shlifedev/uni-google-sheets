using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameTable.Item.Weapons.Load();

        foreach(var v in GameTable.Item.Weapons.WeaponsList)
        {
            Debug.Log(v.index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
