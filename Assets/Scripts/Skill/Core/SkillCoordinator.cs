using UnityEngine;

public class SkillCoordinator : MonoBehaviour
{
    [Header("Modules")]
    [SerializeField] private PushToSkillInput inputHandler;
    [SerializeField] private SttService sttService;
    [SerializeField] private SkillCommandParser commandParser;
    [SerializeField] private SkillExecutor skillExecutor;

    [Header("Feedback")]
    [SerializeField] private SkillCameraShake cameraShake;
    [SerializeField] private HitStopController hitStop;

    [Header("Result UI")]
    [SerializeField] private SkillResultView skillResultView;

    private bool _isProcessing;

    private void OnEnable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnSkillPressed += HandleSkillPressed;
            inputHandler.OnSkillReleased += HandleSkillReleased;
            inputHandler.OnSkillCanceled += HandleSkillCanceled;
        }
    }

    private void OnDisable()
    {
        if (inputHandler != null)
        {
            inputHandler.OnSkillPressed -= HandleSkillPressed;
            inputHandler.OnSkillReleased -= HandleSkillReleased;
            inputHandler.OnSkillCanceled -= HandleSkillCanceled;
        }
    }

    private void HandleSkillPressed()
    {
        if (_isProcessing)
        {
            ProjectLogger.Warning(SkillTestMessages.Processing);
            return;
        }

        if (sttService == null)
        {
            ProjectLogger.Error("SkillCoordinator: SttService가 연결되지 않았습니다.");

            if (skillResultView != null)
                skillResultView.ShowFailed(SkillTestMessages.MissingModules);

            return;
        }

        var result = sttService.StartRecording();

        if (!result.IsSuccess)
        {
            ProjectLogger.Error($"스킬 녹음 시작 실패: {result.ErrorMessage}");

            if (skillResultView != null)
                skillResultView.ShowFailed(result.ErrorMessage);

            return;
        }
    }

    private async void HandleSkillReleased()
    {
        if (_isProcessing)
        {
            ProjectLogger.Warning(SkillTestMessages.Processing);
            return;
        }

        if (sttService == null || commandParser == null || skillExecutor == null)
        {
            ProjectLogger.Error("SkillCoordinator: 필요한 모듈이 연결되지 않았습니다.");

            if (skillResultView != null)
                skillResultView.ShowFailed(SkillTestMessages.MissingModules);

            return;
        }

        _isProcessing = true;

        try
        {
            var sttResult = await sttService.StopRecordingAndTranscribeAsync();

            if (!sttResult.IsSuccess)
            {
                ProjectLogger.Warning($"스킬 STT 실패: {sttResult.ErrorMessage}");

                if (skillResultView != null)
                    skillResultView.ShowFailed(sttResult.ErrorMessage);

                return;
            }

            string recognizedText = sttResult.Data;
            ProjectLogger.STT($"스킬 명령 인식 결과: {recognizedText}");

            if (skillResultView != null)
                skillResultView.ShowRecognized(recognizedText);

            SkillId skillId = commandParser.Parse(recognizedText);

            if (skillId == SkillId.None)
            {
                ProjectLogger.Warning($"알 수 없는 스킬 명령: {recognizedText}");

                if (skillResultView != null)
                    skillResultView.ShowFailed(SkillTestMessages.UnknownSkill);

                return;
            }

            string skillName = SkillNameProvider.GetKoreanName(skillId);
            SkillCastResult castResult = skillExecutor.Execute(skillId);

            if (!castResult.IsSuccess)
            {
                ProjectLogger.Warning($"스킬 실행 실패: {castResult.Message}");

                if (skillResultView != null)
                    skillResultView.ShowFailed(castResult.Message);

                return;
            }

            if (skillResultView != null)
                skillResultView.ShowSkillActivated(skillId);

            if (hitStop != null)
                hitStop.Play(skillId);

            if (cameraShake != null)
                cameraShake.Shake(skillId);
        }
        finally
        {
            _isProcessing = false;
        }
    }

    private void HandleSkillCanceled()
    {
        ProjectLogger.Warning("스킬 입력이 너무 짧아 취소되었습니다.");

        if (sttService != null)
            sttService.CancelRecording();

        if (skillResultView != null)
            skillResultView.ShowFailed("입력이 너무 짧습니다.");
    }
}