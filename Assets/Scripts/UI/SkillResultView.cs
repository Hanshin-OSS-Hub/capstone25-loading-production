using System.Collections;
using TMPro;
using UnityEngine;

public class SkillResultView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text resultText;

    [Header("Timing")]
    [SerializeField] private float fadeInDuration = 0.1f;
    [SerializeField] private float showDuration = 0.9f;
    [SerializeField] private float fadeOutDuration = 0.2f;

    private Coroutine _routine;

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
        if (resultText != null)
            resultText.text = message;

        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        if (root != null)
            root.SetActive(true);

        yield return Fade(0f, 1f, fadeInDuration);

        yield return new WaitForSecondsRealtime(showDuration);

        yield return Fade(1f, 0f, fadeOutDuration);

        HideImmediately();

        _routine = null;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (canvasGroup == null)
            yield break;

        float elapsed = 0f;

        canvasGroup.alpha = from;

        if (duration <= 0f)
        {
            canvasGroup.alpha = to;
            yield break;
        }

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }

    private void HideImmediately()
    {
        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (root != null)
            root.SetActive(false);
    }
}