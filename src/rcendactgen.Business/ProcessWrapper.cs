using System.Diagnostics;
using rcendactgen.Models;

namespace rcendactgen.Business;

public class ProcessWrapper : IProcessWrapper
{
    public ProcessWrapperModel Start(string command, string args)
    {
        DateTime procStartTime;
        var proc = Process.Start(command, args);
        if (proc == null) throw new NullReferenceException("Process Object is Null");
        
        // Sometimes process exits too quickly and the StartTime value is not available anymore
        try
        {
            procStartTime = proc.StartTime;

        }
        catch (Exception)
        {
            procStartTime = DateTime.Now;
        }
        
        return new ProcessWrapperModel
        {
            Id = proc.Id,
            ProcessName = proc.ProcessName,
            StartTime = procStartTime
        };
    }

    public ProcessWrapperModel GetCurrentProcess(bool useDifferentTimestamp = false)
    {
        var proc = Process.GetCurrentProcess();
        return new ProcessWrapperModel
        {
            Id = proc.Id,
            ProcessName = proc.ProcessName,
            StartTime = useDifferentTimestamp ? DateTime.Now : proc.StartTime
        };
    }
}