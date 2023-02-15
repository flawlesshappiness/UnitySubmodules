using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogController : Singleton
{
    public static LogController Instance { get { return Instance<LogController>(); } }

    private List<Message> logged_messages = new List<Message>();

    public List<Message> LoggedMessages { get { return logged_messages.ToList(); } }

    private Color c_exception = new Color(0.9f, 0.3f, 0.3f);

    public class Message
    {
        public float time;
        public string message;

        public Message(float time, string message)
        {
            this.time = time;
            this.message = message;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        Application.logMessageReceived += LogException;
    }

    private void LogException(string condition, string stacktrace, LogType type)
    {
        if(type == LogType.Exception)
        {
            var m = new Message(Time.time, stacktrace);
            LogMessage($"{condition.Color(c_exception)}\n{stacktrace}");
        }
    }

    public void LogMessage(string message)
    {
        var m = new Message(Time.time, message);
        logged_messages.Add(m);
        Debug.Log($"LogMessage: {message}");
    }
}