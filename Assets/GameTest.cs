using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameTable.Item.Armor.Load();
        foreach (var v in GameTable.Item.Armor.ArmorList)
        {
            Debug.Log(v.index +" , " + v.uiDesc);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
