using System;
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
                SystemErrorType.STTFailed,
                "음성 인식 모듈이 연결되지 않았습니다."
            );
        }

        if (audioData == null || audioData.Samples == null || audioData.Samples.Length == 0)
        {
            ProjectLogger.Warning("전사할 오디오 데이터가 비어 있습니다.");
            return OperationResult<string>.Fail(
                SystemErrorType.STTFailed,
                "녹음된 음성이 없습니다."
            );
        }

        try
        {
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
                    SystemErrorType.STTFailed,
                    "음성 인식 결과를 가져오지 못했습니다."
                );
            }

            if (string.IsNullOrWhiteSpace(result.Result))
            {
                ProjectLogger.Warning("음성 인식 결과가 비어 있습니다.");
                return OperationResult<string>.Fail(
                    SystemErrorType.EmptySTTResult,
                    "음성이 인식되지 않았습니다."
                );
            }

            ProjectLogger.STT($"인식 결과: {result.Result}");
            return OperationResult<string>.Success(result.Result);
        }
        catch (Exception e)
        {
            ProjectLogger.Error($"STT 처리 중 예외 발생: {e.Message}");

            return OperationResult<string>.Fail(
                SystemErrorType.STTFailed,
                "음성 인식 중 오류가 발생했습니다."
            );
        }
    }
}