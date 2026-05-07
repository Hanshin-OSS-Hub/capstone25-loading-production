public class SkillCastResult
{
    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }

    private SkillCastResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static SkillCastResult Success(string message)
    {
        return new SkillCastResult(true, message);
    }

    public static SkillCastResult Fail(string message)
    {
        return new SkillCastResult(false, message);
    }
}