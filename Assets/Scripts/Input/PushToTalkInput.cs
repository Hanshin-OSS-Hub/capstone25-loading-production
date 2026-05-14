using System;
using UnityEngine;

public class PushToTalkInput : MonoBehaviour
{
    [SerializeField] private KeyCode talkKey = KeyCode.T;

    [Header("Input Guard")]
    [SerializeField] private float minimumHoldTime = 0.25f;

    public event Action OnTalkPressed;
    public event Action OnTalkReleased;
    public event Action OnTalkCanceled;

    private bool _isPressed;
    private float _pressedTime;

    private void Update()
    {
        if (Input.GetKeyDown(talkKey))
        {
            _isPressed = true;
            _pressedTime = Time.time;

            ProjectLogger.Input("대화 입력 시작");
            OnTalkPressed?.Invoke();
        }

        if (Input.GetKeyUp(talkKey))
        {
            if (!_isPressed)
                return;

            _isPressed = false;

            float heldTime = Time.time - _pressedTime;

            if (heldTime < minimumHoldTime)
            {
                ProjectLogger.Warning($"대화 입력이 너무 짧습니다. holdTime={heldTime:F2}s");

                OnTalkCanceled?.Invoke();
                return;
            }

            ProjectLogger.Input("대화 입력 종료");
            OnTalkReleased?.Invoke();
        }
    }
}