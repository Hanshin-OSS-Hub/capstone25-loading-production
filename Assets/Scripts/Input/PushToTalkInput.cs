using System;
using UnityEngine;

public class PushToTalkInput : MonoBehaviour
{
    public KeyCode talkKey = KeyCode.T;

    public event Action OnTalkPressed;
    public event Action OnTalkReleased;

    void Update()
    {
        if (Input.GetKeyDown(talkKey))
        {
            ProjectLogger.Input("대화 입력 시작");
            OnTalkPressed?.Invoke();
        }

        if (Input.GetKeyUp(talkKey))
        {
            ProjectLogger.Input("대화 입력 종료");
            OnTalkReleased?.Invoke();
        }
    }
}