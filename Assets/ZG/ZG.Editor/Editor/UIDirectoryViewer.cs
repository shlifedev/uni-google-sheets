using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;
using Hamster.ZG.Http.Protocol;
using static FileData;
using Hamster.ZG;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

public class FileData
{
    public enum FileType
    {
        Folder, ParentFolder, Excel, Unknown
    }

    /// <summary>
    /// 파일 타입이 폴더이면 존재하거나 없음.
    /// </summary>
    public string parentFolderID;
    public List<FileData> childFiles = new List<FileData>();

    public string id;

    public VisualElement uiElement;
    public VisualElement iconElement => uiElement.Q("Icon");
    public Label FileNameElement => uiElement.Q("FileName") as Label;



    public FileType type;
    public string url = "";
    public string fileName = "";



    static System.DateTime latestClickTime = System.DateTime.MinValue;

    public static FileData CreateFolderInstance(string fileName, string url, string id, string parentFolder = null)
    {
        FileData file = new FileData();
        file.fileName = fileName;
        file.type = FileData.FileType.Folder;
        file.uiElement = UIDirectoryViewer.CreateDirectoryElement(fileName);
        file.id = id;
        file.url = url;
        file.parentFolderID = parentFolder;
        AddClickEvent(file);
        return file;
    }
    public static FileData CreateExcelInstance(string fileName, string url, string id)
    {
        FileData file = new FileData();
        file.fileName = fileName;
        file.type = FileData.FileType.Excel;
        file.uiElement = UIDirectoryViewer.CreateExcelElement(fileName);
        file.id = id;
        file.url = url;
        AddClickEvent(file);
        return file;
    }
    public static FileData CreateParentFolderInstance(string parentID)
    {
        FileData file = new FileData();
        file.fileName = "..";
        file.type = FileData.FileType.ParentFolder;
        file.uiElement = UIDirectoryViewer.CreateDirectoryElement("..");
        file.id = parentID;
        file.url = null;
        file.parentFolderID = parentID;
        AddClickEvent(file);
        return file;
    }



    static void OnClickEvent(MouseDownEvent _event, FileData file)
    {


        if (latestClickTime != System.DateTime.Now && System.DateTime.MinValue != latestClickTime)
        {
            var timeDist = (System.DateTime.Now - latestClickTime).Milliseconds;
            //더블클릭 판정
            if (timeDist <= 200)
            {
                latestClickTime = System.DateTime.MinValue;

                if (file.type == FileType.Folder)
                {
                    UIDirectoryViewer.Instance.CurrentAssetPath = Path.Combine(UIDirectoryViewer.Instance.CurrentAssetPath, file.fileName);
                    UIDirectoryViewer.LoadFolder(file.id);
                }
                else if (file.type == FileType.ParentFolder)
                {

                    DirectoryInfo di = new DirectoryInfo(UIDirectoryViewer.Instance.CurrentAssetPath);
                    var parent = di.Parent;
                    var appPath = Application.dataPath.Replace("\\","/");
                    appPath = appPath.Remove(appPath.Length - 6, 6);

                    var prevPath =  di.Parent.FullName.Replace("\\","/");
                    prevPath = prevPath.Replace(appPath, null);


                    UIDirectoryViewer.Instance.CurrentAssetPath = prevPath;
                    UIDirectoryViewer.LoadFolder(file.id);

                }
                else if (file.type == FileType.Excel)
                {
                    Application.OpenURL(file.url);
                }
                return;
            }
        }

        /* 일반 클릭 판정 */
        latestClickTime = System.DateTime.Now;
    }
    /// <summary>
    /// 클릭 이벤트 추가
    /// </summary>
    /// <param name="file"></param>
    private static void AddClickEvent(FileData file)
    {
        file.uiElement.RegisterCallback<UnityEngine.UIElements.MouseDownEvent>(x =>
        {
            OnClickEvent(x, file);
        });
    }

    public void AddChild(FileData file)
    {
        this.childFiles.Add(file);
        file.parentFolderID = this.id;


        childFiles = childFiles.OrderBy(x => x.type).ToList();
    }

}
public class UIDirectoryViewer : EditorWindow
{
    static FileData currentViewfile = null;
    public static string RootFolderID
    {
        get
        {
            return ZGSetting.GoogleFolderID;
        }
    }

    public static UIDirectoryViewer Instance = null;
    static UnityEngine.UIElements.ScrollView scrollView;

    public static FileData CurrentViewFile
    {
        get => currentViewfile;
        set
        {
            currentViewfile = value;
            UpdateCurrentViewFiles();
        }
    }

    public string CurrentAssetPath { get => this.masterPath; set => this.masterPath = value; }

    [MenuItem("HamsterLib/ZGS/Manager")]
    public static void CreateInstance()
    {
        if (Instance == null)
        {
            if (string.IsNullOrEmpty(ZGSetting.GoogleFolderID) || string.IsNullOrEmpty(ZGSetting.ScriptURL))
            {
                EditorUtility.DisplayDialog("Require Setting!", "Cannot Open ZGS Menu. Please Setting Complete!", "OK");
                
                //for recursive calling
                var trashWindow = UIDirectoryViewer.GetWindow<UIDirectoryViewer>();
                trashWindow.Close();

                return;
            }

            var assets = AssetDatabase.FindAssets("ZG.UIDirectoryViewer");
            var uxml = AssetDatabase.GUIDToAssetPath(assets.ToList().Find(x=>AssetDatabase.GUIDToAssetPath(x).Contains(".uxml")));
 

            ZeroGoogleSheet.Init(new UnityGSParser(), new UnityFileReader());
            /* Load UI Directory View */
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxml);
            Instance = GetWindow<UIDirectoryViewer>();
            Instance.titleContent = new GUIContent("UIDirectoryViewer");
            var ud = uiAsset.CloneTree();
 
            var uss = AssetDatabase.GUIDToAssetPath(assets.ToList().Find(x=>AssetDatabase.GUIDToAssetPath(x).Contains(".uss")));
            ud.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(uss));
 
            Instance.rootVisualElement.Add(ud);

            scrollView = Instance.rootVisualElement.Query("FileGroup").First() as ScrollView;
            /* Scroll View To Grid View Script Custom */
            scrollView.contentContainer.style.flexDirection = new StyleEnum<FlexDirection>() { value = FlexDirection.Row };
            scrollView.contentContainer.style.flexWrap = new StyleEnum<Wrap>() { value = Wrap.Wrap };
            scrollView.contentContainer.style.flexShrink = new StyleFloat(1);
            scrollView.contentContainer.style.overflow = new StyleEnum<Overflow>(Overflow.Visible);


            //Generate Event Add

            AddGenerateBtnEvent();
            AddGithubBtnEvent();
            AddOpenEvent();
            AddSettingBtnEvent();
            AddCreateDefaultTableEvent();
            LoadRootFolder();
        }
        else
        {
            Instance.Show();
        }
    }


    static void AddCreateDefaultTableEvent()
    {

        var btn = Instance.rootVisualElement.Q("createDefaultTableBtn") as Button;

        var createDefaultTableNameField = Instance.rootVisualElement.Q("createDefaultTableNameField") as TextField;
        btn.clicked += ()=>
        {

            if (Application.isPlaying == false)
            {
                UnityEditorWebRequest.Instance.POST_CreateDefaultTable(CurrentViewFile.id, createDefaultTableNameField.value, json =>
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
                        CurrentViewFile.AddChild(CreateExcelInstance(result.fileName, result.url, result.fileID));
                        UpdateCurrentViewFiles();
                    }
                    else
                    {
                        Debug.Log(json);
                    }
                });
            }
            else
            {
                UnityPlayerWebRequest.Instance.POST_CreateDefaultTable(CurrentViewFile.id, createDefaultTableNameField.value, json =>
                {
                    if (json == "failed")
                    {
                        EditorUtility.DisplayDialog("Failed Create Default Table!", "Table Create Failed.", "OK");
                        return;
                    }
                    var result = JsonConvert.DeserializeObject<CreateDefaultTableResult>(json);
                    if (result != null)
                    {
                        CurrentViewFile.AddChild(CreateExcelInstance(result.fileName, result.url, result.fileID));
                        UpdateCurrentViewFiles();
                    }
                });
            }
        };
    }
    static void AddOpenEvent()
    {
        var open = Instance.rootVisualElement.Q("OpenFolder") as Label;
        open.RegisterCallback<MouseDownEvent>(x =>
        {
            if (CurrentViewFile != null)
            {
                Application.OpenURL(CurrentViewFile.url);
            }

        });
    }
    static void AddGithubBtnEvent()
    {
        var github = Instance.rootVisualElement.Q("Github") as Label;
        github.RegisterCallback<MouseDownEvent>(x =>
        {
            Application.OpenURL("https://github.com/shlifedev/UnityGoogleSheet");
        });
    }
    static void AddSettingBtnEvent()
    {
        var setting = Instance.rootVisualElement.Q("Setting") as Label;
        setting.RegisterCallback<MouseDownEvent>(x =>
        {
            UISetting.CreateInstance();
        });
    }
    static void AddGenerateBtnEvent()
    {
        var generate = Instance.rootVisualElement.Q("Generate") as Label;
        generate.RegisterCallback<MouseDownEvent>(x =>
        {
            foreach (var file in CurrentViewFile.childFiles)
            {
                if (file.type == FileType.Excel)
                {
                    if (Application.isPlaying == false)
                    {
                        UnityEditorWebRequest.Instance.GET_TableData(file.id, (x1, x2) =>
                        {
                            ZeroGoogleSheet.DataParser.ParseSheet(x2, true, true, new UnityFileWriter());
                        });
                    }
                    else
                    {
                        UnityPlayerWebRequest.Instance.GET_TableData(file.id, (x1, x2) =>
                        {
                            ZeroGoogleSheet.DataParser.ParseSheet(x2, true, true, new UnityFileWriter());
                        });
                    }
                }
            }
        });
    }


    /// <summary>
    /// 현재 보고있는 폴더의 파일들 표시
    /// </summary>
    private static void UpdateCurrentViewFiles()
    {

        if (CurrentViewFile != null)
        {

            scrollView.Clear();
            foreach (var file in CurrentViewFile.childFiles)
            {
                scrollView.Add(file.uiElement);
            }
        }
    }

    private string masterPath = "Assets/";
    public void OnGUI()
    {
 
        if (Instance == null)
        {

            CreateInstance();
        }
        return;
        if (Application.isPlaying == false)
        {
            if (Instance == null)
            {
                CreateInstance();
            }
        }
        else
        {
            GUILayout.Label("Currently PlayMode Not Support..");
        }
    }
    public static void LoadRootFolder()
    {
        if (Application.isPlaying == false)
        {
            UnityEditorWebRequest.Instance.GET_ReqFolderFiles(RootFolderID, x =>
            {
                CreateFileList(x, true, RootFolderID);
            });
        }
        else
        {
            UnityPlayerWebRequest.Instance.GET_ReqFolderFiles(RootFolderID, x =>
            {
                CreateFileList(x, true, RootFolderID);
            });
        }
    }
    public static void LoadFolder(string folderId)
    {
        if (Application.isPlaying == false)
        {
            UnityEditorWebRequest.Instance.GET_ReqFolderFiles(folderId, x =>
            {
                CreateFileList(x, false, folderId);
            });
        }
        else
        {
            UnityPlayerWebRequest.Instance.GET_ReqFolderFiles(folderId, x =>
            {
                CreateFileList(x, false, folderId);
            });
        }
    }

    private static void CreateFileList(GetFolderInfo folder, bool root, string folderID = null)
    {

        FileData file = null;
        if (root)
        {
            file = FileData.CreateFolderInstance("root", $"https://drive.google.com/drive/folders/{folderID}", RootFolderID, null);
        }
        else
        {
            var str = CurrentViewFile.id;
            file = FileData.CreateFolderInstance(folderID, $"https://drive.google.com/drive/folders/{folderID}", folderID, str);
        }


        for (int i = 0; i < folder.fileID.Count; i++)
        {


            if (folder.fileType[i] == (int)FileType.Excel)
            {
                file.AddChild(FileData.CreateExcelInstance(folder.fileName[i], folder.url[i], folder.fileID[i]));
            }
            if (folder.fileType[i] == (int)FileType.Folder)
            {
                file.AddChild(FileData.CreateFolderInstance(folder.fileName[i], folder.url[i], folder.fileID[i], file.id));
            }

        }
        if (folderID != RootFolderID)
        {
            file.AddChild(FileData.CreateParentFolderInstance(CurrentViewFile.id));
        }
        CurrentViewFile = file;
    }


#region visual_element_creator


    public static VisualElement CreateDirectoryElement(string directoryName)
    {
        var assets = AssetDatabase.FindAssets("ZG.UIFile");
        var uxml = AssetDatabase.GUIDToAssetPath(assets.ToList().Find(x=>AssetDatabase.GUIDToAssetPath(x).Contains(".uxml")));


        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxml);
        var fileElement = uiAsset.CloneTree().Query().Name("File").First() as VisualElement;
        var label = fileElement.Query().Name("FileName").First() as Label;
        label.text = directoryName;

        var iconElement = fileElement.Query().Name("Icon").First() as VisualElement;
        var iconResource = Resources.Load<Texture2D>("FolderIcon");

        iconElement.style.backgroundImage = iconResource;
        iconElement.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0));
        return fileElement;
    }
    public static VisualElement CreateExcelElement(string fileName)
    {
        var assets = AssetDatabase.FindAssets("ZG.UIFile");
        var uxml = AssetDatabase.GUIDToAssetPath(assets.ToList().Find(x=>AssetDatabase.GUIDToAssetPath(x).Contains(".uxml")));


        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxml);
        var fileElement = uiAsset.CloneTree().Query().Name("File").First() as VisualElement;
        var label = fileElement.Query().Name("FileName").First() as Label;
        label.text = fileName;

        var iconElement = fileElement.Query().Name("Icon").First() as VisualElement;

        var iconResource = Resources.Load("ExcelIcon") as Texture2D;
        iconElement.style.backgroundImage = new StyleBackground(iconResource);
        iconElement.style.backgroundColor = new StyleColor(new Color(0, 0, 0, 0));
        return fileElement;
    }
#endregion
}