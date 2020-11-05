using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GDExplorerSkinMaker : MonoBehaviour
{
    public GUIStyle data;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int width = 600;
    public void OnTop()
    { 
        EditorGUILayout.BeginHorizontal();
        {
            for (int i = 0; i < 4; i++)
            { 
                if(GUILayout.Button("btn", data))
                {
                    Debug.Log("hello");
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    public void OnGUI()
    {
        OnTop();
    }

}
