#if UNITY_EDITOR || UNITY_BUILD 
using UnityEditor;
using UnityEngine;

namespace UGS.Editor
{
    public class UGSSetting : EditorWindow
    {
        static UGSSetting instance;
        private bool foldout = false;
        [MenuItem("Window/HamsterLib/Debug/DebugMode")]
        public static void ToggleDebugMode()
        {
            EditorPrefsManager.Toggle("UGS.DebugMode");
        }

        public static bool IsDebugMode()
        {
            return EditorPrefsManager.Get<bool>("UGS.DebugMode");
        }


        [MenuItem("HamsterLib/UGS/Setting", priority = 10000)]
        public static void CreateInstance()
        {
            // Get existing open window or if none, make a new one:
            instance = (UGSSetting)EditorWindow.GetWindow(typeof(UGSSetting));
            instance.Show();
        }

        private string _googleScriptURL;
        private string _password;
        private string _googleFolderId;
        private string _generateCodePath;
        private string _jsonDataPath;


        private string _cdn;
        private bool _usePatch;

        public void OnEnable()
        {
            _googleScriptURL = UGSettingObjectWrapper.ScriptURL;
            _password = UGSettingObjectWrapper.ScriptPassword;
            _googleFolderId = UGSettingObjectWrapper.GoogleFolderID;
            _generateCodePath = UGSettingObjectWrapper.GenerateCodePath;
            _jsonDataPath = UGSettingObjectWrapper.JsonDataPath;
        }

        public void EditorPrefsToggle(string id)
        {
            EditorPrefsManager.Toggle(id);
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void CreateAssetWhenReady()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorApplication.delayCall += CreateAssetWhenReady;
                return;
            }
            EditorApplication.delayCall += CreateAssetNow;
        }

        public static void CreateAssetNow()
        {

            var data = EditorPrefsManager.Get<string>("UGSetting.ScriptPassword");
            var data2 = EditorPrefsManager.Get<string>("UGSetting.GoogleFolderID");
            var data3 = EditorPrefsManager.Get<string>("UGSetting.ScriptURL");
            if (data == null || data2 == null || data3 == null) return;

            UGSettingObjectWrapper.ScriptURL = data3;
            UGSettingObjectWrapper.ScriptPassword = data;
            UGSettingObjectWrapper.GoogleFolderID = data2;

        }
        GUIStyle _fontsizeBig;
        GUIStyle _fontsizeBigGreen;
        GUIStyle _fontMiddleRed;
        GUIStyle _fontMiddelGreen;

        Vector2 scrollPos;
        public void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            var fontsizeBig = new GUIStyle(GUI.skin.label);
            fontsizeBig.fontSize = 18;

            var fontsizeBigGreen = new GUIStyle(GUI.skin.label);
            fontsizeBigGreen.normal.textColor = new Color(0, 1, 0, 1);
            fontsizeBigGreen.fontSize = 18;

            var fontMiddleRed = new GUIStyle(GUI.skin.label);
            fontMiddleRed.normal.textColor = new Color(1, 0, 0, 1);
            fontMiddleRed.fontSize = 15;
            var fontMiddelGreen = new GUIStyle(GUI.skin.label);
            fontMiddelGreen.normal.textColor = new Color(0, 1, 0, 1);
            fontMiddelGreen.fontSize = 15;


            var toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.fontSize = 13;
            GUILayout.Space(10);


            GUILayout.Label("인증정보 세팅", fontsizeBig);

            EditorGUILayout.BeginHorizontal();
            _googleScriptURL = EditorGUILayout.TextField("구글 스크립트 URL", _googleScriptURL);
            if (EditorGUILayout.LinkButton("여기에서 도움말을 확인하세요."))
            {
                Application.OpenURL("https://shlifedev.gitbook.io/unitygooglesheets/getting-start/apps-script");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _password = EditorGUILayout.TextField("스크립트 Password", _password);
            if (EditorGUILayout.LinkButton("여기에서 도움말을 확인하세요."))
            {
                Application.OpenURL("https://shlifedev.gitbook.io/unitygooglesheets/getting-start/apps-script");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _googleFolderId = EditorGUILayout.TextField("구글폴더 ID", _googleFolderId);
            if (EditorGUILayout.LinkButton("여기에서 도움말을 확인하세요."))
            {
                Application.OpenURL("https://shlifedev.gitbook.io/unitygooglesheets/getting-start/google-drive");
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            foldout = EditorGUILayout.Foldout(foldout, "Generate Options");
            if (foldout)
            {
                GUILayout.Label("데이터 생성경로", fontsizeBig);
                GUILayout.Label("*가능한 수정하지 말고 사용하시길 바랍니다.");
                GUILayout.BeginHorizontal();
                _generateCodePath = EditorGUILayout.TextField("Generated Code Save Path", _generateCodePath);
                if (GUILayout.Button("Set Directory"))
                {

                    var path = EditorUtility.OpenFolderPanel("Select Your Generated Code Save Path", "Assets/", "Assets/");
                    string relativepath = null;
                    if (path.StartsWith(Application.dataPath))
                    {
                        relativepath = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    if (string.IsNullOrEmpty(relativepath) == false)
                        _generateCodePath = relativepath;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                _jsonDataPath = EditorGUILayout.TextField("Generated Json Save Path", _jsonDataPath);
                if (GUILayout.Button("Set Directory"))
                {
                    var path = EditorUtility.OpenFolderPanel("Select Your Generated Json Save Path", "Assets/", "Assets/");
                    string relativepath = null;
                    if (path.StartsWith(Application.dataPath))
                    {
                        relativepath = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    if (string.IsNullOrEmpty(relativepath) == false)
                        _jsonDataPath = relativepath;
                }
                GUILayout.EndHorizontal();
            }


            GUILayout.Space(10);



            GUILayout.Label("보안 설정", fontsizeBig);
            if (EditorGUILayout.LinkButton("여기에서 도움말을 확인하세요."))
            {
                Application.OpenURL("https://shlifedev.gitbook.io/unitygooglesheets/additional/apps-script-backend-security");
            }
            var isUsedSecurityMode = DefineSymbolManager.IsUsed("UGS_SECURITY_MODE");
            if (isUsedSecurityMode)
            {
                GUILayout.Label("Security Mode Enabled!", fontMiddelGreen);
                GUILayout.Label("보안 모드가 활성화 되었습니다.");
                GUILayout.Label("보안 모드는 LiveLoad, LiveWrite 기능을 사용하지 못하고 빌드시 세팅정보 일부를 제외합니다. 위 도움말을 확인해주세요");
            }
            else
            {
                GUILayout.Label("Security Mode Disabled!", fontMiddleRed);
            }

            if (isUsedSecurityMode)
            {
                GUI.backgroundColor = new Color(0, 1, 0, 1);

                if (GUILayout.Button("Disable Security Mode"))
                {

                    DefineSymbolManager.RemoveDefineSymbol("UGS_SECURITY_MODE");
                }
            }
            else
            {
                GUI.backgroundColor = new Color(1, 0, 0, 1);
                if (GUILayout.Button("Enable Security Mode"))
                {
                    EditorPrefsManager.Set<string>("UGSetting.ScriptPassword", UGSettingObjectWrapper.ScriptPassword);
                    EditorPrefsManager.Set<string>("UGSetting.GoogleFolderID", UGSettingObjectWrapper.GoogleFolderID);
                    EditorPrefsManager.Set<string>("UGSetting.ScriptURL", UGSettingObjectWrapper.ScriptURL);

                    DefineSymbolManager.AddDefineSymbols("UGS_SECURITY_MODE");

                }
            }


            GUI.backgroundColor = new Color(1, 1, 1, 1);
            if (GUILayout.Button("Save"))
            {
                UGSettingObject setting = Resources.Load<UGSettingObject>("UGSettingObject");
                UGSettingObjectWrapper.ScriptURL = _googleScriptURL;
                UGSettingObjectWrapper.ScriptPassword = _password;
                UGSettingObjectWrapper.GoogleFolderID = _googleFolderId;
                UGSettingObjectWrapper.GenerateCodePath = _generateCodePath;
                UGSettingObjectWrapper.JsonDataPath = _jsonDataPath;


                EditorPrefsManager.Set<string>("UGSetting.ScriptPassword", UGSettingObjectWrapper.ScriptPassword);
                EditorPrefsManager.Set<string>("UGSetting.GoogleFolderID", UGSettingObjectWrapper.GoogleFolderID);
                EditorPrefsManager.Set<string>("UGSetting.ScriptURL", UGSettingObjectWrapper.ScriptURL);



                EditorUtility.SetDirty(setting);
                AssetDatabase.SaveAssets();
            }
            if (EditorPrefsManager.Get<bool>("UGS.DebugMode"))
            {
                if (GUILayout.Button("Load Debug Data"))
                {
                    this._googleScriptURL = "https://script.google.com/macros/s/AKfycbxpqlYM5SfX0pL2RHzgiT_cFykKFLkcr_PgzU1KKnVx2Aa6YNN3/exec";
                    this._googleFolderId = "1fanZLAqJEvy366vw3dOHQ4o9rhqcz-vI";
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
#endif