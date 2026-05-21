using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
    private const float DefaultSkillHeightOffset = 1f;
    private const float StormMinHeightOffset = 0.5f;

    [Header("Owner")]
    [SerializeField] private Transform caster;
    [SerializeField] private Transform forwardReference;

    [Header("Blade")]
    [SerializeField] private GameObject bladeProjectilePrefab;
    [SerializeField] private int bladeCount = 4;
    [SerializeField] private float bladeHorizontalSpacing = 0.8f;
    [SerializeField] private float bladeVerticalSpacing = 0.45f;
    [SerializeField] private float bladeForwardOffset = 1.0f;

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
            return FailMissingPrefab(SkillId.Blade);

        Vector3 forward = GetAimDirection();

        for (int i = 0; i < bladeCount; i++)
        {
            Vector3 spawnPosition = GetBladeSpawnPosition(i, forward);

            GameObject blade = Instantiate(
                bladeProjectilePrefab,
                spawnPosition,
                Quaternion.LookRotation(forward)
            );

            InitializeBladeProjectile(blade, forward);
        }

        return SkillCastResult.Success(SkillNameProvider.GetCastMessage(SkillId.Blade));
    }

    private SkillCastResult ExecuteStorm()
    {
        if (stormPrefab == null)
            return FailMissingPrefab(SkillId.Storm);

        Vector3 forward = GetAimDirection();
        Vector3 spawnPosition = GetStormSpawnPosition(forward);

        Instantiate(stormPrefab, spawnPosition, Quaternion.identity);

        return SkillCastResult.Success(SkillNameProvider.GetCastMessage(SkillId.Storm));
    }

    private SkillCastResult ExecuteBarrier()
    {
        if (barrierPrefab == null)
            return FailMissingPrefab(SkillId.Barrier);

        Vector3 forward = GetHorizontalDirection();
        Vector3 spawnPosition = GetBarrierSpawnPosition(forward);
        Quaternion rotation = Quaternion.LookRotation(forward);

        Instantiate(barrierPrefab, spawnPosition, rotation);

        return SkillCastResult.Success(SkillNameProvider.GetCastMessage(SkillId.Barrier));
    }

    private SkillCastResult ExecuteDash()
    {
        if (dashRunner == null)
        {
            ProjectLogger.Warning("DashSkillRunner가 연결되지 않았습니다.");
            return SkillCastResult.Fail("돌진 모듈이 연결되지 않았습니다.");
        }

        Vector3 forward = GetHorizontalDirection();

        dashRunner.RunDash(
            forward,
            dashDistance,
            dashDuration,
            dashSpeedBuffDuration,
            dashSpeedBuffMultiplier
        );

        return SkillCastResult.Success(SkillNameProvider.GetCastMessage(SkillId.Dash));
    }

    private SkillCastResult FailMissingPrefab(SkillId skillId)
    {
        string skillName = SkillNameProvider.GetKoreanName(skillId);
        string message = $"{skillName} 프리팹이 연결되지 않았습니다.";

        ProjectLogger.Warning(message);
        return SkillCastResult.Fail(message);
    }

    private void InitializeBladeProjectile(GameObject blade, Vector3 direction)
    {
        if (blade == null)
            return;

        BladeSkillProjectile projectile = blade.GetComponent<BladeSkillProjectile>();

        if (projectile != null)
        {
            projectile.Initialize(direction);
        }
    }

    private Vector3 GetBladeSpawnPosition(int index, Vector3 forward)
    {
        Transform dirRef = forwardReference != null ? forwardReference : caster;

        Vector3 right = dirRef.right;
        Vector3 up = dirRef.up;

        Vector2[] offsets =
        {
            new Vector2(-1f,  1f), // 좌상
            new Vector2( 1f,  1f), // 우상
            new Vector2(-1f, -1f), // 좌하
            new Vector2( 1f, -1f)  // 우하
        };

        Vector2 selectedOffset = offsets[index % offsets.Length];

        Vector3 spawnOffset =
            right * selectedOffset.x * bladeHorizontalSpacing +
            up * selectedOffset.y * bladeVerticalSpacing +
            forward * bladeForwardOffset;

        return caster.position + Vector3.up * DefaultSkillHeightOffset + spawnOffset;
    }

    private Vector3 GetStormSpawnPosition(Vector3 forward)
    {
        Vector3 spawnPosition =
            caster.position +
            Vector3.up * DefaultSkillHeightOffset +
            forward * stormSpawnDistance;

        float minY = caster.position.y + StormMinHeightOffset;

        if (spawnPosition.y < minY)
        {
            spawnPosition.y = minY;
        }

        return spawnPosition;
    }

    private Vector3 GetBarrierSpawnPosition(Vector3 forward)
    {
        return caster.position + forward * barrierSpawnDistance;
    }

    private Vector3 GetAimDirection()
    {
        Transform dirRef = forwardReference != null ? forwardReference : caster;

        Vector3 direction = dirRef.forward;

        if (direction.sqrMagnitude < 0.01f)
            direction = caster.forward;

        return direction.normalized;
    }

    private Vector3 GetHorizontalDirection()
    {
        Transform dirRef = forwardReference != null ? forwardReference : caster;

        Vector3 direction = dirRef.forward;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            direction = caster.forward;

        direction.y = 0f;
        return direction.normalized;
    }
}