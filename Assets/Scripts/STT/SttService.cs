using System.Threading.Tasks;
using UnityEngine;

public class SttService : MonoBehaviour
{
    [Header("Modules")]
    [SerializeField] private MicrophoneRecorder recorder;
    [SerializeField] private WhisperTranscriber transcriber;

    public OperationResult<bool> StartRecording()
    {
        if (recorder == null)
        {
            ProjectLogger.Error("SttService: recorder가 연결되지 않았습니다.");
            return OperationResult<bool>.Fail(
                ConversationErrorType.Unknown,
                "녹음 모듈이 연결되지 않았습니다."
            );
        }

        return recorder.StartRecording();
    }

    public async Task<OperationResult<string>> StopRecordingAndTranscribeAsync()
    {
        if (recorder == null || transcriber == null)
        {
            ProjectLogger.Error("SttService: recorder 또는 transcriber가 연결되지 않았습니다.");
            return OperationResult<string>.Fail(
                ConversationErrorType.Unknown,
                "음성 인식 모듈이 연결되지 않았습니다."
            );
        }

        var stopResult = recorder.StopRecording();

        if (!stopResult.IsSuccess)
        {
            return OperationResult<string>.Fail(
                stopResult.ErrorType,
                stopResult.ErrorMessage
            );
        }

        var sttResult = await transcriber.TranscribeAsync(stopResult.Data);

        if (!sttResult.IsSuccess)
        {
            return OperationResult<string>.Fail(
                sttResult.ErrorType,
                sttResult.ErrorMessage
            );
        }

        return OperationResult<string>.Success(sttResult.Data);
    }
}