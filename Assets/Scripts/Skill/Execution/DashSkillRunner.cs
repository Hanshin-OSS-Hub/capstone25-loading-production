using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DashSkillRunner : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private bool _isDashing;
    private float _speedBuffMultiplier = 1f;
    private Coroutine _speedBuffRoutine;

    private void Reset()
    {
        controller = GetComponent<CharacterController>();
    }

    public void RunDash(Vector3 direction, float distance, float duration, float buffDuration, float buffMultiplier)
    {
        if (_isDashing)
            return;

        StartCoroutine(DashRoutine(direction, distance, duration));

        if (_speedBuffRoutine != null)
            StopCoroutine(_speedBuffRoutine);

        _speedBuffRoutine = StartCoroutine(SpeedBuffRoutine(buffDuration, buffMultiplier));
    }

    private IEnumerator DashRoutine(Vector3 direction, float distance, float duration)
    {
        _isDashing = true;

        float elapsed = 0f;
        float speed = distance / duration;

        while (elapsed < duration)
        {
            controller.Move(direction.normalized * speed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _isDashing = false;
    }

    private IEnumerator SpeedBuffRoutine(float duration, float multiplier)
    {
        _speedBuffMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        _speedBuffMultiplier = 1f;
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