using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LogController : Singleton
{
    private const string LOG_FILENAME = "log.txt";

    public static LogController Instance { get { return Instance<LogController>(); } }

    private static List<Message> logged_messages = new List<Message>();
    public List<Message> LoggedMessages => logged_messages.ToList();

    public class Message
    {
        public string timeUtc;
        public string timeGame;
        public string message;
        public LogType log_type;

        public Message(string message)
        {
            this.message = message;
        }

        public string GetDebugMessage() => $"[{timeGame}] {message}";

        public string GetLogMessage() => $"[{timeUtc}] {message}";
    }

    protected override void Initialize()
    {
        base.Initialize();
        Application.logMessageReceived += LogUnhandledException;

        LogMessage("LogController initialized");
    }

    private void OnApplicationQuit()
    {
        WriteToPersistentData();
    }

    private void WriteToPersistentData()
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, LOG_FILENAME);
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var m in logged_messages)
                {
                    try
                    {
                        var message = m.GetLogMessage();
                        writer.WriteLine(message);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Failed to write log file to persistent data");
        }
    }

    public static void LogException(Exception e)
    {
        LogMessage($"{e.Message}\n{e.StackTrace}", LogType.Exception);
    }

    private void LogUnhandledException(string condition, string stacktrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            LogMessage($"ERROR: Unhandled exception\n{condition}\n{stacktrace}", LogType.Exception);
        }

        WriteToPersistentData();
    }

    public static void LogMethod(string message = "", [CallerFilePath] string file = "", [CallerMemberName] string caller = "")
    {
        if (string.IsNullOrEmpty(caller)) return;
        if (string.IsNullOrEmpty(file)) return;
        var filename = Path.GetFileNameWithoutExtension(file);
        LogMessage(string.IsNullOrEmpty(message) ? $"{filename}.{caller}" : $"{filename}.{caller}: {message}");
    }

    public static void LogMessage(string message, LogType type = LogType.Log)
    {
        var m = CreateMessage(message);
        m.log_type = type;
        logged_messages.Add(m);
    }

    private static Message CreateMessage(string message)
    {
        try
        {
            return new Message(message)
            {
                timeGame = Time.unscaledTime.ToString("0.00"),
                timeUtc = DateTime.UtcNow.ToString("hh:mm:ss.f", CultureInfo.InvariantCulture)
            };
        }
        catch (Exception e)
        {
            return new Message(message);
        }
    }
}