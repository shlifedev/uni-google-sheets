namespace GoogleSheet.Protocol.v2.Req
{
    public class ReadSpreadSheetReqModel : Model
    {
        public string fileId;

        public ReadSpreadSheetReqModel(string fileId)
        {
            this.instruction = (int)EInstruction.READ_SPREADSHEET;
            this.fileId = fileId;
        }
    }
}
