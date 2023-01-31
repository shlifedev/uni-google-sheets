namespace GoogleSheet.IO
{
    public interface IFIleWriter
    {
        void WriteCS(string writePath, string content);
        void WriteData(string writePath, string content);
    }
}
