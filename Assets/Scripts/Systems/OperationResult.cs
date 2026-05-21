public class OperationResult<T>
{
    public bool IsSuccess { get; private set; }
    public T Data { get; private set; }
    public SystemErrorType ErrorType { get; private set; }
    public string ErrorMessage { get; private set; }

    private OperationResult() { }

    public static OperationResult<T> Success(T data)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data,
            ErrorType = SystemErrorType.None,
            ErrorMessage = string.Empty
        };
    }

    public static OperationResult<T> Fail(SystemErrorType errorType, string errorMessage)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Data = default,
            ErrorType = errorType,
            ErrorMessage = errorMessage
        };
    }
}