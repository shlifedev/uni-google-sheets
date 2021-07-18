using Hamster.ZG;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SecurityBuildPipeline : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
 
    public void OnPostprocessBuild(BuildReport report)
    { 
    }

 
    public void OnPreprocessBuild(BuildReport report)
    {
        var confirm = UnityEditor.EditorPrefs.GetBool("UGS.BuildMsg", false);
        if (!confirm)
        {
            var res = UnityEditor.EditorUtility.DisplayDialog("UGS Warning", "[번역필요] UGS 세팅파일은 Resources 폴더에 포함됩니다. 이 경우 api키가 유출될 수 있기 때문에 릴리즈 빌드에서는 세팅파일을 포함하지 않는것을 권고합니다. ", "OK!");
            if (res)
            {
                UnityEditor.EditorPrefs.SetBool("UGS.BuildMsg", true);
            } 

        } 
    }
     
}
