using System.Threading.Tasks;
using UnityEngine;
using Whisper;

public class WhisperTranscriber : MonoBehaviour
{
    [SerializeField] private WhisperManager whisperManager;

    public async Task<OperationResult<string>> TranscribeAsync(AudioRecordingData audioData)
    {
        if (whisperManager == null)
        {
            ProjectLogger.Error("WhisperManager가 연결되지 않았습니다.");
            return OperationResult<string>.Fail(
                ConversationErrorType.STTFailed,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.STTFailed)
            );
        }

        ProjectLogger.STT("음성 인식 시작");

        var result = await whisperManager.GetTextAsync(
            audioData.Samples,
            audioData.Frequency,
            audioData.Channels
        );

        if (result == null)
        {
            ProjectLogger.Warning("음성 인식 결과가 null입니다.");
            return OperationResult<string>.Fail(
                ConversationErrorType.STTFailed,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.STTFailed)
            );
        }

        if (string.IsNullOrWhiteSpace(result.Result))
        {
            ProjectLogger.Warning("음성 인식 결과가 비어 있습니다.");
            return OperationResult<string>.Fail(
                ConversationErrorType.EmptySTTResult,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.EmptySTTResult)
            );
        }

        ProjectLogger.STT($"인식 결과: {result.Result}");
        return OperationResult<string>.Success(result.Result);
    }
}