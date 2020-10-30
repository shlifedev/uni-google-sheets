 
 
 # UnityGoogleSheet 
You can manage your game data very easily, flexible, fast!  
 It's Free! Optionally, If you want to sponsor a developer, buy it from the Asset Store! [here]()
 
 ## ScreenShot
![](https://i.imgur.com/ZBtiwmD.png)

 ## Download
  https://github.com/shlifedev/UnityGoogleSheet/releases
 
 ## Require
  - Unity 2020 (Require Latest UIElement) (I Will Support Unity 2017)
  - Newtonsoft.Json
  
 # Features
 - Support All C# Type & Your Custom Class&Struct Type.
 - Support Code Generator
 - Very Friendly User-Interface
 - Sync GoogleDrive GUI
 - Read From GoogleSheet & Write To GoogleSheet
  
 # How to Setup
 Setup Tutorial Video : [Video](https://www.youtube.com/watch?v=WUyZypX3NHw&feature=youtu.be)  
 Script URL : [Script URL Here](https://script.google.com/d/1XukliHOlfrmX26xvYEA3r2ZBaRoRh7baWLvDv56e9Ix3eIBBF1VDyq2W/edit)  

 Included Example! : [Example Here](https://github.com/shlifedev/UnityGoogleSheet/tree/main/Assets/ZG/ZG.Examples)  
 Example Sheet Datas : [Example Sheet Here](https://drive.google.com/drive/folders/189ozeSkUZpseEBWCOvGlN4FKefwz25Q6?usp=sharing)  
 
 # CustomType Add Example
 You can implemented new type using the Type attribute. Just implement the Read&Write Function.
  
 
  ```csharp
    [Type(typeof(Vector3), new string[] { "Vector3", "vector3"})]
    public struct Vector3Type : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
             // write your read code :D
        }
    }
 ```
 
 
  ```csharp
    [Type(typeof((int, int)), new string[] { "(int,int)", "(Int32,Int32)" })]
    public class IntTupleX2Type : IType
    {
        public object DefaultValue => null;
        public object Read(string value)
        {
            var datas = ReadUtil.GetBracketValueToArray(value);
            if(datas.Length == 0 || datas.Length == 1 || datas.Length > 2) return DefaultValue;
            else 
                return (datas[0], datas[1]); 
        }
    }
 ```

# How to Write Data To GoogleSheet
 Use Write Method In Your Generated Script 
 
 
Example Generate Script..
```cs
namespace Example
{
    [Hamster.ZG.Attribute.TableStruct]
    public class Data : ITable
    { 
        static bool isLoaded = false;
        public static string spreadSheetID = "1oPGjxVw6cKptBeDFn9xBrhkjqoInxaSr_Y7FUyZG3bQ"; // it is file id
        public static string sheetID = "0"; // it is sheet id
        public static Dictionary<int, Data> DataMap = new Dictionary<int, Data>(); 
        public static List<Data> DataList = new List<Data>();  
        public static UnityFileReader reader = new UnityFileReader();

        //Datas
      	public Int32 UnitIndex;
	public String Data;

        public static void Write(Data data)
        { 
            ...
        }  
        public static void Load(bool forceReload = false)
        {
             ...
        } 
    }
}
        
```
```cs
   Example.Data.Write(new Example.Data(){
          UnitIndex = 1001,
          Data = "data
   });
   
```
 
## Future Plans
 - UIElement to IMGUI
 - Asset Store
