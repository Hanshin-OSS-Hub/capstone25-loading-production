using System.Collections.Generic;
using UnityEngine;

public class SkillCommandParser : MonoBehaviour
{
    [Header("Recognition Settings")]
    [SerializeField, Range(0.5f, 1.0f)]
    private float similarityThreshold = 0.75f;

    private readonly Dictionary<SkillId, string> standardCommands = new()
    {
        { SkillId.Blade, "단검" },
        { SkillId.Storm, "바람" },
        { SkillId.Barrier, "방패" },
        { SkillId.Dash, "돌진" }
    };

    private readonly Dictionary<SkillId, string[]> aliases = new()
    {
        { SkillId.Blade, new[] { "단검", "단거", "단건", "단겸", "당검" } },
        { SkillId.Storm, new[] { "바람", "바란", "바랑", "파람", "바룸" } },
        { SkillId.Barrier, new[] { "방패", "방페", "방베", "반패" } },
        { SkillId.Dash, new[] { "돌진", "돌지", "도진", "돌징" } }
    };

    public SkillId Parse(string recognizedText)
    {
        if (string.IsNullOrWhiteSpace(recognizedText))
            return SkillId.None;

        string normalizedText = Normalize(recognizedText);

        if (string.IsNullOrWhiteSpace(normalizedText))
            return SkillId.None;

        SkillId aliasMatched = MatchByAlias(normalizedText);

        if (aliasMatched != SkillId.None)
        {
            ProjectLogger.UI(
                $"스킬 명령 매칭: {recognizedText} → {standardCommands[aliasMatched]}"
            );

            return aliasMatched;
        }

        SkillId correctedSkill = MatchBySimilarity(
            normalizedText,
            out string correctedCommand,
            out float similarity
        );

        if (correctedSkill != SkillId.None)
        {
            ProjectLogger.UI(
                $"스킬 명령 보정: {recognizedText} → {correctedCommand} (유사도: {similarity:F2})"
            );

            return correctedSkill;
        }

        ProjectLogger.Warning(
            $"스킬 명령 판정 실패: {recognizedText} / normalized={normalizedText}"
        );

        return SkillId.None;
    }

    private string Normalize(string input)
    {
        return input.Trim()
                    .Replace(" ", "")
                    .Replace(".", "")
                    .Replace(",", "")
                    .Replace("!", "")
                    .Replace("?", "")
                    .Replace("~", "")
                    .Replace("요", "")
                    .Replace("을", "")
                    .Replace("를", "")
                    .Replace("이", "")
                    .Replace("가", "")
                    .Replace("은", "")
                    .Replace("는", "")
                    .Replace("해줘", "")
                    .Replace("써줘", "")
                    .Replace("사용", "")
                    .Replace("발동", "")
                    .Replace("시전", "");
    }

    private SkillId MatchByAlias(string normalizedText)
    {
        foreach (var pair in aliases)
        {
            foreach (string alias in pair.Value)
            {
                string normalizedAlias = Normalize(alias);

                if (normalizedText == normalizedAlias ||
                    normalizedText.Contains(normalizedAlias))
                {
                    return pair.Key;
                }
            }
        }

        return SkillId.None;
    }

    private SkillId MatchBySimilarity(
        string normalizedText,
        out string bestCommand,
        out float bestSimilarity)
    {
        SkillId bestSkill = SkillId.None;

        bestCommand = "";
        bestSimilarity = 0f;

        foreach (var pair in standardCommands)
        {
            string command = Normalize(pair.Value);

            float similarity = CalculateSimilarity(normalizedText, command);

            if (similarity > bestSimilarity)
            {
                bestSimilarity = similarity;
                bestCommand = pair.Value;
                bestSkill = pair.Key;
            }
        }

        if (bestSimilarity >= similarityThreshold)
            return bestSkill;

        return SkillId.None;
    }

    private float CalculateSimilarity(string source, string target)
    {
        int maxLength = Mathf.Max(source.Length, target.Length);

        if (maxLength == 0)
            return 1f;

        int distance = LevenshteinDistance(source, target);

        return 1f - (float)distance / maxLength;
    }

    private int LevenshteinDistance(string source, string target)
    {
        int[,] dp = new int[source.Length + 1, target.Length + 1];

        for (int i = 0; i <= source.Length; i++)
            dp[i, 0] = i;

        for (int j = 0; j <= target.Length; j++)
            dp[0, j] = j;

        for (int i = 1; i <= source.Length; i++)
        {
            for (int j = 1; j <= target.Length; j++)
            {
                int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                dp[i, j] = Mathf.Min(
                    Mathf.Min(
                        dp[i - 1, j] + 1,
                        dp[i, j - 1] + 1
                    ),
                    dp[i - 1, j - 1] + cost
                );
            }
        }

        return dp[source.Length, target.Length];
    }
}