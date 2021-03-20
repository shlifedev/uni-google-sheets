using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadMeScene : MonoBehaviour
{
    public GameObject wait;
    public GameObject step3_hide_btn;
    public GameObject step3_succesfulltText;

    public GameObject clickMe;
    
    public void OnClickCopy()
    {
        wait.SetActive(true);
        Hamster.ZG.UnityPlayerWebRequest.Instance.CopyExamples(Hamster.ZG.ZGSetting.GoogleFolderID, (x) => {
            wait.SetActive(false);
            Application.OpenURL($"https://drive.google.com/drive/u/0/folders/{x}");
            Hamster.ZG.ZGSetting.GoogleFolderID = x;
            step3_hide_btn?.SetActive(true);
            step3_succesfulltText.SetActive(true);
            clickMe.SetActive(false);
        });
    }
}
