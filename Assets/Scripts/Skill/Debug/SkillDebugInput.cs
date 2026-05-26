using UnityEngine;

public class SkillDebugInput : MonoBehaviour
{
    [SerializeField] private SkillExecutor skillExecutor;
    [SerializeField] private SkillCooldownManager cooldownManager;
    [SerializeField] private SkillCameraShake cameraShake;
    [SerializeField] private HitStopController hitStop;
    [SerializeField] private SkillResultView skillResultView;

    private void Update()
    {
        if (skillExecutor == null)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
            DebugCast(SkillId.Blade);

        if (Input.GetKeyDown(KeyCode.X))
            DebugCast(SkillId.Storm);

        if (Input.GetKeyDown(KeyCode.C))
            DebugCast(SkillId.Barrier);

        if (Input.GetKeyDown(KeyCode.V))
            DebugCast(SkillId.Dash);
    }

    private void DebugCast(SkillId skillId)
    {
        if (cooldownManager != null && !cooldownManager.CanUse(skillId))
        {
            float remainingTime = cooldownManager.GetRemainingTime(skillId);
            string skillName = SkillNameProvider.GetKoreanName(skillId);
            string message = $"{skillName} 스킬은 쿨타임 중입니다. ({remainingTime:F1}초)";

            ProjectLogger.Warning(message);

            if (skillResultView != null)
                skillResultView.ShowFailed(message);

            return;
        }

        SkillCastResult result = skillExecutor.Execute(skillId);

        if (!result.IsSuccess)
        {
            ProjectLogger.Warning($"[디버그] 실패: {result.Message}");

            if (skillResultView != null)
                skillResultView.ShowFailed(result.Message);

            return;
        }

        if (cooldownManager != null)
            cooldownManager.StartCooldown(skillId);

        if (skillResultView != null)
            skillResultView.ShowSkillActivated(skillId);

        if (hitStop != null)
            hitStop.Play(skillId);

        if (cameraShake != null)
            cameraShake.Shake(skillId);
    }
}