using UnityEngine;
using Cinemachine;

public class SkillCameraShake : MonoBehaviour
{
    [Header("Impulse Source")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Shake Settings")]
    [SerializeField] private float daggerShake = 0.2f;
    [SerializeField] private float windShake = 0.1f;
    [SerializeField] private float shieldShake = 0.2f;
    [SerializeField] private float dashShake = 0.3f;

    private void Reset()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(SkillId skillId)
    {
        if (impulseSource == null)
        {
            ProjectLogger.Warning("SkillCameraShake: CinemachineImpulseSource가 연결되지 않았습니다.");
            return;
        }

        float force = GetShakeForce(skillId);
        impulseSource.GenerateImpulse(force);
    }

    private float GetShakeForce(SkillId skillId)
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
                return 0.1f;
        }
    }
}