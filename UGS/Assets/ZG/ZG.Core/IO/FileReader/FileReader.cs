using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hamster.ZG.IO.FileReader
{
    public class FileReader : IFileReader
    {
        public string ReadData(string fileName)
        {
            Directory.CreateDirectory("ZGS/Resources/ZGS.Data");
            Directory.CreateDirectory("ZGS/Scripts/ZGS.Struct");

            return System.IO.File.ReadAllText("ZGS/Resources/ZGS.Data/" + fileName+".json");
        } 
    }
}
