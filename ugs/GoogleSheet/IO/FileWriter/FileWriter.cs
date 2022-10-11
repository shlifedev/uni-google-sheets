using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.IO.FileWriter
{    
    public class FileWriter : IFIleWriter
    {
        public void WriteCS(string writePath, string content)
        { 
            Directory.CreateDirectory("TableScript/"); 
            System.IO.File.WriteAllText("TableScript/" + writePath + ".cs", content);
        }

        public void WriteData(string writePath, string content)
        {          
            Directory.CreateDirectory("CachedJson/");
            System.IO.File.WriteAllText("CachedJson/" + writePath + ".json", content);
        }
    }
}
