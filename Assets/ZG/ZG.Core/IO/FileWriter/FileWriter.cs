using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.IO.FileWriter
{
    public class FileWriter : IFIleWriter
    {
        public void WriteCS(string writePath, string content)
        { 
            Directory.CreateDirectory("ZGS/Scripts/ZGS.Struct"); 
            System.IO.File.WriteAllText("ZGS/Scripts/ZGS.Struct/" + writePath + ".cs", content);
        }

        public void WriteData(string writePath, string content)
        {          
            Directory.CreateDirectory("ZGS/Resources/ZGS.Data");
            System.IO.File.WriteAllText("ZGS/Resources/ZGS.Data/" + writePath + ".json", content);
        }
    }
}
