using UnityEngine;

public class MicrophoneRecorder : MonoBehaviour
{
    [Header("Recording Settings")]
    public int maxDurationSeconds = 30;
    public int sampleRate = 16000;

    private AudioClip _recordingClip;
    private string _micName;
    private bool _isRecording;

    public bool IsRecording => _isRecording;
    public bool HasMicrophone => !string.IsNullOrEmpty(_micName);

    void Awake()
    {
        if (Microphone.devices.Length > 0)
        {
            _micName = Microphone.devices[0];
        }
        else
        {
            ProjectLogger.Warning("사용 가능한 마이크가 없습니다.");
        }
    }

    public OperationResult<bool> StartRecording()
    {
        if (_isRecording)
            return OperationResult<bool>.Success(true);

        if (string.IsNullOrEmpty(_micName))
        {
            ProjectLogger.Warning("마이크가 설정되지 않았습니다.");
            return OperationResult<bool>.Fail(
                ConversationErrorType.NoMicrophone,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.NoMicrophone)
            );
        }

        _recordingClip = Microphone.Start(_micName, false, maxDurationSeconds, sampleRate);

        if (_recordingClip == null)
        {
            ProjectLogger.Error("녹음을 시작하지 못했습니다.");
            return OperationResult<bool>.Fail(
                ConversationErrorType.RecordingFailed,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.RecordingFailed)
            );
        }

        _isRecording = true;
        ProjectLogger.Record("녹음 시작");
        return OperationResult<bool>.Success(true);
    }

    public OperationResult<AudioRecordingData> StopRecording()
    {
        if (!_isRecording)
        {
            ProjectLogger.Warning("녹음 중이 아닌 상태에서 종료 요청이 들어왔습니다.");
            return OperationResult<AudioRecordingData>.Fail(
                ConversationErrorType.RecordingFailed,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.RecordingFailed)
            );
        }

        int lastPos = Microphone.GetPosition(_micName);
        Microphone.End(_micName);
        _isRecording = false;

        if (_recordingClip == null || lastPos <= 0)
        {
            ProjectLogger.Warning("녹음 데이터가 없습니다.");
            return OperationResult<AudioRecordingData>.Fail(
                ConversationErrorType.EmptyRecording,
                SystemMessageProvider.GetErrorMessage(ConversationErrorType.EmptyRecording)
            );
        }

        int channels = _recordingClip.channels;
        int frequency = _recordingClip.frequency;
        float[] samples = new float[lastPos * channels];
        _recordingClip.GetData(samples, 0);

        ProjectLogger.Record("녹음 종료");

        AudioRecordingData data = new AudioRecordingData(samples, frequency, channels);
        return OperationResult<AudioRecordingData>.Success(data);
    }
}