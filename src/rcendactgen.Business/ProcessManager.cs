using rcendactgen.Models;

namespace rcendactgen.Business;

public class ProcessManager
{
    private readonly ILogManager _logManager;
    private readonly IProcessWrapper _processWrapper;

    public ProcessManager(ILogManager logManager, IProcessWrapper processWrapper)
    {
        _logManager = logManager;
        _processWrapper = processWrapper;
    }

    public ProcessManager()
    {
        _logManager = new LogManager();
        _processWrapper = new ProcessWrapper();
    }
    public void StartProcess(string fullCommand)
    {
        var commandArr = fullCommand.Split(" ").ToList();
        string baseCommand = commandArr[0];
        commandArr.Remove(commandArr.First());
        string args = string.Join(" ", commandArr);
        var proc = _processWrapper.Start(baseCommand, args);
        
        ProcessStartActivity activity = new ProcessStartActivity
        {
            ProcessId = proc.Id,
            ProcessName = proc.ProcessName,
            UserName = Environment.UserName,
            ProcessCommandLine = fullCommand,
            StartTime = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
        };
        _logManager.WriteLog(activity);

    }
}