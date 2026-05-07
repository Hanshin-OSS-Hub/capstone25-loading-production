using UnityEngine;

public class SkillCommandParser : MonoBehaviour
{
    public SkillId Parse(string recognizedText)
    {
        if (string.IsNullOrWhiteSpace(recognizedText))
            return SkillId.None;

        string text = Normalize(recognizedText);

        if (text.Contains("칼날"))
            return SkillId.Blade;

        if (text.Contains("폭풍"))
            return SkillId.Storm;

        if (text.Contains("장벽"))
            return SkillId.Barrier;

        if (text.Contains("돌진"))
            return SkillId.Dash;

        return SkillId.None;
    }

    private string Normalize(string input)
    {
        return input.Trim()
                    .Replace(" ", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("!", "")
                    .Replace("?", "");
    }
}