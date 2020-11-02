using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example3Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityGoogleSheet.LoadData<Example3.CustomType.Data>(); 
        Debug.Log("data1" + " ---- " + "data2");
        foreach (var value in Example3.CustomType.Data.DataList)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = value.data3; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
