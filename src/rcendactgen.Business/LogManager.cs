using Newtonsoft.Json;
using rcendactgen.Common;
using rcendactgen.Models;

namespace rcendactgen.Business;
/// <summary>
/// LogManager business class to handle the LogRecord ActivityLog object
/// </summary>
public class LogManager : ILogManager
{
    private readonly ILogger _logger;
    private static LogRecord _logRecord = new LogRecord();

    public LogManager(ILogger logger)
    {
        _logger = logger;
        CreateLog();
    }

    public LogManager()
    {
        _logger = new Logger();
        CreateLog();
    }

    public void WriteLog<T>(T obj)
    {
        Type tp = obj.GetType();
        if (tp == typeof(ProcessStartActivity))
        {
            ProcessStartActivity? processStartActivity = obj as ProcessStartActivity;
            _logRecord.ProcessStartActivities.Add(processStartActivity);
        }
        else if (tp == typeof(FileActivity))
        {
            FileActivity? fileActivity = obj as FileActivity;
            _logRecord.FileActivities.Add(fileActivity);
        }
        else if (tp == typeof(NetworkActivity))
        {
            NetworkActivity? networkActivity = obj as NetworkActivity;
            _logRecord.NetworkActivities.Add(networkActivity);
        }
        _logger.WriteToLog(_logRecord);
        #if DEBUG
        WriteToConsole();
        #endif
    }

    public void WriteError(string message, Exception ex)
    {
        Console.WriteLine(@$"{message}. This is the available error: {ex.Message}");
    }
    private void CreateLog()
    {
        _logger.CreateFile();
    }

    private void WriteToConsole()
    {
        Console.WriteLine(JsonConvert.SerializeObject(_logRecord, Newtonsoft.Json.Formatting.Indented));
    }
}