using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class LogController : Singleton
{
    public static LogController Instance { get { return Instance<LogController>(); } }

    private List<Message> logged_messages = new List<Message>();

    public List<Message> LoggedMessages { get { return logged_messages.ToList(); } }

    private Color c_exception = new Color(0.9f, 0.3f, 0.3f);

    private const string LOG_FILENAME = "log.txt";

    public class Message
    {
        public string timeUtc;
        public string timeGame;
        public string message;
        public Color color = Color.white;

        public Message(string message)
        {
            this.message = message;
        }

        public string GetDebugMessage() => $"[{timeGame}] {message.Color(color)}";

        public string GetLogMessage() => $"[{timeUtc}] {message}";
    }

    protected override void Initialize()
    {
        base.Initialize();
        Application.logMessageReceived += LogException;

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

    public void LogException(Exception e)
    {
        try
        {
            LogMessage(e.Message).color = c_exception;
            LogMessage(e.StackTrace);
        }
        catch (Exception ex)
        {
            Debug.Log("Failed to log exception: " + ex.Message);
        }
    }

    private void LogException(string condition, string stacktrace, LogType type)
    {
        try
        {
            if (type == LogType.Exception)
            {
                LogMessage($"{condition}").color = c_exception;
                LogMessage($"{stacktrace}");
            }
        }
        catch (Exception ex)
        {
            LogMessage("Failed to log exception properly. Trying again:");
            LogMessage(condition);
            LogMessage(stacktrace);
        }

        WriteToPersistentData();
    }

    public Message LogMessage(string message)
    {
        try
        {
            var m = new Message(message)
            {
                timeGame = Time.time.ToString(),
                timeUtc = DateTime.UtcNow.ToString("hh:mm:ss.f", CultureInfo.InvariantCulture)
            };

            logged_messages.Add(m);
            return m;
        }
        catch (Exception e)
        {
            var m = new Message(message);
            logged_messages.Add(m);
            return m;
        }
    }
}