using System;
using System.Collections.Generic;

public static class Log
{
    private static List<string> logEntries = new List<string>();

    public static void Write(string message)
    {
        string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        logEntries.Add(timestampedMessage);

        UnityEngine.Debug.Log(timestampedMessage);
    }

    public static List<string> GetLogs()
    {
        return new List<string>(logEntries);
    }

    public static void Clear()
    {
        logEntries.Clear();
    }
}