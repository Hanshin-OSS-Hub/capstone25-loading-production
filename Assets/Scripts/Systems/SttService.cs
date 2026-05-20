using System;
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

        try
        {
            return recorder.StartRecording();
        }
        catch (Exception e)
        {
            ProjectLogger.Error($"녹음 시작 중 예외 발생: {e.Message}");

            return OperationResult<bool>.Fail(
                ConversationErrorType.STTFailed,
                "녹음을 시작하지 못했습니다."
            );
        }
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

        try
        {
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
        catch (Exception e)
        {
            ProjectLogger.Error($"STT 서비스 처리 중 예외 발생: {e.Message}");

            return OperationResult<string>.Fail(
                ConversationErrorType.STTFailed,
                "음성 인식 처리 중 오류가 발생했습니다."
            );
        }
    }

    public void CancelRecording()
    {
        if (recorder == null)
        {
            ProjectLogger.Warning(
                "SttService: recorder가 없어 녹음을 취소할 수 없습니다."
            );

            return;
        }

        try
        {
            recorder.CancelRecording();
        }
        catch (Exception e)
        {
            ProjectLogger.Warning($"녹음 취소 중 예외 발생: {e.Message}");
        }
    }
}