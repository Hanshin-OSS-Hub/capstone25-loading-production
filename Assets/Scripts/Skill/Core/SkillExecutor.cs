using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    [Header("Owner")]
    [SerializeField] private Transform caster;
    [SerializeField] private Transform forwardReference;

    [Header("Blade")]
    [SerializeField] private GameObject bladeProjectilePrefab;
    [SerializeField] private int bladeCount = 4;
    [SerializeField] private float bladeSpawnRadius = 1.2f;

    [Header("Storm")]
    [SerializeField] private GameObject stormPrefab;
    [SerializeField] private float stormSpawnDistance = 4f;

    [Header("Barrier")]
    [SerializeField] private GameObject barrierPrefab;
    [SerializeField] private float barrierSpawnDistance = 3f;

    [Header("Dash")]
    [SerializeField] private DashSkillRunner dashRunner;
    [SerializeField] private float dashDistance = 4f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float dashSpeedBuffDuration = 2f;
    [SerializeField] private float dashSpeedBuffMultiplier = 1.25f;

    private void Reset()
    {
        caster = transform;
        forwardReference = transform;
        dashRunner = GetComponent<DashSkillRunner>();
    }

    public SkillCastResult Execute(SkillId skillId)
    {
        if (caster == null)
        {
            ProjectLogger.Error("SkillExecutor: caster가 연결되지 않았습니다.");
            return SkillCastResult.Fail("시전자 정보가 연결되지 않았습니다.");
        }

        switch (skillId)
        {
            case SkillId.Blade:
                return ExecuteBlade();

            case SkillId.Storm:
                return ExecuteStorm();

            case SkillId.Barrier:
                return ExecuteBarrier();

            case SkillId.Dash:
                return ExecuteDash();

            default:
                ProjectLogger.Warning("SkillExecutor: 실행할 스킬이 없습니다.");
                return SkillCastResult.Fail("실행할 스킬이 없습니다.");
        }
    }

    private SkillCastResult ExecuteBlade()
    {
        if (bladeProjectilePrefab == null)
        {
            string skillName = SkillNameProvider.GetKoreanName(SkillId.Blade);
            ProjectLogger.Warning($"{skillName} 프리팹이 연결되지 않았습니다.");
            return SkillCastResult.Fail($"{skillName} 프리팹이 연결되지 않았습니다.");
        }

        Transform dirRef = forwardReference != null ? forwardReference : caster;
        Vector3 forward = dirRef.forward;
        forward.y = 0f;
        forward.Normalize();

        for (int i = 0; i < bladeCount; i++)
        {
            float angle = (360f / bladeCount) * i;
            Vector3 offset = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * bladeSpawnRadius;
            Vector3 spawnPosition = caster.position + Vector3.up * 1f + offset;

            GameObject blade = Instantiate(
                bladeProjectilePrefab,
                spawnPosition,
                Quaternion.LookRotation(forward)
            );

            BladeSkillProjectile projectile = blade.GetComponent<BladeSkillProjectile>();
            if (projectile != null)
            {
                projectile.Initialize(forward);
            }
        }

        ProjectLogger.UI($"스킬 실행: {SkillNameProvider.GetKoreanName(SkillId.Blade)}");
        return SkillCastResult.Success(SkillNameProvider.GetMuninnCastMessage(SkillId.Blade));
    }

    private SkillCastResult ExecuteStorm()
    {
        if (stormPrefab == null)
        {
            string skillName = SkillNameProvider.GetKoreanName(SkillId.Storm);
            ProjectLogger.Warning($"{skillName} 프리팹이 연결되지 않았습니다.");
            return SkillCastResult.Fail($"{skillName} 프리팹이 연결되지 않았습니다.");
        }

        Transform dirRef = forwardReference != null ? forwardReference : caster;
        Vector3 forward = dirRef.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 spawnPosition = caster.position + forward * stormSpawnDistance;
        Instantiate(stormPrefab, spawnPosition, Quaternion.identity);

        ProjectLogger.UI($"스킬 실행: {SkillNameProvider.GetKoreanName(SkillId.Storm)}");
        return SkillCastResult.Success(SkillNameProvider.GetMuninnCastMessage(SkillId.Storm));
    }

    private SkillCastResult ExecuteBarrier()
    {
        if (barrierPrefab == null)
        {
            string skillName = SkillNameProvider.GetKoreanName(SkillId.Barrier);
            ProjectLogger.Warning($"{skillName} 프리팹이 연결되지 않았습니다.");
            return SkillCastResult.Fail($"{skillName} 프리팹이 연결되지 않았습니다.");
        }

        Transform dirRef = forwardReference != null ? forwardReference : caster;
        Vector3 forward = dirRef.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 spawnPosition = caster.position + forward * barrierSpawnDistance;
        Quaternion rotation = Quaternion.LookRotation(forward);

        Instantiate(barrierPrefab, spawnPosition, rotation);

        ProjectLogger.UI($"스킬 실행: {SkillNameProvider.GetKoreanName(SkillId.Barrier)}");
        return SkillCastResult.Success(SkillNameProvider.GetMuninnCastMessage(SkillId.Barrier));
    }

    private SkillCastResult ExecuteDash()
    {
        if (dashRunner == null)
        {
            ProjectLogger.Warning("DashSkillRunner가 연결되지 않았습니다.");
            return SkillCastResult.Fail("돌진 모듈이 연결되지 않았습니다.");
        }

        Transform dirRef = forwardReference != null ? forwardReference : caster;
        Vector3 forward = dirRef.forward;
        forward.y = 0f;
        forward.Normalize();

        dashRunner.RunDash(
            forward,
            dashDistance,
            dashDuration,
            dashSpeedBuffDuration,
            dashSpeedBuffMultiplier
        );

        ProjectLogger.UI($"스킬 실행: {SkillNameProvider.GetKoreanName(SkillId.Dash)}");
        return SkillCastResult.Success(SkillNameProvider.GetMuninnCastMessage(SkillId.Dash));
    }
}