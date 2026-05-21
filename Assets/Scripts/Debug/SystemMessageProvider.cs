public static class SystemMessageProvider
{
    public static string GetErrorMessage(SystemErrorType errorType)
    {
        switch (errorType)
        {
            case SystemErrorType.NoMicrophone:
                return "사용 가능한 마이크가 없습니다.";

            case SystemErrorType.RecordingFailed:
                return "녹음을 시작하지 못했습니다.";

            case SystemErrorType.EmptyRecording:
                return "녹음된 음성이 없습니다.";

            case SystemErrorType.STTFailed:
                return "음성 인식에 실패했습니다.";

            case SystemErrorType.EmptySTTResult:
                return "음성을 인식하지 못했습니다.";

            case SystemErrorType.ParsingFailed:
                return "응답을 해석하지 못했습니다.";

            default:
                return "알 수 없는 오류가 발생했습니다.";
        }
    }
}