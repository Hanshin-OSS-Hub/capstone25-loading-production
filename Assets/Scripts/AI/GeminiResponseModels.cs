using System.Collections.Generic;

[System.Serializable]
public class GeminiGenerateContentResponse
{
    public List<GeminiCandidate> candidates;
    public GeminiPromptFeedback promptFeedback;
}

[System.Serializable]
public class GeminiCandidate
{
    public GeminiContent content;
    public string finishReason;
    public int index;
}

[System.Serializable]
public class GeminiContent
{
    public List<GeminiPart> parts;
    public string role;
}

[System.Serializable]
public class GeminiPart
{
    public string text;
}

[System.Serializable]
public class GeminiPromptFeedback
{
    public List<GeminiSafetyRating> safetyRatings;
    public string blockReason;
}

[System.Serializable]
public class GeminiSafetyRating
{
    public string category;
    public string probability;
}