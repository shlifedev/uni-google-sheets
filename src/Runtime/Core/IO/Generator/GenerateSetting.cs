namespace GoogleSheet.IO.Generator
{
    public class GenerateSetting
    {
        public GenerateSetting(string jsonSavePath, string jsonLoadPath, string chsarpSavePath)
        {
            this.jsonSavePath = jsonSavePath;
            this.jsonLoadPath = jsonLoadPath;
            this.chsarpSavePath = chsarpSavePath;
        }
        public string jsonSavePath;
        public string jsonLoadPath;
        public string chsarpSavePath;

    }
}
