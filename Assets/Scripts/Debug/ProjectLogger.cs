using UnityEngine;

public static class ProjectLogger
{
    public static void Input(string message)
    {
        Debug.Log($"<color=#64B5F6>[INPUT]</color> {message}");
    }

    public static void Record(string message)
    {
        Debug.Log($"<color=#FFB74D>[RECORD]</color> {message}");
    }

    public static void STT(string message)
    {
        Debug.Log($"<color=#4DD0E1>[STT]</color> {message}");
    }

    public static void UI(string message)
    {
        Debug.Log($"<color=#BA68C8>[UI]</color> {message}");
    }

    public static void Warning(string message)
    {
        Debug.LogWarning($"<color=#FFA726>[WARNING]</color> {message}");
    }

    public static void Error(string message)
    {
        Debug.LogError($"<color=#EF5350>[ERROR]</color> {message}");
    }
}