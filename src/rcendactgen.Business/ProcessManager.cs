using rcendactgen.Common;
using rcendactgen.Models;

namespace rcendactgen.Business;
/// <summary>
/// ProcessManager business class to build ProcessStartActivity Obj that gets passed to LogManager
/// </summary>
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
        ProcessWrapperModel proc = null;
        try
        {
            var commandArr = fullCommand.Split(" ").ToList();
            string baseCommand = commandArr[0];
            commandArr.Remove(commandArr.First());
            string args = string.Join(" ", commandArr);
            proc = _processWrapper.Start(baseCommand, args);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error starting the process.
The current activity will not be logged to the activity log", ex);
            return;
        }
        try
        {
            ProcessStartActivity activity = new ProcessStartActivity
            {
                ProcessId = proc.Id,
                ProcessName = proc.ProcessName,
                UserName = CurrentUserInfo.GetCurrentUserWithDomain(),
                ProcessCommandLine = fullCommand,
                StartTime = proc.StartTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            };
            _logManager.WriteLog(activity);
        }
        catch (Exception ex)
        {
            _logManager.WriteError(@"There was an error logging the activity", ex);
        }
    }
}