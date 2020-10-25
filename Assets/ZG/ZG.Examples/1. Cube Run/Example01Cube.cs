using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example01Cube : MonoBehaviour
{
    public int index;
    public static bool CubeBalanaceLoaded = false;
    public CubeBalance.Data data;

    private void Awake()
    {
        if(CubeBalanaceLoaded == false)
        {
            CubeBalance.Data.Load();
            CubeBalanaceLoaded = true; 
        }

 
        data = CubeBalance.Data.DataMap[index];
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(0,0, data.speed * Time.deltaTime);
    }
}
