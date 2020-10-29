
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

            var textFieldScriptURL = Instance.rootVisualElement.Q("SCRIPT_URL") as TextField;
            var textFieldGoogleFolderID = Instance.rootVisualElement.Q("GFID") as TextField;
            var toggleSavePath = Instance.rootVisualElement.Q("SavePathSyncToggle") as Toggle;

            textFieldScriptURL.value = ZGSetting.ScriptURL;
            textFieldGoogleFolderID.value = ZGSetting.GoogleFolderID;
            toggleSavePath.value = ZGSetting.SavePathSyncToggle;


            Instance.rootVisualElement.Q("Save").RegisterCallback<ClickEvent>(x => {
                ZGSetting.ScriptURL = textFieldScriptURL.value;
                ZGSetting.GoogleFolderID = textFieldGoogleFolderID.value;
                ZGSetting.SavePathSyncToggle = toggleSavePath.value;
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