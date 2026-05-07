using UnityEngine;

public class PersonaProvider : MonoBehaviour
{
    [Header("Persona File")]
    [SerializeField] private TextAsset personaTextAsset;

    [Header("Fallback Persona")]
    [TextArea(5, 20)]
    [SerializeField] private string fallbackPersona =
        "당신은 차분하고 신중한 안내자다. 짧고 명확하게 답하라.";

    public string GetPersona()
    {
        if (personaTextAsset != null && !string.IsNullOrWhiteSpace(personaTextAsset.text))
        {
            return personaTextAsset.text.Trim();
        }

        ProjectLogger.Warning("페르소나 파일이 비어 있거나 연결되지 않아 fallback 페르소나를 사용합니다.");
        return fallbackPersona.Trim();
    }
}