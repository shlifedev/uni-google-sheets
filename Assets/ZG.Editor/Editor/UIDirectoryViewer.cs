using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;
using Hamster.ZG.Http.Protocol;
using static UIFile;
using Hamster.ZG;

public class UIFile
{
    public enum FileType
    {
        Folder, ParentFolder, Excel, Unknown
    }

    /// <summary>
    /// 파일 타입이 폴더이면 존재하거나 없음.
    /// </summary>
    public string parentFolderID;
    public List<UIFile> childFiles = new List<UIFile>();
     
    public string id;

    public VisualElement uiElement;
    public VisualElement iconElement => uiElement.Q("Icon");
    public Label FileNameElement => uiElement.Q("FileName") as Label;



    public FileType type;
    public string url = "";
    public string fileName = "";



    static System.DateTime latestClickTime = System.DateTime.MinValue; 

    public static UIFile CreateFolderInstance(string fileName, string url, string id, string parentFolder = null)
    {
        UIFile file = new UIFile();
        file.fileName = fileName;
        file.type = UIFile.FileType.Folder;
        file.uiElement = UIDirectoryViewer.CreateDirectoryElement(fileName);
        file.id = id;
        file.url = url;
        file.parentFolderID = parentFolder;
        AddClickEvent(file);
        return file;
    }
    public static UIFile CreateExcelInstance(string fileName, string url, string id)
    {
        UIFile file = new UIFile();
        file.fileName = fileName;
        file.type = UIFile.FileType.Excel;
        file.uiElement = UIDirectoryViewer.CreateExcelElement(fileName);
        file.id = id;
        file.url = url;
        AddClickEvent(file);
        return file;
    }
    public static UIFile CreateParentFolderInstance()
    {
        UIFile file = new UIFile();
        file.fileName = "..";
        file.type = UIFile.FileType.ParentFolder;
        file.uiElement = UIDirectoryViewer.CreateDirectoryElement("..");
        file.id = null;
        file.url = null;
        AddClickEvent(file);
        return file;
    }



    static void OnClickEvent(ClickEvent _event, UIFile file)
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
                    UIDirectoryViewer.LoadFolder(file.id);   
                }
                else if (file.type == FileType.ParentFolder)
                {
                    if(file.parentFolderID == UIDirectoryViewer.RootFolderID)
                    {
                        UIDirectoryViewer.LoadRootFolder();
                    }
                    else
                    {
                        UIDirectoryViewer.LoadFolder(file.parentFolderID);
                    }
                 
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
    private static void AddClickEvent(UIFile file)
    {
        file.uiElement.RegisterCallback<UnityEngine.UIElements.ClickEvent>(x => { 
            OnClickEvent(x, file);
        });
    }
     
    public void AddChild(UIFile file)
    {
        this.childFiles.Add(file);
        file.parentFolderID = this.id;
         
        if (file.type == FileType.Folder) 
            file.AddChild(CreateParentFolderInstance()); 

        childFiles = childFiles.OrderBy(x => x.type).ToList();
    }

}
public class UIDirectoryViewer : EditorWindow
{ 
    static UIFile currentViewfile = null; 
    public static string RootFolderID
    {
        get
        {
            return ZGSetting.GoogleFolderID;
        }
    }

    public static UIDirectoryViewer Instance = null;
    static UnityEngine.UIElements.ScrollView scrollView;

    public static UIFile CurrentViewFile 
    {
        get => currentViewfile; 
        set
        {
            currentViewfile = value;
            UpdateCurrentViewFiles();
        }
    }
     
    [MenuItem("HamsterLib/ZGS/Manager")]
    public static void CreateInstance()
    {
         
        if (Instance == null)
        {
            if(string.IsNullOrEmpty(ZGSetting.GoogleFolderID) || string.IsNullOrEmpty(ZGSetting.ScriptURL))
            {
                EditorUtility.DisplayDialog("Require Setting!", "Cannot Open ZGS Menu. Please Setting Complete!", "OK");
                return;
            }
            ZeroGoogleSheet.Init(new UnityGSParser(), new UnityFileReader());
            /* Load UI Directory View */
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/ZG.Editor/Editor/UIDirectoryViewer.uxml");
            Instance = GetWindow<UIDirectoryViewer>(); 
            Instance.titleContent = new GUIContent("UIDirectoryViewer");
            Instance.rootVisualElement.Add(uiAsset.CloneTree());

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
            LoadRootFolder();
        }
        else
        {
            Instance.Show();
        }
    }

    static void AddOpenEvent()
    {
        var open = Instance.rootVisualElement.Q("OpenFolder") as Label;
        open.RegisterCallback<ClickEvent>(x => {
            if(CurrentViewFile != null)
            {
                Application.OpenURL(CurrentViewFile.url);
            }
            
        });
    }
    static void AddGithubBtnEvent()
    {
        var github = Instance.rootVisualElement.Q("Github") as Label;
        github.RegisterCallback<ClickEvent>(x => {
           Application.OpenURL("https://github.com/shlifedev/UnityGoogleSheet");
        });
    }
    static void AddSettingBtnEvent()
    {
        var setting = Instance.rootVisualElement.Q("Setting") as Label;
        setting.RegisterCallback<ClickEvent>(x => {
           UISetting.CreateInstance();
        });
    }
    static void AddGenerateBtnEvent()
    {
        var generate = Instance.rootVisualElement.Q("Generate") as Label;
        generate.RegisterCallback<ClickEvent>(x => {
            foreach (var file in CurrentViewFile.childFiles)
            {
                if (file.type == FileType.Excel)
                {
                    UnityEditorWebRequest.Instance.GetTableData(file.id, (x1, x2) => {
                        ZeroGoogleSheet.DataParser.ParseSheet(x2, true, true, new UnityFileWriter());
                    });
                }
            }
        });
    }
    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        GetWindow<UIDirectoryViewer>().Close();  
    }

    public void OnEnable()
    {
        //if(wnd == null)
        //{
        //    CreateInstance();
        //}
    }

    /// <summary>
    /// 현재 보고있는 폴더의 파일들 표시
    /// </summary>
    private static void UpdateCurrentViewFiles()
    {
        
        if(CurrentViewFile != null)
        {
 
            scrollView.Clear();
            foreach(var file in CurrentViewFile.childFiles)
            {
                scrollView.Add(file.uiElement);
            }
        }
    }

    public static void LoadRootFolder()
    {

        UnityEditorWebRequest.Instance.GetFolderFiles(RootFolderID, x => {
            CreateFileList(x, true, RootFolderID);
        });
    }
    public static void LoadFolder(string folderId)
    {

        UnityEditorWebRequest.Instance.GetFolderFiles(folderId, x => {
            CreateFileList(x, false, folderId);
        });
    }

    private static void CreateFileList(GetFolderInfo folder, bool root, string folderID = null)
    {
         
        UIFile file = null;
        if (root)
        {
            file = UIFile.CreateFolderInstance("root", $"https://drive.google.com/drive/folders/{folderID}", RootFolderID, null);
        }
        else
        {
            file = UIFile.CreateFolderInstance("root", $"https://drive.google.com/drive/folders/{folderID}", RootFolderID, null);
        }

        
        for (int i = 0; i < folder.fileID.Count; i++) {

 
            if (folder.fileType[i] == (int)FileType.Excel)
            {
                file.AddChild(UIFile.CreateExcelInstance(folder.fileName[i], folder.url[i], folder.fileID[i]));
            }
            if (folder.fileType[i] == (int)FileType.Folder)
            {
                file.AddChild(UIFile.CreateFolderInstance(folder.fileName[i], folder.url[i], folder.fileID[i], file.id)); 
            } 
          
        }
        if (root == false)
        {
            file.AddChild(UIFile.CreateParentFolderInstance());
        }
        CurrentViewFile = file;
    }

    #region test
    private static void AddTestUIFile()
    {
        UIFile file = UIFile.CreateFolderInstance("root", null, null);
        file.AddChild(UIFile.CreateExcelInstance("Unit", null, null)); 
        file.AddChild(UIFile.CreateExcelInstance("Quest", null, null));
        file.AddChild(UIFile.CreateExcelInstance("Items", null, null));

        var test1 = UIFile.CreateFolderInstance("GachaDatas", null, null);
        file.AddChild(test1);

        var test2 = UIFile.CreateFolderInstance("Localization", null, null);
        file.AddChild(test2);

        test1.AddChild(UIFile.CreateExcelInstance("WeaponGacha", null, null));
        test1.AddChild(UIFile.CreateExcelInstance("ArmorGacha", null, null));


        test2.AddChild(UIFile.CreateExcelInstance("Localization_ko_kr",null,null));
        test2.AddChild(UIFile.CreateExcelInstance("Localization_en_us",null,null)); 

        CurrentViewFile = file; 
    }
    #endregion
    #region visual_element_creator
    public static VisualElement CreateDirectoryElement(string directoryName)
    { 
        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/ZG.Editor/Editor/UIFile.uxml");
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
        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/ZG.Editor/Editor/UIFile.uxml");
        var fileElement = uiAsset.CloneTree().Query().Name("File").First() as VisualElement;
        var label = fileElement.Query().Name("FileName").First() as Label;
            label.text = fileName;

        var iconElement = fileElement.Query().Name("Icon").First() as VisualElement;

        var iconResource = Resources.Load("ExcelIcon") as Texture2D;
        iconElement.style.backgroundImage = new StyleBackground(iconResource);
        iconElement.style.backgroundColor = new StyleColor(new Color(0,0,0,0));
        return fileElement;
    }
    #endregion
}