using UnityEngine;

public class SkillDebugInput : MonoBehaviour
{
    [SerializeField] private SkillExecutor skillExecutor;
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
        ProjectLogger.Input($"[디버그] 스킬 직접 실행: {skillId}");

        SkillCastResult result = skillExecutor.Execute(skillId);

        if (!result.IsSuccess)
        {
            ProjectLogger.Warning($"[디버그] 실패: {result.Message}");

            if (skillResultView != null)
                skillResultView.ShowFailed(result.Message);
                
            return;
        }

        ProjectLogger.UI($"[디버그] 성공: {result.Message}");

        if (skillResultView != null)
            skillResultView.ShowSkillActivated(skillId);

        if (hitStop != null)
            hitStop.Play(skillId);

        if (cameraShake != null)
            cameraShake.Shake(skillId);
    }
}