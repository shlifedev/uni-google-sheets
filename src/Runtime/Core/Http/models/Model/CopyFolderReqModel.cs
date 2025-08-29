namespace GoogleSheet.Protocol.v2.Req
{
    public class CopyFolderReqModel : Model
    {
        public string folderId;

        public CopyFolderReqModel(string folderId)
        {
            this.folderId = folderId;
            this.instruction = (int)EInstruction.COPY_FOLDER;
        }
    }
}
