using UnityEngine;

public class ConversationCoordinator : MonoBehaviour
{
    [Header("Modules")]
    [SerializeField] private PushToTalkInput inputHandler;
    [SerializeField] private SttService sttService;
    [SerializeField] private GeminiClient geminiClient;

    [Header("UI")]
    [SerializeField] private SpeechBubbleView playerBubble;
    [SerializeField] private SpeechBubbleView npcBubble;

    private bool _isProcessing;
    private ConversationState _currentState = ConversationState.Idle;

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnTalkPressed += HandleTalkPressed;
            inputHandler.OnTalkReleased += HandleTalkReleased;
            inputHandler.OnTalkCanceled += HandleTalkCanceled;
        }
    }

    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnTalkPressed -= HandleTalkPressed;
            inputHandler.OnTalkReleased -= HandleTalkReleased;
            inputHandler.OnTalkCanceled -= HandleTalkCanceled;
        }
    }

    private void Start()
    {
        SetState(ConversationState.Idle);
    }

    private void SetState(ConversationState newState)
    {
        _currentState = newState;

        switch (_currentState)
        {
            case ConversationState.Idle:
                if (playerBubble != null)
                    playerBubble.SetText(SystemMessageProvider.Idle);
                
                if (npcBubble != null)
                    npcBubble.SetText("명령을 기다리는 중입니다.");
                break;

            case ConversationState.Recording:
                if (playerBubble != null)
                    playerBubble.SetText(SystemMessageProvider.Recording);
                break;

            case ConversationState.Transcribing:
                if (playerBubble != null)
                    playerBubble.SetText(SystemMessageProvider.Transcribing);
                break;

            case ConversationState.GeneratingResponse:
                if (npcBubble != null)
                    npcBubble.SetText(SystemMessageProvider.Generating);
                break;

            case ConversationState.Completed:
                break;

            case ConversationState.Error:
                break;
        }
    }

    private void ShowError(ConversationErrorType errorType)
    {
        string message = SystemMessageProvider.GetErrorMessage(errorType);

        if (_currentState == ConversationState.Recording || _currentState == ConversationState.Transcribing)
        {
            if (playerBubble != null)
                playerBubble.SetText(message);
        }
        else
        {
            if (npcBubble != null)
                npcBubble.SetText(message);
        }

        _currentState = ConversationState.Error;
    }

    private void HandleTalkPressed()
    {
        if (_isProcessing)
        {
            ProjectLogger.Warning("이미 처리 중입니다.");
            return;
        }

        if (sttService == null)
        {
            ProjectLogger.Error("ConversationCoordinator: SttService가 연결되지 않았습니다.");
            ShowError(ConversationErrorType.Unknown);
            return;
        }

        var recordStartResult = sttService.StartRecording();

        if (!recordStartResult.IsSuccess)
        {
            ShowError(recordStartResult.ErrorType);
            return;
        }

        SetState(ConversationState.Recording);
    }

    private async void HandleTalkReleased()
    {
        if (_isProcessing)
        {
            ProjectLogger.Warning("이미 처리 중이라 입력을 무시합니다.");
            return;
        }

        if (sttService == null)
        {
            ProjectLogger.Error("ConversationCoordinator: SttService가 연결되지 않았습니다.");
            ShowError(ConversationErrorType.Unknown);
            return;
        }

        _isProcessing = true;

        try
        {
            SetState(ConversationState.Transcribing);

            var sttResult = await sttService.StopRecordingAndTranscribeAsync();

            if (!sttResult.IsSuccess)
            {
                ShowError(sttResult.ErrorType);
                return;
            }

            if (playerBubble != null)
            {
                playerBubble.SetText(sttResult.Data);
                ProjectLogger.UI($"플레이어 말풍선 반영: {sttResult.Data}");
            }

            SetState(ConversationState.GeneratingResponse);

            if (geminiClient == null)
            {
                ProjectLogger.Error("ConversationCoordinator: GeminiClient가 연결되지 않았습니다.");
                ShowError(ConversationErrorType.Unknown);
                return;
            }

            var llmResult = await geminiClient.GenerateReplyAsync(sttResult.Data);

            if (!llmResult.IsSuccess)
            {
                ShowError(llmResult.ErrorType);
                return;
            }

            if (npcBubble != null)
            {
                npcBubble.SetText(llmResult.Data);
                ProjectLogger.UI($"NPC 말풍선 반영: {llmResult.Data}");
            }

            _currentState = ConversationState.Completed;
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private void HandleTalkCanceled()
    {
        ProjectLogger.Warning("대화 입력이 너무 짧아 취소되었습니다.");

        if (sttService != null)
            sttService.CancelRecording();

        SetState(ConversationState.Idle);

        if (playerBubble != null)
            playerBubble.SetText("조금 더 길게 말해주세요.");

        if (npcBubble != null)
            npcBubble.SetText("소리가 닿기 전에 사라졌습니다.");
    }
}