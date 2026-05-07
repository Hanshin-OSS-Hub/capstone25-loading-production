using System;
using UnityEngine;

public class PushToSkillInput : MonoBehaviour
{
    [SerializeField] private KeyCode skillKey = KeyCode.R;

    public event Action OnSkillPressed;
    public event Action OnSkillReleased;

    private void Update()
    {
        if (Input.GetKeyDown(skillKey))
        {
            ProjectLogger.Input("스킬 입력 시작");
            OnSkillPressed?.Invoke();
        }

        if (Input.GetKeyUp(skillKey))
        {
            ProjectLogger.Input("스킬 입력 종료");
            OnSkillReleased?.Invoke();
        }
    }
}