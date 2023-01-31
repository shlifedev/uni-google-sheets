namespace GoogleSheet.Protocol.v2.Req
{
    public class CopyExampleReqModel : Model
    {
        public CopyExampleReqModel()
        {
            this.instruction = (int)EInstruction.COPY_EXAMPLE;
        }
    }
}
