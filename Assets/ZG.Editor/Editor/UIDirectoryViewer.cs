using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;
using Hamster.ZG.Http.Protocol;
using static UIFile;

public class UIFile
{
    public enum FileType
    {
        Folder, ParentFolder, Excel, Unknown
    }

    /// <summary>
    /// 파일 타입이 폴더이면 존재하거나 없음.
    /// </summary>
    public UIFile parentFolder; 
    public List<UIFile> childFiles = new List<UIFile>();
     
    public string id;

    public VisualElement uiElement;
    public VisualElement iconElement => uiElement.Q("Icon");
    public Label FileNameElement => uiElement.Q("FileName") as Label;



    public FileType type;
    public string url = "";
    public string fileName = "";



    static System.DateTime latestClickTime = System.DateTime.MinValue; 

    public static UIFile CreateFolderInstance(string fileName, string url, string id)
    {
        UIFile file = new UIFile();
        file.fileName = fileName;
        file.type = UIFile.FileType.Folder;
        file.uiElement = UIDirectoryViewer.CreateDirectoryElement(fileName);
        file.id = id;
        file.url = url;
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
                    UIDirectoryViewer.CurrentViewFile = file;
                }
                if (file.type == FileType.ParentFolder)
                {
                    //페런드폴더 객체의 부모(폴더)의 객체=> 진짜객체
                    UIDirectoryViewer.CurrentViewFile = file.parentFolder.parentFolder;
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
        file.parentFolder = this; 
         
        if (file.type == FileType.Folder) 
            file.AddChild(CreateParentFolderInstance()); 
        childFiles = childFiles.OrderBy(x => x.type).ToList();
    }

}
public class UIDirectoryViewer : EditorWindow
{




    /// <summary>
    /// 현재 보고있는 파일
    /// </summary>
    static UIFile currentViewfile = null; 


    static UIDirectoryViewer wnd = null;
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

    [MenuItem("Window/UIElements/UIDirectoryViewer")]
    public static void ShowExample()
    { 
        /* Load UI Directory View */
        VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/ZG.Editor/Editor/UIDirectoryViewer.uxml");
        wnd = GetWindow<UIDirectoryViewer>();
        wnd.titleContent = new GUIContent("UIDirectoryViewer");
        wnd.rootVisualElement.Add(uiAsset.CloneTree()); 
        scrollView = wnd.rootVisualElement.Query("FileGroup").First() as ScrollView; 
        /* Scroll View To Grid View Script Custom */
        scrollView.contentContainer.style.flexDirection = new StyleEnum<FlexDirection>() { value = FlexDirection.Row };
        scrollView.contentContainer.style.flexWrap = new StyleEnum<Wrap>() { value = Wrap.Wrap};
        scrollView.contentContainer.style.flexShrink = new StyleFloat(1);
        scrollView.contentContainer.style.overflow = new StyleEnum<Overflow>(Overflow.Visible); 
        LoadGoogleFolder();
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

    private static void LoadGoogleFolder()
    {
        UnityEditorWebRequest.Instance.GetFolderFiles("1EoXKE6nzb9nqAsYCSO3R5YqYxA2pd_TK", x => {
            CreateFileList(x);      
        });
    }

    private static void CreateFileList(GetFolderInfo folder)
    {
        UIFile file = UIFile.CreateFolderInstance("root", null, null);
        for (int i = 0; i < folder.fileID.Count; i++) {

            Debug.Log(folder.fileName);
            if (folder.fileType[i] == (int)FileType.Excel)
            {
                file.AddChild(UIFile.CreateExcelInstance(folder.fileName[i], folder.url[i], folder.fileID[i]));
            }
            if (folder.fileType[i] == (int)FileType.Folder)
            {
                file.AddChild(UIFile.CreateFolderInstance(folder.fileName[i], folder.url[i], folder.fileID[i]));
            }
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