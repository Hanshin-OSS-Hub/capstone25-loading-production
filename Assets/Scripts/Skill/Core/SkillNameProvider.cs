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

    public static string GetMuninnCastMessage(SkillId skillId)
    {
        switch (skillId)
        {
            case SkillId.Blade:
                return "단검이 하늘을 가른다.";

            case SkillId.Storm:
                return "바람이 흐른다.";

            case SkillId.Barrier:
                return "방패가 길을 막는다.";

            case SkillId.Dash:
                return "돌진한다.";

            default:
                return "알 수 없는 힘이 스쳐 지나간다.";
        }
    }
}