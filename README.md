 
 
 # Unity ZeroGoogleSheet
 Free, And You can manage your game data very easily, flexible, fast!
 
 ## Download
  https://github.com/shlifedev/UnityGoogleSheet/releases
 
 ## Require
  - Unity 2019.1 ++ (Require UIElement Support)
  - Newtonsoft.Json
  
 # Features
 - Support All C# Type & Your Custom Class&Struct Type.
 - Support Code Generator
 - Very Friendly User-Interface
 - Sync GoogleDrive GUI
 ![N](https://github.com/shlifedev/UnityGoogleSheet/blob/main/app.gif?raw=true)
 # How to Setup
 Tutorial Video : https://www.youtube.com/watch?v=WUyZypX3NHw&feature=youtu.be  
 Script URL : https://script.google.com/d/1XukliHOlfrmX26xvYEA3r2ZBaRoRh7baWLvDv56e9Ix3eIBBF1VDyq2W/edit

 # CustomType Add Example
 You can implemented new type using the Type attribute. And implement the Read function.
  
 
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
 
