
using Hamster.ZG;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
 
public class UISetting : EditorWindow
{
     

    static UISetting Instance;
    [MenuItem("HamsterLib/ZGS/Setting")]
    public static void CreateInstance()
    {  
        if (Instance == null)
        {
            var assets = AssetDatabase.FindAssets("ZG.UISetting"); 
            var uxml = AssetDatabase.GUIDToAssetPath(assets.ToList().Find(x=>AssetDatabase.GUIDToAssetPath(x).Contains(".uxml")));
             
            ZeroGoogleSheet.Init(new UnityGSParser(), new UnityFileReader());
            /* Load UI Directory View */
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxml) ;
            Instance = GetWindow<UISetting>();
            Instance.titleContent = new GUIContent("UISetting");
            Instance.rootVisualElement.Add(uiAsset.CloneTree());
            Instance.maxSize = new Vector2(500, 150); 
            var tf_url = Instance.rootVisualElement.Q("SCRIPT_URL") as TextField;
            var tf_gfid = Instance.rootVisualElement.Q("GFID") as TextField;

            tf_url.value = ZGSetting.ScriptURL;
            tf_gfid.value = ZGSetting.GoogleFolderID;

            Instance.rootVisualElement.Q("Save").RegisterCallback<ClickEvent>(x => {
                ZGSetting.ScriptURL = tf_url.value;
                ZGSetting.GoogleFolderID = tf_gfid.value;
            });

        }
        else
        {
            Instance.Show();
        }
    }

   
    public void OnGUI()
    {
        if(Instance == null)
        {
            CreateInstance();
        }
    }
}