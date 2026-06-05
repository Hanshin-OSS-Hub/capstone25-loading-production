using System.Collections.Generic;
using UnityEngine;

public class SkillCooldownManager : MonoBehaviour
{
    [Header("Cooldown Settings")]
    [SerializeField] private float defaultCooldown = 5f;

    private readonly Dictionary<SkillId, float> _cooldownEndTimes = new Dictionary<SkillId, float>();

    public bool CanUse(SkillId skillId)
    {
        return GetRemainingTime(skillId) <= 0f;
    }

    public void StartCooldown(SkillId skillId)
    {
        _cooldownEndTimes[skillId] = Time.time + defaultCooldown;
    }

    public float GetRemainingTime(SkillId skillId)
    {
        if (!_cooldownEndTimes.TryGetValue(skillId, out float endTime))
            return 0f;

        return Mathf.Max(0f, endTime - Time.time);
    }

    public float GetCooldownDuration()
    {
        return defaultCooldown;
    }
}