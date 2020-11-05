using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example0NextBtn : MonoBehaviour
{
    public GameObject disableObject;
    public GameObject nextEnableObject;
    public void OnClick()
    {
        this.disableObject?.SetActive(false);
        this.nextEnableObject?.SetActive(true); 
    }
}
