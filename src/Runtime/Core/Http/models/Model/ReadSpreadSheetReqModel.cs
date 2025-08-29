namespace GoogleSheet.Protocol.v2.Req
{
    public class GetDriveDirectoryReqModel : Model
    {
        public string folderId;

        public GetDriveDirectoryReqModel(string folderId)
        {
            this.instruction = (int)EInstruction.SEARCH_GOOGLE_DRIVE;
            this.folderId = folderId;
        }
    }
}
