using System.Diagnostics;
using rcendactgen.Models;

namespace rcendactgen.Business;

public class ProcessManager
{
    private readonly ILogManager _logManager;

    public ProcessManager(ILogManager logManager)
    {
        _logManager = logManager;
    }

    public ProcessManager()
    {
        _logManager = new LogManager();
    }
    public void StartProcess(string fullCommand)
    {
        var commandArr = fullCommand.Split(" ").ToList();
        string baseCommand = commandArr[0];
        commandArr.Remove(commandArr.First());
        string args = string.Join(" ", commandArr);
        var proc = Process.Start(baseCommand, args);
        
        ProcessStartActivity activity = new ProcessStartActivity
        {
            ProcessId = proc.Id,
            ProcessName = proc.ProcessName,
            UserName = Environment.UserName,
            ProcessCommandLine = fullCommand,
            StartTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        };
        _logManager.WriteLog(activity);

    }
}