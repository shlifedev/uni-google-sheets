using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheet.IO.FileReader
{
    public class FileReader : IFileReader
    {
        public string ReadData(string fileName)
        { 
            Directory.CreateDirectory("CachedJson/");
            Directory.CreateDirectory("TableScripts/");
   
            return System.IO.File.ReadAllText("CachedJson/" + fileName+".json");
        } 
    }
}
