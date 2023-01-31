
/// <summary>
/// 벨류 파싱에 에러가 생기면 발생
/// </summary>
public class UGSValueParseException : System.Exception
{
    public UGSValueParseException(string message) : base(message)
    {
    }
}