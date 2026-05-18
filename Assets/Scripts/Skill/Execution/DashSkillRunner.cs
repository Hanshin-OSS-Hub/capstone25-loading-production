using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DashSkillRunner : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController controller;

    [Header("Dash Visual")]
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private ParticleSystem dashStartEffect;
    [SerializeField] private ParticleSystem dashEndEffect;

    private bool _isDashing;
    private float _speedBuffMultiplier = 1f;
    private Coroutine _speedBuffRoutine;

    private void Reset()
    {
        controller = GetComponent<CharacterController>();
        dashTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        DisableDashVisuals();
    }

    public void RunDash(
        Vector3 direction,
        float distance,
        float duration,
        float buffDuration,
        float buffMultiplier)
    {
        if (_isDashing)
            return;

        if (controller == null)
        {
            ProjectLogger.Warning("DashSkillRunner: CharacterController가 연결되지 않았습니다.");
            return;
        }

        if (direction.sqrMagnitude <= 0.001f)
        {
            ProjectLogger.Warning("DashSkillRunner: 돌진 방향이 올바르지 않습니다.");
            return;
        }

        StartCoroutine(DashRoutine(direction, distance, duration));

        if (_speedBuffRoutine != null)
            StopCoroutine(_speedBuffRoutine);

        _speedBuffRoutine = StartCoroutine(SpeedBuffRoutine(buffDuration, buffMultiplier));
    }

    private IEnumerator DashRoutine(Vector3 direction, float distance, float duration)
    {
        _isDashing = true;

        Vector3 dashDirection = direction.normalized;
        float safeDuration = Mathf.Max(duration, 0.01f);
        float elapsed = 0f;
        float speed = distance / safeDuration;

        EnableDashVisuals();

        if (dashStartEffect != null)
            dashStartEffect.Play();

        while (elapsed < safeDuration)
        {
            controller.Move(dashDirection * speed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (dashEndEffect != null)
            dashEndEffect.Play();

        DisableDashVisuals();

        _isDashing = false;
    }

    private IEnumerator SpeedBuffRoutine(float duration, float multiplier)
    {
        _speedBuffMultiplier = multiplier;

        yield return new WaitForSeconds(duration);

        _speedBuffMultiplier = 1f;
        _speedBuffRoutine = null;
    }

    private void EnableDashVisuals()
    {
        if (dashTrail != null)
        {
            dashTrail.Clear();
            dashTrail.enabled = true;
            dashTrail.emitting = true;
        }
    }

    private void DisableDashVisuals()
    {
        if (dashTrail != null)
        {
            dashTrail.emitting = false;
            dashTrail.enabled = false;
        }
    }

    public float GetSpeedBuffMultiplier()
    {
        return _speedBuffMultiplier;
    }

    public bool IsDashing()
    {
        return _isDashing;
    }
}