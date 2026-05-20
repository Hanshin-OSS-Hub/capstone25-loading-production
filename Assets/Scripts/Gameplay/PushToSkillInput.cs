using System;
using UnityEngine;

public class PushToSkillInput : MonoBehaviour
{
    [SerializeField] private KeyCode skillKey = KeyCode.R;

    [Header("Input Guard")]
    [SerializeField] private float minimumHoldTime = 0.25f;

    public event Action OnSkillPressed;
    public event Action OnSkillReleased;
    public event Action OnSkillCanceled;

    private bool _isPressed;
    private float _pressedTime;

    private void Update()
    {
        if (Input.GetKeyDown(skillKey))
        {
            _isPressed = true;
            _pressedTime = Time.time;

            OnSkillPressed?.Invoke();
        }

        if (Input.GetKeyUp(skillKey))
        {
            if (!_isPressed)
                return;

            _isPressed = false;

            float heldTime = Time.time - _pressedTime;

            if (heldTime < minimumHoldTime)
            {
                ProjectLogger.Warning($"스킬 입력이 너무 짧습니다. holdTime={heldTime:F2}s");

                OnSkillCanceled?.Invoke();
                return;
            }

            OnSkillReleased?.Invoke();
        }
    }
}