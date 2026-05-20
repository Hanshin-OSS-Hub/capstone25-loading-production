using System.IO;
using UnityEngine;

public static class ConfigLoader
{
    public static string GeminiApiKey { get; private set; } = "";

    public static void Load()
    {
        string path = Path.Combine(Application.dataPath, "../config.json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("config.json 파일이 없습니다.");
            GeminiApiKey = "";
            return;
        }

        string json = File.ReadAllText(path);

        ConfigData data = JsonUtility.FromJson<ConfigData>(json);

        if (data == null || string.IsNullOrWhiteSpace(data.GEMINI_API_KEY))
        {
            Debug.LogWarning("GEMINI_API_KEY가 비어 있습니다.");
            GeminiApiKey = "";
            return;
        }

        GeminiApiKey = data.GEMINI_API_KEY;
    }

    [System.Serializable]
    private class ConfigData
    {
        public string GEMINI_API_KEY;
    }
}