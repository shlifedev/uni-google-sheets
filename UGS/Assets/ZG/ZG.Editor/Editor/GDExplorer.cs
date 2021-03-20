#if UNITY_EDITOR
using Hamster.ZG;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class CachedResource
{
    public static Dictionary<string, Texture2D> cached = new Dictionary<string, Texture2D>();
    public static Texture2D LoadTextureFromResource(string path)
    {
       
        if(cached.ContainsKey(path))
        {
            return cached[path];
        }
        else
        {
            var tex2D = Resources.Load<Texture2D>(path);
            cached.Add(path, tex2D);
            return tex2D;
        }
    }
}
public class UGSSetting : EditorWindow
{
    static UGSSetting instance;
    [MenuItem("HamsterLib/UGS/Setting")]
    public static void CreateInstance()
    {
        // Get existing open window or if none, make a new one:
        instance = (UGSSetting)EditorWindow.GetWindow(typeof(UGSSetting));
        instance.maxSize = new Vector2(500, 250);
        instance.minSize = new Vector2(500, 250);
        instance.Show(); 
    }
    public string GoogleScriptURL;
    public string Password;
    public string GoogleFolderId;

    public void OnEnable()
    { 
        if (GoogleScriptURL == null)
        {
            GoogleScriptURL = ZGSetting.ScriptURL;
        }
        if (Password == null)
        {
            Password = ZGSetting.ScriptPassword;
        }
        if (GoogleFolderId == null)
        {
            GoogleFolderId = ZGSetting.GoogleFolderID;
        }
    }
    public void OnGUI()
    {

        GoogleScriptURL = EditorGUILayout.TextField("GoogleScriptURL", GoogleScriptURL);
        Password = EditorGUILayout.TextField("Script Password", Password);
        GoogleFolderId = EditorGUILayout.TextField("Google Folder Id", GoogleFolderId);
        if(GUILayout.Button("Save"))
        {
            ZGSettingObject setting = Resources.Load<ZGSettingObject>("ZGSettingObject");
            ZGSetting.ScriptURL = GoogleScriptURL;
            ZGSetting.ScriptPassword = Password;
            ZGSetting.GoogleFolderID = GoogleFolderId; 
            EditorUtility.SetDirty(setting);
            AssetDatabase.SaveAssets();
        }

    }
}
 
public class GDExplorer : EditorWindow
{
    static GDExplorer instance;
    [MenuItem("HamsterLib/UGS/Manager")]
    static void Init()
    {
        if (string.IsNullOrEmpty(ZGSetting.GoogleFolderID) || string.IsNullOrEmpty(ZGSetting.ScriptURL))
        {
            EditorUtility.DisplayDialog("Require Setting!", "Cannot Open ZGS Menu. Please Setting Complete!", "OK"); 
            //for recursive calling
            var trashWindow = GDExplorer.GetWindow<GDExplorer>();
            trashWindow.Close(); 
            return;
        }
        // Get existing open window or if none, make a new one:
        Instance = (GDExplorer)EditorWindow.GetWindow(typeof(GDExplorer));
        Instance.Show();
    }

    public GUIStyle GS_TopBtn
    {
        get
        {
            GUIStyle gs = new GUIStyle();
            gs.normal.textColor = new Color(1, 1, 1, 1);
            gs.normal.background = CachedResource.LoadTextureFromResource("TopBg") as Texture2D;
            gs.alignment = TextAnchor.MiddleCenter;
            gs.fontSize = 12;
            gs.fixedHeight = 30;
            gs.margin = new RectOffset(0, 0, 0, 0);
            gs.padding = new RectOffset(0, 0, 0, 0);
            return gs;
        }
    }
    public GUIStyle GS_ExcelIcon
    {
        get
        {
            GUIStyle gs = new GUIStyle();
            gs.normal.textColor = new Color(1, 1, 1, 1);
            gs.normal.background = CachedResource.LoadTextureFromResource("ExcelIcon");
            gs.alignment = TextAnchor.MiddleCenter; ;
            gs.margin = new RectOffset(0, 0, 0, 0);
            gs.padding = new RectOffset(0, 0, 0, 0);
            return gs;
        }
    }
    public GUIStyle GS_FolderIcon
    {
        get
        {
            GUIStyle gs = new GUIStyle();
            gs.normal.textColor = new Color(1, 1, 1, 1);
            gs.normal.background = CachedResource.LoadTextureFromResource("FolderIcon") ;
            gs.alignment = TextAnchor.MiddleCenter; ;
            gs.margin = new RectOffset(0, 0, 0, 0);
            gs.padding = new RectOffset(0, 0, 0, 0);
            return gs;
        }
    }
    public GUIStyle GS_FileBackground
    {
        get
        {
            GUIStyle gs = new GUIStyle();
            gs.normal.background = CachedResource.LoadTextureFromResource("Transparency") ;
            gs.margin = new RectOffset(0, 0, 0, 0);
            gs.padding = new RectOffset(0, 0, 0, 0);
            return gs;
        }
    }
    public GUIStyle GS_FileName
    {
        get
        {
            GUIStyle gs = new GUIStyle();
            gs.normal.textColor = new Color(1, 1, 1, 1);
            gs.alignment = TextAnchor.MiddleCenter; ;
            gs.clipping = TextClipping.Overflow;
            return gs;
        }
    }
    public enum FileType
    {
        Folder, ParentFolder, Excel, Unknown
    }
    public class FileData
    {
        public FileType type;
        public string id = "";
        public string url = "";
        public string fileName = "";

        public FileData(FileType type, string url, string id, string fileName)
        {
            this.type = type;
            this.url = url;
            this.fileName = fileName;
            this.id = id;
        }
    }

    public List<FileData> loadedFileData = new List<FileData>();
    public static GDExplorer Instance
    {
        get
        {
            if (instance == null) instance = (GDExplorer)EditorWindow.GetWindow(typeof(GDExplorer));
            return instance;
        }
        set => instance = value;
    }

    public Vector2 data;

    public bool createWait = false;
    public void CreateFileDatas(string id)
    {
        loadedFileData.Clear();

        if (createWait) return;
        createWait = true;
        UnityEditorWebRequest.Instance.SearchGoogleDriveDirectory(id, x =>
        {
            if (id != ZGSetting.GoogleFolderID)
            {
                loadedFileData.Add(new FileData(FileType.ParentFolder, ZGSetting.GoogleFolderID, ZGSetting.GoogleFolderID, "../"));

            }
            else
            {
                currentViewFolderId = id;
            }
            for (int i = 0; i < x.fileID.Count; i++)
            {
                loadedFileData.Add(new FileData((FileType)x.fileType[i], x.url[i], x.fileID[i], x.fileName[i]));
            }
            createWait = false;
        });
    }

    static System.DateTime latestClickTime = System.DateTime.MinValue;
    public static Stack<string> prevFolderIdStack = new Stack<string>();
    public static string currentViewFolderId;



    public static void Generate()
    {
        ZeroGoogleSheet.Init(new UnityGSParser(), new UnityFileReader());
        if (Application.isPlaying == false)
        {
            foreach (var file in GDExplorer.Instance.loadedFileData)
            {
                if (file.type == FileType.Excel)
                {
                    if (Application.isPlaying == false)
                    {
//                        Debug.Log(GDExplorer.Instance.loadedFileData.Count +"," + file.id); 
                        UnityEditorWebRequest.Instance.ReadGoogleSpreadSheet(file.id, (x1, x2) =>
                        {
                            ZeroGoogleSheet.DataParser.ParseSheet(x2, true, true, new UnityFileWriter());
                        });
                    } 
                    else //Currently Not Support With PlayMode
                    {
                        UnityPlayerWebRequest.Instance.ReadGoogleSpreadSheet(file.id, (x1, x2) =>
                        {
                            ZeroGoogleSheet.DataParser.ParseSheet(x2, true, true, new UnityFileWriter());
                        });
                    }
                }   
            }
        }
        else
        {
            UnityEditor.EditorUtility.DisplayDialog("UGS Dialog", "Generate Not Support With PlayMode. \n\nBut If you want update Game data at runtime, run Data.LoadFromGoogleSheet(..) method!", "OK");
            Debug.Log("Generate Not Support With PlayMode, We will update it");
        }
    }
    public static void Setting()
    {
          UGSSetting.CreateInstance();
    }
    public static void Document()
    {
          Application.OpenURL("https://cheeseallergyhamster.gitbook.io/unitygooglesheet/");
    }
    public void DoubleClickCheck(FileData data)
    {
        if (latestClickTime != System.DateTime.Now && System.DateTime.MinValue != latestClickTime)
        {
            var timeDist = (System.DateTime.Now - latestClickTime).Milliseconds;
            //더블클릭 판정
            if (timeDist <= 200)
            {
                latestClickTime = System.DateTime.MinValue;
                if (data.type == FileType.Folder)
                {
                    prevFolderIdStack.Push(currentViewFolderId);
                    currentViewFolderId = data.id;
                    CreateFileDatas(data.id);
                }
                else if (data.type == FileType.ParentFolder)
                {
                    var prevFolder = prevFolderIdStack.Pop();
                    currentViewFolderId = prevFolder;
                    CreateFileDatas(prevFolder);
                }
                else if (data.type == FileType.Excel)
                {
                    Application.OpenURL(data.url);
                }
                return;
            }
        }

        /* 일반 클릭 판정 */
        latestClickTime = System.DateTime.Now;

    }
    public void OnExplorer()
    {
        var rect = GUILayoutUtility.GetRect(Instance.position.width, Instance.position.height - 100);
        data = GUI.BeginScrollView(rect, data, new Rect(0, 0, 0, 1000), false, true);
        int itemWIdth = 160;
        int itemHeight = 160;
        int currentItemXPos = 0;
        int currentItemYPos = (int)rect.y;
        for (int i = 0; i < loadedFileData.Count; i++)
        {
            if (i == 0)
                currentItemXPos = 0;
            else
                currentItemXPos += itemWIdth;

            if (currentItemXPos >= Instance.position.width - 50)
            {
                currentItemYPos += itemHeight;
                currentItemXPos = 0;
            }
            if (GUI.Button(new Rect(currentItemXPos, currentItemYPos, itemWIdth, itemHeight), "", GS_FileBackground))
            {
                DoubleClickCheck(loadedFileData[i]);
            }
            if (loadedFileData[i].type == FileType.Excel)
            {
                GUI.Box(new Rect(currentItemXPos + (itemWIdth / 4), currentItemYPos + (itemWIdth / 4), itemWIdth / 2, itemHeight / 2), "", GS_ExcelIcon);
            }
            if (loadedFileData[i].type == FileType.Folder)
            {
                GUI.Box(new Rect(currentItemXPos + (itemWIdth / 4), currentItemYPos + (itemWIdth / 4), itemWIdth / 2, itemHeight / 2), "", GS_FolderIcon);
            }
            if (loadedFileData[i].type == FileType.ParentFolder)
            {
                GUI.Box(new Rect(currentItemXPos + (itemWIdth / 4), currentItemYPos + (itemWIdth / 4), itemWIdth / 2, itemHeight / 2), "", GS_FolderIcon);
            }

            var fileName = loadedFileData[i].fileName;
            

            GUI.Label(new Rect(currentItemXPos, (currentItemYPos + itemHeight / 3), itemWIdth, itemHeight), fileName, GS_FileName);
        }
        GUI.EndScrollView();
    }
    public string defaultFileName = "DefaultTable";
    public void OnBottom()
    {
        GUILayout.Label("Default Sheet Creator");
        defaultFileName = GUILayout.TextField(defaultFileName);
        if (GUILayout.Button("Create Default Sheet"))
        {
            if (Application.isPlaying == false)
            {
                UnityEditorWebRequest.Instance.CreateDefaultTable(currentViewFolderId, defaultFileName, json =>
                {
                    if (json == "failed")
                    {
                        EditorUtility.DisplayDialog("Failed Create Default Table!", "Table Create Failed.", "OK");
                        return;
                    }
                    var result = JsonConvert.DeserializeObject<CreateDefaultTableResult>(json);
                    if (result != null)
                    {
                        Debug.Log("Created!");
                        loadedFileData.Add(new FileData(FileType.Excel, result.url, result.fileID, result.fileName)); 
                    }
                    else
                    {
                        Debug.Log(json);
                    }
                });
            }
            else
            {
                UnityPlayerWebRequest.Instance.CreateDefaultTable(currentViewFolderId, defaultFileName, json =>
                {
                    if (json == "failed")
                    {
                        EditorUtility.DisplayDialog("Failed Create Default Table!", "Table Create Failed.", "OK");
                        return;
                    }
                    var result = JsonConvert.DeserializeObject<CreateDefaultTableResult>(json);
                    if (result != null)
                    {
                        loadedFileData.Add(new FileData(FileType.Excel, result.url, result.fileID, result.fileName)); 
                    }
                });
            }
        }
    }
    List<(string, System.Action)> topMenus = new List<(string, System.Action)>()
    {
("Open",()=>{
       Application.OpenURL($"https://drive.google.com/drive/folders/" + currentViewFolderId);

    }),
("Generate",()=>{

    Generate();
}),
("Setting",()=>{
Setting();
}),
("Document",()=>{
Document();
}),
    };
    public void OnTop()
    { 
        EditorGUILayout.BeginHorizontal();
        {
            for (int i = 0; i < topMenus.Count; i++)
            {
                if (GUILayout.Button(topMenus[i].Item1, GS_TopBtn))
                {
                    topMenus[i].Item2?.Invoke();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public void OnFocus()
    {
        if (loadedFileData.Count == 0)
        {
            CreateFileDatas(ZGSetting.GoogleFolderID);
        } 
    }
    public void OnGUI()
    {
      
     
        OnTop();
        OnExplorer();
        OnBottom();
    }

}
#endif