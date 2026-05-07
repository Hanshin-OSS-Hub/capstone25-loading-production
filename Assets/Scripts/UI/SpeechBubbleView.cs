using TMPro;
using UnityEngine;

public class SpeechBubbleView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private GameObject bubbleObject;

    public void SetText(string message)
    {
        if (bubbleObject != null)
            bubbleObject.SetActive(true);

        if (bubbleText != null)
            bubbleText.text = message;
    }
}