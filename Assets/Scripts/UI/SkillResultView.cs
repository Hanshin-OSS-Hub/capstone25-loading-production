using System.Collections;
using TMPro;
using UnityEngine;

public class SkillResultView : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private float showDuration = 1.5f;

    private Coroutine _hideRoutine;

    private void Awake()
    {
        HideImmediately();
    }

    public void ShowRecognized(string recognizedText)
    {
        Show($"인식됨: {recognizedText}");
    }

    public void ShowSkillActivated(SkillId skillId)
    {
        string skillName = SkillNameProvider.GetKoreanName(skillId);
        Show($"스킬 발동: {skillName}");
    }

    public void ShowFailed(string message)
    {
        Show($"인식 실패: {message}");
    }

    public void Show(string message)
    {
        if (root != null)
            root.SetActive(true);

        if (resultText != null)
            resultText.text = message;

        if (_hideRoutine != null)
            StopCoroutine(_hideRoutine);

        _hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);
        HideImmediately();
    }

    private void HideImmediately()
    {
        if (root != null)
            root.SetActive(false);
    }
}