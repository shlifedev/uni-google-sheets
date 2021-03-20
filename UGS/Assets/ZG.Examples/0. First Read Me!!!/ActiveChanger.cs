using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveChanger : MonoBehaviour
{
    public GameObject disableObject;
    public GameObject nextEnableObject;
    public void OnClick()
    {
        this.disableObject?.SetActive(false);
        this.nextEnableObject?.SetActive(true); 
    }
}
