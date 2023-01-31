namespace GoogleSheet.Protocol.v2.Req
{
    public class WriteObjectReqModel : Model
    {
        public string fileID;
        public string sheetID;
        public string key;
        [Newtonsoft.Json.JsonProperty("value")]
        public string[] values;
        public WriteObjectReqModel(string fileID, string sheetID, string key, string[] values)
        {
            this.instruction = (int)EInstruction.WRITE_OBJECT;
            this.fileID = fileID;
            this.sheetID = sheetID;
            this.key = key;
            this.values = values;
        }
    }
}
