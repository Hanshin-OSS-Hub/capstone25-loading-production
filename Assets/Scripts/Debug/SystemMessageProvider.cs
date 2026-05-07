public static class SystemMessageProvider
{
    public const string Idle = "T: 대화  |  R: 스킬\nQ: 속성 선택";
    public const string Recording = "녹음 중...";
    public const string Transcribing = "음성 인식 중...";
    public const string Generating = "응답을 생성 중입니다...";
    public const string Completed = "응답이 완료되었습니다.";

    public static string GetErrorMessage(ConversationErrorType errorType)
    {
        switch (errorType)
        {
            case ConversationErrorType.NoMicrophone:
                return "사용 가능한 마이크가 없습니다.";
            case ConversationErrorType.RecordingFailed:
                return "녹음을 시작하지 못했습니다.";
            case ConversationErrorType.EmptyRecording:
                return "녹음된 음성이 없습니다.";
            case ConversationErrorType.STTFailed:
                return "음성 인식에 실패했습니다.";
            case ConversationErrorType.EmptySTTResult:
                return "음성을 인식하지 못했습니다.";
            case ConversationErrorType.LLMRequestFailed:
                return "응답 생성에 실패했습니다.";
            case ConversationErrorType.EmptyLLMResponse:
                return "응답 내용이 비어 있습니다.";
            case ConversationErrorType.ParsingFailed:
                return "응답을 해석하지 못했습니다.";
            default:
                return "알 수 없는 오류가 발생했습니다.";
        }
    }
}