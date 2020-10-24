using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerData.Data2.Load();
        foreach(var value in PlayerData.Data2.Data2List)
        {
            Debug.Log(value.index +"," + value.position);
        }

    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
