using UnityEngine;

public class SkillCoordinator : MonoBehaviour
{
    [Header("Modules")]
    [SerializeField] private PushToSkillInput inputHandler;
    [SerializeField] private SttService sttService;
    [SerializeField] private SkillCommandParser commandParser;
    [SerializeField] private SkillExecutor skillExecutor;

    [Header("UI (Optional)")]
    [SerializeField] private SpeechBubbleView playerBubble;
    [SerializeField] private SpeechBubbleView npcBubble;

    private bool _isProcessing;

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnSkillPressed += HandleSkillPressed;
            inputHandler.OnSkillReleased += HandleSkillReleased;
        }
    }

    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnSkillPressed -= HandleSkillPressed;
            inputHandler.OnSkillReleased -= HandleSkillReleased;
        }
    }

    private void HandleSkillPressed()
    {
        if (_isProcessing)
        {
            ProjectLogger.Warning(SkillTestMessages.Processing);

            if (npcBubble != null)
                npcBubble.SetText(SkillTestMessages.Processing);

            return;
        }

        if (sttService == null)
        {
            ProjectLogger.Error("SkillCoordinator: SttService가 연결되지 않았습니다.");

            if (npcBubble != null)
                npcBubble.SetText(SkillTestMessages.MissingModules);

            return;
        }

        var result = sttService.StartRecording();

        if (!result.IsSuccess)
        {
            ProjectLogger.Error($"스킬 녹음 시작 실패: {result.ErrorMessage}");

            if (playerBubble != null)
                playerBubble.SetText(result.ErrorMessage);

            if (npcBubble != null)
                npcBubble.SetText("스킬 녹음을 시작하지 못했습니다.");

            return;
        }

        ProjectLogger.Record("스킬 녹음 시작");

        if (playerBubble != null)
            playerBubble.SetText(SkillTestMessages.Recording);
    }

    private async void HandleSkillReleased()
    {
        if (_isProcessing)
        {
            ProjectLogger.Warning(SkillTestMessages.Processing);

            if (npcBubble != null)
                npcBubble.SetText(SkillTestMessages.Processing);

            return;
        }

        if (sttService == null || commandParser == null || skillExecutor == null)
        {
            ProjectLogger.Error("SkillCoordinator: 필요한 모듈이 연결되지 않았습니다.");

            if (npcBubble != null)
                npcBubble.SetText(SkillTestMessages.MissingModules);

            return;
        }

        _isProcessing = true;

        try
        {
            if (playerBubble != null)
                playerBubble.SetText(SkillTestMessages.Transcribing);

            var sttResult = await sttService.StopRecordingAndTranscribeAsync();

            if (!sttResult.IsSuccess)
            {
                ProjectLogger.Error($"스킬 STT 실패: {sttResult.ErrorMessage}");

                if (playerBubble != null)
                    playerBubble.SetText(sttResult.ErrorMessage);

                if (npcBubble != null)
                    npcBubble.SetText("스킬 명령 인식에 실패했습니다.");

                return;
            }

            string recognizedText = sttResult.Data;
            ProjectLogger.STT($"스킬 명령 인식 결과: {recognizedText}");

            if (playerBubble != null)
                playerBubble.SetText($"인식 결과: {recognizedText}");

            SkillId skillId = commandParser.Parse(recognizedText);

            if (skillId == SkillId.None)
            {
                ProjectLogger.Warning($"알 수 없는 스킬 명령: {recognizedText}");

                if (npcBubble != null)
                    npcBubble.SetText(SkillTestMessages.UnknownSkill);

                return;
            }

            string skillName = SkillNameProvider.GetKoreanName(skillId);
            ProjectLogger.UI($"스킬 판정 성공: {skillName}");

            SkillCastResult castResult = skillExecutor.Execute(skillId);

            if (!castResult.IsSuccess)
            {
                ProjectLogger.Warning($"스킬 실행 실패: {castResult.Message}");

                if (npcBubble != null)
                    npcBubble.SetText(castResult.Message);

                return;
            }

            ProjectLogger.UI($"스킬 실행 성공: {castResult.Message}");

            if (npcBubble != null)
                npcBubble.SetText(castResult.Message);
        }
        finally
        {
            _isProcessing = false;
        }
    }
}