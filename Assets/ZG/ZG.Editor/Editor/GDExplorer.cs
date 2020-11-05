using Hamster.ZG;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GDExplorer : EditorWindow
{
    static GDExplorer instance;
    [MenuItem("Windows/HamsterLib/Development.../GDExplorer")]
    static void Init()
    {
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
            gs.normal.background = Resources.Load("TopBg") as Texture2D;
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
            gs.normal.background = Resources.Load("ExcelIcon") as Texture2D;
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
            gs.normal.background = Resources.Load("FolderIcon") as Texture2D;
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
            gs.normal.background = Resources.Load("Transparency") as Texture2D;
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
            if(id != ZGSetting.GoogleFolderID)
            { 
                loadedFileData.Add(new FileData(FileType.ParentFolder, ZGSetting.GoogleFolderID, ZGSetting.GoogleFolderID, "../"));

            }
            else
            {
                currentViewFolderId = id;
            }
            for(int i = 0; i < x.fileID.Count; i++)
            { 
                loadedFileData.Add(new FileData((FileType)x.fileType[i], x.url[i], x.fileID[i],  x.fileName[i]));
            }
            createWait = false;
        });
    }

    static System.DateTime latestClickTime = System.DateTime.MinValue;
    public Stack<string> prevFolderIdStack = new Stack<string>();
    public string currentViewFolderId;
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
        int itemWIdth = 128;
        int itemHeight = 128;
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
            GUI.Label(new Rect(currentItemXPos, (currentItemYPos + itemHeight / 3), itemWIdth, itemHeight), loadedFileData[i].fileName, GS_FileName);
        }
        GUI.EndScrollView();
    }
    public void OnBottom()
    {
        GUILayout.Label("Default Sheet Creator");
        GUILayout.TextField("dd");
        GUILayout.Button("Create Default Sheet");
    }
    List<(string, System.Action)> topMenus = new List<(string, System.Action)>()
    {
("Open",()=>{ }),
("Generate",()=>{ }),
("Setting",()=>{ }),
("Github",()=>{ }), 
    };
    public void OnTop()
    {
        GDExplorer window = (GDExplorer)EditorWindow.GetWindow(typeof(GDExplorer));
   
        EditorGUILayout.BeginHorizontal();
        {
            for (int i = 0; i < topMenus.Count; i++)
            {
                if(GUILayout.Button(topMenus[i].Item1, GS_TopBtn));
                {
                    topMenus[i].Item2?.Invoke();
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    public void OnGUI()
    {
        if(loadedFileData.Count == 0)
        { 
            CreateFileDatas(ZGSetting.GoogleFolderID);
        }
        OnTop();
        OnExplorer();
        OnBottom();
    }

}
