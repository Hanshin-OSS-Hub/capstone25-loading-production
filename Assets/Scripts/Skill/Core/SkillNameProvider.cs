using UnityEngine;

public static class SkillNameProvider
{
    public static string GetKoreanName(SkillId skillId)
    {
        switch (skillId)
        {
            case SkillId.Blade:
                return "단검";

            case SkillId.Storm:
                return "바람";

            case SkillId.Barrier:
                return "방패";

            case SkillId.Dash:
                return "돌진";

            default:
                return "알 수 없음";
        }
    }
}