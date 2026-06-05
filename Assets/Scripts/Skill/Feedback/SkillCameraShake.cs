using UnityEngine;

public class SkillCameraShake : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CameraMovement cameraMovement;

    [Header("Shake Amount")]
    [SerializeField] private float daggerShake = 0.05f;
    [SerializeField] private float windShake = 0.03f;
    [SerializeField] private float shieldShake = 0.04f;
    [SerializeField] private float dashShake = 0.08f;

    [Header("Shake Duration")]
    [SerializeField] private float daggerDuration = 0.08f;
    [SerializeField] private float windDuration = 0.05f;
    [SerializeField] private float shieldDuration = 0.07f;
    [SerializeField] private float dashDuration = 0.12f;

    private void Reset()
    {
        if (Camera.main != null)
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
    }

    private void Awake()
    {
        if (cameraMovement == null && Camera.main != null)
            cameraMovement = Camera.main.GetComponent<CameraMovement>();
    }

    public void Shake(SkillId skillId)
    {
        if (cameraMovement == null)
        {
            ProjectLogger.Warning("SkillCameraShake: CameraMovement가 연결되지 않았습니다.");
            return;
        }

        cameraMovement.Shake(
            GetShakeAmount(skillId),
            GetShakeDuration(skillId)
        );
    }

    private float GetShakeAmount(SkillId skillId)
    {
        switch (skillId)
        {
            case SkillId.Blade:
                return daggerShake;
            case SkillId.Storm:
                return windShake;
            case SkillId.Barrier:
                return shieldShake;
            case SkillId.Dash:
                return dashShake;
            default:
                return 0.03f;
        }
    }

    private float GetShakeDuration(SkillId skillId)
    {
        switch (skillId)
        {
            case SkillId.Blade:
                return daggerDuration;
            case SkillId.Storm:
                return windDuration;
            case SkillId.Barrier:
                return shieldDuration;
            case SkillId.Dash:
                return dashDuration;
            default:
                return 0.05f;
        }
    }
}