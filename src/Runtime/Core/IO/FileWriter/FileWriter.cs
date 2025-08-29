using System.IO;

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
