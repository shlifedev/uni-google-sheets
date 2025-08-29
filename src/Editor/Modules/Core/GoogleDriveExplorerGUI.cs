#if UNITY_EDITOR || UNITY_BUILD 
using GoogleSheet.Protocol.v2.Req;
using System.Collections.Generic;
using System.Linq;
using UGS.IO;
using UnityEditor;
using UnityEngine;
namespace UGS.Editor
{
    public class GoogleDriveExplorerGUI : EditorWindow
    {
        static GoogleDriveExplorerGUI instance;
        [MenuItem("HamsterLib/UGS/Manager", priority = -9999)]
        public static void Init()
        {
            var isUsedSecurityMode = DefineSymbolManager.IsUsed("UGS_SECURITY_MODE");
            if (isUsedSecurityMode)
            {
                EditorUtility.DisplayDialog("Security Mode Enabled..", "보안 모드가 활성화 되었으므로 사용할 수 없습니다. UGS 메뉴 세팅에서 이 옵션을 수동으로 꺼야합니다.", "OK");
                return;
            }


            if (string.IsNullOrEmpty(UGSettingObjectWrapper.GoogleFolderID) || string.IsNullOrEmpty(UGSettingObjectWrapper.ScriptURL))
            {
                EditorUtility.DisplayDialog("Require Setting!", "누락된 세팅이 있습니다. UGS 메뉴를 열어 세팅을 완료해주시길 바랍니다.", "OK");
                return;
            }
            var gdWindow = GoogleDriveExplorerGUI.GetWindow<GoogleDriveExplorerGUI>();
            if (gdWindow)
                gdWindow.Close();

            Instance = (GoogleDriveExplorerGUI)EditorWindow.GetWindow(typeof(GoogleDriveExplorerGUI));
            Instance.Show();

        }

        public string DefaultFileName = "DefaultTable";
        public GUIStyle TopButtonGui
        {
            get
            {
                var gs = new GUIStyle();
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
        public GUIStyle ExcelIconGui
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
        public GUIStyle FolderIconGui
        {
            get
            {
                GUIStyle gs = new GUIStyle();
                gs.normal.textColor = new Color(1, 1, 1, 1);
                gs.normal.background = CachedResource.LoadTextureFromResource("FolderIcon");
                gs.alignment = TextAnchor.MiddleCenter; ;
                gs.margin = new RectOffset(0, 0, 0, 0);
                gs.padding = new RectOffset(0, 0, 0, 0);
                return gs;
            }
        }
        public GUIStyle FileBackgroundGui
        {
            get
            {
                GUIStyle gs = new GUIStyle();
                gs.normal.background = CachedResource.LoadTextureFromResource("Transparency");
                gs.margin = new RectOffset(0, 0, 0, 0);
                gs.padding = new RectOffset(0, 0, 0, 0);
                return gs;
            }
        }
        public GUIStyle FileNameGui
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

        [System.Serializable]
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

        private List<(string, System.Action)> _topMenus = new List<(string, System.Action)>()
        {
            ("Open",()=>{ Application.OpenURL($"https://drive.google.com/drive/folders/" + currentViewFolderId); }),
            ("Generate",Generate),
            ("Refresh", () =>
            {
                GoogleDriveExplorerGUI.instance.loadedFileData.Clear();
                GoogleDriveExplorerGUI.instance.CreateFileDatas(UGSettingObjectWrapper.GoogleFolderID);
            }),
            ("Setting",Setting),
            ("Document",Document),
        };

        [SerializeField]
        public List<FileData> loadedFileData = new List<FileData>();
        public static GoogleDriveExplorerGUI Instance
        {
            get
            {
                if (instance == null) instance = (GoogleDriveExplorerGUI)EditorWindow.GetWindow(typeof(GoogleDriveExplorerGUI));
                return instance;
            }
            set => instance = value;
        }

        public Vector2 ScrollViewRect;

        public bool IsWaitForCreate = false;

        public static void OnEditorError(System.Exception e)
        {

            //bool p = (UnityEditor.EditorUtility.DisplayDialog("UGS Exception", "Error! \n\n " + e.Message, "Open Setting..", "dev:debug gui"));
            //if(p)
            //{ 
            //    var gdWindow = GoogleDriveExplorerGUI.GetWindow<GoogleDriveExplorerGUI>();
            //    if(gdWindow && gdWindow.docked)
            //        gdWindow.Close();

            //    try
            //    {
            //        var instanced = UGSSetting.GetWindow<UGSSetting>();
            //        if(instanced != null)
            //        {
            //            if (instance.docked)
            //            {
            //                instance.Close();
            //            }
            //        }
            //        var window = UGSSetting.CreateWindow<UGSSetting>();
            //        if (window.docked == false)
            //        {
            //            window.Show();
            //            float width = window.position.width;
            //            float height = window.position.height;
            //            float x = (Screen.currentResolution.width - width) / 2;
            //            float y = (Screen.currentResolution.height - height) / 2;
            //            window.position = new Rect(x, y, width, height);
            //            window.Focus();
            //        }
            //        }
            //    catch { }


            //}


        }
        public void CreateFileDatas(string id)
        {
            loadedFileData.Clear();

            if (IsWaitForCreate) return;
            IsWaitForCreate = true;
            UnityEditorWebRequest.Instance.GetDriveDirectory(new GetDriveDirectoryReqModel(id), OnEditorError, x =>
            {
                if (id != UGSettingObjectWrapper.GoogleFolderID)
                {
                    loadedFileData.Add(new FileData(FileType.ParentFolder, UGSettingObjectWrapper.GoogleFolderID, UGSettingObjectWrapper.GoogleFolderID, "../"));
                }
                else
                {
                    currentViewFolderId = id;
                }
                for (int i = 0; i < x.fileId.Count; i++)
                {
                    loadedFileData.Add(new FileData((FileType)x.fileType[i], x.url[i], x.fileId[i], x.fileName[i]));
                }
                IsWaitForCreate = false;
            });
        }

        static System.DateTime latestClickTime = System.DateTime.MinValue;
        public static Stack<string> prevFolderIdStack = new Stack<string>();
        public static string currentViewFolderId;
#if UNITY_EDITOR
        /// <summary>
        /// weplanb 메타데이터 쓰기
        /// </summary>
        /// <param name="datas"></param>
        private static void WriteMetaData(List<FileData> datas)
        {

            UGSettingObject setting = Resources.Load<UGSettingObject>("UGSettingObject");
            string ugsMetaFileName = "meta.bin";
            var list = datas.Select(x => x.fileName).ToList();
            var joined = string.Join(",", list);
            var content = UGS.Unused.Base64Utils.Encode(joined);

            System.IO.File.WriteAllText(System.IO.Path.Combine(setting.DataPath, ugsMetaFileName), content);

            AssetDatabase.Refresh();
        }
#endif
        public static void Generate()
        {
            GoogleSheet.GoogleSpreadSheets.Init(new UnityGSParser(), new UnityFileReader());
            var files = GoogleDriveExplorerGUI.GetWindow<GoogleDriveExplorerGUI>().loadedFileData;
            if (Application.isPlaying == false)
            {
                foreach (var file in files)
                {
                    if (file.type == FileType.Excel)
                    {
                        if (Application.isPlaying == false)
                        {
                            UnityEditorWebRequest.Instance.ReadSpreadSheet(new ReadSpreadSheetReqModel(file.id), OnEditorError, (x) =>
                            {
                                GoogleSheet.GoogleSpreadSheets.DataParser.ParseSheet(x, true, true, new UnityFileWriter());
                            });
                        }
                        else //Currently Not Support With PlayMode
                        {
                            UnityPlayerWebRequest.Instance.ReadSpreadSheet(new ReadSpreadSheetReqModel(file.id), OnEditorError, (x) =>
                            {
                                GoogleSheet.GoogleSpreadSheets.DataParser.ParseSheet(x, true, true, new UnityFileWriter());
                            });
                        }
                    }
                }

                // weplanb
#if UNITY_EDITOR
                WriteMetaData(GoogleDriveExplorerGUI.GetWindow<GoogleDriveExplorerGUI>().loadedFileData);
#endif
            }
            else
            {
                UnityEditor.EditorUtility.DisplayDialog("UGS Dialog", "Generate Not Support In Play/Runtime Mode.", "OK");
            }
        }
        public static void Setting()
        {
            UGSSetting.CreateInstance();
        }
        public static void Document()
        {
            Application.OpenURL("https://shlifedev.gitbook.io/unitygooglesheet/");
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
            ScrollViewRect = GUI.BeginScrollView(rect, ScrollViewRect, new Rect(0, 0, 0, 1000), false, true);
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
                if (GUI.Button(new Rect(currentItemXPos, currentItemYPos, itemWIdth, itemHeight), "", FileBackgroundGui))
                {
                    DoubleClickCheck(loadedFileData[i]);
                }
                if (loadedFileData[i].type == FileType.Excel)
                {
                    GUI.Box(new Rect(currentItemXPos + (itemWIdth / 4), currentItemYPos + (itemWIdth / 4), itemWIdth / 2, itemHeight / 2), "", ExcelIconGui);
                }
                if (loadedFileData[i].type == FileType.Folder)
                {
                    GUI.Box(new Rect(currentItemXPos + (itemWIdth / 4), currentItemYPos + (itemWIdth / 4), itemWIdth / 2, itemHeight / 2), "", FolderIconGui);
                }
                if (loadedFileData[i].type == FileType.ParentFolder)
                {
                    GUI.Box(new Rect(currentItemXPos + (itemWIdth / 4), currentItemYPos + (itemWIdth / 4), itemWIdth / 2, itemHeight / 2), "", FolderIconGui);
                }

                var fileName = loadedFileData[i].fileName;


                GUI.Label(new Rect(currentItemXPos, (currentItemYPos + itemHeight / 3), itemWIdth, itemHeight), fileName, FileNameGui);
            }
            GUI.EndScrollView();
        }



        public void OnBottom()
        {
            GUILayout.Label("시트 샘플 생성");
            DefaultFileName = GUILayout.TextField(DefaultFileName);
            if (GUILayout.Button("시트 샘플 생성"))
            {
                if (Application.isPlaying == false)
                {
                    UnityEditorWebRequest.Instance.CreateDefaultSheet(new CreateDefaultReqModel(currentViewFolderId, DefaultFileName), OnEditorError, x =>
                    {
                        var result = x;
                        loadedFileData.Add(new FileData(FileType.Excel, result.url, result.fileID, result.fileName));
                    });
                }
                else
                {
                    UnityPlayerWebRequest.Instance.CreateDefaultSheet(new CreateDefaultReqModel(currentViewFolderId, DefaultFileName), OnEditorError, x =>
                    {
                        var result = x;
                        if (result != null)
                            loadedFileData.Add(new FileData(FileType.Excel, result.url, result.fileID, result.fileName));
                    });
                }
            }
        }




        public void OnTop()
        {
            EditorGUILayout.BeginHorizontal();
            {
                for (int i = 0; i < _topMenus.Count; i++)
                {
                    if (GUILayout.Button(_topMenus[i].Item1, TopButtonGui))
                    {
                        _topMenus[i].Item2?.Invoke();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }


        public void Refresh()
        {
        }
        public void OnFocus()
        {
            if (loadedFileData.Count == 0)
            {
                Debug.Log("Realod Sheet Folder");
                CreateFileDatas(UGSettingObjectWrapper.GoogleFolderID);
            }
        }
        public void OnGUI()
        {
            OnTop();
            OnExplorer();
            OnBottom();
        }

    }
}
#endif
