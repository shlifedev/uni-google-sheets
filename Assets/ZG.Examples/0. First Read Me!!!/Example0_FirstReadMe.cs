using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example0_FirstReadMe : MonoBehaviour
{
    public GameObject wait;
    public GameObject step3_hide_btn;
    public void OnClickCopy()
    {
        wait.SetActive(true);
        Hamster.ZG.UnityPlayerWebRequest.Instance.CopyExamples(Hamster.ZG.ZGSetting.GoogleFolderID, (x) => {
            wait.SetActive(false);
            Application.OpenURL($"https://drive.google.com/drive/u/0/folders/{x}");
            Hamster.ZG.ZGSetting.GoogleFolderID = x;
            step3_hide_btn?.SetActive(true);
#if UNITY_EDITOR

            UnityEditor.EditorUtility.DisplayDialog("UnityGoogleSheet", $"Your UnityGoogleSheet Setting ['GoogleFolderId'] Autumatic Update For Example!\n\n Updated Sheet Id : {x}", "OK");
#endif
        });
    }
}
