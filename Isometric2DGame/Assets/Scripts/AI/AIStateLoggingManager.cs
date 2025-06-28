using UnityEngine;

public static class AIStateLoggingManager
{
    private static float lastLogTime = 0f;
    private static string lastMessage = "";
    private static readonly float logInterval = 1f; // Interval in seconds

    /// <summary>
    /// Logs a message if at least logInterval seconds have passed since the last log of the same message.
    /// </summary>
    public static void Log(string message)
    {
        if (message != lastMessage || Time.time - lastLogTime >= logInterval)
        {
            Debug.Log(message);
            lastLogTime = Time.time;
            lastMessage = message;
        }
    }

    public static void LogStateEnter(string stateName)
    {
        Debug.Log($"Enemy entered {stateName} state.");
    }

    public static void LogStateExit(string stateName)
    {
        Debug.Log($"Enemy exited {stateName} state.");
    }
}
