using System.Collections;
using UnityEngine;

public class HitStopController : MonoBehaviour
{
    [Header("Hit Stop Settings")]
    [SerializeField] private float daggerStopDuration = 0.03f;
    [SerializeField] private float windStopDuration = 0.01f;
    [SerializeField] private float shieldStopDuration = 0.025f;
    [SerializeField] private float dashStopDuration = 0.05f;

    private Coroutine _hitStopRoutine;
    private const float NormalTimeScale = 1f;

    public void Play(SkillId skillId)
    {        
        float duration = GetDuration(skillId);

        if (duration <= 0f)
            return;

        if (_hitStopRoutine != null)
        {
            StopCoroutine(_hitStopRoutine);
            _hitStopRoutine = null;
            Time.timeScale = NormalTimeScale;
        }

        _hitStopRoutine = StartCoroutine(HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = NormalTimeScale;
        _hitStopRoutine = null;
    }

    private void OnDisable()
    {
        Time.timeScale = NormalTimeScale;
    }

    private void OnDestroy()
    {
        Time.timeScale = NormalTimeScale;
    }

    private float GetDuration(SkillId skillId)
    {
        switch (skillId)
        {
            case SkillId.Blade:
                return daggerStopDuration;
            case SkillId.Storm:
                return windStopDuration;
            case SkillId.Barrier:
                return shieldStopDuration;
            case SkillId.Dash:
                return dashStopDuration;
            default:
                return 0f;
        }
    }
}